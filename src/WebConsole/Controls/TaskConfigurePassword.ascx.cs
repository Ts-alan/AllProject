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

using System.Text;
using System.Security.Cryptography;

using ARM2_dbcontrol.Generation;
using ARM2_dbcontrol.Tasks;
using ARM2_dbcontrol.Filters;

/// <summary>
/// Данная задача предназначена для установки пароля на комплекс
/// </summary>
public partial class Controls_TaskConfigurePassword : System.Web.UI.UserControl,ITask
{
    private string tagPassword = "SecurityOptions";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            InitFields();
    }

    private bool _hideHeader = false;

    public bool HideHeader
    {
        get { return _hideHeader; }
        set { _hideHeader = value; }
    }

    public void InitFields()
    {
        if (HideHeader) HeaderName.Visible = false;

    }

    public bool ValidateFields()
    {
        //Неплохо бы на что-нибудь проверить..
        //Validation vld = new Validation(tboxPassword.Text);
        
        return true;
    }


    public TaskUserEntity GetCurrentState()
    {
        TaskUserEntity task = new TaskUserEntity();

        task.Type = TaskType.ConfigurePassword;

        ARM2_dbcontrol.Generation.XmlBuilder xml =
     new ARM2_dbcontrol.Generation.XmlBuilder("password");

        string str = tboxPassword.Text;
        if (str != "")
        {
            //Вычисляем хэш и конвертим в base64
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(str));
            str = "reg_binary:" + Anchor.ConvertToDumpString(data); //Convert.ToBase64String(data);
            //
        }
        else
            str = "-";
        xml.AddNode(tagPassword, str);
        xml.AddNode("Vba32CCUser", Anchor.GetStringForTaskGivedUser());
        xml.AddNode("Type", "ConfigurePassword");
        xml.Generate();

        task.Param = xml.Result;

        return task;
    }

    public void LoadState(TaskUserEntity task)
    {
        if (task.Type != TaskType.ConfigurePassword)
            throw new ArgumentException(Resources.Resource.ErrorInvalidTaskType);

        XmlTaskParser pars = new XmlTaskParser(task.Param);


        tboxPassword.Text = pars.GetValue(tagPassword).Replace("reg_binary:", "");
    }

    #region Property

    public string Password
    {
        get { return this.tagPassword; }
    }

    #endregion

}
