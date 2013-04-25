using System;
using System.Collections.Generic;
using System.Text;

namespace ARM2_dbcontrol.Tasks.Scheduler
{
    public struct SchedulerRule
    {
        public string ActionName;
        public PeriodicityEnum Periodicity;
        public bool Enabled;
        public ActionTypeEnum ActionType;

        //Process
        public string Path;
        public string Parameters;

        //Periodicity
        public int RunEvery;
        public DateTime RunAt;
        public bool IsRunAt;
        public bool IsRunMissedTask;
        public bool[] ArrDaysRun;
        public string NumberDaysMonth;
        public DateTime FixedDate;

        //Scan
        public ScanMode Object_ScanMode;
        public List<string> Object_Pathes;
        public ScanType Object_ScanType;
        public string Object_Custom;
        public string Object_Default_Including;
        public string Object_Default_Excluding;
        public bool Object_Thorough;
        public bool Object_Ware;
        public bool Object_Memory;
        public bool Object_BootSectors;
        public bool Object_AtStartup;
        public bool Object_Installers;
        public bool Object_Mail;
        public bool Object_Archives;
        public int Object_MaxSize;

        public bool Actions_Infected_Cure;
        public bool Actions_Infected_Delete;
        public bool Actions_Infected_ToQtn;
        public bool Actions_Infected_DeleteArchive;
        public bool Actions_Infected_DeleteMail;
        public bool Actions_Suspicious_ToQtn;
        public bool Actions_Suspicious_Delete;
        public Heuristic Actions_Heuristic;

        public bool Report_Report_InfoFile;
        public bool Report_Report_AddToFile;
        public bool Report_Report_CleanToFile;
        public bool Report_InfectedList_IntoFile;
        public bool Report_InfectedList_AddToFile;
        public string Report_Report_Path;
        public string Report_InfectedList_Path;

        public bool Additional_Cache;
        public bool Additional_Interrupt;
        public bool Additional_Bases;
        public bool Additional_Sound;
        public bool Additional_HideWindow;
        public string Additional_Path;

    }

    public enum PeriodicityEnum
    {
        Minutes = 0,
        Hours,
        Days,
        Weeks,
        Months,
        FixedDate
    }

    public enum ActionTypeEnum
    {
        Process = 0,
        Scan,
        Update
    }

    public enum ScanMode
    {
        Fast = 0,
        Safe,
        Excess
    }

    public enum ScanType
    {
        All = 0,
        Default,
        Custom
    }

    public enum Heuristic
    {
        Disabled = 0,
        Optimal,
        Maximum,
        Excessive
    }
}