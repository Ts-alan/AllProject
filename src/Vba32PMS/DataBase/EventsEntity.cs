using System;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService.DataBase
{
	/// <summary>
	/// Модифицированный класс, представляющий событие, отсылаемое 
    /// на родительский ЦУ
	/// </summary>
	public class EventsEntity : ICloneable
	{
        protected String computer = String.Empty;
        protected String _event = String.Empty;
		protected DateTime eventTime = DateTime.MinValue;
        protected String component = String.Empty;
		protected String _object = String.Empty;
		protected String comment = String.Empty;

		//Default constructor
		public EventsEntity() {}

        public EventsEntity(String computer, String _event, DateTime eventTime,
            String component, String _object,String comment)
        {
            this.computer = computer;
            this._event = _event;
            this.eventTime = eventTime;
            this.component = component;
            this._object = _object;
            this.comment = comment;
        }


		#region Public Properties
		
        public String Computer
        {
            get { return computer; }
            set { computer = value; }
        }

        public String Event
        {
            get { return _event; }
            set { _event = value; }
        }

		public DateTime EventTime
		{
			get {return eventTime;}
			set {eventTime = value;}
		}

        public String Component
        {
            get { return component; }
            set { component = value; }
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

		#endregion
		
		/// <summary>
		/// Create and return clone object
		/// </summary>
		/// <returns>Clone object</returns>
        public virtual Object Clone()
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

