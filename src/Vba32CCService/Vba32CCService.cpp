//////////////////////////////////////
//	File: Vba32CCService.cpp		//
//	Main application implementation	//
//	(c) 2007 VirusBlokAda Ltd.		//
//////////////////////////////////////


//////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "vba_common/service/SCMCtrl.h"
#include "vba_common/service/ServiceStatus.h"
#include "vba_common/service/ServiceCtrl.h"
#include "vba_common/service/IOCP.h"

#include "startup_routines.h"
#include "BusinessLogic.h"

// Keyfile library (wrapper)
#include "keyfile_wrapper/keyfile_wrapper.h"

// Update implementation
#include "update.h"

// Network routines
#include "Network.h"

// Exception & dumps
#include "Exception.h"


//////////////////////////////////////////////////////////////////////////////


enum COMPKEY
{
	CK_SERVICECONTROL,
	CK_PACKET
};


//////////////////////////////////////////////////////////////////////////////


CServiceStatus	g_ssCC;
CSCMCtrl		g_scm;
CServiceCtrl	g_sc;


static CONST INT MAX_LOGFILE_SIZE = 1024; // kilobytes

PTSTR	g_service_name =                _T("VbaControlCenter");
PCTSTR	g_service_display_name =        _T("Vba32 Control Center");
PTSTR	g_service_display_description =	_T("Provides Vba32 antivirus remote control. ")
										_T("Is a part of Vba32 Control Center server. ")
										_T("Gathers AV statistics from Vba32 installed on workstations. ")
										_T("If this service is stopped, remote control ")
										_T("and statistics gathering will be unavaliable.");


//////////////////////////////////////////////////////////////////////////////


// ServiceMain function
VOID WINAPI CCServiceMain(DWORD dwArgc, PTSTR* pszAgrv);
// Service control handler
DWORD WINAPI CCHandlerEx(DWORD dwControl, DWORD dwEventType, PVOID pvEventData, PVOID pvContext);
// Watches directory changes & makes daily key checks
VOID StartWatchingKey();
// Thread function for watching key file
DWORD WINAPI KeyWatchingThread(LPVOID p_parameter);
// Create thread that performs periodical program update
VOID StartPeriodicalUpdate();
// Thread function for program update
DWORD WINAPI UpdatingThread(LPVOID p_parameter);


//////////////////////////////////////////////////////////////////////////////

INT WINAPI _tWinMain(HINSTANCE hInst, HINSTANCE, PTSTR /*pszCmdLine*/, int)
{
	INT ret = 0;

    // Exception handling
	SetUnhandledExceptionFilter(OnProcessException);

	p_logfile->StartLogging(_T("Vba32CC.log"), MAX_LOGFILE_SIZE /* Logfile size limit */);

    INT nArgc = __argc;
#ifdef UNICODE
	PCTSTR* p_cmdline_argv = (PCTSTR*) CommandLineToArgvW(GetCommandLine(), &nArgc);
#else
	PCTSTR* p_cmdline_argv = (PCTSTR*) __argv;
#endif

	if (nArgc < 2){
		// Starting service main, used only by SCM
		SERVICE_TABLE_ENTRY ServiceTable[] = {	
			{g_service_name, CCServiceMain},
			{0, 0}
		};
		if (!StartServiceCtrlDispatcher(ServiceTable)){
			DWORD err = GetLastError();
			// Error starting service
			p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_SERVICE_START_ERROR), err, FormatErrorMessage(err));
			ret = -1;
			// Exit program
			goto cleanup;
		}
		p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_END));
	}
	else{
		// Parsing command line
		if ((!_tcsicmp(p_cmdline_argv[1], _T("help"))) || (!_tcsicmp(p_cmdline_argv[1], _T("/?")))){
			// Information message

#pragma todo(Заточить под Висту WTSSendMessage)

			TCHAR szAppTitle[100] = _T("");
			_tcscpy(szAppTitle, LoadStringFromResource(IDS_APPTITLE));
			MessageBox(	0,
						LoadStringFromResource(IDS_MESSAGE_USAGE),
						szAppTitle,
						MB_ICONINFORMATION + MB_OK);
		} // _tcsicmp ("help")

		else if(!_tcsicmp(p_cmdline_argv[1], _T("install"))){
			// Installing service
	        TCHAR module_full_path[_MAX_PATH + 2];
	        GetModuleFileName(0, module_full_path + 1, _MAX_PATH);
            module_full_path[0] = _T('\"');
            _tcscat(module_full_path, _T("\""));
            // More rights needed than for the global object
            CSCMCtrl scm_create(SC_MANAGER_CREATE_SERVICE);
            DWORD err = g_sc.InstallAndOpen(    scm_create,
									            g_service_name,
									            g_service_display_name,
									            g_service_display_description,
									            SERVICE_WIN32_OWN_PROCESS,
									            SERVICE_AUTO_START,
									            SERVICE_ERROR_IGNORE,
									            module_full_path,
									            0,
									            0,
									            0,
									            0,
									            0);
			if (err != NO_ERROR){
				// Service installation failed
				p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_SERVICE_INSTALL_ERROR), err, FormatErrorMessage(err));
				ret = -1;
				// Exit program
				goto cleanup;
			}
	        // Adding Windows XP SP2, Windows Server 2003, Windows Vista
            // built-in firewall exclusions
	        AddApplicationToFirewallExclusions(_T("Vba32 Control Center"));

			p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_SERVICE_INSTALL_SUCCESS));

