using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

namespace VirusBlokAda.CC.DataBase
{
	/// <summary>
	/// Summary description for SettingsEntity.
	/// </summary>
	public class SettingsEntity : ICloneable
	{				
		protected Boolean showComputerID;
        protected Boolean showIPAdress = true;
		protected Boolean showComputerName=true;
        protected Boolean showControlCenter = true;
        protected Boolean showDomainName = true;
        protected Boolean showUserLogin = true;
        protected Boolean showOSType = true;
        protected Boolean showRAM = true;
        protected Boolean showCPUClock = true;
        protected Boolean showRecentActive = true;
        protected Boolean showLatestUpdate = true;
        protected Boolean showVba32Version = true;
        protected Boolean showLatestInfected = true;
        protected Boolean showLatestMalware = true;
        protected Boolean showVba32Integrity = true;
        protected Boolean showVba32KeyValid = true;
        protected Boolean showDescription = true;
        protected Boolean showPolicyName = true;
		//

        protected Int32 intervalAutoUpdateComputers = 3600;
        protected Int32 intervalAutoUpdateEvents = 300;
        protected Int32 intervalAutoUpdateTasks = 600;
        protected Int32 intervalAutoUpdateProcesses = 600;
        protected Int32 intervalAutoUpdateComponents = 600;
        protected Int32 intervalAutoUpdateTasksInstall = 600;

        protected Boolean enableAutoUpdateComputers = true;
        protected Boolean enableAutoUpdateEvents = true;
        protected Boolean enableAutoUpdateTasks = true;
        protected Boolean enableAutoUpdateProcesses = true;
        protected Boolean enableAutoUpdateComponents = true;
        protected Boolean enableAutoUpdateTasksInstall = true;
		
		//Default constructor
		public SettingsEntity() {}
		
		//Constructor
		public SettingsEntity(
			Boolean showComputerID,
			Boolean showIPAdress,
			Boolean showComputerName,
			Boolean showControlCenter,
			Boolean showDomainName,
			Boolean showUserLogin,
			Boolean showOSType,
			Boolean showRAM,
			Boolean showCPUClock,
            Boolean showRecentActive,
			Boolean showLatestUpdate,
			Boolean showVba32Version,
			Boolean showLatestInfected,
			Boolean showLatestMalware,
			Boolean showVba32Integrity,
			Boolean showVba32KeyValid,
            Boolean showPolicyName,
            Boolean showDescription,
            Int32 intervalAutoUpdateComputers,
            Int32 intervalAutoUpdateEvents,
            Int32 intervalAutoUpdateTasks,
            Int32 intervalAutoUpdateProcesses,
            Int32 intervalAutoUpdateComponents,
            Int32 intervalAutoUpdateTasksInstall,
            Boolean enableAutoUpdateComputers,
            Boolean enableAutoUpdateEvents,
            Boolean enableAutoUpdateTasks,
            Boolean enableAutoUpdateProcesses,
            Boolean enableAutoUpdateComponents,
            Boolean enableAutoUpdateTasksInstall
			//
			) 
		{
			this.showComputerID = showComputerID;
			this.showIPAdress = showIPAdress;
			this.showComputerName = showComputerName;
			this.showControlCenter = showControlCenter;
			this.showDomainName = showDomainName;
			this.showUserLogin = showUserLogin;
			this.showOSType = showOSType;
			this.showRAM = showRAM;
			this.showCPUClock = showCPUClock;
			this.showRecentActive = showRecentActive;
			this.showLatestUpdate = showLatestUpdate;
			this.showVba32Version = showVba32Version;
			this.showLatestInfected = showLatestInfected;
			this.showLatestMalware = showLatestMalware;
			this.showVba32Integrity = showVba32Integrity;
			this.showVba32KeyValid = showVba32KeyValid;
            this.showPolicyName = showPolicyName;
            this.showDescription = showDescription;

            this.intervalAutoUpdateComputers = intervalAutoUpdateComputers;
            this.intervalAutoUpdateEvents = intervalAutoUpdateEvents;
            this.intervalAutoUpdateTasks = intervalAutoUpdateTasks;
            this.intervalAutoUpdateProcesses = intervalAutoUpdateProcesses;
            this.intervalAutoUpdateComponents = intervalAutoUpdateComponents;
            this.intervalAutoUpdateTasksInstall = intervalAutoUpdateTasksInstall;

            this.enableAutoUpdateComputers = enableAutoUpdateComputers;
            this.enableAutoUpdateEvents = enableAutoUpdateEvents;
            this.enableAutoUpdateTasks = enableAutoUpdateTasks;
            this.enableAutoUpdateProcesses = enableAutoUpdateProcesses;
            this.enableAutoUpdateComponents = enableAutoUpdateComponents;
            this.enableAutoUpdateTasksInstall = enableAutoUpdateTasksInstall;
		}
		
		#region Public Properties	

		public Boolean ShowComputerID
		{
			get {return showComputerID;}
			set {showComputerID = value;}
		}

		public Boolean ShowIPAdress
		{
			get {return showIPAdress;}
			set {showIPAdress = value;}
		}

		public Boolean ShowComputerName
		{
			get {return showComputerName;}
			set {showComputerName = value;}
		}

		public Boolean ShowControlCenter
		{
			get {return showControlCenter;}
			set {showControlCenter = value;}
		}

		public Boolean ShowDomainName
		{
			get {return showDomainName;}
			set {showDomainName = value;}
		}

		public Boolean ShowUserLogin
		{
			get {return showUserLogin;}
			set {showUserLogin = value;}
		}

