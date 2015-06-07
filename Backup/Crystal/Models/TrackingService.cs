using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Crystal.DomainModel;
using Crystal.BusinessLogic;

namespace Crystal.Models
{
    public interface ITrackingService
    {
        void AddToTrackingList(TrackedItem item);
        void RemoveFromTrackingList(TrackedItem item);
        bool IsTracked(TrackedItem item);
        string GetItemName();
        IEnumerable<TrackedItem> GetUserTrackedItems(int? userId = null);
    }

    

    public abstract class TrackingService : ITrackingService
    {
        private readonly ITrackingDataManager dataManager;

        public TrackingService(ITrackingDataManager dm)
        {
            dataManager = dm;
        }

        public abstract string GetItemName();
        public abstract bool IsTracked(TrackedItem item);

        public void AddToTrackingList(TrackedItem item)
        {

            if (!IsTracked(item))
            {
                item.RecordId = Guid.NewGuid();
                dataManager.AddItemTo(item);
                HttpContext.Current.Session[GetItemName()] = null;
            }

        }

        public void RemoveFromTrackingList(TrackedItem item)
        {
            if (IsTracked(item))
            {
                dataManager.RemoveItemFrom(item);
                HttpContext.Current.Session[GetItemName()] = null;
            }
        }

        public IEnumerable<TrackedItem> GetUserTrackedItems(int? userId = null)
        {
            var trackedItems = (IEnumerable<TrackedItem>)HttpContext.Current.Session[GetItemName()];

            if (trackedItems == null)
            {
                if (!userId.HasValue)
                {
                    userId = System.Web.Security.Membership.GetUser(false).ProviderUserKey.GetHashCode();
                }
                trackedItems = dataManager.GetTrackedItemsByUser(userId.Value);
                HttpContext.Current.Session[GetItemName()] = trackedItems;
            }
            return trackedItems;
        }
    }

    public class DeviceTrackingService : TrackingService
    {
        public DeviceTrackingService(ITrackingDataManager dm) : base(dm){}

        public override string GetItemName()
        {
            return "TrackedDevices";
        }

        public override bool IsTracked(TrackedItem item)
        {
            var trackedItems = GetUserTrackedItems(item.UserId).Cast<TrackedDevice>();

            var test = trackedItems.FirstOrDefault(a => a.ProdNumber == item.ProdNumber && a.DeviceNumber == (item as TrackedDevice).DeviceNumber);
            return test != null;
        }
    }

    public class BatchTrackingService : TrackingService
    {
        public BatchTrackingService(ITrackingDataManager dm) : base(dm){}

        public override string GetItemName()
        {
            return "TrackedBatches";
        }

        public override bool IsTracked(TrackedItem item)
        {
            var trackedItems = GetUserTrackedItems(item.UserId).Cast<TrackedBatch>();

            var test = trackedItems.FirstOrDefault(a => a.ProdNumber == item.ProdNumber && a.BatchNumber == (item as TrackedBatch).BatchNumber);
            return test != null;
        }
    }
}

