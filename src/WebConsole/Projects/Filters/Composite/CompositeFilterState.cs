using System;
using System.Collections.Generic;
using System.Text;
using Common.Collection;
using Filters.Primitive;

namespace Filters.Composite
{
    public class CompositeFilterState
    {
        public CompositeFilterState()
        {
        }

        public CompositeFilterState(string _name, SerializableDictionary<string, PrimitiveFilterState> _state)
        {
            state = _state;
            name = _name;
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private SerializableDictionary<string, PrimitiveFilterState> state;
        public SerializableDictionary<string, PrimitiveFilterState> State
        {
            get { return state; }
            set { state = value; }
        }
    }
}
