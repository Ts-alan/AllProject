using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

using Vba32.ControlCenter.PeriodicalMaintenanceService.Xml;
using VirusBlokAda.CC.Common;

namespace Vba32.ControlCenter.PeriodicalMaintenanceService
{
    public partial class Vba32PMS : ServiceBase
    {
        //Потоки
        private Thread thread;
        private ManualResetEvent shutdownEvent;
        private TimeSpan delay;

        #region Variables
        private Boolean isShutdown = false;
        private Boolean maintenanceEnabled = true;
        private String connectionString = String.Empty; //строка подключения
        private String path;                            //путь к файлам
        private String server;                          //вышестоящий сервер
        private Int32 port=666;                           //порт
        private Int32 maxFileLength=8000666;              //максимальный размер файлов с событиями(впрочем, при большой выборке он может быть больше)
        private String filePrefix = "ETS-";             //префикс файлов


        private DateTime lastSelectDate;                //время последней выборки
        private DateTime lastSendDate;                  //время последней успешной отсылки данных
        private DateTime nextSendDate;                  //время отсылки по расписанию 

        private Int32 deliveryTimeoutCheck=30;            //интервал опроса
        private Int32 dataSendInterval = 66;              //интервал отсылки сообщений
        private Int32 daysToDelete = 66;
        private Int32 taskDaysToDelete = 66;
        private Int32 compDaysToDelete = 0;        
        private Int32 hourIntervalToSend = 4;             //интервал отсылки сообщений в часах

        private String registryControlCenterKeyName;     //путь к настройкам центра управления 
        private String machineName;                     //Имя компа. Под этим именем присылаются на родительский центр события
        private String ipAddress = String.Empty;        //IP адрес в пакет SystemInfo
        
        #endregion


        #region Конструктор, OnStart, OnStop
        public Vba32PMS()
        {
            InitializeComponent();
            try
            {
                //Проверка на битность ОС
                if (Marshal.SizeOf(typeof(IntPtr)) == 8)
                    registryControlCenterKeyName = "SOFTWARE\\Wow6432Node\\Vba32\\ControlCenter\\";
                else
                    registryControlCenterKeyName = "SOFTWARE\\Vba32\\ControlCenter\\";

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

                //Выведем инфу в лог
                StringBuilder builder = new StringBuilder(128);
                builder.AppendFormat("NetBIOS name: {0}, IP-address: {1}", machineName, ipAddress);
                LoggerPMS.log.Info(builder.ToString());

                if (!ReadSettingsFromRegistry())
                {
                    LoggerPMS.log.Fatal("Vba32PMS.OnStart():: Load settings from registry error. Initialization is impossible.");
                    return;
                }

                delay = new TimeSpan(0, 0, deliveryTimeoutCheck);

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
                        SkipReRead();//Настройки успешно считаны, удаляем флаг

                if (!isShutdown)
                    CheckDeliveryState(connectionString);
                else
                    return false;

                if (maintenanceEnabled)
                {
                    LoggerPMS.log.Debug("2. Check the send events necessity. ");
                    LoggerPMS.log.Debug("DateTime.Now=" + DateTime.Now + " nextSendDate=" + nextSendDate);
                    if (DateTime.Compare(DateTime.Now, nextSendDate) == 1)
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
                                while (DateTime.Compare(DateTime.Now, nextSendDate) == 1)
                                {
                                    switch (dataSendInterval)
                                    {
                                        case 0:
                                            nextSendDate = nextSendDate.AddDays(1);
                                            break;
                                        case 1:
                                            nextSendDate = nextSendDate.AddDays(7);
                                            break;
                                        case 2:
                                            nextSendDate = nextSendDate.AddMonths(1);
                                            break;
                                        case 3:
                                            nextSendDate = nextSendDate.AddHours(hourIntervalToSend);
                                            break;
                                        default:
                                            nextSendDate = nextSendDate.AddDays(1);
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
                    LoggerPMS.log.Debug("7. Сжатие базы... ");
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
