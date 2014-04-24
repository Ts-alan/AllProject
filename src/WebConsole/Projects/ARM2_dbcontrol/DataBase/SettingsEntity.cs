using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

namespace VirusBlokAda.Vba32CC.DataBase
{
	/// <summary>
	/// Summary description for SettingsEntity.
	/// </summary>
	public class SettingsEntity : ICloneable
	{				
		protected bool showComputerID;
        protected bool showIPAdress = true;
		protected bool showComputerName=true;
        protected bool showControlCenter = true;
        protected bool showDomainName = true;
        protected bool showUserLogin = true;
        protected bool showOSType = true;
        protected bool showRAM = true;
        protected bool showCPUClock = true;
        protected bool showRecentActive = true;
        protected bool showLatestUpdate = true;
        protected bool showVba32Version = true;
        protected bool showLatestInfected = true;
        protected bool showLatestMalware = true;
        protected bool showVba32Integrity = true;
        protected bool showVba32KeyValid = true;
        protected bool showDescription = true;
        protected bool showPolicyName = true;
		//

        protected int intervalAutoUpdateComputers = 3600;
        protected int intervalAutoUpdateEvents = 300;
        protected int intervalAutoUpdateTasks = 600;
        protected int intervalAutoUpdateProcesses = 600;
        protected int intervalAutoUpdateComponents = 600;
        protected int intervalAutoUpdateTasksInstall = 600;

        protected bool enableAutoUpdateComputers = true;
        protected bool enableAutoUpdateEvents = true;
        protected bool enableAutoUpdateTasks = true;
        protected bool enableAutoUpdateProcesses = true;
        protected bool enableAutoUpdateComponents = true;
        protected bool enableAutoUpdateTasksInstall = true;
		
		//Default constructor
		public SettingsEntity() {}
		
		//Constructor
		public SettingsEntity(
			bool showComputerID,
			bool showIPAdress,
			bool showComputerName,
			bool showControlCenter,
			bool showDomainName,
			bool showUserLogin,
			bool showOSType,
			bool showRAM,
			bool showCPUClock,
            bool showRecentActive,
			bool showLatestUpdate,
			bool showVba32Version,
			bool showLatestInfected,
			bool showLatestMalware,
			bool showVba32Integrity,
			bool showVba32KeyValid,
            bool showPolicyName,
            bool showDescription,
            int intervalAutoUpdateComputers,
            int intervalAutoUpdateEvents,
            int intervalAutoUpdateTasks,
            int intervalAutoUpdateProcesses,
            int intervalAutoUpdateComponents,
            int intervalAutoUpdateTasksInstall,
            bool enableAutoUpdateComputers,
            bool enableAutoUpdateEvents,
            bool enableAutoUpdateTasks,
            bool enableAutoUpdateProcesses,
            bool enableAutoUpdateComponents,
            bool enableAutoUpdateTasksInstall
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

		public bool ShowComputerID
		{
			get {return showComputerID;}
			set {showComputerID = value;}
		}

		public bool ShowIPAdress
		{
			get {return showIPAdress;}
			set {showIPAdress = value;}
		}

		public bool ShowComputerName
		{
			get {return showComputerName;}
			set {showComputerName = value;}
		}

		public bool ShowControlCenter
		{
			get {return showControlCenter;}
			set {showControlCenter = value;}
		}

		public bool ShowDomainName
		{
			get {return showDomainName;}
			set {showDomainName = value;}
		}

		public bool ShowUserLogin
		{
			get {return showUserLogin;}
			set {showUserLogin = value;}
		}

		public bool ShowOSType
		{
			get {return showOSType;}
			set {showOSType = value;}
		}

		public bool ShowRAM
		{
			get {return showRAM;}
			set {showRAM = value;}
		}

		public bool ShowCPUClock
		{
			get {return showCPUClock;}
			set {showCPUClock = value;}
		}

		public bool ShowRecentActive
		{
			get {return showRecentActive;}
			set {showRecentActive = value;}
		}

		public bool ShowLatestUpdate
		{
			get {return showLatestUpdate;}
			set {showLatestUpdate = value;}
		}

		public bool ShowVba32Version
		{
			get {return showVba32Version;}
			set {showVba32Version = value;}
		}

		public bool ShowLatestInfected
		{
			get {return showLatestInfected;}
			set {showLatestInfected = value;}
		}

		public bool ShowLatestMalware
		{
			get {return showLatestMalware;}
			set {showLatestMalware = value;}
		}

		public bool ShowVba32Integrity
		{
			get {return showVba32Integrity;}
			set {showVba32Integrity = value;}
		}

		public bool ShowVba32KeyValid
		{
			get {return showVba32KeyValid;}
			set {showVba32KeyValid = value;}
		}

        public bool ShowPolicyName
        {
            get { return showPolicyName; }
            set { showPolicyName = value; }
        }

        public bool ShowDescription
        {
            get { return showDescription; }
            set { showDescription = value; }
        }

        public int IntervalAutoUpdateComputers
        {
            get{return intervalAutoUpdateComputers;}
            set{intervalAutoUpdateComputers = value;}
        }

         public int IntervalAutoUpdateEvents
        {
            get{return intervalAutoUpdateEvents;}
            set{intervalAutoUpdateEvents = value;}
        }

         public int IntervalAutoUpdateTasks
        {
            get{return intervalAutoUpdateTasks;}
            set{intervalAutoUpdateTasks = value;}
        }

         public int IntervalAutoUpdateProcesses
        {
            get{return intervalAutoUpdateProcesses;}
            set{intervalAutoUpdateProcesses = value;}
        }

         public int IntervalAutoUpdateComponents
        {
            get{return intervalAutoUpdateComponents;}
            set{intervalAutoUpdateComponents = value;}
        }

         public int IntervalAutoUpdateTasksInstall
         {
             get { return intervalAutoUpdateTasksInstall; }
             set { intervalAutoUpdateTasksInstall = value; }
         }

        public bool EnableAutoUpdateComputers
        {
            get { return enableAutoUpdateComputers; }
            set { enableAutoUpdateComputers = value; }
        }

        public bool EnableAutoUpdateEvents
        {
            get { return enableAutoUpdateEvents; }
            set { enableAutoUpdateEvents = value; }
        }

        public bool EnableAutoUpdateTasks
        {
            get { return enableAutoUpdateTasks; }
            set { enableAutoUpdateTasks = value; }
        }

        public bool EnableAutoUpdateProcesses
        {
            get { return enableAutoUpdateProcesses; }
            set { enableAutoUpdateProcesses = value; }
        }

        public bool EnableAutoUpdateComponents
        {
            get { return enableAutoUpdateComponents; }
            set { enableAutoUpdateComponents = value; }
        }
        public bool EnableAutoUpdateTasksInstall
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
		public string Serialize() 
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
		public SettingsEntity Deserialize(string settings)
		{
			XmlSerializer xmlser = new XmlSerializer(this.GetType());
			StringReader reader = new StringReader(settings);			
			return (SettingsEntity)xmlser.Deserialize(reader);
		}

		#endregion

		
	}

}
