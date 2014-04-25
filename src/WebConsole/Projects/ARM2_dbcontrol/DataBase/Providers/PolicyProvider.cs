using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.DataBase;
using System.Data;
using System.Data.SqlClient;

namespace VirusBlokAda.CC.DataBase
{
    public class PolicyProvider
    {        
        PolicyManager policyMng;
        DeviceManager deviceMng;
        DevicePolicyManager devicePolicyMng;

        #region Dictionary

        private Dictionary<string, string> _computerPolicy = new Dictionary<string, string>();

        public Dictionary<string, string> ComputerPolicy
        {
            get { return _computerPolicy; }
            set { _computerPolicy = value; }
        }

        #endregion

        #region Constructions

        public PolicyProvider(string connectionString)
        {
            ConnectionString = connectionString;
            
            InitManagers();
        }

        #endregion


        private void InitManagers()
        {
            policyMng = new PolicyManager(_connection);
            deviceMng = new DeviceManager(_connection);
            devicePolicyMng = new DevicePolicyManager(_connection);
        }


        #region Response logic

        public string GetResponse(string computerName, string ip, string currentHash)
        {
            try
            {
                if (String.IsNullOrEmpty(computerName))
                    throw new ArgumentException("Computer name is not valid");

                string hash = "";
                string policy = "";
                lock (_computerPolicy)
                {
                    //Computer name is exist in dictionary
                    if (ComputerPolicy.ContainsKey(computerName))
                    {
                        hash = ComputerPolicy[computerName];
                        //hash is valid
                        if (hash == currentHash)
                            return GetNoChangeRequired();
                        else
                            //hash is invalid. need to get new 
                            ComputerPolicy.Remove(computerName);
                    }
                }
                //Getting policy to this computer..
                //Here we call database stored procedure
                GeneralPolicy gp = new GeneralPolicy(computerName, ip, Connection);
                policy = gp.GeneralizePolicy;
                hash = gp.Hash;
                lock (_computerPolicy)
                {
                    ComputerPolicy.Add(computerName, hash);
                }

                if (String.IsNullOrEmpty(hash))
                    return GetNoPolicy();
                
                return GetNewPolicy(policy, hash);
            }
            catch (ArgumentException argex)
            {
                return GetFault("0", argex.Message);
            }
            catch (Exception ex)
            {
                return GetFault("13", ex.Message);
            }

           // return GetFault("666", "Unknown server error");

        }

        private string GetNoChangeRequired()
        {
            return @"<Body><Policy><Status>OK</Status></Policy></Body>";
        }

        private string GetNoPolicy()
        {
            return @"<Body><Policy><Status>NO</Status></Policy></Body>";
        }

        private string GetNewPolicy(string policy, string hash)
        {
            return String.Format(
                @"<Body><Policy><Hash>{0}</Hash><Content><![CDATA[{1}]]></Content></Policy></Body>",
                hash, policy);
        }

        private string GetFault(string code, string reason)
        {
            return String.Format(
                @"<Body><Fault><Code>{0}</Code><Reason>{1}</Reason></Fault></Body>",
                code, reason);
        }

        #endregion


        #region DataBase logic

        private string _connectionString;

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }


        private SqlConnection _connection;

        public SqlConnection Connection
        {
            get 
            {
                lock (this)
                {
                    if (_connection == null)
                    {
                        _connection = new SqlConnection(ConnectionString);
                        _connection.Open();
                    }

                    if ((_connection.State == ConnectionState.Closed) ||
                    (_connection.State == ConnectionState.Broken))
                    {
                        if (_connection.State == ConnectionState.Broken)
                            _connection.Close();
                        //SqlConnection.ClearPool(_connection); //??
                        _connection.Open();
                    }

                    InitManagers();
                    
                }

                return _connection; 
            }
            /*private set 
            { 
                _connection = value;
                if ((_connection.State == ConnectionState.Closed) ||
                    (_connection.State == ConnectionState.Broken))
                {
                    _connection.Open();
                }
            }*/
        }

        #endregion


        #region Change object logic


