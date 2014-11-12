using System;

namespace VirusBlokAda.CC.DataBase
{
	/// <summary>
	/// IMPORTANT: This class should never be manually edited.
	/// This entity class represents the properties and methods of a EventTypes.
	/// </summary>
	public class EventTypesEntity : ICloneable
	{
		protected Int16 iD = Int16.MinValue;
		protected String eventName = String.Empty;
		protected String color = String.Empty;
        protected Boolean send = false;
        protected Boolean noDelete = false;
        protected Boolean notify = false;
		
		//Default constructor
		public EventTypesEntity() {}
		
		//Constructor
		public EventTypesEntity(
			Int16 iD,
			String eventName,
			String color,
            Boolean send,
            Boolean noDelete,
            Boolean notify) 
		{
			this.iD = iD;
			this.eventName = eventName;
			this.color = color;
            this.send = send;
            this.noDelete = noDelete;
            this.notify = notify;
		}
		
		#region Public Properties
		
		public Int16 ID
		{
			get {return iD;}
			set {iD = value;}
		}

		public String EventName
		{
			get {return eventName;}
			set {eventName = value;}
		}

		public String Color
		{
			get {return color;}
			set {color = value;}
		}

        public Boolean Send
        {
            get { return send; }
            set { send = value; }
        }

        public Boolean NoDelete
        {
            get { return noDelete; }
            set { noDelete = value; }
        }

        public Boolean Notify
        {
            get { return notify; }
            set { notify = value; }
        }

		#endregion
		
		/// <summary>
		/// Create and return clone object
		/// </summary>
		/// <returns>Clone object</returns>
		public virtual object Clone()
		{
			return new EventTypesEntity(
				this.iD,
				this.eventName,
				this.color,
                this.send,
                this.noDelete,
                this.notify);
				
		}
		
	}
}

