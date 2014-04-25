using System;

namespace VirusBlokAda.CC.DataBase
{
    public class InstallationTaskEntity:ICloneable
    {        	
		protected string computerName = String.Empty;
		protected string ipAddress = String.Empty;
        protected string taskType = String.Empty;
        protected string vba32Version = String.Empty;
        protected string status = String.Empty;
        protected DateTime installationDate = DateTime.MinValue;
        protected Int16? exitCode;
        protected string error;
        protected Int64 id;


		//Default constructor
		public InstallationTaskEntity() {}
		
		//Constructor
        public InstallationTaskEntity(
            Int64 id, string computerName, string ipAddress, string taskType, string vba32Version, string status, Int16? exitCode, string error): this(computerName, ipAddress, taskType, vba32Version, status, exitCode, error)
        {
            this.id = id;
        }

        public InstallationTaskEntity(
           string computerName, string ipAddress, string taskType, string vba32Version, string status, Int16? exitCode, string error)
        {            
            this.computerName = computerName;
            this.ipAddress = ipAddress;
            this.taskType = taskType;
            this.vba32Version = vba32Version;
            this.status = status;
            this.exitCode = exitCode;
            this.error = error;
        }
		
		#region Public Properties

        public Int64 ID
        {
            get { return id; }
            set { id = value; }
        }

        public string ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }

        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public string TaskType
        {
            get { return taskType; }
            set { taskType = value; }
        }

        public DateTime InstallationDate
        {
            get { return installationDate; }
            set { installationDate = value; }
        }

        public string Vba32Version
        {
            get { return vba32Version; }
            set { vba32Version = value; }
        }

		public string Status
		{
			get {return status;}
			set {status = value;}
		}

		public Int16? ExitCode
		{
			get {return exitCode;}
			set {exitCode = value;}
		}

        public string Error
        {
            get { return error; }
            set { error = value; }
        }

		#endregion
		
		/// <summary>
		/// Create and return clone object
		/// </summary>
		/// <returns>Clone object</returns>
		public virtual object Clone()
		{
            return new InstallationTaskEntity(
                    this.id,
                    this.computerName,
                    this.ipAddress,
                    this.taskType,
                    this.vba32Version,
                    this.status,
                    this.exitCode,
                    this.error);				
		}        
    }
}
