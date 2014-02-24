using System;
using System.Collections.Generic;
using System.Text;
using Tasks.Attributes;

namespace Tasks.Entities
{
    [TaskEntity("test")]
    public class TestTaskEntity:TaskEntity
    {
        public TestTaskEntity()
            : base("Test")
        {
        }
        private string _parameter1;
        [TaskEntityStringProperty("Parameter1")]
        public string Parameter1
        {
            get { return _parameter1; }
            set { _parameter1 = value; }
        }

        private string _parameter2;
        [TaskEntityStringProperty("Parameter2")]
        public string Parameter2
        {
            get { return _parameter2; }
            set { _parameter2 = value; }
        }

        [Obsolete("Необходимо переопределить")]
        public override string ToTaskXml()
        {
            throw new NotImplementedException();
        }
    }
}
