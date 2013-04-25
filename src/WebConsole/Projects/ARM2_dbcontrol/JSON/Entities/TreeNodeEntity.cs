using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace VirusBlokAda.Vba32CC.JSON
{
    /// <summary>
    /// General group entity
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TreeNodeEntity
    {
        #region Property

        private String _nodeId;
        [JsonProperty("id")]
        public String NodeID
        {
            get { return _nodeId; }
            set { _nodeId = value; }
        }

        private String _nodeName;
        [JsonProperty("text")]
        public String NodeName
        {
            get { return _nodeName; }
            set { _nodeName = value; }
        }

        private String _parentId = null;
        [JsonProperty("parentId", NullValueHandling = NullValueHandling.Ignore)]
        public String ParentID
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        private String _comment;
        [JsonProperty("comment", NullValueHandling = NullValueHandling.Ignore)]
        public String Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        private Boolean _isLeaf;
        [JsonProperty("isLeaf")]
        public Boolean IsLeaf
        {
            get { return _isLeaf; }
            set { _isLeaf = value; }
        }

        #endregion

        #region Constructors
        public TreeNodeEntity() { }
        public TreeNodeEntity(String nodeId, String nodeName, String parentId, String comment, Boolean isLeaf)
        {
            _nodeId = nodeId;
            _nodeName = nodeName;
            _parentId = parentId;
            _comment = comment;
            _isLeaf = isLeaf;
        }
        public TreeNodeEntity(String nodeId, String nodeName, Boolean isLeaf)
            : this(nodeId, nodeName, null, null, isLeaf)
        {
        }
        #endregion
    }
}