#pragma todo(Написать запуск сервиса)

		} // _tcsicmp ("install")

		else if ((!_tcsicmp(p_cmdline_argv[1], _T("uninstall"))) || (!_tcsicmp(p_cmdline_argv[1], _T("remove")))){
			// Uninstalling service
			if (g_sc.Open(g_scm, TRUE, g_service_name, SERVICE_STOP | DELETE))
            {
			    g_sc.Control(SERVICE_CONTROL_STOP);
                if (g_sc.Delete())
                {
			        p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_SERVICE_UNINSTALL_SUCCESS));
		            ret = 0;
		            // Exit program
		            goto cleanup;
                }
            }
		    // Service installation failed
            DWORD err = GetLastError();
		    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_SERVICE_UNINSTALL_ERROR), err, FormatErrorMessage(err));
		    ret = -1;
		    // Exit program
		    goto cleanup;
		} // _tcsicmp ("uninstall")

		else if(!_tcsicmp(p_cmdline_argv[1], _T("start"))){
			// Checking current service status
			if (g_sc.Open(g_scm, TRUE, g_service_name, SERVICE_QUERY_STATUS | SERVICE_START))
            {
			    SERVICE_STATUS ss;
			    g_sc.QueryStatus(&ss);
			    if (ss.dwCurrentState == SERVICE_RUNNING || ss.dwCurrentState == SERVICE_START_PENDING)
                {
				    // Cannot start service
				    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_SERVICE_ALREADY_RUNNING));
				    ret = -1;
				    // Exit program
				    goto cleanup;
			    }
    			if (g_sc.Start(_T("")))
                {
                    // Successful
				    ret = 0;
				    // Exit program
				    goto cleanup;
                }
            }
			// Starting service failed
            DWORD err = GetLastError();
			p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_SERVICE_START_ERROR), err, FormatErrorMessage(err));
			ret = -1;
			// Exit program
			goto cleanup;
		} // _tcsicmp ("start")

		else if(!_tcsicmp(p_cmdline_argv[1], _T("stop"))){
			// Checking current service status
			if (g_sc.Open(g_scm, TRUE, g_service_name, SERVICE_QUERY_STATUS | SERVICE_STOP))
            {
    			SERVICE_STATUS ss;
			    g_sc.QueryStatus(&ss);
			    if (ss.dwCurrentState == SERVICE_STOPPED || ss.dwCurrentState == SERVICE_STOP_PENDING){
				    // Cannot stop service
				    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_SERVICE_ALREADY_STOPPED));
				    ret = -1;
				    // Exit program
				    goto cleanup;
			    }
		        if (g_sc.Control(SERVICE_CONTROL_STOP)){
                    // Successful
				    ret = 0;
				    // Exit program
				    goto cleanup;
		        }
            }
			// Stopping service failed
            DWORD err = GetLastError();
			p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_SERVICE_STOP_ERROR), err, FormatErrorMessage(err));
			ret = -1;
			// Exit program
			goto cleanup;
		} // _tcsicmp ("stop")

		else if(!_tcsicmp(p_cmdline_argv[1], _T("restart"))){
			// Restarting service
			if (g_sc.Open(g_scm, TRUE, g_service_name, SERVICE_START | SERVICE_STOP))
            {
			    g_sc.Control(SERVICE_CONTROL_STOP);
                Sleep(7000);
			    if (g_sc.Start(_T(""))){
                    // Successful
				    ret = 0;
				    // Exit program
				    goto cleanup;
			    }
            }
			// Starting service failed
            DWORD err = GetLastError();
			p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_SERVICE_START_ERROR), err, FormatErrorMessage(err));
			ret = -1;
			// Exit program
			goto cleanup;
		} // _tcsicmp ("restart")

		else if(!_tcsicmp(p_cmdline_argv[1], _T("debug"))){
			// Debugging service
			p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_DEBUG));
			CCServiceMain(0, 0);
			p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_DEBUG_END));
		} // _tcsicmp ("debug")

		else if(!_tcsicmp(p_cmdline_argv[1], _T("status"))){
			// Reporting service status to log and as return code
			if (g_sc.Open(g_scm, TRUE, g_service_name, SERVICE_QUERY_STATUS))
            {
			    SERVICE_STATUS ss;
			    g_sc.QueryStatus(&ss);
			    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_STATUS), g_arrszServiceStatus[ss.dwCurrentState]);
			    ret = ss.dwCurrentState;
            }
            else
            {
			    // Opening service failed
                DWORD err = GetLastError();
			    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_SERVICE_OPEN_ERROR), err, FormatErrorMessage(err));
			    ret = -1;
			    // Exit program
			    goto cleanup;
            }
		} // _tcsicmp ("status")

		else if(!_tcsicmp(p_cmdline_argv[1], _T("dump"))){
			// Creating fake exception
			RaiseException(1, 0/*EXCEPTION_NONCONTINUABLE*/, 0, 0);
		} // _tcsicmp ("dump")
	} // else (nArgc < 2)

