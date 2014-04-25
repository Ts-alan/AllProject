using System;
using Newtonsoft.Json;

namespace VirusBlokAda.CC.JSON
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ComponentJSONEntity
    {
        #region Properties
        private String _name = String.Empty;
        [JsonProperty("name")]
        public String Name
        {
            get { return _name; }
        }

        private String _state = String.Empty;
        [JsonProperty("state")]
        public String State
        {
            get { return _state; }
            set { _state = value; }
        }

        private String _version = String.Empty;
        [JsonProperty("version")]
        public String Version
        {
            get { return _version; }
            set { _version = value; }
        }
        #endregion

        #region Constructors
        public ComponentJSONEntity() { }
        public ComponentJSONEntity(String name, String state, String version)
        {
            this._name = name;
            this._state = state;
            this._version = version;
        }
        #endregion
    }
}
