using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;

namespace ARM2_dbcontrol.Filters
{
    //!--OPTM Класс должен быть статическим или, по-крайней мере, методы
    /// <summary>
    /// Данный класс реализует проверку значений на соответствие шаблону
    /// Вообще, его лучше было бы сделать статическим
    /// </summary>
    public class Validation
    {
        private int MaxLength = Int16.MaxValue;
        private string val;


        public Validation(string val)
        {
            this.val = val;
        }

        public bool CheckStringValue()
        {
            Regex reg = new Regex(@"^[a-zA-Z\u0451\u0401а-яА-Я_0-9-]+$");
           Match match = reg.Match(val);
           if ((!match.Success)||val.Length==0)
               return false;

            return true;
        }

        public bool CheckStringFilterName()
        {
            Regex reg = new Regex(@"^[a-zA-Z_а-яА-Я0-9\u0020\u0451\u0401.\-]+$");
            Match match = reg.Match(val);
            if ((!match.Success) || val.Length == 0)
                return false;
            MaxLength = 40;
            return CheckStringLength();
        }

        public bool CheckStringOSName()
        {
           return true;
        }

        public bool CheckIPzone()
        {
            Regex reg = new Regex(@"^[0-9*_.]+$");
            Match match = reg.Match(val);
            if ((!match.Success) || val.Length == 0)
                return false;

            return true;
        }

        public bool CheckStringEnRuValue()
        {
            Regex reg = new Regex(@"^[a-zA-Zа-яА-Я_0-9]+$");
            Match match = reg.Match(val);
            if ((!match.Success) || val.Length == 0)
                return false;

            return true;
        }

        public bool CheckUserLogin()
        {
            Regex reg = new Regex(@"^[a-zA-Zа-яА-Я_0-9.]+$");
            Match match = reg.Match(val);
            if ((!match.Success) || val.Length == 0)
                return false;

            return true;
        }

        public bool CheckStringToFilter()
        {
            //\u0451 - маленькое ё, \u0401 - большое Ё
            Regex reg = new Regex(@"^[а-яА-Я\u0451\u0401a-zA-Z_0-9=*.\u0020:/\\\-\{\}#&%@()]+$");
            Match match = reg.Match(val);
            if (!match.Success) 
                return false;
            return true;
        }

        public bool CheckFileName()
        {
            Regex reg = new Regex(@"[\w\.\-]*.\w*");
            Match match = reg.Match(val);
            if (!match.Success)
                return false;
            return true;
        }

        public bool CheckPath()
        {
            return true;
        }

        public bool CheckStringToDescription()
        {
            Regex reg = new Regex(@"^[а-яА-Яa-zA-Z_0-9\u0451\u0401*.\u0020,!?)(]+$");
            Match match = reg.Match(val);
            if (!match.Success)
                return false;
            return true;
        }

        public bool CheckStringToTask()
        {
            Regex reg = new Regex(@"<\w+>");
            Match match = reg.Match(val);
            if (match.Success)
                return false;
            reg = new Regex(@"<>");
            match = reg.Match(val);
            if (match.Success)
                return false;
            return true;
        }

        public bool CheckStringToExtension()
        {
            Regex reg = new Regex(@"^[a-zA-Z_0-9*.?]+$");
            Match match = reg.Match(val);
            if (!match.Success)
                return false;
            return true;
        }

        public bool CheckStringLength()
        {
            if(val.Length > MaxLength)
                return false;
            return true;
        }

//Check digital values
        public bool CheckIntToFilter()
        {
            Regex reg = new Regex(@"^\d+-\d+$");
            Match match = reg.Match(val);
            if (!match.Success)
                return false;
      
            return true;
        }

        public bool CheckSize()
        {
            Regex reg = new Regex(@"^\d+$");
            Match match = reg.Match(val);
            if (!match.Success)
                return false;

            return true;
        }



        public bool CheckPositiveInteger()
        {
            try
            {
                ushort i = UInt16.Parse(val);
                if (i == 0)
                    throw new Exception();
            }
            catch
            {
                return false;
            }
           
            return true;
        }

        public bool CheckPercent()
        {
            try
            {
                int a = Convert.ToInt16(val);
                if ((a < 0) || (a > 100))
                    throw new Exception();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool CheckInterval()
        {
            try
            {
                int a = Convert.ToInt32(val);
                if ((a < 10) || (a > 86400))
                    throw new Exception();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool CheckUInt32()
        {
            try
            {
                uint a = Convert.ToUInt32(val);

            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool CheckUInt16()
        {
            try
            {
                int a = Convert.ToInt16(val);
            }
            catch
            {
                return false;
            }
            return true;
        }

//Check other values
        /// <summary>
        /// To Control center: check exist ip address
        /// </summary>
        /// <returns></returns>
        public bool CheckIP()
        {
           Regex reg = new Regex(@"^([0-9]|[0-9][0-9]|[01][0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[0-9][0-9]|[01][0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$");
           
            Match match = reg.Match(val);
            if (!match.Success)
                return false;
            return true;
        }

        public bool CheckEmail()
        {
            return Regex.IsMatch(val, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        #region Property

        public string Value
        {
            get
            {
                return this.val;
            }
            set
            {
                this.val = value;
            }
        }

        #endregion
    }
}