        public ComputersEntity GetComputerByID(int id)
        {
            using (VlslVConnection conn = new VlslVConnection(ConnectionString))
            {
                conn.OpenConnection();

                ComputersManager cm = new ComputersManager(conn);
                return cm.GetComputer(id);
            }
        }

        /// <summary>
        /// Clear cache values that use this policy
        /// </summary>
        /// <param name="policy"></param>
        private void ChangePolicy(Policy policy)
        {
            
            List<ComputersEntity> computers = policyMng.GetComputersByPolicy(policy);

            List<string> compNames = new List<string>();
            foreach (ComputersEntity comp in computers)
                compNames.Add(comp.ComputerName);

            ChangeDevicePolicy(compNames);

            /*lock (_computerPolicy)
            {
                Dictionary<string, string>.KeyCollection keys = ComputerPolicy.Keys;
                foreach (string item in keys)
                {
                    if (compNames.Contains(item))
                        ComputerPolicy.Remove(item);
                }
            }*/
        }

        /// <summary>
        /// Clear cache values that use this computer
        /// </summary>
        /// <param name="computerName"></param>
        private void ChangeDevicePolicy(string computerName)
        {
            lock (_computerPolicy)
            {
                ComputerPolicy.Remove(computerName);
            }
        }

        private void ChangeDevicePolicyByGroup()
        {
            List<ComputersEntity> list;
            using (VlslVConnection conn = new VlslVConnection(ConnectionString))
            {
                conn.OpenConnection();
                GroupManager gm = new GroupManager(conn);
                list = gm.GetComputersWithoutGroup();
            }

            if (list == null) return;
            List<String> compNames = new List<String>();
            foreach (ComputersEntity comp in list)
            {
                compNames.Add(comp.ComputerName);
            }

            ChangeDevicePolicy(compNames);
        }

        private void ChangeDevicePolicyByGroup(Int32 groupId)
        {
            List<String> list;
            using (VlslVConnection conn = new VlslVConnection(ConnectionString))
            {
                conn.OpenConnection();
                GroupManager gm = new GroupManager(conn);
                list = gm.GetAllComputersNameByGroup(groupId);
            }

            if (list == null) return;
            ChangeDevicePolicy(list);
        }

        /// <summary>
        /// Clear cache values that use this computer
        /// </summary>
        /// <param name="computerName"></param>
        private void ChangeDevicePolicy(List<string> computers)
        {
            foreach (string item in computers)
                ChangeDevicePolicy(item);
        }

        public void ClearCache()
        {
            lock (_computerPolicy)
            {
                ComputerPolicy = new Dictionary<string, string>();
            }
        }

        #endregion


        #region Admins action at policy


        /// <summary>
        /// Add new policy to database
        /// </summary>
        /// <param name="policy">Policy name</param>
        /// <param name="content">Body of policy</param>
        public void AddPolicy(Policy policy)
        {
            policyMng.Connection = Connection;
            policyMng.Add(policy);
        }

        /// <summary>
        /// Remove policy
        /// </summary>
        /// <param name="policy">policy name</param>
        public void RemovePolicy(Policy policy)
        {
            policyMng.Connection = Connection;

            ChangePolicy(policy);

            policyMng.Delete(policy);

            
        }


        public void RemovePolicy(string policyName)
        {
            policyMng.Connection = Connection;

            Policy policy = policyMng.GetPolicyByName(policyName);
            RemovePolicy(policy);

        }

        /// <summary>
        /// Add list of computer to selected policy
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="computers"></param>
        public void AddComputersToPolicy(Policy policy, List<string> computers)
        {
            policyMng.Connection = Connection;

            policyMng.RemoveComputersFromAllPolicies(computers);
            policyMng.AddComputersToPolicy(policy, computers);

            ChangeDevicePolicy(computers);
        }

        /// <summary>
        /// Remove list of computer from all policies
        /// </summary>
        public void RemoveComputersFromAllPolicies(List<string> computers)
        {
            policyMng.Connection = Connection;

            policyMng.RemoveComputersFromAllPolicies(computers);

            ChangeDevicePolicy(computers);
        }
            

