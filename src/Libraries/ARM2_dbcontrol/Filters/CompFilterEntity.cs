using System;


namespace ARM2_dbcontrol.Filters
{
	/// <summary>
	/// Фильтры компов
	/// </summary>
	public class CompFilterEntity: FilterEntity
	{
		//private string computerName = String.Empty;//
		private string userLogin = String.Empty;
		private string iPAddress = String.Empty; //
		private string latestMalware = String.Empty;
		private string domainName = String.Empty;//
		private string vbaVersion = String.Empty;
		private string ram = String.Empty;
		private string cpu = String.Empty;//
		private string osType = String.Empty;
		private DateTime recentActiveFrom = DateTime.MinValue;//
		private DateTime recentActiveTo = DateTime.MinValue;//
		private DateTime latestUpdateFrom = DateTime.MinValue;
		private DateTime latestUpdateTo = DateTime.MinValue;
		private DateTime latestInfectedFrom = DateTime.MinValue;
		private DateTime latestInfectedTo = DateTime.MinValue;
        private int latestInfectedIntervalIndex = Int32.MinValue;

        public int LatestInfectedIntervalIndex
        {
            get { return latestInfectedIntervalIndex; }
            set { latestInfectedIntervalIndex = value; }
        }

        private int recentActiveIntervalIndex = Int32.MinValue;

        public int RecentActiveIntervalIndex
        {
            get { return recentActiveIntervalIndex; }
            set { recentActiveIntervalIndex = value; }
        }
        private int latestUpdateIntervalIndex = Int32.MinValue;

        public int LatestUpdateIntervalIndex
        {
            get { return latestUpdateIntervalIndex; }
            set { latestUpdateIntervalIndex = value; }
        }

        private int latestInfectedIntervalModeIndex = 0;

        public int LatestInfectedIntervalModeIndex
        {
            get { return latestInfectedIntervalModeIndex; }
            set { latestInfectedIntervalModeIndex = value; }
        }
        private int recentActiveIntervalModeIndex = 0;

        public int RecentActiveIntervalModeIndex
        {
            get { return recentActiveIntervalModeIndex; }
            set { recentActiveIntervalModeIndex = value; }
        }
        private int latestUpdateIntervalModeIndex = 0;

        public int LatestUpdateIntervalModeIndex
        {
            get { return latestUpdateIntervalModeIndex; }
            set { latestUpdateIntervalModeIndex = value; }
        }

		private string vbaKeyValid = String.Empty;
		private string vbaIntegrity = String.Empty;
		private string controlCenter = String.Empty;
        private string description = String.Empty;

        private string termUserLogin = "AND";
        private string termIPAddress = "AND";
        private string termLatestMalware = "AND";
        private string termDomainName = "AND";
        private string termVbaVersion = "AND";
        private string termRam = "AND";
        private string termCpu = "AND";
        private string termOsType = "AND";
        private string termRecentActive = "AND";
        private string termLatestUpdate = "AND";
        private string termLatestInfected = "AND";
        private string termVbaKeyValid = "AND";
        private string termVbaIntegrity = "AND";
        private string termControlCenter = "AND";
        private string termDescription = "AND";

        #region constructors
        public CompFilterEntity()
		{
			//
			// TODO: Add constructor logic here
			//
            //base.dirtybit = false;
		}

