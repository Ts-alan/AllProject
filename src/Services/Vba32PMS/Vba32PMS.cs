using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

using Vba32.ControlCenter.PeriodicalMaintenanceService.Xml;
using VirusBlokAda.CC.Common;
using VirusBlokAda.CC.Settings.Entities;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService
{
    public partial class Vba32PMS : ServiceBase
    {
        //������
        private Thread thread;
        private ManualResetEvent shutdownEvent;
        private TimeSpan delay;

        #region Variables
        private Boolean isShutdown = false;
        private PMSSettingsEntity settingsPMS;
        private String connectionString = String.Empty; //������ �����������
        private String path;                            //���� � ������        
        private String filePrefix = "ETS-";             //������� ������
        private String machineName;                     //��� �����. ��� ���� ������ ����������� �� ������������ ����� �������
        private String ipAddress = String.Empty;        //IP ����� � ����� SystemInfo
        
        #endregion


        #region �����������, OnStart, OnStop
        public Vba32PMS()
        {
            InitializeComponent();
            try
            {
                path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName)+"\\";
                                
                path += "PMSDeferredEvents\\";
                LoggerPMS.log.Info("Vba32PMS.Constructor():: Done.");
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error(String.Format("Vba32PMS():: {0}", ex.Message));
            }
        }

        protected override void OnStart(String[] args)
        {
            try
            {
                LoggerPMS.log.Info("Vba32PMS.OnStart()::Enter.");

                machineName = Environment.MachineName;
                ipAddress = GetIP(machineName);

                LoggerPMS.log.Info(String.Format("NetBIOS name: {0}, IP-address: {1}", machineName, ipAddress));

                if (!ReadSettingsFromRegistry())
                {
                    LoggerPMS.log.Fatal("Vba32PMS.OnStart():: Load settings from registry error. Initialization is impossible.");
                    return;
                }

                delay = new TimeSpan(0, 0, settingsPMS.DeliveryTimeoutCheck.Value);

                ThreadStart ts = new ThreadStart(this.ServiceMain);
                shutdownEvent = new ManualResetEvent(false);
                thread = new Thread(ts);
                thread.Start();
            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.OnStart()::" + ex.Message);
            }
        }

        protected override void OnStop()
        {
            LoggerPMS.log.Info("Vba32PMS.OnStop():: Enter.");

            StopService();
            base.OnStop();
        }

        protected override void OnShutdown()
        {
            LoggerPMS.log.Info("Vba32PMS.OnShutdown():: Enter.");

            StopService();
            base.OnShutdown();
        }

        private void StopService()
        {
            LoggerPMS.log.Debug("StopService():: Enter.");
            isShutdown = true;

            if (shutdownEvent != null)
                shutdownEvent.Set();

           if (thread != null)
                thread.Join(TimeSpan.FromSeconds(10));
        }

        #endregion

        protected void ServiceMain()
        {
            LoggerPMS.log.Debug("Vba32PMS.ServiceMain():: Enter.");
            Boolean signaled = false;
            Boolean returnCode = false;

            while (true)
            {
                returnCode = Execute();
                LoggerPMS.log.Debug("Wait...");
                signaled = shutdownEvent.WaitOne(delay, true);
                if (signaled == true)
                {
                    LoggerPMS.log.Debug("Came completion signal, exit from the loop.");
                    break;
                }
            }
        }

        protected Boolean Execute()
        {
            try
            {
                LoggerPMS.log.Debug("Check the read settings necessity.");
                if (IsReRead())
                    if (ReadSettingsFromRegistry())
                        SkipReRead();//��������� ������� �������, ������� ����

                if (!isShutdown)
                    CheckDeliveryState(connectionString);
                else
                    return false;

                if (settingsPMS.MaintenanceEnabled)
                {
                    LoggerPMS.log.Debug("2. Check the send events necessity. ");
                    LoggerPMS.log.Debug("DateTime.Now=" + DateTime.Now + " nextSendDate=" + settingsPMS.NextSendDate);
                    if (DateTime.Compare(DateTime.Now, settingsPMS.NextSendDate.Value) == 1)
                    {
                        LoggerPMS.log.Debug("Need to send events.");
                        if (!DataBaseToXml())
                            LoggerPMS.log.Debug("Method DataBaseToXml() return false.");
                        if (SendSystemInfo())
                        {
                            if (!SendEventsFromFiles())
                                LoggerPMS.log.Debug("Method SendEventsFromFiles() return false");
                            else
                            {
                                LoggerPMS.log.Debug("Need to change the next sending date.");
                                while (DateTime.Compare(DateTime.Now, settingsPMS.NextSendDate.Value) == 1)
                                {
                                    switch (settingsPMS.DataSendInterval.Value)
                                    {
                                        case 0:
                                            settingsPMS.NextSendDate = settingsPMS.NextSendDate.Value.AddDays(1);
                                            break;
                                        case 1:
                                            settingsPMS.NextSendDate = settingsPMS.NextSendDate.Value.AddDays(7);
                                            break;
                                        case 2:
                                            settingsPMS.NextSendDate = settingsPMS.NextSendDate.Value.AddMonths(1);
                                            break;
                                        case 3:
                                            settingsPMS.NextSendDate = settingsPMS.NextSendDate.Value.AddHours(settingsPMS.HourIntervalToSend.Value);
                                            break;
                                        default:
                                            settingsPMS.NextSendDate = settingsPMS.NextSendDate.Value.AddDays(1);
                                            break;
                                    }
                                }
                                WriteSettingsToRegistry();
                            }
                        }
                        else
                            LoggerPMS.log.Debug("Method SendSystemInfo() return false");
                    }
                    else
                    {
                        LoggerPMS.log.Debug("No need to send events.");
                    }
                }

                LoggerPMS.log.Debug("3. Send configure agent tasks.");
                if (!isShutdown)
                    ConfigureAgent(connectionString);
                else
                    return false;

                LoggerPMS.log.Debug("4. Clear old events. ");
                if (!isShutdown)
                    ClearOldEvents(connectionString);
                else
                    return false;

                LoggerPMS.log.Debug("5. Clear old tasks. ");
                if (!isShutdown)
                    ClearOldTasks(connectionString);
                else
                    return false;

                LoggerPMS.log.Debug("6. Clear old computers. ");
                if (!isShutdown)
                    ClearOldComputers(connectionString);
                else
                    return false;

                /*
                    LoggerPMS.log.Debug("7. ������ ����... ");
                    if (!isShutdown)
                        CompressDB(connectionString);
                 */

            }
            catch (Exception ex)
            {
                LoggerPMS.log.Error("Vba32PMS.Execute()::" + ex.Message);
                return false;
            }
            return true;
        }

    }
}
