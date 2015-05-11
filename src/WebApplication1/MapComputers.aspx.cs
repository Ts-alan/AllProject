using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using VirusBlokAda.CC.DataBase;
using System.Collections.Generic;

public partial class MapComputers : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = Resources.Resource.ComputersMap;
        RegisterHeader();
        if (!Page.IsPostBack)
        {
            InitFields();
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        base.Page_Init(sender, e);
    }

    private void RegisterHeader()
    {
        RegisterScript(@"js/MapComputers.js");
    }

    protected override void InitFields()
    {
        List<ComputersEntity> list = new List<ComputersEntity>();
        ItemsMap.InnerHtml = String.Empty;

        list = DBProviders.Group.GetComputersWithoutGroup();
        ItemsMap.InnerHtml += String.Format("<div style=\"float:left;width: 99%;\">{0}</div>", GenerateHtmlForListComputers(list));
        
        bool isIncluded = (list.Count != 0);

        foreach (Group group in DBProviders.Group.GetGroups())
        {
            list = DBProviders.Group.GetComputersByGroup(group.ID);
            ItemsMap.InnerHtml += String.Format("<div class=\"title\" style=\"float:left;width: 99%;margin-top: {0}px;\">{1}</div><div style=\"width: 99%;\">{2}</div>", isIncluded ? 30 : 5, group.Name, GenerateHtmlForListComputers(list));
            isIncluded = (list.Count != 0);
        }
        
    }

    private string GenerateHtmlForListComputers(List<ComputersEntity> list)
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        if (list != null)
        {
            foreach (ComputersEntity entity in list)
            {
                string className = GetClassName(entity);
                builder.AppendFormat("<div class=\'item {0}\' name=\'{1}\'><div class=\'itemName\'>{2}</div></div>", className, entity.ComputerName, entity.ComputerName.Length > 10 ? entity.ComputerName.Insert(10, "\r\n") : entity.ComputerName);
            }
        }

        return builder.ToString();
    }

    private string GetClassName(ComputersEntity entity)
    {
        TimeSpan time = DateTime.Now.Subtract(entity.RecentActive);
        if (time.Days != 0 || time.Hours != 0 || time.Minutes >= 10) return "vbagrey";
        List<ComponentsEntity> list;

        list = DBProviders.Component.List(String.Format("ComputerName = \'{0}\' AND  ComponentName = \'{{A3F5FCA0-46DC-4328-8568-5FDF961E87E6}}\'", entity.ComputerName), null, 1, Int16.MaxValue);
        
        if (list != null && list.Count == 1)        
        {
            bool isMonitorOn = false;
            bool isLoaderOn = false;


           
            isLoaderOn = (list[0].ComponentState == "On" || list[0].ComponentState == "Off");
            
            isMonitorOn = (list[0].ComponentState == "On");
            
           

            if (entity.Vba32KeyValid && entity.Vba32Integrity && isLoaderOn && isMonitorOn) return "vbagreen";
            else 
            {
                if (entity.Vba32KeyValid && entity.Vba32Integrity && isLoaderOn) return "vbayellow";
                else
                {
                    if (!entity.Vba32KeyValid || !entity.Vba32Integrity || !isLoaderOn) return "vbared";
                }
            }
            
        }

        return "vbagrey";
    }
}