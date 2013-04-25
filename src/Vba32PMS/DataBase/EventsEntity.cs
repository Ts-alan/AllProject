using System;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService.DataBase
{
	/// <summary>
	/// Модифицированный класс, представляющий событие, отсылаемое 
    /// на родительский АРМ
	/// </summary>
	public class EventsEntity : ICloneable
	{

        protected string computer = String.Empty;
        protected string _event = String.Empty;
		protected DateTime eventTime = DateTime.MinValue;
        protected string component = String.Empty;
		protected string _object = String.Empty;
		protected string comment = String.Empty;

		//Default constructor
		public EventsEntity() {}

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

