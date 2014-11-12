using System;
using System.Collections.Generic;
using System.Text;
using Interop.vsisLib;

namespace VirusBlokAda.Vba32CC.Service.VSIS
{
    public struct UpdateProperties
    {
        public String AuthorityName;
        public String AuthorityPassword;
        public String ProxyAuthorityName;
        public String ProxyAuthorityPassword;
        public UInt32 ProxyType;
        public String ProxyAddress;
        public UInt32 ProxyPort;
        public String[] UpdatePathes;
        public String TempFolder;
        public PairString[] ExpandPathesList;
    }
}