        /// <summary>
        /// Edit selected rule
        /// </summary>
        /// <param name="policy">Policy name</param>
        /// <param name="content">Body of policy</param>
        public void EditPolicy(Policy policy)
        {
            policyMng.Connection = Connection;
            
            policyMng.Edit(policy);

            
            ChangePolicy(policy);

            if (policy.Name == policyMng.GetDefaultPolicyName())
            {
                List<string> compNames = new List<string>();
                foreach (ComputersEntity comp in policyMng.GetComputersWithoutPolicyPage(1, Int16.MaxValue, "ComputerName ASC"))
                    compNames.Add(comp.ComputerName);
                ChangeDevicePolicy(compNames);
            }
        }

        public string GetDefaultPolicyName()
        {
            policyMng.Connection = Connection;
            return policyMng.GetDefaultPolicyName();
        }

        /// <summary>
        /// Remove selected computers from policy
        /// </summary>
        /// <param name="computers"></param>
        public void RemoveComputersFromPolicy(Policy policy, List<string> computers)
        {
            policyMng.Connection = Connection;
            policyMng.RemoveComputersFromPolicy(policy, computers);

            ChangeDevicePolicy(computers);
        }

        public Policy GetPolicyByName(string name)
        {
            policyMng.Connection = Connection;
            return policyMng.GetPolicyByName(name);
        }

        public Policy GetPolicyToComputer(string computerName)
        {
            policyMng.Connection = Connection;
            return policyMng.GetPolicyToComputer(computerName);
        }

        /// <summary>
        /// Clear all policies
        /// </summary>
        public void ClearAllPolicy()
        {
            policyMng.Connection = Connection;
            policyMng.ClearAllPolicy();
            ClearCache();
        }

        #endregion


        #region Admins action at devices

        public Device GetDevice(int id)
        {
            deviceMng.Connection = Connection;
            return deviceMng.GetDevice(id);
        }

        public Device AddDevice(Device device)
        {
            deviceMng.Connection = Connection;
            return deviceMng.Add(device);
        }

        public Device EditDevice(Device device)
        {
            deviceMng.Connection = Connection;
            return deviceMng.Edit(device);
        }

        public void DeleteDevice(Device device)
        {

            List<string> computers = new List<string>();

            DevicePolicyManager dpm = new DevicePolicyManager(Connection);

            foreach (ComputersEntity item
                in dpm.GetComputersByDevice(device))
                computers.Add(item.ComputerName);

            ChangeDevicePolicy(computers);

            deviceMng.Connection = Connection;
            deviceMng.Delete(device);

            
        }

        public void ChangeDevicePolicyStatusForComputer(DevicePolicy devicePolicy)
        {
            devicePolicyMng.Connection = Connection;
            devicePolicyMng.ChangeDevicePolicyStatusForComputer(devicePolicy);
            ChangeDevicePolicy(devicePolicy.Computer.ComputerName);
        }

        public void ChangeDevicePolicyStatusForComputer(Int16 deviceID ,Int16 computerID ,string  state)
        {
            devicePolicyMng.Connection = Connection;
            devicePolicyMng.ChangeDevicePolicyStatusForComputer(deviceID ,computerID ,state);
            ChangeDevicePolicy(GetComputerByID(computerID).ComputerName);
        }

        public DevicePolicy GetDevicePolicyByID(int id)
        {
             devicePolicyMng.Connection = Connection;
             return devicePolicyMng.GetDevicePolicyByID(id);
        }

        public void DeleteDevicePolicyByID(int id)
        {
            devicePolicyMng.Connection = Connection;
            DevicePolicy dp = devicePolicyMng.GetDevicePolicyByID(id);
            devicePolicyMng.DeleteDevicePolicyByID(id);
            ChangeDevicePolicy(dp.Computer.ComputerName);
        }

        public void RemoveDevicePolicyGroup(int devID, int groupID)
        {
            devicePolicyMng.Connection = Connection;
            devicePolicyMng.RemoveDevicePolicyGroup(devID, groupID);
            ChangeDevicePolicyByGroup(groupID);
        }

