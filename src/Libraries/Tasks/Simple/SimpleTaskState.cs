﻿using System;
using System.Collections.Generic;
using System.Text;
using VirusBlokAda.CC.Tasks.Common;
using VirusBlokAda.CC.Tasks.Entities;

namespace VirusBlokAda.CC.Tasks.Simple
{
    public class SimpleTaskState : TaskState
    {
        #region ITask
        public override bool IsActive()
        {
            return IsSelected;
        }

        public override string GetXmlString()
        {
            return entity.ToXmlString();
        }

        public override string GetTaskXml()
        {
            return entity.ToTaskXml();
        }

        public override string GetTaskName()
        {
            return entity.Type;
        }

        #endregion

        #region Properties
        private SimpleTaskEntityEnum _taskType;
        private TaskEntity _entity = null;
        protected TaskEntity entity
        {
            get
            {
                if (_entity == null)
                {
                    _entity = SimpleTaskEntityBuilder.Create(_taskType);
                }
                return _entity;
            }
        }
        #endregion

        #region Constructor
        public SimpleTaskState(SimpleTaskEntityEnum taskType)
        {
            if (taskType == SimpleTaskEntityEnum.None)
            {
                throw new ArgumentException(
                        "SimpleTaskEntityEnum passed to SimpleTaskState constructor can't be of None type", 
                        "taskType");
            }
            _taskType = taskType;
        }
        #endregion

        
    }
}