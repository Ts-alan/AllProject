using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tasks.Common;
using Tasks.Entities;

/// <summary>
/// Control test task options: use as guide to create options for other tasks
/// must implement ITaskOptions and ITaskOptionsHelper<XTaskEntity> where XTaskEntity corresponds to the control
/// </summary>
public partial class Controls_TestTaskOptions : System.Web.UI.UserControl, ITaskOptions, ITaskOptionsHelper<TestTaskEntity>
{
    #region Page LifeCycle
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion

    #region Control LifeCycle
    protected override void OnInit(EventArgs e)
    {
        //control must be hidden, to decrease the amount of html send to client browser
        Visible = false;
        base.OnInit(e);
    }

    #endregion

    #region ITaskOptions
    //load field values from entity (outer method)
    public void LoadTaskEntity(TaskEntity entity)
    {
        //convert TaskEntity to TestTaskEntity
        //call LoadTaskEntity method from ITaskOptionsHelper<TestTaskEntity>
        LoadTaskEntity(ConvertTaskEntity(entity));
    }

    //save field values to entity (outer method)
    public TaskEntity SaveTaskEntity(TaskEntity oldEntity, out bool changed)
    {
        //convert TaskEntity to TestTaskEntity
        //call SaveTaskEntity method from ITaskOptionsHelper<TestTaskEntity>
        return SaveTaskEntity(ConvertTaskEntity(oldEntity), out changed);
    }

    //ClientID of div that contains all elements of this user control, and will be showed
    //in TaskOptionsDialog
    public string DivOptionsClientID
    {
        get
        {

            return tskTest.ClientID;
        }
    }

    public Type TaskType
    {
        //return type of TestTaskEntity
        get { return typeof(TestTaskEntity); }
    }
    #endregion

    #region ITaskOptionsHelper
    //load field values from entity (inner method)
    public void LoadTaskEntity(TestTaskEntity entity)
    {
        TextBox1.Text = entity.Parameter1;
        TextBox2.Text = entity.Parameter2;
    }

    //save field values to entity (inner method)
    public TestTaskEntity SaveTaskEntity(TestTaskEntity oldEntity, out bool changed)
    {

        TestTaskEntity entity = new TestTaskEntity();
        entity.Parameter1 = TextBox1.Text;
        entity.Parameter2 = TextBox2.Text;
        changed = !oldEntity.Equals(entity);
        return entity;
    }

    //convert TaskEntity to TestTaskEntity type corresponding this control
    public TestTaskEntity ConvertTaskEntity(TaskEntity entity)
    {
        TestTaskEntity processEntity = entity as TestTaskEntity;
        if ((entity as TestTaskEntity) == null)
        {
            processEntity = new TestTaskEntity();
        }
        return processEntity;
    }
    #endregion
}