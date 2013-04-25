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

namespace Vba32.ControlCenter.PeriodicalMaintenanceService
{
    public partial class Vba32PMS : ServiceBase
    {
        //Потоки
        private Thread thread;
        private ManualResetEvent shutdownEvent;
        private TimeSpan delay;

        #region Variables
        private bool isShutdown = false;
        private bool maintenanceEnabled = true;
        private string connectionString = String.Empty; //строка подключения
        private string path;                            //путь к файлам
        private string server;                          //вышестоящий сервер
        private int port=666;                           //порт
        private int maxFileLength=8000666;              //максимальный размер файлов с событиями(впрочем, при большой выборке он может быть больше)
        private string filePrefix = "ETS-";             //префикс файлов


        private DateTime lastSelectDate;                //время последней выборки
        private DateTime lastSendDate;                  //время последней успешной отсылки данных
        private DateTime nextSendDate;                  //время отсылки по расписанию 

        private int deliveryTimeoutCheck=30;            //интервал опроса
        private int dataSendInterval = 66;              //интервал отсылки сообщений
        private int daysToDelete = 66;
        private int taskDaysToDelete = 66;
        private int allowLog = 0;                       //Возможно, стоит определить перечисление для уровня логгирования
        private int hourIntervalToSend = 4;             //интервал отсылки сообщений в часах

        private string registryControlCenterKeyName;     //путь к настройкам центра управления 
        private string machineName;                     //Имя компа. Под этим именем присылаются на родительский центр события
        private string ipAddress = String.Empty;        //IP адрес в пакет SystemInfo

        private Logger log;                             //Класс, позволяющий записывать в файл на диске строки-сообщения
        
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
                log = new Logger(path + AppDomain.CurrentDomain.FriendlyName + ".log", Encoding.Default, true);
                path += "PMSDeferredEvents\\";
                LogMessage("Vba32PMS.Constructor()::Отработал");
            }
            catch (Exception ex)
            {
                string errorMessage = "Vba32PMS():: " + ex.Message;
                LogMessage(errorMessage);
                EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, errorMessage, EventLogEntryType.Error);   
            }

        }

        protected override void OnStart(string[] args)
        {
            try
            {
                LogMessage("Vba32PMS.OnStart()::запущен");

                machineName = Environment.MachineName;
                ipAddress = GetIP(machineName);

                //Выведем инфу в лог
                StringBuilder builder = new StringBuilder(128);
                builder.AppendFormat("NetBIOS имя: {0}, IP-адрес: {1}", machineName, ipAddress);
                LogMessage(builder.ToString());

                if (!ReadSettingsFromRegistry())
                {
                    LogError("Vba32PMS.OnStart()::Ошибка при загрузке настроек из реестра. Дальнейшая инициализация невозможна",
                        EventLogEntryType.Error);
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
                LogError("Vba32PMS.OnStart()::" + ex.Message,
                    EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            LogMessage("Vba32PMS.OnStop():: запущен");

            StopService();
            base.OnStop();
        }

        protected override void OnShutdown()
        {
            LogMessage("Vba32PMS.OnShutdown():: запущен");

            StopService();
            base.OnShutdown();
        }

        private void StopService()
        {
            isShutdown = true;

            if (shutdownEvent != null)
                shutdownEvent.Set();

           if (thread != null)
                thread.Join(TimeSpan.FromSeconds(10));
        }

        #endregion

        protected void ServiceMain()
        {
            LogMessage("Vba32PMS.ServiceMain():: Запущен");
            bool signaled = false;
            bool returnCode = false;

            while (true)
            {
                returnCode = Execute();
                signaled = shutdownEvent.WaitOne(delay, true);
                if (signaled == true)
                {
                    //LogMessage("Пришел сигнал о завершении, выходим из цикла");
                    break;
                }
                
            }
        }

        protected bool Execute()
        {
            try
            {
                //Проверяем необходимость считывания настроек..
                if (IsReRead())
                    if(ReadSettingsFromRegistry())
                        SkipReRead();//Настройки успешно считаны, удаляем флаг

                if (!isShutdown)
                    CheckDeliveryState(connectionString);
                else
                    return false;

                if (maintenanceEnabled)
                {
                    // LogMessage("1. Меняем статус задач.. ");
                    
                    //LogMessage("2. Проверяем необходимость отсылки событий.. ");
                    LogMessage("DateTime.Now=" + DateTime.Now + " nextSendDate=" + nextSendDate);
                    if (DateTime.Compare(DateTime.Now, nextSendDate) == 1)
                    {
                        LogMessage("Необходимо отослать события");
                        if (!DataBaseToXml())
                            LogMessage("Метод DataBaseToXml вернул false");
                        if (SendSystemInfo())
                        {
                            if (!SendEventsFromFiles())
                                LogMessage("Метод SendEventsFromFiles вернул false");
                            else
                            {
                                LogMessage("Необходимо изменить дату следующей отсылки");
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
                            LogMessage("Метод SendSystemInfo вернул false");
                    }
                    else
                    {
                        LogMessage("Необходимости в отсылке событий нет");
                    }
                }

                    //LogMessage("3. Чистим от старых событий.. ");
                    if (!isShutdown)
                        ClearOldEvents(connectionString);
                    else
                        return false;

                    //LogMessage("4. Чистим от старых задач.. ");
                    if (!isShutdown)
                        ClearOldTasks(connectionString);
                    else
                        return false;

                    //LogMessage("5. Сжатие базы... ");
                    if (!isShutdown)
                        CompressDB(connectionString);
                
            }
            catch(Exception ex)
            {
                LogError("Vba32PMS.Execute()::" + ex.Message,
                    EventLogEntryType.Error);
               return false;
            }
            
            return true;
        }

        #region Logging
        private void LogError(string errorMessage, EventLogEntryType eventLogType)
        {
            try
            {
                Debug.WriteLine(errorMessage);
                log.Write(errorMessage);
                EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, errorMessage, eventLogType);
            }
            catch
            {
            }

        }

        private void LogMessage(string errorMessage)
        {
            try
            {
                Debug.WriteLine(errorMessage);
                if (allowLog > 0)
                {
                    log.Write(errorMessage);
                }
            }
            catch
            {
            }
        }
        #endregion

    }
}
