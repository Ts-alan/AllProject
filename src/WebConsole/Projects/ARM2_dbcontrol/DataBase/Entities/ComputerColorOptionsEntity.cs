using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace VirusBlokAda.CC.DataBase
{    
    public enum EPriorityComputerColor
    {
        LastUpdate,
        LastActive,
        LastInfected,
        Key,
        Integrity
    }
    
    public class ComputerColorOptionsEntity
    {
        private List<EPriorityComputerColor> _priority;
        //Last Update
        protected ColorOption _lastUpdateOption1;
        protected ColorOption _lastUpdateOption2;
        protected ColorOption _lastUpdateOption3;
        //Last Active
        protected ColorOption _lastActiveOption1;
        protected ColorOption _lastActiveOption2;
        protected ColorOption _lastActiveOption3;
        //Last Infection
        protected ColorOption _lastInfectionOption1;
        protected ColorOption _lastInfectionOption2;
        protected ColorOption _lastInfectionOption3;
        //Key
        protected ColorOption _keyColor;
        //Integrity
        protected ColorOption _integrityColor;
        //Good State
        protected ColorOption _goodStateColor;

        #region Properties

        public List<EPriorityComputerColor> Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public ColorOption GoodStateColor
        {
            get { return _goodStateColor; }
            set { _goodStateColor = value; }
        }

        public ColorOption KeyColor
        {
            get { return _keyColor; }
            set
            {
                _keyColor = value;
            }
        }
        public ColorOption IntegrityColor
        {
            get { return _integrityColor; }
            set
            {
                _integrityColor = value;
            }
        }
        public ColorOption LastUpdateOption1
        {
            get { return _lastUpdateOption1; }
            set
            {
                _lastUpdateOption1 = value;
            }
        }
        public ColorOption LastUpdateOption2
        {
            get { return _lastUpdateOption2; }
            set
            {
                _lastUpdateOption2 = value;
            }
        }
        public ColorOption LastUpdateOption3
        {
            get { return _lastUpdateOption3; }
            set
            {
                _lastUpdateOption3 = value;
            }
        }

        public ColorOption LastActiveOption1
        {
            get { return _lastActiveOption1; }
            set
            {
                _lastActiveOption1 = value;
            }
        }
        public ColorOption LastActiveOption2
        {
            get { return _lastActiveOption2; }
            set
            {
                _lastActiveOption2 = value;
            }
        }
        public ColorOption LastActiveOption3
        {
            get { return _lastActiveOption3; }
            set
            {
                _lastActiveOption3 = value;
            }
        }

        public ColorOption LastInfectionOption1
        {
            get { return _lastInfectionOption1; }
            set
            {
                _lastInfectionOption1 = value;
            }
        }
        public ColorOption LastInfectionOption2
        {
            get { return _lastInfectionOption2; }
            set
            {
                _lastInfectionOption2 = value;
            }
        }
        public ColorOption LastInfectionOption3
        {
            get { return _lastInfectionOption3; }
            set
            {
                _lastInfectionOption3 = value;
            }
        }
        #endregion

        #region Constructors
        public ComputerColorOptionsEntity() 
        {
            _priority = new List<EPriorityComputerColor>();            
        }

        public ComputerColorOptionsEntity(List<EPriorityComputerColor> priority, ColorOption lu1, ColorOption lu2, ColorOption lu3, ColorOption li1, ColorOption li2, ColorOption li3, ColorOption la1, ColorOption la2, ColorOption la3, ColorOption key, ColorOption integrity, ColorOption goodState)
        {
            _priority = priority;                        

            _lastUpdateOption1 = lu1;
            _lastUpdateOption2 = lu2;
            _lastUpdateOption3 = lu3;

            _lastInfectionOption1 = li1;
            _lastInfectionOption2 = li2;
            _lastInfectionOption3 = li3;

            _lastActiveOption1 = la1;
            _lastActiveOption2 = la2;
            _lastActiveOption3 = la3;

            _keyColor = key;
            _integrityColor = integrity;

            _goodStateColor = goodState;
        }
        #endregion

        public static ColorOption GetColorOptions(string color, int colorIndex, int time, bool ishour)
        {
            ColorOption option = new ColorOption();
            option.Color = color;
            option.Time = time;
            option.isHour = ishour;
            option.ColorIndex = colorIndex;

            return option;
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
        public ComputerColorOptionsEntity Deserialize(string options)
        {
            if (String.IsNullOrEmpty(options))
                return new ComputerColorOptionsEntity();
            XmlSerializer xmlser = new XmlSerializer(this.GetType());
            StringReader reader = new StringReader(options);
            return (ComputerColorOptionsEntity)xmlser.Deserialize(reader);
        }

        #endregion

    }

    public struct ColorOption
    {
        public string Color;
        public int Time;
        public bool isHour;
        public int ColorIndex;
    }
}
