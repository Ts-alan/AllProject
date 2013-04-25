using System;
using System.Collections.Generic;
using System.Web;
using ARM2_dbcontrol.DataBase;
using System.Configuration;

/// <summary>
/// Summary description for TasksDataContainer
/// </summary>

public static class TasksDataContainer
{
    public static List<TaskEntityShow> Get(String where, String SortExpression, Int32 maximumRows, Int32 startRowIndex)
    {
        List<TaskEntityShow> list = new List<TaskEntityShow>();
        if (maximumRows < 1) return list;

        String orderBy = "DateIssued DESC";
        String[] parts = SortExpression.Split(' ');
        Boolean descending = false;

        if (parts.Length > 0 && parts[0] != "")
        {
            if (parts.Length > 1)
            {
                descending = parts[1].ToLower().Contains("desc");
            }
            System.Reflection.PropertyInfo prop = typeof(TaskEntity).GetProperty(parts[0]);
            if (prop == null)
            {
                throw new Exception("No property '" + parts[0] + "' in EventsEntity");
            }

            orderBy = String.Format("{0} {1}", parts[0], descending ? "DESC" : "ASC");
        }

        using (VlslVConnection conn = new VlslVConnection(
           ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            TaskManager db = new TaskManager(conn);
            conn.OpenConnection();
            conn.CheckConnectionState(true);

            foreach (TaskEntity ent in db.List(where, orderBy, (Int32)((Double)startRowIndex / (Double)maximumRows) + 1, maximumRows))
            {
                list.Add(new TaskEntityShow(ent.ID, ent.TaskName, ent.ComputerName, DatabaseNameLocalization.GetNameForCurrentCulture(ent.TaskState), ent.DateIssued, ent.DateComplete, ent.DateUpdated, ent.TaskParams, ent.TaskUser));
            }

            conn.CloseConnection();
        }
        
        return list;
    }

    public static Int32 Count(String where)
    {
        Int32 count = 0;

        using (VlslVConnection conn = new VlslVConnection(
           ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            TaskManager db = new TaskManager(conn);
            conn.OpenConnection();
            conn.CheckConnectionState(true);

            count = db.Count(where);

            conn.CloseConnection();
        }

        return count;
    }

    public static List<String> GetTaskStates()
    {
        List<String> list = new List<String>();
        using (VlslVConnection conn = new VlslVConnection(
           ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString))
        {
            TaskManager db = new TaskManager(conn);
            conn.OpenConnection();
            conn.CheckConnectionState(true);

            list = db.ListTaskStates();

            conn.CloseConnection();
        }

        return list;
    }

    
}

public class TaskEntityShow : TaskEntity
{
    private String asDateIssued = String.Empty;
    private String asDateComplete = String.Empty;
    private String asDateUpdated = String.Empty;

    #region Constructors
    //Default constructor
    public TaskEntityShow() { }

    //Constructor
    public TaskEntityShow(Int64 iD, String taskName, String computerName, String taskState, DateTime dateIssued,
        DateTime dateComplete, DateTime dateUpdated, String taskParams, String taskUser) :
        base(iD, taskName, computerName, taskState, dateIssued, dateComplete, dateUpdated, taskParams, taskUser)
    {
        this.asDateIssued = (dateIssued == DateTime.MinValue) ? "-" : dateIssued.ToString();
        this.asDateComplete = (dateComplete == DateTime.MinValue) ? "-" : dateComplete.ToString();
        this.asDateUpdated = (dateUpdated == DateTime.MinValue) ? "-" : dateUpdated.ToString();
    }
    #endregion

    #region Public Properties
    public String AsDateIssued
    {
        get { return asDateIssued; }
        set { asDateIssued = value; }
    }

    public String AsDateComplete
    {
        get { return asDateComplete; }
        set { asDateComplete = value; }
    }

    public String AsDateUpdated
    {
        get { return asDateUpdated; }
        set { asDateUpdated = value; }
    }
    #endregion

    /// <summary>
    /// Create and return clone object
    /// </summary>
    /// <returns>Clone object</returns>
    public override object Clone()
    {
        return new TaskEntityShow(
                this.iD,
                this.taskName,
                this.computerName,
                this.taskState,
                this.dateIssued,
                this.dateComplete,
                this.dateUpdated,
                this.taskParams,
                this.taskUser);
    }

}