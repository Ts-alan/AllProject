using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Security.Cryptography;
using VirusBlokAda.CC.Common.Xml;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Tasks.ConfigureJournalEvent;

namespace VirusBlokAda.CC.DataBase
{
    internal class GeneralPolicy
    {
        private readonly Object lockToken = new Object();

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

        public GeneralPolicy(String computerName, String ip, String connectionString)
        {
            lock (lockToken)
            {
                VerifyComputer(computerName, ip);

                PolicyManager pm = new PolicyManager(connectionString);
                DevicePolicyManager dpm = new DevicePolicyManager(connectionString);

                List<IConfigureTask> tasks = new List<IConfigureTask>();
                String devicePolicy;

                StringBuilder sb = new StringBuilder(2048);
                XmlBuilder xml = new XmlBuilder();
                sb.Append(xml.Top);
                sb.Append(@"<Tasks>");

                PolicyParser parser = null;
                try
                {
                    parser = new PolicyParser(pm.GetPolicyToComputer(computerName).Content);
                }
                catch
                {
                    //if exception has occured we force to recreate connection in next request
                    throw new Exception("Unable to get policy from database. Connection will be zero");
                }

                tasks.Add(ConvertTask(parser, TaskType.ConfigureLoader));
                tasks.Add(ConvertTask(parser, TaskType.ConfigureMonitor));
                tasks.Add(ConvertTask(parser, TaskType.ConfigureQuarantine));
                tasks.Add(ConvertTask(parser, TaskType.MonitorOn));

                try
                {
                    IConfigureTask task = ConvertTask(parser, TaskType.JornalEvents);
                    devicePolicy = dpm.GetPolicyToComputer(computerName, (task == null ? "" : task.GetTask()));
                }
                catch
                {
                    throw new Exception("Unable to get device policy from database. Connection will be zero");
                }

                Boolean isEmptyTasks = true;
                foreach (IConfigureTask tsk in tasks)
                {
                    if (tsk != null)
                    {
                        sb.Append(tsk.GetTask());
                        isEmptyTasks = false;
                    }
                }

                if (isEmptyTasks && String.IsNullOrEmpty(devicePolicy))
                    return;
                
                sb.Append(devicePolicy);

                sb.Append(@"</Tasks>");

                GeneralizePolicy = sb.ToString();

                EncryptPolicy();

                CalculateHash();
            }
        }

        private static IConfigureTask ConvertTask(PolicyParser parser, TaskType taskType)
        {
            String xml = VirusBlokAda.CC.Common.Anchor.FromBase64String(parser.GetParam(taskType.ToString()));
            IConfigureTask task = CreateTask(taskType);
            task.LoadFromXml(xml);
            return task;
        }

        private static IConfigureTask CreateTask(TaskType taskType)
        {
            switch (taskType)
            {
                case TaskType.ConfigureLoader:
                    return new TaskConfigureLoader();
                case TaskType.ConfigureMonitor:
                    return new TaskConfigureMonitor();
                case TaskType.ConfigureQuarantine:
                    return new TaskConfigureQuarantine();
                case TaskType.JornalEvents:
                    return new JournalEvent();
                case TaskType.MonitorOn:
                    return new TaskMonitorOnOff();
                default:
                    throw new ArgumentException("TaskType is not defined.");
            }
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