cleanup:
	return (ret);
}


//////////////////////////////////////////////////////////////////////////////


VOID WINAPI CCServiceMain(DWORD dwArgc, PTSTR* pszAgrv)
{
	// Create the completion port and save its handle in a global
	// variable so that the Handler function can access it
	CIOCP iocp(0);

	g_ssCC.Initialize(	g_service_name,
						CCHandlerEx,
						(PVOID)&iocp,
						TRUE);
	g_ssCC.AcceptControls(SERVICE_ACCEPT_STOP | SERVICE_ACCEPT_SHUTDOWN);
	g_ssCC.ReportUltimateState();

    //  Initializing service
	p_logfile->AddToLog(_T(""));
	p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_START));

    // Starting up WinSock
    int result = NO_ERROR;
    WORD version_requested = MAKEWORD(2, 2);
    WSADATA wsa_data;
     
    result = WSAStartup(version_requested, &wsa_data);
    if (result)
    {
	    // Error initializing Windows Sockets
	    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_SOCKET_INIT_ERROR), result, FormatErrorMessage(result));
        g_ssCC.SetUltimateState(SERVICE_STOPPED);
        return;
    }

    // Starting up DCOM
    HRESULT hr = S_OK;
    hr = CoInitializeEx(0, COINIT_MULTITHREADED);
    if (FAILED(hr))
    {
	    // Error initializing Windows Sockets
	    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_COM_INIT_ERROR), hr, FormatErrorMessage(hr));
        g_ssCC.SetUltimateState(SERVICE_STOPPED);
        WSACleanup();
        return;
    }
	hr =  CoInitializeSecurity(	0, 
								-1,							// COM negotiates service
								0,							// Authentication services
								0,							// Reserved
								RPC_C_AUTHN_LEVEL_DEFAULT,	// Default authentication 
								RPC_C_IMP_LEVEL_IMPERSONATE,// Default Impersonation  
								0,							// Authentication info
								EOAC_NONE,					// Additional capabilities 
								0);
    if (FAILED(hr))
    {
	    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_COM_INIT_ERROR), hr, FormatErrorMessage(hr));
        g_ssCC.SetUltimateState(SERVICE_STOPPED);
        CoUninitialize();
        WSACleanup();
        return;
    }

    // Checking integrity
    BOOL is_integral = CheckProgramIntegrity();

	// Checking key file validity
    BOOL is_key_valid = CheckKeyFileValidity(TRUE);

    if (!is_integral && !is_key_valid)
    {
        // Fatal situation, cannot continue
#pragma todo(Сделать запись в системный журнал)
    	p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_FATAL));
        g_ssCC.SetUltimateState(SERVICE_STOPPED);
        CoUninitialize();
        WSACleanup();
        return;
    }

    // Setting timer for key reload & file notification
    StartWatchingKey();

    // Updating program
    StartPeriodicalUpdate();

    // Starting up business logic
    hr = business_logic.Initialize();
    if (FAILED(hr))
    {
	    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_PARSER_INIT_ERROR), hr, FormatErrorMessage(hr));
        g_ssCC.SetUltimateState(SERVICE_STOPPED);
        CoUninitialize();
        WSACleanup();
        return;
    }
	p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_PARSER_STARTED));
    hr = business_logic.SetCallbacks();
    if (FAILED(hr))
    {
	    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_PARSER_INIT_ERROR), hr, FormatErrorMessage(hr));
        g_ssCC.SetUltimateState(SERVICE_STOPPED);
        CoUninitialize();
        WSACleanup();
        return;
    }

    /*
    if (is_key_valid)
    {
	    // Starting up Windows Sockets
        StartupNetwork();
    }
    */

    // Service control loop
	ULONG_PTR CompKey = CK_SERVICECONTROL;
	DWORD dwControl = SERVICE_CONTROL_CONTINUE;
	OVERLAPPED* po;
	DWORD dwNumBytes;

	do
    {
		switch (CompKey)
        {
		case CK_SERVICECONTROL:
			// We got a new control code
			switch (dwControl) {
			case SERVICE_CONTROL_CONTINUE:
				g_ssCC.ReportUltimateState();
				break;

			case SERVICE_CONTROL_STOP:
			case SERVICE_CONTROL_SHUTDOWN:
				g_ssCC.ReportUltimateState();
				break;
			}
			break;

		case CK_PACKET:
			break;

		} // switch(CompKey)

		if (g_ssCC != SERVICE_STOPPED)
        {
			// Sleep until a control code comes in or a client connects
			iocp.GetStatus(&CompKey, &dwNumBytes, &po);
			dwControl = dwNumBytes;
		}
	} while (g_ssCC != SERVICE_STOPPED);

    // Shutting down WinSock
    WSACleanup();

    // Shutting down DCOM
    CoUninitialize();
}


