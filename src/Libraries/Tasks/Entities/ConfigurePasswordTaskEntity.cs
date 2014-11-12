using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Tasks.Attributes;
using System.Security.Cryptography;
using VirusBlokAda.CC.Tasks.Service;

namespace VirusBlokAda.CC.Tasks.Entities
{
    [Serializable]
    [TaskEntity("task")]
    public class ConfigurePasswordTaskEntity : TaskEntity
    {
        private string tagPassword = "SecurityOptions";

        public ConfigurePasswordTaskEntity() : base("ConfigurePassword")
        {
        
        }

        public ConfigurePasswordTaskEntity(string password)
            : this()
        {
            _password = password;
        }

        private string _password;
        [TaskEntityStringProperty("Password")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        [Obsolete("Необходимо переопределить")]
        public override string ToTaskXml()
        {
            StringBuilder result = new StringBuilder(256);

            string str = Password;
            if (str != "")
            {
                //Вычисляем хэш и конвертим в base64
                MD5 md5Hasher = MD5.Create();
                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(str));
                str = "reg_binary:" + VirusBlokAda.CC.Common.Anchor.ConvertToDumpString(data); //Convert.ToBase64String(data);
                //
            }
            else
                str = "-";

            result.Append("<TaskConfigureSettings>");
            result.Append("<password>");
            result.AppendFormat("<{0}>{1}{/0}>", tagPassword, str);
            result.Append("</password>");
            result.Append("</TaskConfigureSettings>");
            return result.ToString();
        }
    }
}
