using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class UpdateEntity
    {
        #region Fields

        private DateTime _DeployDatetime;
        private String _BuildID;
        private String _Description;
        private UpdateStateEnum _State;

        #endregion

        #region Properties

        public DateTime DeployDatetime
        {
            get { return _DeployDatetime; }
            set { _DeployDatetime = value; }
        }

        public String BuildID
        {
            get { return _BuildID; }
            set { _BuildID = value; }
        }

        public String Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public UpdateStateEnum State
        {
            get { return _State; }
            set { _State = value; }
        }


        #endregion

        #region Constructors

        public UpdateEntity()
        {
            _DeployDatetime = DateTime.MinValue;
        }

        #endregion
    }
}
