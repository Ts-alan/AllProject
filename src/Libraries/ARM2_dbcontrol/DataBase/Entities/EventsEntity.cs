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
		protected DateTime eventTime = DateTime.MinValue;	
		protected String _object = String.Empty;
		protected String comment = String.Empty;

        protected String eventName = String.Empty;
        protected String color = String.Empty;
        protected String computerName = String.Empty;
        protected String componentName = String.Empty;
        protected String ipAddress = String.Empty;
        protected String description = String.Empty;

        private String[] comment_parts;

		//Default constructor
		public EventsEntity() {}
		
		//Constructor
		public EventsEntity(
			DateTime eventTime,
			String _object,
			String comment) 
		{
            //this.iD = iD;
            //this.computerID = computerID;
            //this.eventID = eventID;
			this.eventTime = eventTime;
            //this.componentID = componentID;
			this._object = _object;
			this.comment = comment;
            this.comment_parts = new String[0];
		}

        public EventsEntity(StringDictionary name_value_map)
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
            if (!String.IsNullOrEmpty(name_value_map["Count"]))
            {
                Int32 count = Convert.ToInt32(name_value_map["Count"]);
                this.comment_parts = new String[count];
                for (Int32 index = 0; index < count; index++)
                {
                    this.comment_parts[index] = name_value_map[String.Format("Param{0}", index)];
                }
            }
        }
		
		#region Public Properties

        public String[] Comment_parts
        {
            get { return comment_parts; }
        }

        public String ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }

        public String EventName
        {
            get { return eventName; }
            set { eventName = value; }
        }

		public DateTime EventTime
		{
			get {return eventTime;}
			set {eventTime = value;}
		}

        public String ComponentName
        {
            get { return componentName; }
            set { componentName = value; }
        }

		public String Object
		{
			get {return _object;}
			set {_object = value;}
		}

		public String Comment
		{
			get {return comment;}
			set {comment = value;}
		}

        public String Color
        {
            get { return color; }
            set { color = value; }
        }

        public String IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public String Description
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
					this.eventTime,
					this._object,
					this.comment);				
		}
		
	}
}

