using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace VirusBlokAda.CC.RemoteOperations.Net
{
    public class IPRange
    {
        private readonly IPAddress _from;
        public IPAddress From
        {
            get { return _from; }
        }
        private readonly IPAddress _to;
        public IPAddress To
        {
            get { return _to; }
        }
        public IPRange(IPAddress ip1, IPAddress ip2)
        {
            UInt32 lip1 = IPAddressHelper.IPAddressToLongBackwards(ip1);
            UInt32 lip2 = IPAddressHelper.IPAddressToLongBackwards(ip2);
            if (lip1 < lip2)
            {
                _from = ip1;
                _to = ip2;
            }
            else
            {
                _from = ip2;
                _to = ip1;
            }
        }

        public override bool Equals(object obj)
        {
            IPRange r = (obj as IPRange);
            if (r == null) return false;
            return (r._to == this._to && this._from == r._from);
        }

        public override int GetHashCode()
        {
            UInt32 from = IPAddressHelper.IPAddressToLongBackwards(_from);
            UInt32 to = IPAddressHelper.IPAddressToLongBackwards(_to);
            UInt64 key = ((UInt64)from << 32) + to;
            key = (~key) + (key << 18);
            key = key ^ (key >> 31);
            key = key * 21;
            key = key ^ (key >> 11);
            key = key + (key << 6);
            key = key ^ (key >> 22);
            return (int)key;
        }
    }
}
