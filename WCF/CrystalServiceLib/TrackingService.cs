using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Crystal.BusinessLogic;
using Crystal.DomainModel;
using EFContext;

namespace WcfGetDataLib
{
    [ServiceContract]
    public interface ITrackingDataManagers
    {
        [OperationContract]
        void AddDeviceItemTo(TrackedDevice item);

        [OperationContract]
        void RemoveDeviceItemFrom(TrackedDevice item);

        [OperationContract]
        IEnumerable<TrackedDevice> GetTrackedDeviceItemsByUser(int userId);

        [OperationContract]
        void AddBatchItemTo(TrackedBatch item);

        [OperationContract]
        void RemoveBatchItemFrom(TrackedBatch item);

        [OperationContract]
        IEnumerable<TrackedBatch> GetTrackedBatchItemsByUser(int userId);

    }



    public class WcfTrackService : ITrackingDataManagers
    {

        ITrackingDataManager td;

        public void AddDeviceItemTo(TrackedDevice item)
        {
            td = new DeviceDataManagerEF();
            td.AddItemTo(item);

        }

        public void RemoveDeviceItemFrom(TrackedDevice item)
        {
            td = new DeviceDataManagerEF();
            td.RemoveItemFrom(item);
        }

        public IEnumerable<TrackedDevice> GetTrackedDeviceItemsByUser(int userId)
        {
            td = new DeviceDataManagerEF();
            return td.GetTrackedItemsByUser(userId).Cast<TrackedDevice>();
        }

        public void AddBatchItemTo(TrackedBatch item)
        {
            td = new BatchDataManagerEF();
            td.AddItemTo(item);

        }

        public void RemoveBatchItemFrom(TrackedBatch item)
        {
            td = new BatchDataManagerEF();
            td.RemoveItemFrom(item);
        }

        public IEnumerable<TrackedBatch> GetTrackedBatchItemsByUser(int userId)
        {
            td = new BatchDataManagerEF();
            return td.GetTrackedItemsByUser(userId).Cast<TrackedBatch>();
        }


    }


}