//////////////////////////////////////////////////////////////////////////////


DWORD WINAPI CCHandlerEx(DWORD dwControl, DWORD dwEventType, PVOID pvEventData, PVOID pvContext)
{
	DWORD dwRet = ERROR_CALL_NOT_IMPLEMENTED;
	BOOL fPostControlToServiceThread = FALSE;
    CAutoCloseHandle update_stop_event = 0;
    CAutoCloseHandle key_watching_stop_event = 0;

    switch (dwControl)
    {
	case SERVICE_CONTROL_STOP:
	case SERVICE_CONTROL_SHUTDOWN:

#pragma todo(написать завершение потоков сервиса при выключении компьютера)
        ShutdownNetwork();
        // Business logic shutdown is being carried out while destructing object
	    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_PARSER_SHUTDOWN));

        // Shutting down update thread
        update_stop_event = OpenEvent( EVENT_MODIFY_STATE,
                                       FALSE,
                                       _T("Vba32CC_stop_update_thread"));
        if (update_stop_event.IsValid())
        {
            if (!SetEvent(update_stop_event))
            {
                DEBUG_Message3((_T("CCHandlerEx(): failed to set update_stop event")));
            }
            else
            {
                DEBUG_Message3((_T("CCHandlerEx(): update_stop event set successfully")));
            }
        }
        else
        {
            DEBUG_Message1((_T("CCHandlerEx(): failed to open update_stop event")));
        }

        // Shutting down key watching thread
        key_watching_stop_event = OpenEvent( EVENT_MODIFY_STATE,
                                             FALSE,
                                             _T("Vba32CC_stop_key_watching_thread"));
        if (key_watching_stop_event.IsValid())
        {
            if (!SetEvent(key_watching_stop_event))
            {
                DEBUG_Message3((_T("CCHandlerEx(): failed to set key_stop event")));
            }
            else
            {
                DEBUG_Message3((_T("CCHandlerEx(): key_stop event set successfully")));
            }
        }
        else
        {
            DEBUG_Message1((_T("CCHandlerEx(): failed to open key_stop event")));
        }

		g_ssCC.SetUltimateState(SERVICE_STOPPED, 2000);
		fPostControlToServiceThread = TRUE;
		break;

	case SERVICE_CONTROL_CONTINUE:
		g_ssCC.SetUltimateState(SERVICE_RUNNING, 2000);
		fPostControlToServiceThread = TRUE;
		break;

	case SERVICE_CONTROL_INTERROGATE:
		g_ssCC.ReportStatus();
		break;

	case SERVICE_CONTROL_PARAMCHANGE:
		break;

	case SERVICE_CONTROL_PAUSE:
	case SERVICE_CONTROL_DEVICEEVENT:
	case SERVICE_CONTROL_HARDWAREPROFILECHANGE:
	case SERVICE_CONTROL_POWEREVENT:
		break;
	}
	if (fPostControlToServiceThread)
    {
		// The Handler thread is very simple and executes very quickly because
		// it just passes the control code off to the ServiceMain thread.
		CIOCP* piocp = (CIOCP*) pvContext;
		piocp->PostStatus(CK_SERVICECONTROL, dwControl);
		dwRet = NO_ERROR;
	}

	return (dwRet);
}


