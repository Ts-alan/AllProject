using Crystal.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crystal.BusinessLogic.Abstract
{
    public interface IUserTrackedLogic
    {
        IEnumerable<ObservedDevice> GetTrackedDevicesDetail(IEnumerable<TrackedDevice> devices);
        IEnumerable<ObservedBatch> GetTrackedBatchesDetail(IEnumerable<TrackedBatch> batches);
    }
}
