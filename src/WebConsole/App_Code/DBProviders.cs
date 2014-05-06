using System;
using System.Collections.Generic;
using System.Web;
using VirusBlokAda.CC.DataBase;
using System.Configuration;

/// <summary>
/// Summary description for DBProviders
/// </summary>
public static class DBProviders
{
    public static PolicyProvider Policy
    {
        get
        {
            PolicyProvider provider = HttpContext.Current.Session[PolicyProvider.ProviderName] as PolicyProvider;
            if (provider == null)
            {
                provider = new PolicyProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Session[PolicyProvider.ProviderName] = provider;
            }

            return provider;
        }
    }

    public static ComponentsProvider Component
    {
        get
        {
            ComponentsProvider provider = HttpContext.Current.Session[ComponentsProvider.ProviderName] as ComponentsProvider;
            if (provider == null)
            {
                provider = new ComponentsProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Session[ComponentsProvider.ProviderName] = provider;
            }

            return provider;
        }
    }

    public static ComputerProvider Computer
    {
        get
        {
            ComputerProvider provider = HttpContext.Current.Session[ComputerProvider.ProviderName] as ComputerProvider;
            if (provider == null)
            {
                provider = new ComputerProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Session[ComputerProvider.ProviderName] = provider;
            }

            return provider;
        }
    }

    public static DataBaseProvider DataBase
    {
        get
        {
            DataBaseProvider provider = HttpContext.Current.Session[DataBaseProvider.ProviderName] as DataBaseProvider;
            if (provider == null)
            {
                provider = new DataBaseProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Session[DataBaseProvider.ProviderName] = provider;
            }

            return provider;
        }
    }

    public static EventProvider Event
    {
        get
        {
            EventProvider provider = HttpContext.Current.Session[EventProvider.ProviderName] as EventProvider;
            if (provider == null)
            {
                provider = new EventProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Session[EventProvider.ProviderName] = provider;
            }

            return provider;
        }
    }

    public static GroupProvider Group
    {
        get
        {
            GroupProvider provider = HttpContext.Current.Session[GroupProvider.ProviderName] as GroupProvider;
            if (provider == null)
            {
                provider = new GroupProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Session[GroupProvider.ProviderName] = provider;
            }

            return provider;
        }
    }

    public static InstallationTaskProvider InstallationTask
    {
        get
        {
            InstallationTaskProvider provider = HttpContext.Current.Session[InstallationTaskProvider.ProviderName] as InstallationTaskProvider;
            if (provider == null)
            {
                provider = new InstallationTaskProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Session[InstallationTaskProvider.ProviderName] = provider;
            }

            return provider;
        }
    }

    public static ProcessProvider Process
    {
        get
        {
            ProcessProvider provider = HttpContext.Current.Session[ProcessProvider.ProviderName] as ProcessProvider;
            if (provider == null)
            {
                provider = new ProcessProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Session[ProcessProvider.ProviderName] = provider;
            }

            return provider;
        }
    }

    public static ScanningObjectProvider ScanningObject
    {
        get
        {
            ScanningObjectProvider provider = HttpContext.Current.Session[ScanningObjectProvider.ProviderName] as ScanningObjectProvider;
            if (provider == null)
            {
                provider = new ScanningObjectProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Session[ScanningObjectProvider.ProviderName] = provider;
            }

            return provider;
        }
    }

    public static TaskProvider Task
    {
        get
        {
            TaskProvider provider = HttpContext.Current.Session[TaskProvider.ProviderName] as TaskProvider;
            if (provider == null)
            {
                provider = new TaskProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Session[TaskProvider.ProviderName] = provider;
            }

            return provider;
        }
    }

    public static TemporaryGroupProvider TemporaryGroup
    {
        get
        {
            TemporaryGroupProvider provider = HttpContext.Current.Session[TemporaryGroupProvider.ProviderName] as TemporaryGroupProvider;
            if (provider == null)
            {
                provider = new TemporaryGroupProvider(ConfigurationManager.ConnectionStrings["ARM2DataBase"].ConnectionString);
                HttpContext.Current.Session[TemporaryGroupProvider.ProviderName] = provider;
            }

            return provider;
        }
    }
}