        public void RemoveDevicePolicyWithoutGroup(int devID)
        {
            devicePolicyMng.Connection = Connection;
            devicePolicyMng.RemoveDevicePolicyWithoutGroup(devID);
            ChangeDevicePolicyByGroup();
        }

        public void AddDevicePolicy(DevicePolicy devicePolicy)
        {
            devicePolicyMng.Connection = Connection;
            devicePolicyMng.Add(devicePolicy);
            ChangeDevicePolicy(devicePolicy.Computer.ComputerName);
        }

        public DevicePolicy AddDevicePolicyToComputer(DevicePolicy devicePolicy)
        {
            devicePolicyMng.Connection = Connection;
            DevicePolicy dp = devicePolicyMng.AddToComputer(devicePolicy);
            if (dp.Device.ID != 0)
                ChangeDevicePolicy(devicePolicy.Computer.ComputerName);
            return dp;
        }

        public Device AddDeviceToGroup(int groupID,Device device)
        {
            devicePolicyMng.Connection = Connection;
            Device dev = devicePolicyMng.AddToGroup(groupID,device);
            ChangeDevicePolicyByGroup(groupID);
            return dev;
        }

        public Device AddDeviceToWithoutGroup( Device device)
        {
            devicePolicyMng.Connection = Connection;
            Device dev = devicePolicyMng.AddToWithoutGroup(device);
            ChangeDevicePolicyByGroup();
            return dev;
        }

        #endregion


        #region Statistic actions

        /// <summary>
        /// Return all policies by specific device id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<DevicePolicy> GetPoliciesByDevice(Device device)
        {
            devicePolicyMng.Connection = Connection;
            return devicePolicyMng.GetPoliciesByDevice(device);
        }

        public List<DevicePolicy> GetDevicesPoliciesByComputer(string computerName)
        {
            devicePolicyMng.Connection = Connection;
            return devicePolicyMng.GetDeviceEntitiesFromComputer(computerName);
        }

