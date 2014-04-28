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
        private String _connectionString;
        private VlslVConnection _connection;

        private PolicyManager policyMng;
        private DeviceManager deviceMng;
        private DevicePolicyManager devicePolicyMng;

        #region Dictionary

        private Dictionary<String, String> _computerPolicy = new Dictionary<String, String>();
        public Dictionary<String, String> ComputerPolicy
        {
            get { return _computerPolicy; }
            set { _computerPolicy = value; }
        }

        #endregion

        #region Constructions

        public PolicyProvider(String connectionString)
        {
            this._connectionString = connectionString;
            _connection = new VlslVConnection(_connectionString);
            InitManagers();
        }

        private void InitManagers()
        {
            policyMng = new PolicyManager(_connection);
            deviceMng = new DeviceManager(_connection);
            devicePolicyMng = new DevicePolicyManager(_connection);
        }

        #endregion

        #region Response logic

        public String GetResponse(String computerName, String ip, String currentHash)
        {
            try
            {
                if (String.IsNullOrEmpty(computerName))
                    throw new ArgumentException("Computer name is not valid");

                String hash = "";
                String policy = "";
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
                GeneralPolicy gp = new GeneralPolicy(computerName, ip, _connection);
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

        private String GetNoChangeRequired()
        {
            return @"<Body><Policy><Status>OK</Status></Policy></Body>";
        }

        private String GetNoPolicy()
        {
            return @"<Body><Policy><Status>NO</Status></Policy></Body>";
        }

        private String GetNewPolicy(String policy, String hash)
        {
            return String.Format(
                @"<Body><Policy><Hash>{0}</Hash><Content><![CDATA[{1}]]></Content></Policy></Body>",
                hash, policy);
        }

        private String GetFault(String code, String reason)
        {
            return String.Format(
                @"<Body><Fault><Code>{0}</Code><Reason>{1}</Reason></Fault></Body>",
                code, reason);
        }

        #endregion

        #region Change object logic

        public ComputersEntity GetComputerByID(Int32 id)
        {
                ComputersManager cm = new ComputersManager(_connection);
                return cm.GetComputer(id);
        }

        /// <summary>
        /// Clear cache values that use this policy
        /// </summary>
        /// <param name="policy"></param>
        private void ChangePolicy(Policy policy)
        {
            List<ComputersEntity> computers = policyMng.GetComputersByPolicy(policy);

            List<String> compNames = new List<String>();
            foreach (ComputersEntity comp in computers)
                compNames.Add(comp.ComputerName);

            ChangeDevicePolicy(compNames);
        }

        /// <summary>
        /// Clear cache values that use this computer
        /// </summary>
        /// <param name="computerName"></param>
        private void ChangeDevicePolicy(String computerName)
        {
            lock (_computerPolicy)
            {
                ComputerPolicy.Remove(computerName);
            }
        }

        private void ChangeDevicePolicyByGroup()
        {
            GroupManager gm = new GroupManager(_connection);
            List<ComputersEntity> list= gm.GetComputersWithoutGroup();

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
               GroupManager gm = new GroupManager(_connection);
            List<String> list = gm.GetAllComputersNameByGroup(groupId);

            if (list == null) return;
            ChangeDevicePolicy(list);
        }

        /// <summary>
        /// Clear cache values that use this computer
        /// </summary>
        /// <param name="computerName"></param>
        private void ChangeDevicePolicy(List<String> computers)
        {
            foreach (String item in computers)
                ChangeDevicePolicy(item);
        }

        public void ClearCache()
        {
            lock (_computerPolicy)
            {
                ComputerPolicy = new Dictionary<String, String>();
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
            policyMng.Add(policy);
        }

        /// <summary>
        /// Remove policy
        /// </summary>
        /// <param name="policy">policy name</param>
        public void RemovePolicy(Policy policy)
        {
            ChangePolicy(policy);
            policyMng.Delete(policy);
        }


        public void RemovePolicy(String policyName)
        {
            Policy policy = policyMng.GetPolicyByName(policyName);
            RemovePolicy(policy);
        }

        /// <summary>
        /// Add list of computer to selected policy
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="computers"></param>
        public void AddComputersToPolicy(Policy policy, List<String> computers)
        {
            policyMng.RemoveComputersFromAllPolicies(computers);
            policyMng.AddComputersToPolicy(policy, computers);

            ChangeDevicePolicy(computers);
        }

        /// <summary>
        /// Remove list of computer from all policies
        /// </summary>
        public void RemoveComputersFromAllPolicies(List<String> computers)
        {
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
            policyMng.Edit(policy);
            
            ChangePolicy(policy);

            if (policy.Name == PolicyManager.GetDefaultPolicyName())
            {
                List<String> compNames = new List<String>();
                foreach (ComputersEntity comp in policyMng.GetComputersWithoutPolicyPage(1, Int16.MaxValue, "ComputerName ASC"))
                    compNames.Add(comp.ComputerName);
                ChangeDevicePolicy(compNames);
            }
        }

        public String GetDefaultPolicyName()
        {
            return PolicyManager.GetDefaultPolicyName();
        }

        /// <summary>
        /// Remove selected computers from policy
        /// </summary>
        /// <param name="computers"></param>
        public void RemoveComputersFromPolicy(Policy policy, List<String> computers)
        {
            policyMng.RemoveComputersFromPolicy(policy, computers);

            ChangeDevicePolicy(computers);
        }

        public Policy GetPolicyByName(String name)
        {
            return policyMng.GetPolicyByName(name);
        }

        public Policy GetPolicyToComputer(String computerName)
        {
            return policyMng.GetPolicyToComputer(computerName);
        }

        /// <summary>
        /// Clear all policies
        /// </summary>
        public void ClearAllPolicy()
        {
            policyMng.ClearAllPolicy();
            ClearCache();
        }

        #endregion

        #region Admins action at devices

        public Device GetDevice(Int32 id)
        {
            return deviceMng.GetDevice(id);
        }

        public Device AddDevice(Device device)
        {
            return deviceMng.Add(device);
        }

        public Device EditDevice(Device device)
        {
            return deviceMng.Edit(device);
        }

        public void DeleteDevice(Device device)
        {
            List<String> computers = new List<String>();

            foreach (ComputersEntity item in devicePolicyMng.GetComputersByDevice(device))
                computers.Add(item.ComputerName);

            ChangeDevicePolicy(computers);

            deviceMng.Delete(device);
        }

        public void ChangeDevicePolicyStatusForComputer(DevicePolicy devicePolicy)
        {
            devicePolicyMng.ChangeDevicePolicyStatusForComputer(devicePolicy);
            ChangeDevicePolicy(devicePolicy.Computer.ComputerName);
        }

        public void ChangeDevicePolicyStatusForComputer(Int16 deviceID ,Int16 computerID ,String  state)
        {
            devicePolicyMng.ChangeDevicePolicyStatusForComputer(deviceID ,computerID ,state);
            ChangeDevicePolicy(GetComputerByID(computerID).ComputerName);
        }

        public DevicePolicy GetDevicePolicyByID(Int32 id)
        {
             return devicePolicyMng.GetDevicePolicyByID(id);
        }

        public void DeleteDevicePolicyByID(Int32 id)
        {
            DevicePolicy dp = devicePolicyMng.GetDevicePolicyByID(id);
            devicePolicyMng.DeleteDevicePolicyByID(id);
            ChangeDevicePolicy(dp.Computer.ComputerName);
        }

        public void RemoveDevicePolicyGroup(Int32 devID, Int32 groupID)
        {
            devicePolicyMng.RemoveDevicePolicyGroup(devID, groupID);
            ChangeDevicePolicyByGroup(groupID);
        }

        public void RemoveDevicePolicyWithoutGroup(Int32 devID)
        {
            devicePolicyMng.RemoveDevicePolicyWithoutGroup(devID);
            ChangeDevicePolicyByGroup();
        }

        public void AddDevicePolicy(DevicePolicy devicePolicy)
        {
            devicePolicyMng.Add(devicePolicy);
            ChangeDevicePolicy(devicePolicy.Computer.ComputerName);
        }

        public DevicePolicy AddDevicePolicyToComputer(DevicePolicy devicePolicy)
        {
            DevicePolicy dp = devicePolicyMng.AddToComputer(devicePolicy);
            if (dp.Device.ID != 0)
                ChangeDevicePolicy(devicePolicy.Computer.ComputerName);
            return dp;
        }

        public Device AddDeviceToGroup(Int32 groupID,Device device)
        {
            Device dev = devicePolicyMng.AddToGroup(groupID,device);
            ChangeDevicePolicyByGroup(groupID);
            return dev;
        }

        public Device AddDeviceToWithoutGroup( Device device)
        {
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
            return devicePolicyMng.GetPoliciesByDevice(device);
        }

        public List<DevicePolicy> GetDevicesPoliciesByComputer(String computerName)
        {
            return devicePolicyMng.GetDeviceEntitiesFromComputer(computerName);
        }

        public List<DevicePolicy> GetDevicesPoliciesByComputer(Int32 id)
        {
            ComputersManager cmng = new ComputersManager(_connection);
            return GetDevicesPoliciesByComputer(cmng.GetComputer(id).ComputerName);
        }
        
        public List<Device> GetDevicesList(Int32 index, Int32 pageCount, String where, String orderBy)
        {
            return deviceMng.GetDevicesList(index, pageCount,where, orderBy);
        }

        public Int32 GetDevicesCount(String where)
        {
            return deviceMng.GetDevicesCount(where);
        }

        public List<Device> GetDevicesList()
        {
            return deviceMng.GetDevicesList(1, Int32.MaxValue,null, null);
        }

        public List<Device> GetDevicesList(Int32 index, Int32 pageCount)
        {
            return deviceMng.GetDevicesList(index, pageCount,
                null, null);
        }

        public List<Policy> GetPolicyTypes()
        {
            return policyMng.GetPolicyType();
        }

        public List<String> GetAllPolicyTypesNames()
        {
            List<String> names = new List<String>();
            foreach (Policy policy in GetPolicyTypes())
                names.Add(policy.Name);

            return names;
        }

        public List<ComputersEntity> GetComputersByPolicyPage(Policy policy, Int32 index, Int32 pageCount, String orderBy)
        {
            ComputersManager compMng = new ComputersManager(_connection);

            return compMng.List(String.Format("[PolicyID] = {0}", policy.ID), orderBy, index, pageCount);
        }

        public Int32 GetComputerByPolicyCount(Policy policy)
        {
            ComputersManager compMng = new ComputersManager(_connection);

            return compMng.Count(String.Format("[PolicyID] = {0}", policy.ID));
        }

        public List<ComputersEntity> GetComputersWithoutPolicyPage(Int32 index, Int32 pageCount, String orderBy)
        {
            return policyMng.GetComputersWithoutPolicyPage(index, pageCount, orderBy);
        }

        public Int32 GetComputerWithoutPolicyCount()
        {
            return policyMng.GetComputersWithoutPolicyCount();
        }

        public List<DevicePolicy> GetUnknownDevicesPolicyPage(Int32 index, Int32 pageCount, String where, String orderBy)
        {
            String totalWhere = "StateName LIKE 'Undefined'";
            if(!String.IsNullOrEmpty(where)) 
                totalWhere  = String.Format("{0} AND {1}",totalWhere,where);

            return devicePolicyMng.GetDevicesPolicyPage(index, pageCount, totalWhere, orderBy);
        }

        public Int32 GetUnknownDevicesPolicyPageCount(String where)
        {
            String totalWhere = "StateName LIKE 'Undefined'";
            if (!String.IsNullOrEmpty(where))
                totalWhere = String.Format("{0} AND {1}", totalWhere, where);

            return devicePolicyMng.GetDevicesPolicyPageCount(totalWhere);
        }

        public List<ComputersEntity> GetComputersByPolicy(Policy policy)
        {
            return policyMng.GetComputersByPolicy(policy);
        }
        
        public List<DevicePolicy> GetDevicesPoliciesByGroup(Int32 groupID)
        {
            return devicePolicyMng.GetDeviceEntitiesFromGroup(groupID);
        }
        
        public List<DevicePolicy> GetDevicesPoliciesWithoutGroup()
        {
            return devicePolicyMng.GetDeviceEntitiesWithoutGroup();
        }

        public void ChangeDevicePolicyStatusForGroup(Int16 deviceID, Int32 groupID, String state)
        {
            devicePolicyMng.ChangeDevicePolicyStatusForGroup(deviceID, groupID, state);
            ChangeDevicePolicyByGroup(groupID);
        }

        public void ChangeDevicePolicyStatusWithoutGroup(Int16 deviceID, String state)
        {
            devicePolicyMng.ChangeDevicePolicyStatusToWithoutGroup(deviceID,state);
            ChangeDevicePolicyByGroup();
        }

        public List<String> GetPolicyStates()
        {
            return devicePolicyMng.GetPolicyStates();
        }

        public Int32 GetDeviceCount(String where)
        {
            return deviceMng.GetDeviceCount(where);
        }

        public List<DevicePolicy> GetComputerListByDeviceID(Device device)
        {
            return devicePolicyMng.GetComputerListByDeviceID(device);
        }

        public List<DevicePolicy> GetUnknownDevicesList(Int32 index, Int32 pageCount, String where, String orderBy)
        {
            return devicePolicyMng.GetUnknownDevicesList(index, pageCount,
                where, orderBy);
        }

        public Int32 GetUnknownDeviceCount(String where)
        {
            return devicePolicyMng.GetUnknownDeviceCount(where);
        }

        public List<DevicePolicy> GetUnknownDevicesList()
        {
            return devicePolicyMng.GetUnknownDevicesList(1, Int32.MaxValue,
                null, null);
        }

        public List<DevicePolicy> GetUnknownDevicesList(Int32 index, Int32 pageCount)
        {
            return devicePolicyMng.GetUnknownDevicesList(index, pageCount,
                null, null);
        }
        #endregion
    }
}
