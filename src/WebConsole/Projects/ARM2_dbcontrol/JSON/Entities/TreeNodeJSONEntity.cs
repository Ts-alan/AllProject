using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VirusBlokAda.CC.JSON
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TreeNodeJSONEntity
    {
        #region Properties
        private String _id = String.Empty;
        [JsonProperty("id")]
        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _text = String.Empty;
        [JsonProperty("text")]
        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private String _nodeType = String.Empty;
        [JsonProperty("type")]
        public String NodeType
        {
            get { return _nodeType; }
            set { _nodeType = value; }
        }

        private String _qtip = String.Empty;
        [JsonProperty("qtip", NullValueHandling = NullValueHandling.Ignore)]
        public String Qtip
        {
            get { return _qtip; }
            set { _qtip = value; }
        }
        private NodeState _state;
        [JsonProperty("state")]
        public NodeState State
        {
            get { return _state; }
            set { _state = value; }
        }


        private String _ipAddress = String.Empty;
        [JsonProperty("ip", NullValueHandling = NullValueHandling.Ignore)]
        public String IPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        private String _OSName = String.Empty;
        [JsonProperty("os", NullValueHandling = NullValueHandling.Ignore)]
        public String OSName
        {
            get { return _OSName; }
            set { _OSName = value; }
        }

        private List<TreeNodeJSONEntity> _children = null;
        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public List<TreeNodeJSONEntity> Children
        {
            get { return _children; }
            set { _children = value; }
        }


        #endregion

        #region Constructors
        public TreeNodeJSONEntity()
        {
            _children = new List<TreeNodeJSONEntity>();
        }
        public TreeNodeJSONEntity(String text, String id, String nodeType, String qtip,NodeState state,List<TreeNodeJSONEntity> children)
            : this(text, id, nodeType, qtip,state)
        {
            this._children = children;
        }

  
            public TreeNodeJSONEntity(String text, String id, String nodeType, String qtip,NodeState state
           ,String ipAddress, String osName,List<TreeNodeJSONEntity> children)
            : this(text, id, nodeType, qtip,state, children)
        {
            this._ipAddress = ipAddress;
            this._OSName = osName;
        }

        public TreeNodeJSONEntity(String text, String id, String nodeType, String qtip,NodeState state)
        {
            this._text = text;
            this._id = id;
            this._nodeType = nodeType;
            this._qtip = qtip;
            this._state = state;
            this._ipAddress = null;
            this._OSName = null;

            _children = new List<TreeNodeJSONEntity>();
        }
        #endregion
    }
    public struct NodeState
    {
        public Boolean opened { get; set; }
        public Boolean disabled { get; set; }
        public Boolean @checked { get; set; }
        

        public NodeState(bool isOpened, bool? isSelected, bool isDisabled):this()
        {
            if (isSelected == null) @checked = false;
            else @checked = (bool)isSelected;
            opened = isOpened;           
            disabled = isDisabled;
        }
    }
}
