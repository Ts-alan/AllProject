using System;

namespace VirusBlokAda.CC.DataBase
{
	/// <summary>
	/// IMPORTANT: This class should never be manually edited.
	/// This entity class represents the properties and methods of a EventTypes.
	/// </summary>
	public class EventTypesEntity : ICloneable
	{
				
		protected short iD = Int16.MinValue;
		protected string eventName = String.Empty;
		protected string color = String.Empty;
        protected bool send = false;
        protected bool noDelete = false;
        protected bool notify = false;
		
		//Default constructor
		public EventTypesEntity() {}
		
		//Constructor
		public EventTypesEntity(
			short iD,
			string eventName,
			string color,
            bool send,
            bool noDelete,
            bool notify) 
		{
			this.iD = iD;
			this.eventName = eventName;
			this.color = color;
            this.send = send;
            this.noDelete = noDelete;
            this.notify = notify;
		}
		
		#region Public Properties
		
		public short ID
		{
			get {return iD;}
			set {iD = value;}
		}

		public string EventName
		{
			get {return eventName;}
			set {eventName = value;}
		}

		public string Color
		{
			get {return color;}
			set {color = value;}
		}

        public bool Send
        {
            get { return send; }
            set { send = value; }
        }

        public bool NoDelete
        {
            get { return noDelete; }
            set { noDelete = value; }
        }

        public bool Notify
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