		public Boolean ShowOSType
		{
			get {return showOSType;}
			set {showOSType = value;}
		}

		public Boolean ShowRAM
		{
			get {return showRAM;}
			set {showRAM = value;}
		}

		public Boolean ShowCPUClock
		{
			get {return showCPUClock;}
			set {showCPUClock = value;}
		}

		public Boolean ShowRecentActive
		{
			get {return showRecentActive;}
			set {showRecentActive = value;}
		}

		public Boolean ShowLatestUpdate
		{
			get {return showLatestUpdate;}
			set {showLatestUpdate = value;}
		}

		public Boolean ShowVba32Version
		{
			get {return showVba32Version;}
			set {showVba32Version = value;}
		}

		public Boolean ShowLatestInfected
		{
			get {return showLatestInfected;}
			set {showLatestInfected = value;}
		}

		public Boolean ShowLatestMalware
		{
			get {return showLatestMalware;}
			set {showLatestMalware = value;}
		}

		public Boolean ShowVba32Integrity
		{
			get {return showVba32Integrity;}
			set {showVba32Integrity = value;}
		}

		public Boolean ShowVba32KeyValid
		{
			get {return showVba32KeyValid;}
			set {showVba32KeyValid = value;}
		}

        public Boolean ShowPolicyName
        {
            get { return showPolicyName; }
            set { showPolicyName = value; }
        }

        public Boolean ShowDescription
        {
            get { return showDescription; }
            set { showDescription = value; }
        }

        public Int32 IntervalAutoUpdateComputers
        {
            get{return intervalAutoUpdateComputers;}
            set{intervalAutoUpdateComputers = value;}
        }

         public Int32 IntervalAutoUpdateEvents
        {
            get{return intervalAutoUpdateEvents;}
            set{intervalAutoUpdateEvents = value;}
        }

         public Int32 IntervalAutoUpdateTasks
        {
            get{return intervalAutoUpdateTasks;}
            set{intervalAutoUpdateTasks = value;}
        }

         public Int32 IntervalAutoUpdateProcesses
        {
            get{return intervalAutoUpdateProcesses;}
            set{intervalAutoUpdateProcesses = value;}
        }

         public Int32 IntervalAutoUpdateComponents
        {
            get{return intervalAutoUpdateComponents;}
            set{intervalAutoUpdateComponents = value;}
        }

         public Int32 IntervalAutoUpdateTasksInstall
         {
             get { return intervalAutoUpdateTasksInstall; }
             set { intervalAutoUpdateTasksInstall = value; }
         }

        public Boolean EnableAutoUpdateComputers
        {
            get { return enableAutoUpdateComputers; }
            set { enableAutoUpdateComputers = value; }
        }

        public Boolean EnableAutoUpdateEvents
        {
            get { return enableAutoUpdateEvents; }
            set { enableAutoUpdateEvents = value; }
        }

        public Boolean EnableAutoUpdateTasks
        {
            get { return enableAutoUpdateTasks; }
            set { enableAutoUpdateTasks = value; }
        }

        public Boolean EnableAutoUpdateProcesses
        {
            get { return enableAutoUpdateProcesses; }
            set { enableAutoUpdateProcesses = value; }
        }

        public Boolean EnableAutoUpdateComponents
        {
            get { return enableAutoUpdateComponents; }
            set { enableAutoUpdateComponents = value; }
        }
        public Boolean EnableAutoUpdateTasksInstall
        {
            get { return enableAutoUpdateTasksInstall; }
            set { enableAutoUpdateTasksInstall = value; }
        }

		#endregion
		
		/// <summary>
		/// Create and return clone object
		/// </summary>
		/// <returns>Clone object</returns>
		public virtual object Clone()
		{
			return new SettingsEntity(
				this.showComputerID,
				this.showIPAdress,
				this.showComputerName,
				this.showControlCenter,
				this.showDomainName,
				this.showUserLogin,
				this.showOSType,
				this.showRAM,
				this.showCPUClock,
				this.showRecentActive,
				this.showLatestUpdate,
				this.showVba32Version,
				this.showLatestInfected,
				this.showLatestMalware,
				this.showVba32Integrity,
				this.showVba32KeyValid,
                this.showPolicyName,
                this.showDescription,
                this.intervalAutoUpdateComputers,
                this.intervalAutoUpdateEvents,
                this.intervalAutoUpdateTasks,
                this.intervalAutoUpdateProcesses,
                this.intervalAutoUpdateComponents,
                this.intervalAutoUpdateTasksInstall,
                this.enableAutoUpdateComputers,
                this.enableAutoUpdateEvents,
                this.enableAutoUpdateTasks,
                this.enableAutoUpdateProcesses,
                this.enableAutoUpdateComponents,
                this.enableAutoUpdateTasksInstall
				//
				);			
		}


		#region Serialization

		/// <summary>
		/// Преобразует объект в строку для  сохранения в базе
		/// </summary>
		public String Serialize() 
		{    
			StringWriter writer = new StringWriter();
			XmlSerializer serializer = new XmlSerializer(this.GetType());
			serializer.Serialize(writer, this);  
			return writer.ToString();
		}

		/// <summary>
		/// Извлекает хмлину из базы данных и преобразует в объект
		/// </summary>
		/// <returns>settings entity</returns>
		public SettingsEntity Deserialize(String settings)
		{
            if(String.IsNullOrEmpty(settings))
                return new SettingsEntity();
			XmlSerializer xmlser = new XmlSerializer(this.GetType());
			StringReader reader = new StringReader(settings);			
			return (SettingsEntity)xmlser.Deserialize(reader);
		}

		#endregion

		
	}

}
