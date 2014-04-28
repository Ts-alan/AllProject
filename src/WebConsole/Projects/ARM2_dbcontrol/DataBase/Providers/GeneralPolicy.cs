using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Security.Cryptography;
using VirusBlokAda.CC.Common.Xml;

namespace VirusBlokAda.CC.DataBase
{
    internal class GeneralPolicy
    {
        private String _generalizePolicy = String.Empty;
        public String GeneralizePolicy
        {
            get { return _generalizePolicy; }
            set { _generalizePolicy = value; }
        }

        private String _hash = String.Empty;
        public String Hash
        {
            get { return _hash; }
            set { _hash = value; }
        }

        public GeneralPolicy(String computerName, String ip, VlslVConnection conn)
        {
            VerifyComputer(computerName, ip);

            PolicyManager pm = new PolicyManager(conn);
            DevicePolicyManager dpm = new DevicePolicyManager(conn);

            Policy policy;
            String devicePolicy;
            ///to try to fix 0007326

            //conn isn't thread-safe
            lock (conn)
            {
                try
                {
                    //get policy and usb settings
                    policy = pm.GetPolicyToComputer(computerName);
                    devicePolicy = dpm.GetPolicyToComputer(computerName);    
                }
                catch
                {
                    //if exception has occured we force to recreate connection in next request
                    conn = null;
                    throw new Exception("Unable to get policy from database. Connection will be zero");
                }
            }
            if ((String.IsNullOrEmpty(policy.Content)) && (String.IsNullOrEmpty(devicePolicy)))
                    return;
            

            //build policy
            StringBuilder sb = new StringBuilder(2048);
            XmlBuilder xml = new XmlBuilder();

            sb.Append(xml.Top);
            sb.Append(@"<Tasks>");

            sb.Append(policy.Content);
            sb.Append(devicePolicy);

            sb.Append(@"</Tasks>");

            GeneralizePolicy = sb.ToString();


            EncryptPolicy();

            CalculateHash();

        }

        private bool VerifyComputer(String computerName, String ip)
        {
            //ћожет здесь стоит создать свой словарь computerName - IP

            //throw new ArgumentException("Cannot verify your request");
            return true;
        }

        private void EncryptPolicy()
        {
            if (String.IsNullOrEmpty(GeneralizePolicy))
            {
                return;
            }

            //Encrypt GeneralizePolicy

            Encrypt();

        }

        private void Encrypt()
        {
            byte[] cryptBytes = Encoding.UTF8.GetBytes(GeneralizePolicy);

            int length = cryptBytes.Length;
            for (int i = 0; i < length; ++i)
            {
                cryptBytes[i] ^= 0x17;
            }

            GeneralizePolicy = Encoding.UTF8.GetString(cryptBytes);

        }

        private void CalculateHash()
        {
            if (String.IsNullOrEmpty(GeneralizePolicy))
            {
                Hash = "";
            }
            else
            {
                //Calculate Hash
                MD5 md5Hasher = MD5.Create();
                byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(GeneralizePolicy));

                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                    sBuilder.Append(data[i].ToString("x2"));

                Hash = sBuilder.ToString().ToUpper();
            }
        }


    }
}
