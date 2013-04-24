using System;
using System.Globalization;
using System.Collections.Specialized;


namespace Vba32CC
{
	/// <summary>
    /// Событие ЦУ
	/// </summary>
	public class EventsEntity : ICloneable
	{

        protected string computer = String.Empty;
        protected string _event = String.Empty;
		protected DateTime eventTime = DateTime.Now;
        protected string component = String.Empty;
		protected string _object = String.Empty;
		protected string comment = String.Empty;

		//Default constructor
		public EventsEntity() {}

        public EventsEntity(StringDictionary name_value_map)
        {

            try
            {
                this.Computer = name_value_map["Computer"];
                this.Event = name_value_map["EventName"];
                IFormatProvider format = new CultureInfo("ru-RU");
                DateTime date_time = DateTime.Parse(name_value_map["EventTime"], format);
                this.EventTime = date_time;
                if (name_value_map["Component"] == null)
                {
                    name_value_map["Component"] = "(unknown)";
                }
                this.Component = name_value_map["Component"];
                this.Object = name_value_map["Object"];
                this.Comment = name_value_map["Comment"];

            }
            catch
            {

            }

        }

        public EventsEntity(string computer, string _event, DateTime eventTime,
            string component, string _object,string comment)
        {
            this.computer = computer;
            this._event = _event;
            this.eventTime = eventTime;
            this.component = component;
            this._object = _object;
            this.comment = comment;
        }


		#region Public Properties
		
        public string Computer
        {
            get { return computer; }
            set { computer = value; }
        }

        public string Event
        {
            get { return _event; }
            set { _event = value; }
        }

		public DateTime EventTime
		{
			get {return eventTime;}
			set {eventTime = value;}
		}

        public string Component
        {
            get { return component; }
            set { component = value; }
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

		#endregion
		
		/// <summary>
		/// Create and return clone object
		/// </summary>
		/// <returns>Clone object</returns>
        public virtual object Clone()
        {
            return new EventsEntity(
                            this.computer,
                            this._event,
                            this.eventTime,
                            this.component,
                            this._object,
                            this.comment);
        }
		
	}
}

