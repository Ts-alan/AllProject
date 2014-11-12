using System;

using System.Text.RegularExpressions;

namespace ARM2_dbcontrol.Filters
{
	/// <summary>
	/// Базовый класс для формирования фильтров
	/// </summary>
	public class FilterEntity
	{
		protected string filterName = String.Empty; // Name of filter
		protected bool dirtybit = false;
		protected string sqlWhereStatement = String.Empty;
        private bool _isGroup = false;

        protected string computerName = String.Empty;
        protected string termComputerName = "AND";
        
        #region Properties

        public string ComputerName
        {
            set
            {
                this.computerName = value;
            }
            get
            {
                return this.computerName;
            }
        }

        public string TermComputerName
        {
            set
            {
                this.termComputerName = value;
            }
            get
            {
                return this.termComputerName;
            }
        }

        /// <summary>
        /// Сигнализирует о том, что это группа. Соответственно, правила 
        /// генерации несколько другие
        /// </summary>
        public bool IsGroup
        {
            get
            {
                return _isGroup;
            }
            set
            {
                _isGroup = value;
            }
        }

        public string GetSQLWhereStatement
        {
            get
            {
                if (!String.IsNullOrEmpty(this.sqlWhereStatement))
                    return this.sqlWhereStatement;
                else
                    return null;
            }
            set
            {
                this.sqlWhereStatement = value;
            }
        }

        public string FilterName
        {
            set
            {
                this.filterName = value;
            }
            get
            {
                return this.filterName;
            }
        }

        #endregion
                
		public FilterEntity()
		{}

        public virtual bool CheckFilters()
        {
            return true;
        }
        public virtual bool GenerateSQLWhereStatement()
        {
            return false;
        }