        public List<DevicePolicy> GetDevicesPoliciesByComputer(int id)
        {
            string computerName;
            //crazy to write it..
            try
            {
                using (VlslVConnection conn = new VlslVConnection(ConnectionString))
                {
                    ComputersManager cmng = new ComputersManager(conn);

                    conn.OpenConnection();
                    conn.CheckConnectionState(true);

                    computerName = cmng.GetComputer(id).ComputerName;
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
                return null;
            }
            return GetDevicesPoliciesByComputer(computerName);
        }
        
        public List<Device> GetDevicesList(int index, int pageCount, string where, string orderBy)
        {
            deviceMng.Connection = Connection;
            return deviceMng.GetDevicesList(index, pageCount,
                where, orderBy);
        }

        public int GetDevicesCount(string where)
        {
            deviceMng.Connection = Connection;
            return deviceMng.GetDevicesCount(where);
        }

        public List<Device> GetDevicesList()
        {
            deviceMng.Connection = Connection;
            return deviceMng.GetDevicesList(1, Int32.MaxValue,
                null, null);
        }

        public List<Device> GetDevicesList(int index, int pageCount)
        {
            deviceMng.Connection = Connection;
            return deviceMng.GetDevicesList(index, pageCount,
                null, null);
        }

        public List<Policy> GetPolicyTypes()
        {
            policyMng.Connection = Connection;
            return policyMng.GetPolicyType();
        }

        public List<string> GetAllPolicyTypesNames()
        {

            List<string> names = new List<string>();
            foreach (Policy policy in GetPolicyTypes())
                names.Add(policy.Name);

            return names;
        }

        public List<ComputersEntity> GetComputersByPolicyPage(Policy policy, int index, int pageCount, string orderBy)
        {

            policyMng.Connection = Connection;
            //sql-injection?..
            string where = String.Format("[PolicyID] = {0}", policy.ID);

            return policyMng.GetComputersByPolicyPage(index, pageCount, where, orderBy);
        }

        public int GetComputerByPolicyCount(Policy policy)
        {
            policyMng.Connection = Connection;
            //sql-injection?..
            string where = String.Format("[PolicyID] = {0}", policy.ID);

            return policyMng.GetComputersByPolicyCount(where);
        }

        public List<ComputersEntity> GetComputersWithoutPolicyPage(int index, int pageCount, string orderBy)
        {

            policyMng.Connection = Connection;
            //sql-injection?..

            return policyMng.GetComputersWithoutPolicyPage(index, pageCount, orderBy);
        }

        public int GetComputerWithoutPolicyCount()
        {
            policyMng.Connection = Connection;

            return policyMng.GetComputersWithoutPolicyCount();
        }

        public List<DevicePolicy> GetUnknownDevicesPolicyPage(int index, int pageCount, string where, string orderBy)
        {
            string totalWhere = "StateName LIKE 'Undefined'";
            if(!String.IsNullOrEmpty(where)) 
                totalWhere  = String.Format("{0} AND {1}",totalWhere,where);

            devicePolicyMng.Connection = Connection;
            return devicePolicyMng.GetDevicesPolicyPage(index, pageCount, totalWhere, orderBy);
        }

        public int GetUnknownDevicesPolicyPageCount(string where)
        {
            string totalWhere = "StateName LIKE 'Undefined'";
            if (!String.IsNullOrEmpty(where))
                totalWhere = String.Format("{0} AND {1}", totalWhere, where);

                devicePolicyMng.Connection = Connection;
            return devicePolicyMng.GetDevicesPolicyPageCount(totalWhere);
        }

        public List<ComputersEntity> GetComputersByPolicy(Policy policy)
        {
            policyMng.Connection = Connection;
            return policyMng.GetComputersByPolicy(policy);
        }
        
        public List<DevicePolicy> GetDevicesPoliciesByGroup(Int32 groupID)
        {
            devicePolicyMng.Connection = Connection;
            return devicePolicyMng.GetDeviceEntitiesFromGroup(groupID);
        }
        
        public List<DevicePolicy> GetDevicesPoliciesWithoutGroup()
        {
            devicePolicyMng.Connection = Connection;
            return devicePolicyMng.GetDeviceEntitiesWithoutGroup();
        }

        public void ChangeDevicePolicyStatusForGroup(Int16 deviceID, Int32 groupID, string state)
        {
            devicePolicyMng.Connection = Connection;
            devicePolicyMng.ChangeDevicePolicyStatusForGroup(deviceID, groupID, state);
            ChangeDevicePolicyByGroup(groupID);
        }

        public void ChangeDevicePolicyStatusWithoutGroup(Int16 deviceID, string state)
        {
            devicePolicyMng.Connection = Connection;
            devicePolicyMng.ChangeDevicePolicyStatusToWithoutGroup(deviceID,state);
            ChangeDevicePolicyByGroup();
        }

        public List<string> GetPolicyStates()
        {
            devicePolicyMng.Connection = Connection;
            return devicePolicyMng.GetPolicyStates();
        }

        public Int32 GetDeviceCount(string where)
        {
            deviceMng.Connection = Connection;
            return deviceMng.GetDeviceCount(where);
        }

        public List<DevicePolicy> GetComputerListByDeviceID(Device device)
        {
            devicePolicyMng.Connection = Connection;
            return devicePolicyMng.GetComputerListByDeviceID(device);
        }

        public List<DevicePolicy> GetUnknownDevicesList(int index, int pageCount, string where, string orderBy)
        {
            devicePolicyMng.Connection = Connection;
            
            return devicePolicyMng.GetUnknownDevicesList(index, pageCount,
                where, orderBy);
        }

        public int GetUnknownDeviceCount(string where)
        {
            devicePolicyMng.Connection = Connection;
            return devicePolicyMng.GetUnknownDeviceCount(where);
        }

        public List<DevicePolicy> GetUnknownDevicesList()
        {
            devicePolicyMng.Connection = Connection;
            return devicePolicyMng.GetUnknownDevicesList(1, Int32.MaxValue,
                null, null);
        }

        public List<DevicePolicy> GetUnknownDevicesList(int index, int pageCount)
        {
            devicePolicyMng.Connection = Connection;
            return devicePolicyMng.GetUnknownDevicesList(index, pageCount,
                null, null);
        }
        #endregion

    }
}
