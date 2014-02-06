using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VirusBlokAda.Vba32CC.JSON.Entities
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

        private String _iconClassStyle = String.Empty;
        [JsonProperty("cls")]
        public String IconClassStyle
        {
            get { return _iconClassStyle; }
            set { _iconClassStyle = value; }
        }

        private String _qtip = String.Empty;
        [JsonProperty("qtip", NullValueHandling = NullValueHandling.Ignore)]
        public String Qtip
        {
            get { return _qtip; }
            set { _qtip = value; }
        }

        private Boolean? _isChecked = false;
        [JsonProperty("checked", NullValueHandling=NullValueHandling.Ignore)]
        public Boolean? IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }

        private Boolean _allowDrag = false;
        [JsonProperty("allowDrag")]
        public Boolean AllowDrag
        {
            get { return _allowDrag; }
            set { _allowDrag = value; }
        }

        private Boolean _allowDrop = false;
        [JsonProperty("allowDrop")]
        public Boolean AllowDrop
        {
            get { return _allowDrop; }
            set { _allowDrop = value; }
        }

        private Boolean _isLeaf = false;
        [JsonProperty("leaf")]
        public Boolean IsLeaf
        {
            get { return _isLeaf; }
            set { _isLeaf = value; }
        }

        private Boolean _isExpanded = false;
        [JsonProperty("expanded")]
        public Boolean IsExpanded
        {
            get { return _isExpanded; }
            set { _isExpanded = value; }
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

/*        private CompAdditionalInfo _compAdditionalInfo = null;
        [JsonProperty("compAdditionalInfo", NullValueHandling = NullValueHandling.Ignore)]
        public CompAdditionalInfo ComputerAdditionalInfo
        {
            get { return _compAdditionalInfo; }
            set { _compAdditionalInfo = value; }
        }
*/
        #endregion

        #region Constructors
        public TreeNodeJSONEntity()
        {
            _children = new List<TreeNodeJSONEntity>();
        }
        public TreeNodeJSONEntity(String text, String id, String iconClassStyle, String qtip,
            Boolean? isChecked, Boolean allowDrag, Boolean allowDrop, Boolean isLeaf, Boolean isExpanded, List<TreeNodeJSONEntity> children)
            : this(text, id, iconClassStyle, qtip, isChecked, allowDrag, allowDrop, isLeaf, isExpanded)
        {
            this._children = children;
        }

    /*    public TreeNodeJSONEntity(String text, String id, String iconClassStyle, String qtip,
            Boolean? isChecked, Boolean allowDrag, Boolean allowDrop, Boolean isLeaf, Boolean isExpanded,
            List<TreeNodeJSONEntity> children, CompAdditionalInfo info)
            : this(text, id, iconClassStyle, qtip, isChecked, allowDrag, allowDrop, isLeaf, isExpanded, children)
        {
            this._compAdditionalInfo = info;
        }
*/
            public TreeNodeJSONEntity(String text, String id, String iconClassStyle, String qtip,
            Boolean? isChecked, Boolean allowDrag, Boolean allowDrop, Boolean isLeaf, Boolean isExpanded,String ipAddress, String osName,
            List<TreeNodeJSONEntity> children)
            : this(text, id, iconClassStyle, qtip, isChecked, allowDrag, allowDrop, isLeaf, isExpanded, children)
        {
            this._ipAddress = ipAddress;
            this._OSName = osName;
        }

        public TreeNodeJSONEntity(String text, String id, String iconClassStyle, String qtip,
            Boolean? isChecked, Boolean allowDrag, Boolean allowDrop, Boolean isLeaf, Boolean isExpanded)
        {
            this._text = text;
            this._id = id;
            this._iconClassStyle = iconClassStyle;
            this._qtip = qtip;
            this._isChecked = isChecked;
            this._allowDrag = allowDrag;
            this._allowDrop = allowDrop;
            this._isLeaf = isLeaf;
            this._isExpanded = isExpanded;
            this._ipAddress = null;
            this._OSName = null;

            _children = new List<TreeNodeJSONEntity>();
        }
        #endregion
    }

}