		public CompFilterEntity(string filterName, string computerName, string userLogin, string iPAddress, string latestMalware,
			string domainName, string vbaVersion, string ram, string cpu, string osType,
			DateTime recentActiveFrom, DateTime recentActiveTo,DateTime latestUpdateFrom, DateTime latestUpdateTo,
			DateTime latestInfectedFrom, DateTime latestInfectedTo,string vbaKeyValid,
            string vbaIntegrity, string controlCenter, string description,

            string termComputerName, string termUserLogin, string termIPAddress, string termLatestMalware,
            string termDomainName, string termVbaVersion, string termRam, string termCpu, string termOsType,
            string termRecentActive, string termLatestUpdate, string termLatestInfected,
            string termVbaKeyValid, string termVbaIntegrity, string termControlCenter, string termDescription)
		{

			this.filterName = filterName;
			this.computerName = computerName;
			this.controlCenter = controlCenter;
			this.cpu = cpu;
			this.domainName = domainName;
			this.filterName = filterName;
			this.iPAddress = iPAddress;
			this.latestInfectedFrom = latestInfectedFrom;
			this.latestInfectedTo = latestInfectedTo;
			this.latestMalware = latestMalware;
			this.latestUpdateFrom = latestUpdateFrom;
			this.latestUpdateTo = latestUpdateTo;
			this.osType = osType;
			this.ram = ram;
			this.recentActiveFrom = recentActiveFrom;
			this.recentActiveTo = recentActiveTo;
			this.userLogin = userLogin;
			this.vbaIntegrity = vbaIntegrity;
			this.vbaKeyValid = vbaKeyValid;
			this.vbaVersion = vbaVersion;
            this.description = description;

            this.termComputerName = termComputerName;
            this.termControlCenter = termControlCenter;
            this.termCpu = termCpu;
            this.termDomainName = termDomainName;
            this.termIPAddress = termIPAddress;
            this.termLatestInfected = termLatestInfected;
            this.termLatestMalware = termLatestMalware;
            this.termLatestUpdate = termLatestUpdate;
            this.termOsType = termOsType;
            this.termRam = termRam;
            this.termRecentActive = termRecentActive;
            this.termUserLogin = termUserLogin;
            this.termVbaIntegrity = termVbaIntegrity;
            this.termVbaKeyValid = termVbaKeyValid;
            this.termVbaVersion = termVbaVersion;
            this.termDescription = termDescription;


        }

        #endregion

        /// <summary>
		/// Validate input strings
		/// </summary>
		/// <returns>true/false</returns>
		/// 
		public override bool CheckFilters()
		{

			return true;
		}

