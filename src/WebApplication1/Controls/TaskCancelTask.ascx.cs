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

using ARM2_dbcontrol.Tasks;

/// <summary>
/// Задача по отмене задачи
/// Не сохраняется, без параметров
/// </summary>
public partial class Controls_TaskCancelTask : System.Web.UI.UserControl, ITask
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void InitFields()
    {
    }

    public bool ValidateFields()
    {
        return true;
    }


    public TaskUserEntity GetCurrentState()
    {
        return null;
    }

    public void LoadState(TaskUserEntity task)
    {
    }

}