VOID StartWatchingKey()
{
    // Create thread that continuously monitors file system changes
    CreateThread(   0,
                    0,
                    KeyWatchingThread,
                    0,
                    0,
                    0);
}


DWORD WINAPI KeyWatchingThread(LPVOID p_parameter)
{
    HANDLE events[2] = {0};
    // Event which is used to stop key watching thread
    events[0] = CreateEvent( 0,
                             TRUE,
                             FALSE,
                             _T("Vba32CC_stop_key_watching_thread"));
    if (events[0] == 0)
    {
        // Error creating stop event
        DWORD err = GetLastError();
        DEBUG_Message1((_T("KeyWatchingThread(): error creating key_stop event, error code %d (%s)"), err, FormatErrorMessage(err)));
        return (err);
    }

    std::tstring current_dir;
    vfGetProgramDir(current_dir);
    events[1] = FindFirstChangeNotification( current_dir.c_str(),
                                             FALSE,
                                             FILE_NOTIFY_CHANGE_ATTRIBUTES | FILE_NOTIFY_CHANGE_FILE_NAME);
    if (events[1] == INVALID_HANDLE_VALUE)
    {
        // Error creating change notification event
        DWORD err = GetLastError();
        DEBUG_Message1((_T("KeyWatchingThread(): error creating change notification event, error code %d (%s)"), err, FormatErrorMessage(err)));
        CloseHandle(events[0]);
        return (err);
    }
    while (TRUE)
    {
        // Rereading key at least every hour
        DWORD res = WaitForMultipleObjects(2, events, FALSE, TM_HOUR);
        BOOL was_valid = FALSE;
        BOOL is_valid = FALSE;
        switch (res)
        {
        case WAIT_OBJECT_0:
            // Exiting
            DEBUG_Message1((_T("KeyWatchingThread(): exiting")));
            CloseHandle(events[0]);
            FindCloseChangeNotification(events[1]);
            return 0;
        case WAIT_OBJECT_0 + 1:
        case WAIT_TIMEOUT:
            // Recent key state
            was_valid = keyfile.IsValid();
            CheckKeyFileValidity(FALSE);
            is_valid = keyfile.IsValid();
            // State changed from DEMO to FULL
            if (!was_valid && is_valid)
            {
                // Starting up network
                StartupNetwork();
            }
            if (was_valid && !is_valid)
            {
                // Shutting down network
                ShutdownNetwork();
            }

            // Next notification loop
            if (!FindNextChangeNotification(events[1]))
            {
                return GetLastError();
            }
            break;
        default:
            return (res);
        }
    }
}


VOID StartPeriodicalUpdate()
{
    // Create thread that updates the program in 5 minutes and then every 4 hours
    CreateThread(   0,
                    0,
                    UpdatingThread,
                    0,
                    0,
                    0);
}


DWORD WINAPI UpdatingThread(LPVOID /* p_parameter */)
{
    // Creating update object
    HRESULT init_result = CoInitializeEx(0, COINIT_MULTITHREADED);

    if (FAILED(init_result))
    {
        // COM initialization error
        p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_COM_INIT_ERROR), init_result, FormatErrorMessage(init_result));
        return (init_result);
    }

    // Event which is used to stop update thread
    CAutoCloseHandle update_stop_event = CreateEvent( 0,
                                                      TRUE,
                                                      FALSE,
                                                      _T("Vba32CC_stop_update_thread"));
    if (update_stop_event.IsInvalid())
    {
        // Error creating stop event
        DWORD err = GetLastError();
        DEBUG_Message1((_T("UpdatingThread(): error creating update_stop event, error code %d (%s)"), err, FormatErrorMessage(err)));
        return (err);
    }

    DWORD first_update_delay = 120 * TM_SECOND;
#ifdef _DEBUG
    // For debug versions - 10 seconds, for easier debugging
    first_update_delay = 10 * TM_SECOND;
#endif

    DWORD wait_res = WaitForSingleObject(update_stop_event, first_update_delay);
    if (wait_res == WAIT_OBJECT_0)
    {
        // Stopping update if in progress

        CoUninitialize();
        return 0;
    }

    // Starting update
    DoUpdate();

    while (TRUE)
    {
        DWORD wait_res = WaitForSingleObject(update_stop_event, 4 * TM_HOUR);
        if (wait_res == WAIT_OBJECT_0)
        {
            // Stopping update if in progress
            DEBUG_Message1((_T("UpdatingThread(): exiting")));

            CoUninitialize();
            return 0;
        }

        // Starting update
        DoUpdate();
    }
}

