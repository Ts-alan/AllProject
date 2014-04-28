using System;
using System.Collections.Generic;
using System.Text;

namespace VirusBlokAda.CC.DataBase
{
    public class ComputersEntityEx : ComputersEntity
    {
        #region Properties

        private Group group;
        public Group Group
        {
            get { return group; }
            set { group = value; }
        }

        private Policy policy;
        public Policy Policy
        {
            get { return policy; }
            set { policy = value; }
        }

        private List<ComponentsEntity> components;
        public List<ComponentsEntity> Components
        {
            get { return components; }
            set { components = value; }
        }
        #endregion

        #region Constructors
        public ComputersEntityEx()
        :this(new ComputersEntity(), null)
        { }

        public ComputersEntityEx(ComputersEntity comp, List<ComponentsEntity> components)
            : this(comp, new Group(), new Policy(), components)
        {
        }

        public ComputersEntityEx(ComputersEntity comp, Group group, Policy policy, List<ComponentsEntity> components)
            : base(comp)
        {
            this.group = group;
            this.policy = policy;
            this.components = (components != null) ? components : new List<ComponentsEntity>();
        }

        public ComputersEntityEx(ComputersEntityEx comp)
            : this(comp as ComputersEntity, comp.Group, comp.Policy, comp.Components)
        {
        }

        #endregion

        public override object Clone()
        {
            return new ComputersEntityEx(this);
        }
    }
}
