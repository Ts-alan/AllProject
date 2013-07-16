using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.ComponentModel;

public partial class Controls_MultipleSelectionDropDownList : System.Web.UI.UserControl
{
    #region Properties
    #region Consts
    private const int maxHeightList = 84;
    #endregion

    #region Protected
    protected List<string> selectedValueList
    {
        get
        {
            List<String> result = new List<String>();
            foreach (ListItem li in lboxOptions.Items)
            {
                if (li.Selected)
                {
                    result.Add(li.Value);
                }
            }
            return result;
        }
    }

    protected List<string> selectedTextList
    {
        get
        {
            List<String> result = new List<String>();
            foreach (ListItem li in lboxOptions.Items)
            {
                if (li.Selected)
                {
                    result.Add(li.Text);
                }
            }
            return result;
        }
    }
    #endregion

    #region Public
    public virtual object DataSource
    {
        get
        {
            return lboxOptions.DataSource;
        }
        set
        {
            lboxOptions.DataSource = value;
        }
    }

    public string SelectedValue
    {
        get
        {
            return String.Join("&", selectedValueList.ToArray());
        }
        set
        {
            lboxOptions.ClearSelection();
            if (value == null) return;

            string[] splitted = value.Split(new Char[] { '&' });
            // Select the items from the list
            foreach (string next in splitted)
            {
                ListItem li = lboxOptions.Items.FindByValue(next);
                if (li != null)
                {
                    li.Selected = true;
                }
            }
        }
    }

    public String SelectedText
    {
        get
        {
            return String.Join(", ", selectedTextList.ToArray());
        }
    }
    #endregion
    #endregion

    #region LifeCycle

    public override void DataBind()
    {
        lboxOptions.DataBind();
    }

    private void RegisterScripts()
    {
        //register jQuery
	Page.ClientScript.RegisterClientScriptInclude("jQuery", @"js/jQuery/jquery-1.10.2.min.js");
        Page.ClientScript.RegisterClientScriptInclude("MultipleSelectionDropDownList", @"js/MultipleSelectionDropDownList.js");
        String key = "RegisterClickEvents_" + ClientID;
        String script = String.Format("MultipleSelectionDropDownList.RegisterClickEvents('{0}', '{1}', '{2}', '{3}');",
            divOptions.ClientID, lboxOptions.ClientID, lblSelectedText.ClientID, imgDropDown.ClientID);
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), key, script, true);
    }

    private void Page_Load(object sender, System.EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitControls();                    
        }
        RegisterScripts();
    }

    private void UpdateSelectedText(string text)
    {
        lblSelectedText.Text = text.Length > 25 ? text.Substring(0, 25) + ".." : text;
        lblSelectedText.ToolTip = text;
    }

    private void InitControls()
    {
        lblSelectedText.Text = String.Empty;
    }

    private void SetListHeight(int itemCount)
    {
        Int32 heightList = itemCount == 0 ? maxHeightList : (itemCount * 16 + 4);
        lboxOptions.Height = heightList > maxHeightList ? maxHeightList : heightList;
    }

    private void Page_PreRender(object sender, System.EventArgs e)
    {
        SetListHeight(lboxOptions.Items.Count);
        UpdateSelectedText(SelectedText);
    }

    #endregion
}