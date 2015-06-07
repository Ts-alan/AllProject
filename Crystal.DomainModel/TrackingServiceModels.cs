using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Crystal.DomainModel
{
    
    [DataContract]
    public class TrackedItem
    {
        [DataMember]
        public Guid RecordId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string ProdNumber { get; set; }
    }

    [KnownType(typeof(TrackedItem))]
    [DataContract]
    public class TrackedDevice : TrackedItem
    {
        [DataMember]
        public string DeviceNumber { get; set; }
    }

    [KnownType(typeof(TrackedItem))]
    [DataContract]
    public class TrackedBatch : TrackedItem
    {
        [DataMember]
        public string BatchNumber { get; set; }
    }
}