        /// <summary>
        /// Конструирует where-выражение sql-запроса
        /// </summary>
        /// <param name="keyword"></param>
        private void BuildQuery(string keyword)
        {
            if (termComputerName == keyword)
            {
                string[] array = computerName.Split('&');
                if (array.Length > 1)
                {
                    BuildDifferentQuery("ComputerName", termComputerName, array);
                }
                else
                    sqlWhereStatement += StringValue("ComputerName", computerName, termComputerName);

            }
            if (termCpu == keyword)
                sqlWhereStatement += IntValue("CPUClock", cpu, termCpu);

            if (termDomainName == keyword)
            {
                string[] array = domainName.Split('&');
                if (array.Length > 1)
                {
                    BuildDifferentQuery("DomainName", termDomainName, array);
                }
                else
                    sqlWhereStatement += StringValue("DomainName", domainName, termDomainName);
            }

            if (termIPAddress == keyword)
            {
                string[] array = iPAddress.Split('&');
                if (array.Length > 1)
                {
                    BuildDifferentQuery("IPAddress", termIPAddress, array);
                }
                else
                  sqlWhereStatement += StringIPValue("IPAddress", iPAddress, termIPAddress);
            }

            if (termRecentActive == keyword)
            {
                if (recentActiveIntervalIndex == Int32.MinValue)
                {
                    sqlWhereStatement += DateValue("RecentActive", recentActiveFrom, recentActiveTo, termRecentActive);
                }
                else
                {
                    if (RecentActiveIntervalModeIndex == 0)
                    {
                        sqlWhereStatement += DateValue("RecentActive", DateTime.Now.ToLocalTime().AddYears(-10),
                          base.GetDateInterval(recentActiveIntervalIndex), termRecentActive);
                    }
                    else
                    {
                        sqlWhereStatement += DateValue("RecentActive", base.GetDateInterval(recentActiveIntervalIndex),
                        DateTime.Now.ToLocalTime(), termRecentActive);
                    }
                }
            }

            if (termUserLogin == keyword)
            {
                string[] array = userLogin.Split('&');
                if (array.Length > 1)
                {
                    BuildDifferentQuery("UserLogin", termUserLogin, array);
                }
                else
                    sqlWhereStatement += StringValue("UserLogin", userLogin, termUserLogin);
            }

            if (termLatestMalware == keyword)
            {
                string[] array = latestMalware.Split('&');
                if (array.Length > 1)
                {
                    BuildDifferentQuery("LatestMalware", termLatestMalware, array);
                }
                else
                    sqlWhereStatement += StringValue("LatestMalware", latestMalware, termLatestMalware);
            }

            if (termVbaVersion == keyword)
            {
                string[] array = vbaVersion.Split('&');
                if (array.Length > 1)
                {
                    BuildDifferentQuery("Vba32Version", termVbaVersion, array);
                }
                else
                    sqlWhereStatement += StringValue("Vba32Version", vbaVersion, termVbaVersion);
            }
            
            if (termRam == keyword)
                sqlWhereStatement += IntValue("RAM", ram, termRam);

            if (termOsType == keyword)
            {
                string[] array = osType.Split('&');
                if (array.Length > 1)
                {
                    BuildDifferentQuery("OSName", termOsType, array);
                }
                else
                    sqlWhereStatement += StringValue("OSName", osType, termOsType);                
            }

            if (termLatestUpdate == keyword)
            {
                if (latestUpdateIntervalIndex == Int32.MinValue)
                {
                    sqlWhereStatement += DateValue("LatestUpdate", latestUpdateFrom, latestUpdateTo, termLatestUpdate);
                }
                else
                {
                    if (latestUpdateIntervalModeIndex == 0)
                    {
                        sqlWhereStatement += DateValue("LatestUpdate", DateTime.Now.ToLocalTime().AddYears(-10),
                          base.GetDateInterval(latestUpdateIntervalIndex), termLatestUpdate);
                    }
                    else
                    {
                        sqlWhereStatement += DateValue("LatestUpdate", base.GetDateInterval(latestUpdateIntervalIndex),
                          DateTime.Now.ToLocalTime(), termLatestUpdate);
                    }
                }
               
            }

            if (termLatestInfected == keyword)
            {
                System.Diagnostics.Debug.WriteLine("CompFilter: " + LatestInfectedIntervalIndex);
                if (latestInfectedIntervalIndex == Int32.MinValue)
                    sqlWhereStatement += DateValue("LatestInfected", latestInfectedFrom, latestInfectedTo, termLatestInfected);
                else
                {
                    if (latestInfectedIntervalModeIndex == 0)
                    {
                        sqlWhereStatement += DateValue("LatestInfected", DateTime.Now.ToLocalTime().AddYears(-10),
                            base.GetDateInterval(LatestInfectedIntervalIndex), termLatestInfected);
                    }
                    else
                    {
                        sqlWhereStatement += DateValue("LatestInfected", base.GetDateInterval(latestInfectedIntervalIndex),
                          DateTime.Now.ToLocalTime(), termLatestInfected);
                    }
                }
                System.Diagnostics.Debug.WriteLine("Statement="+sqlWhereStatement);
            }
            
            if (termControlCenter == keyword)
                sqlWhereStatement += BoolValue("ControlCenter", controlCenter, termControlCenter);
            
            if (termVbaIntegrity == keyword)
                sqlWhereStatement += BoolValue("Vba32Integrity", vbaIntegrity, termVbaIntegrity);
            
            if (termVbaKeyValid == keyword)
                sqlWhereStatement += BoolValue("Vba32KeyValid", vbaKeyValid, termVbaKeyValid);

            if (termDescription == keyword)
            {
                string[] array = description.Split('&');
                if (array.Length > 1)
                {
                    BuildDifferentQuery("Description", termDescription, array);
                }
                else
                    sqlWhereStatement += StringValue("Description", description, termDescription);
            }

        }

   


        /// <summary>
		/// Genarate sql where statement
		/// </summary>
		/// <returns> true/false</returns>
		/// 
		public override bool GenerateSQLWhereStatement()
		{
            base.sqlWhereStatement = null;
            base.dirtybit = false;

            //Не спрашивайте, почему это так...

            BuildQuery("AND");
            BuildQuery("NOT");
            BuildQuery("OR");

            //if ((sqlWhereStatement[0] == '(') && (sqlWhereStatement[sqlWhereStatement.Length - 1] == ')'))
            //{
            //    sqlWhereStatement = sqlWhereStatement.Remove(0, 1);
            //    sqlWhereStatement = sqlWhereStatement.Remove(sqlWhereStatement.Length - 1, 1);
            //}
                return true;
		}