        /// <summary>
        /// Формируем несколько логических выражений для одного поля таблицы
        /// </summary>
        /// <param name="name">имя поля таблицы</param>
        /// <param name="term">логическое условие</param>
        /// <param name="array">массив строк-фильтров</param>
        protected void BuildDifferentQuery(string name, string term, string[] array)
        {
            if (dirtybit)
            {
                if (term == "OR")
                    sqlWhereStatement += "OR (";
                else
                    sqlWhereStatement += "AND (";
                dirtybit = false;
            }
            else
                sqlWhereStatement += "(";

            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i] == "")
                    continue;
                if (term != "NOT")
                {
                    sqlWhereStatement += StringValue(name, array[i], "AND");
                    sqlWhereStatement += " OR ";
                }
                else
                {
                    sqlWhereStatement += StringValue(name, array[i], "NOT");
                    sqlWhereStatement += " AND ";
                }
                dirtybit = false;
            }

            string lastValue = array[array.Length - 1];

            if (lastValue != "")
                if (term != "NOT")
                    sqlWhereStatement += StringValue(name, lastValue, "AND");
                else
                    sqlWhereStatement += StringValue(name, lastValue, "NOT");
            else
                sqlWhereStatement += StringValue(name, "DaDaMenyaNeDolzhnoBytVBaseDannyh", "AND");


            sqlWhereStatement += ")";
        }

        protected DateTime GetDateInterval(int index)
        {
            DateTime dt = DateTime.Now.ToLocalTime();
            switch (index)
            {
                case 0:
                    return dt.AddMinutes(-1);
                case 1:
                    return dt.AddMinutes(-5);
                case 2:
                    return dt.AddMinutes(-15);
                case 3:
                    return dt.AddMinutes(-30);
                case 4:
                    return dt.AddHours(-1);
                case 5:
                    return dt.AddHours(-3);
                case 6:
                    return dt.AddHours(-6);
                case 7:
                    return dt.AddHours(-12);
                case 8:
                    return dt.AddDays(-1);
                case 9:
                    return dt.AddDays(-2);
                case 10:
                    return dt.AddDays(-3);
                case 11:
                    return dt.AddDays(-7);
                case 12:
                    return dt.AddDays(-14);
                case 13:
                    return dt.AddDays(-21);
                case 14:
                    return dt.AddMonths(-1);
                case 15:
                    return dt.AddMonths(-2);
                case 16:
                    return dt.AddMonths(-3);
                case 17:
                    return dt.AddMonths(-6);
                case 18:
                    return dt.AddYears(-1);
                default:
                    return dt;
            }

        }


		#region c# to sql types where string 

        protected string BoolValue(string name, string val, string term)
        {
            if (val == String.Empty) { return String.Empty; }

            string final;
            if (term == "NOT")
                final = name + " <> " + val + " ";
            else
                final = name + " = " + val + " ";

            if (dirtybit)
            {
                if (term == "OR")
                    final = " OR " + final;
                else
                    final = " AND " + final;
            }
            dirtybit = true;

            return final;
        }


		/// <summary>
		/// Return filter string to int type
		/// </summary>
		/// <param name="name">name in database</param>
		/// <param name="val">value</param>
		/// <returns>sql string</returns>
        protected string IntValue(string name, string val, string term)
		{	
			if(val == String.Empty)	return String.Empty;
            
            //validation
            Validation vld = new Validation(val);
            if(!vld.CheckIntToFilter())
                throw new ArgumentException("Invalid value: " + name);
          
            //parsing...
            string[] s = val.Split('-');
			
            //s[1] < s[0] - излишество
            if (Convert.ToInt32(s[1]) < Convert.ToInt32(s[0]))
                throw new ArgumentException("Invalid value: " + name);

            string final;
            if (term != "NOT")
            {
                final = name + " BETWEEN " + s[0] + " AND " + s[1] + " ";
                if (dirtybit) final = " " + term + " " + final;
            }
            else
            {
                final = name + " NOT BETWEEN " + s[0] + " AND " + s[1] + " ";
                if (dirtybit) final = " AND " + final;
            }

            dirtybit = true;

			return final;			
		}

     	/// <summary>
		/// Return filter string to data type
		/// </summary>
		/// <param name="name">name in database</param>
		/// <param name="val1">value from</param>
		/// <param name="val2">value to</param>
		/// <returns></returns>
		protected string DateValue(string name, DateTime val1, DateTime val2, string term)
		{
			if((val1 == DateTime.MinValue)||(val2 == DateTime.MinValue)) {return String.Empty;}

            string final;
            if (term != "NOT")
            {
                final = name + " > = " + String.Format("CAST('{0:yyyy}-{0:MM}-{0:dd}T{0:HH}:{0:mm}:{0:ss}' AS SMALLDATETIME)", val1) + " AND " + name + " < = " + String.Format("CAST('{0:yyyy}-{0:MM}-{0:dd}T{0:HH}:{0:mm}:{0:ss}' AS SMALLDATETIME)", val2);
                    
                if (dirtybit) final = " " + term + " (" + final + ")";
            }
            else
            {
                final = name + " < " + String.Format("CAST('{0:yyyy}-{0:MM}-{0:dd}T{0:HH}:{0:mm}:{0:ss}' AS SMALLDATETIME)", val1) + " OR " + name + " > " + String.Format("CAST('{0:yyyy}-{0:MM}-{0:dd}T{0:HH}:{0:mm}:{0:ss}' AS SMALLDATETIME)", val2);
                if (dirtybit) final = " AND (" + final + ")";
            }
			dirtybit = true;
			return final;
		}

        /// <summary>
        /// Return filter string to string type
        /// </summary>
        /// <param name="name">name in database</param>
        /// <param name="val">value</param>
        /// <returns></returns>
        protected string StringValue(string name, string val, string term)
        {
            if (val == String.Empty) { return String.Empty; }

            //validation
            Validation vld = new Validation(val);
            if ((!vld.CheckStringToFilter()) || (!vld.CheckStringLength()))
                throw new ArgumentException("Invalid value: " + name);

            return InternalStringValue(name, val, term);
        }


        /// <summary>
        /// String os name value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        protected string StringOSNameValue(string name, string val, string term)
        {
            if (val == String.Empty) { return String.Empty; }

            //validation
            Validation vld = new Validation(val);
            if ((!vld.CheckStringOSName()) || (!vld.CheckStringLength()))
                throw new ArgumentException("Invalid value: " + name);

            return InternalStringValue(name, val, term);
        }
    
        /// <summary>
        /// String IP value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        protected string StringIPValue(string name, string val, string term)
        {
            if (val == String.Empty) { return String.Empty; }

            //validation
            Validation vld = new Validation(val);
            if ((!vld.CheckIPzone()) || (!vld.CheckStringLength()))
                throw new ArgumentException("Invalid value: " + name);

            return InternalStringValue(name, val, term);
        }

        private string InternalStringValue(string name, string val, string term)
        {
            val = val.TrimEnd(new char[] { ' ' });
            val = val.Replace("%", "[%]");
            val = val.Replace('*', '%');
            
            string final;
            if (term == "NOT")
                final = "(" + name + " NOT LIKE '" + val + "' OR " + name + " IS NULL) ";
            else
                final = name + " LIKE '" + val + "' ";

            if (dirtybit)
            {
                if (term == "OR")
                    final = " OR " + final;
                else
                    final = " AND " + final;
            }
            else
            {
                if (term == "OR")
                    final = "(" + final + " OR " + final + " )";

            }

            dirtybit = true;
            return final;
        }



		#endregion
	}
}