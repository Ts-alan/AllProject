﻿using System;
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

        private QtipJSONEntity _qtip = null;
        [JsonProperty("qtipCfg", NullValueHandling = NullValueHandling.Ignore)]
        public QtipJSONEntity Qtip
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

        private List<TreeNodeJSONEntity> _children = null;
        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public List<TreeNodeJSONEntity> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        private CompAdditionalInfo _compAdditionalInfo = null;
        [JsonProperty("compAdditionalInfo", NullValueHandling = NullValueHandling.Ignore)]
        public CompAdditionalInfo ComputerAdditionalInfo
        {
            get { return _compAdditionalInfo; }
            set { _compAdditionalInfo = value; }
        }
        #endregion

        #region Constructors
        public TreeNodeJSONEntity()
        {
            _qtip = new QtipJSONEntity();
            _children = new List<TreeNodeJSONEntity>();
        }
        public TreeNodeJSONEntity(String text, String id, String iconClassStyle, QtipJSONEntity qtip,
            Boolean? isChecked, Boolean allowDrag, Boolean allowDrop, Boolean isLeaf, Boolean isExpanded, List<TreeNodeJSONEntity> children)
            : this(text, id, iconClassStyle, qtip, isChecked, allowDrag, allowDrop, isLeaf, isExpanded)
        {
            this._children = children;
        }

        public TreeNodeJSONEntity(String text, String id, String iconClassStyle, QtipJSONEntity qtip,
            Boolean? isChecked, Boolean allowDrag, Boolean allowDrop, Boolean isLeaf, Boolean isExpanded,
            List<TreeNodeJSONEntity> children, CompAdditionalInfo info)
            : this(text, id, iconClassStyle, qtip, isChecked, allowDrag, allowDrop, isLeaf, isExpanded, children)
        {
            this._compAdditionalInfo = info;
        }

        public TreeNodeJSONEntity(String text, String id, String iconClassStyle, QtipJSONEntity qtip,
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

            _children = new List<TreeNodeJSONEntity>();
        }
        #endregion
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class QtipJSONEntity
    {
        #region Properties
        private String _xtype = "quicktip";
        [JsonProperty("xtype")]
        public String Xtype
        {
            get { return _xtype; }
        }

        private String _text = String.Empty;
        [JsonProperty("text")]
        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }
        #endregion

        #region Constructors
        public QtipJSONEntity() { }
        public QtipJSONEntity(String text)
        {
            this._text = text;
        }
        #endregion
    }
}
