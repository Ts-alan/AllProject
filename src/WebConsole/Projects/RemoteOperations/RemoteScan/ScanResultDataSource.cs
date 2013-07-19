using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.RemoteOperations.RemoteScan;
using VirusBlokAda.RemoteOperations.RemoteScan.RemoteInfo;
using System.Reflection;
using VirusBlokAda.RemoteOperations.Net;
using System.Net;
using System.Diagnostics;

namespace VirusBlokAda.RemoteOperations.RemoteScan
{
    public class ScanResultDataSource
    {
        private RemoteScanner _remoteScanner = null;

        public ScanResultDataSource(RemoteScanner remoteScanner,
            Dictionary<int, RemoteInfoEntityShow> showingDict)
        {
            _remoteScanner = remoteScanner;
            _showingDict = showingDict;
            if (_showingDict == null)
            {
                _showingDict = new Dictionary<int, RemoteInfoEntityShow>();
            }
        }

        public ScanResultDataSource(RemoteScanner remoteScanner)
            : this(remoteScanner, null)
        { }

        public ScanResultDataSource()
            : this(null)
        { }


        public Dictionary<int, RemoteInfoEntityShow> ShowingDict
        {
            get
            {
                return _showingDict;
            }
        }
        private Dictionary<int, RemoteInfoEntityShow> _showingDict;


        private void UpdateResultDict()
        {
            if (_remoteScanner == null) return;
            if (_remoteScanner.ScanCompletedList == null) return;
            List<RemoteInfoEntity> scannerList = null;
            _remoteScanner.AcquireReaderLockOnCompletedList();
            try
            {
                scannerList = new List<RemoteInfoEntity>(_remoteScanner.ScanCompletedList);
            }
            catch (NullReferenceException)
            {
                //stop called
                return;
            }
            _remoteScanner.ReleaseReaderLockOnCompletedList();

            foreach (RemoteInfoEntity item in scannerList)
            {
                if (scannerList.Count == _showingDict.Count) break;
                if (!_showingDict.ContainsKey(item.Id))
                {
                    RemoteInfoEntityShow rie = new RemoteInfoEntityShow(item);
                    rie.Info = rie.OSVersion;
                    if (String.IsNullOrEmpty(rie.OSVersion))
                    {
                        rie.Info = rie.ErrorInfo;                        
                    }

                    if (String.IsNullOrEmpty(rie.OSVersion))
                    {
                        rie.IsDisabled = true;
                    }
                    else 
                    {
                        rie.IsDisabled = false;
                    }
                    _showingDict.Add(rie.Id, rie);
                }
            }
        }

        public List<RemoteInfoEntityShow> Get(int maximumRows, int startRowIndex, string sortExpression)
        {
            UpdateResultDict();

            List<RemoteInfoEntityShow> list = new List<RemoteInfoEntityShow>();
            List<RemoteInfoEntityShow> listFull = new List<RemoteInfoEntityShow>(_showingDict.Values);

            sortExpression += "";
            string[] parts = sortExpression.Split(' ');
            bool descending = false;
            string property = "";

            if (parts.Length > 0 && parts[0] != "")
            {
                property = parts[0];

                if (parts.Length > 1)
                {
                    descending = parts[1].ToLower().Contains("esc");
                }
                PropertyInfo prop = typeof(RemoteInfoEntityShow).GetProperty(property);
                if (prop == null)
                {
                    throw new Exception("No property '" + property + "' in RemoteInfoEntity");
                }
                if (descending)
                {
                    listFull.Sort(delegate(RemoteInfoEntityShow a, RemoteInfoEntityShow b)
                    {
                        if (property == "IPAddress")
                        {
                            Int64 adr1 = IPAddressHelper.IPAddressToLongBackwards(prop.GetValue(a, null) as IPAddress);
                            Int64 adr2 = IPAddressHelper.IPAddressToLongBackwards(prop.GetValue(b, null) as IPAddress);
                            int result = 0;
                            if (adr1 != adr2)
                            {
                                if (adr1 > adr2) result = 1;
                                else result = -1;
                            }
                            return result;
                        }
                        return (prop.GetValue(b, null) as IComparable).CompareTo(prop.GetValue(a, null));
                    });
                }
                else
                {
                    listFull.Sort(delegate(RemoteInfoEntityShow a, RemoteInfoEntityShow b)
                    {
                        if (property == "IPAddress")
                        {
                            Int64 adr1 = IPAddressHelper.IPAddressToLongBackwards(prop.GetValue(a, null) as IPAddress);
                            Int64 adr2 = IPAddressHelper.IPAddressToLongBackwards(prop.GetValue(b, null) as IPAddress);
                            int result = 0;
                            if (adr1 != adr2)
                            {
                                if (adr1 < adr2) result = 1;
                                else result = -1;
                            }
                            return result;
                        }
                        return (prop.GetValue(a, null) as IComparable).CompareTo(prop.GetValue(b, null));
                    });
                }
            }


            for (int i = startRowIndex; i < startRowIndex + maximumRows; i++)
            {
                if (i >= listFull.Count) break;
                list.Add(listFull[i]);
            }
            return list;
        }

        public int Count()
        {
            return _showingDict.Count;
        }
    }
}