		#region property

		public string Cpu
		{
			set{this.cpu = value;}
			get{return this.cpu;}
		}

		public string DomainName
		{
			set{this.domainName = value;}
			get{return this.domainName;}
		}

		public string IPAddress
		{
			set{this.iPAddress = value;}
			get{return this.iPAddress;}
		}

		public DateTime LatestInfectedFrom
		{
			set{this.latestInfectedFrom = value;}
			get{return this.latestInfectedFrom;}
		}

		public DateTime LatestInfectedTo
		{
			set{this.latestInfectedTo = value;}
			get{return this.latestInfectedTo;}
		}
		public string LatestMalware
		{
			set{this.latestMalware = value;}
			get{return this.latestMalware;}
		}

		public DateTime LatestUpdateFrom
		{
			set{this.latestUpdateFrom = value;}
			get{return this.latestUpdateFrom;}
		}


		public DateTime LatestUpdateTo
		{
			set{this.latestUpdateTo = value;}
			get{return this.latestUpdateTo;}
		}


		public string OsType
		{
			set{this.osType = value;}
			get{return this.osType;}
		}

		public string Ram
		{
			set{this.ram = value;}
			get{return this.ram;}
		}

		public DateTime RecentActiveFrom
		{
			set{this.recentActiveFrom = value;}
			get{return this.recentActiveFrom;}
		}

		public DateTime RecentActiveTo
		{
			set{this.recentActiveTo = value;}
			get{return this.recentActiveTo;}
		}

		public string UserLogin
		{
			set{this.userLogin = value;}
			get{return this.userLogin;}
		}

		public string VbaVersion
		{
			set{this.vbaVersion = value;}
			get{return this.vbaVersion;}
		}

		public string VbaIntegrity
		{
			set{this.vbaIntegrity = value;}
			get{return this.vbaIntegrity;}
		}

		public string VbaKeyValid
		{
			set{this.vbaKeyValid = value;}
			get{return this.vbaKeyValid;}
		}

		public string ControlCenter
		{
			set{this.controlCenter = value;}
			get{return this.controlCenter;}
		}

        public string Description
        {
            set { this.description = value; }
            get { return this.description; }
        }

        public string TermCpu
        {
            set { this.termCpu = value; }
            get { return this.termCpu; }
        }

        public string TermDomainName
        {
            set { this.termDomainName = value; }
            get { return this.termDomainName; }
        }

        public string TermIPAddress
        {
            set { this.termIPAddress = value; }
            get { return this.termIPAddress; }
        }

        public string TermLatestInfected
        {
            set { this.termLatestInfected = value; }
            get { return this.termLatestInfected; }
        }
     
        public string TermLatestMalware
        {
            set { this.termLatestMalware = value; }
            get { return this.termLatestMalware; }
        }

        public string TermLatestUpdate
        {
            set { this.termLatestUpdate = value; }
            get { return this.termLatestUpdate; }
        }


        public string TermOsType
        {
            set { this.termOsType = value; }
            get { return this.termOsType; }
        }

        public string TermRam
        {
            set { this.termRam = value; }
            get { return this.termRam; }
        }

        public string TermRecentActive
        {
            set { this.termRecentActive = value; }
            get { return this.termRecentActive; }
        }
 
        public string TermUserLogin
        {
            set { this.termUserLogin = value; }
            get { return this.termUserLogin; }
        }

        public string TermVbaVersion
        {
            set { this.termVbaVersion = value; }
            get { return this.termVbaVersion; }
        }

        public string TermVbaIntegrity
        {
            set { this.termVbaIntegrity = value; }
            get { return this.termVbaIntegrity; }
        }

        public string TermVbaKeyValid
        {
            set { this.termVbaKeyValid = value; }
            get { return this.termVbaKeyValid; }
        }

        public string TermControlCenter
        {
            set { this.termControlCenter = value; }
            get { return this.termControlCenter; }
        }

        public string TermDescription
        {
            set { this.termDescription = value; }
            get { return this.termDescription; }
        }


		#endregion

	}
}
