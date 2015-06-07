using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crystal.BusinessLogic;
using Crystal.DomainModel;


namespace WcfLogic
{
    //BatchDataManagerWCF
    public class DeviceDataManagerWCF : ITrackingDataManager
    {
        public Plants RepoPlant;
        public WcfLogic.ServiceReference2.TrackingDataManagersClient tr;

        public DeviceDataManagerWCF()
        {
            //RepoPlant = plant;
            tr = new WcfLogic.ServiceReference2.TrackingDataManagersClient();
        }

        public void AddItemTo(TrackedItem item)
        {
           
            tr.AddDeviceItemTo(item as TrackedDevice );
        }

        public void RemoveItemFrom(TrackedItem item)
        {
            tr.RemoveDeviceItemFrom(item as TrackedDevice);
        }

        public IEnumerable<TrackedItem> GetTrackedItemsByUser(int userId)
        {
           return  tr.GetTrackedDeviceItemsByUser(userId);
        }
    }

    public class BatchDataManagerWCF : ITrackingDataManager
    {

        public Plants RepoPlant;
        public WcfLogic.ServiceReference2.TrackingDataManagersClient tr;

        public BatchDataManagerWCF()
        {
            //RepoPlant = plant;
            tr = new WcfLogic.ServiceReference2.TrackingDataManagersClient();
        }

        public void AddItemTo(TrackedItem item)
        {
            tr.AddBatchItemTo(item as TrackedBatch);
        }

        public void RemoveItemFrom(TrackedItem item)
        {
            tr.RemoveBatchItemFrom(item as TrackedBatch);
        }

        public IEnumerable<TrackedItem> GetTrackedItemsByUser(int userId)
        {
           return  tr.GetTrackedBatchItemsByUser(userId);
        }
    }
}
