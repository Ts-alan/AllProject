using System;
using System.Collections.Specialized;

namespace VirusBlokAda.CC.DataBase
{
	/// <summary>
	/// IMPORTANT: This class should never be manually edited.
	/// This entity class represents the properties and methods of a Computers.
	/// </summary>
	public class EventsEntity : ICloneable
	{
				
        //protected int iD = Int32.MinValue;
		//protected short computerID = Int16.MinValue;
		//protected short eventID = Int16.MinValue;
        //protected short componentID = Int16.MinValue;

		protected DateTime eventTime = DateTime.MinValue;	
		protected string _object = String.Empty;
		protected string comment = String.Empty;

        protected string eventName = String.Empty;
        protected string color = String.Empty;
        protected string computerName = String.Empty;
        protected string componentName = String.Empty;
        protected string ipAddress = String.Empty;
        protected string description = String.Empty;


		//Default constructor
		public EventsEntity() {}
		
		//Constructor
		public EventsEntity(
            //int iD,
            //short computerID,
            //short eventID,
			DateTime eventTime,
            //short componentID,
			string _object,
			string comment) 
		{
            //this.iD = iD;
            //this.computerID = computerID;
            //this.eventID = eventID;
			this.eventTime = eventTime;
            //this.componentID = componentID;
			this._object = _object;
			this.comment = comment;
		}

        public EventsEntity(StringDictionary name_value_map)
        {

            try
            {
                this.computerName = name_value_map["Computer"];
                this.eventName = name_value_map["EventName"];
                IFormatProvider format = new System.Globalization.CultureInfo("ru-RU");
                DateTime date_time = DateTime.Parse(name_value_map["EventTime"], format);
                this.eventTime = date_time;
                if (name_value_map["Component"] == null)
                {
                    name_value_map["Component"] = "(unknown)";
                }
                this.componentName = name_value_map["Component"];
                this._object = name_value_map["Object"];
                this.comment = name_value_map["Comment"];
            }
            catch
            {

            }
        }
		
		#region Public Properties
		
        //public int ID
        //{
        //    get {return iD;}
        //    set {iD = value;}
        //}

        //public short ComputerID
        //{
        //    get {return computerID;}
        //    set {computerID = value;}
        //}

        public string ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }

        //public short EventID
        //{
        //    get {return eventID;}
        //    set {eventID = value;}
        //}

        public string EventName
        {
            get { return eventName; }
            set { eventName = value; }
        }

		public DateTime EventTime
		{
			get {return eventTime;}
			set {eventTime = value;}
		}

        //public short ComponentID
        //{
        //    get {return componentID;}
        //    set {componentID = value;}
        //}

        public string ComponentName
        {
            get { return componentName; }
            set { componentName = value; }
        }

		public string Object
		{
			get {return _object;}
			set {_object = value;}
		}

		public string Comment
		{
			get {return comment;}
			set {comment = value;}
		}

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

		#endregion
		
		/// <summary>
		/// Create and return clone object
		/// </summary>
		/// <returns>Clone object</returns>
		public virtual object Clone()
		{
            return new EventsEntity(
                    //this.iD,
                    //this.computerID,
                    //this.eventID,
					this.eventTime,
                    //this.componentID,
					this._object,
					this.comment);				
		}
		
	}
}

