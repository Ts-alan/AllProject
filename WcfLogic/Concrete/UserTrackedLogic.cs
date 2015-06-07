using Crystal.BusinessLogic.Abstract;
using Crystal.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WcfLogic
{
    public partial class WcfLogics : IUserTrackedLogic
    {
        public IEnumerable<ObservedDevice> GetTrackedDevicesDetail(IEnumerable<TrackedDevice> devices)
        {
            return bl.GetTrackedDevicesDetail(devices.ToArray());
        }

        public IEnumerable<ObservedBatch> GetTrackedBatchesDetail(IEnumerable<TrackedBatch> batches)
        {
            return bl.GetTrackedBatchesDetail(batches.ToArray());
        }
    }
}
