#include "stdafx.h"
#include "update.h"


VbaCC_Update::VbaCC_Update():
    mp_update(0),
    mp_wrapped_update(0),
    mp_CPC(0),
    mp_CP(0),
    m_advise_cookie(0),
    mp_update_shell(0)
{
    mp_update_events = new VbaCC_UpdateEvents;
}


VbaCC_Update::~VbaCC_Update()
{
    if (mp_CP)
    {
        mp_CP->Unadvise(m_advise_cookie);
        mp_CP->Release();
    }

    InterfaceWrap<IVbaUpdate>* buf_ptr = 0;
    if (mp_wrapped_update)
    {
        buf_ptr = (InterfaceWrap<IVbaUpdate>*)InterlockedExchangePointer(
                reinterpret_cast<PVOID *>(&mp_wrapped_update), 0);
    }

    if (buf_ptr)
    {
        delete buf_ptr;
        buf_ptr = 0;
    }

    if (mp_update)
    {
        mp_update->Release();
    }

    if (mp_CPC)
    {
        mp_CPC->Release();
    }

    if (mp_update_shell)
    {
        delete mp_update_shell;
        mp_update_shell = 0;
    }
}
    
HRESULT VbaCC_Update::CreateUpdateObject()
{
    HRESULT creation_result = CoCreateInstance( CLSID_CoVbaUpdate,
                                                0,
                                                CLSCTX_INPROC_SERVER | CLSCTX_LOCAL_SERVER, 
                                                IID_IVbaUpdate,
                                                (void**)&mp_update);
    if (SUCCEEDED(creation_result))
    {
        mp_wrapped_update = new InterfaceWrap<IVbaUpdate>(IID_IVbaUpdate, mp_update);
        mp_update_shell = new COMUpdateShell(mp_wrapped_update);
    }

    return (creation_result);
}


HRESULT VbaCC_Update::BindToConnectionPoint()
{
    HRESULT query_result = mp_update->QueryInterface(IID_IConnectionPointContainer,(void**)&mp_CPC);

    if (FAILED(query_result))
    {
		return (query_result);
    }
	
	HRESULT find_result = mp_CPC->FindConnectionPoint(IID__IVbaUpdateEvents,&mp_CP);
    
	if (FAILED(find_result))
    {
		return (find_result);
    }

    IUnknown* p_sink_unknown = 0;

	query_result = mp_update_events->QueryInterface(IID_IUnknown,(void**)&p_sink_unknown);

	return (mp_CP->Advise(p_sink_unknown,&m_advise_cookie));
}


////////////////////////////////////////////////////////////////
DWORD InitializeUpdate(VbaCC_Update& updater, DWORD source_index, std::tstring& source)
{
    std::tstring current_dir;
    vfGetProgramDir(current_dir);
    std::tstring temp_dir = current_dir + _T("update.tmp"); // temporary directory for updates

    // Formatting key name
    TCHAR key_name[MAX_PATH];
    _stprintf(key_name, _T("SOFTWARE\\Vba32\\ControlCenter\\Update\\Source%02d"), source_index);
    CAutoRegCloseKey key;
    LONG result = RegCreateKeyEx( HKEY_LOCAL_MACHINE,
                                  key_name,
                                  0,
                                  0,
                                  REG_OPTION_NON_VOLATILE,
                                  KEY_ALL_ACCESS,
                                  0,
                                  key.GetPtr(),
                                  0);
    if (result != ERROR_SUCCESS)
    {
        return (result);
    }

    // Authorization settings
    VbaAuthSettings authorization;
    DWORD size = sizeof(DWORD);
    DWORD data = 0;
    if (RegQueryValueEx( key,
                         _T("AuthorizationEnabled"),
                         0,
                         0,
                         (LPBYTE)&data,
                         &size) == ERROR_SUCCESS)
    {
        authorization.is_auth_enabled = (data != 0);
    }
    else
    {
        authorization.is_auth_enabled = FALSE;
        data = FALSE;
        RegSetValueEx( key,
                       _T("AuthorizationEnabled"),
                       0,
                       REG_DWORD,
                       (LPBYTE)&data,
                       sizeof(data));
    }
    if (authorization.is_auth_enabled)
    {
        DEBUG_Message1((_T("authorization.is_auth_enabled = true")));
        // Reading authorization settings
        TCHAR auth_name[512] = {0};
        size = 512 * sizeof(TCHAR);
        if (RegQueryValueEx( key,
                             _T("AuthorizationUsername"),
                             0,
                             0,
                             (LPBYTE)auth_name,
                             &size) == ERROR_SUCCESS)
        {
            auth_name[size / sizeof(TCHAR)] = _T('\0');
            authorization.user_login = auth_name;
        }
        DEBUG_Message1((_T("authorization.user_login = %s"), authorization.user_login.c_str()));

        size = 512 * sizeof(TCHAR);
        if (RegQueryValueEx( key,
                             _T("AuthorizationPassword"),
                             0,
                             0,
                             (LPBYTE)auth_name,
                             &size) == ERROR_SUCCESS)
        {
            auth_name[size / sizeof(TCHAR)] = _T('\0');
            authorization.user_password = auth_name;
        }
        DEBUG_Message1((_T("authorization.user_password = %s"), authorization.user_password.c_str()));

        data = 0;
        size = sizeof(DWORD);
        RegQueryValueEx( key,
                         _T("AuthorizationNTLMEnabled"),
                         0,
                         0,
                         (LPBYTE)&data,
                         &size);
        authorization.is_ntlm_enabled = (data != 0);
        DEBUG_Message1((_T("authorization.is_ntlm_enabled = %d"), authorization.is_ntlm_enabled));
    }
    updater.GetUpdateShell()->SetAuthorizationSettings(authorization);

    // Account settings
    VbaAccountSettings account;
    size = sizeof(DWORD);
    data = 0;
    if (RegQueryValueEx( key,
                         _T("ImpersonationAccountEnabled"),
                         0,
                         0,
                         (LPBYTE)&data,
                         &size) == ERROR_SUCCESS)
    {
        account.is_enabled = (data != 0);
    }
    else
    {
        account.is_enabled = FALSE;
        data = FALSE;
        RegSetValueEx( key,
                       _T("ImpersonationAccountEnabled"),
                       0,
                       REG_DWORD,
                       (LPBYTE)&data,
                       sizeof(data));
    }
    if (account.is_enabled)
    {
        DEBUG_Message1((_T("account.is_enabled = true")));
        // Reading authorization settings
        TCHAR auth_name[512] = {0};
        size = 512 * sizeof(TCHAR);
        if (RegQueryValueEx( key,
                             _T("ImpersonationAccountUsername"),
                             0,
                             0,
                             (LPBYTE)auth_name,
                             &size) == ERROR_SUCCESS)
        {
            auth_name[size / sizeof(TCHAR)] = _T('\0');
            account.user_login = auth_name;
        }
        DEBUG_Message1((_T("account.user_login = %s"), account.user_login.c_str()));

        size = 512 * sizeof(TCHAR);
        if (RegQueryValueEx( key,
                             _T("ImpersonationAccountPassword"),
                             0,
                             0,
                             (LPBYTE)auth_name,
                             &size) == ERROR_SUCCESS)
        {
            auth_name[size / sizeof(TCHAR)] = _T('\0');
            account.user_password = auth_name;
        }
        DEBUG_Message1((_T("account.user_password = %s"), account.user_password.c_str()));
    }
    updater.GetUpdateShell()->SetAccountSettings(account);

    // Proxy settings
    VbaProxySettings proxy;
    size = sizeof(DWORD);
    if (RegQueryValueEx( key,
                         _T("ProxyEnabled"),
                         0,
                         0,
                         (LPBYTE)&data,
                         &size) == ERROR_SUCCESS)
    {
        proxy.is_proxy_enabled = (data != 0);
    }
    else
    {
        proxy.is_proxy_enabled = FALSE;
        data = FALSE;
        RegSetValueEx( key,
                       _T("ProxyEnabled"),
                       0,
                       REG_DWORD,
                       (LPBYTE)&data,
                       sizeof(data));
    }
    if (proxy.is_proxy_enabled)
    {
        DEBUG_Message1((_T("proxy.is_proxy_enabled")));
        // Reading authorization settings
        TCHAR proxy_ip[17] = {0};
        size = 17;
        if (RegQueryValueEx( key,
                             _T("ProxyAddress"),
                             0,
                             0,
                             (LPBYTE)proxy_ip,
                             &size) == ERROR_SUCCESS)
        {
            proxy_ip[size / sizeof(TCHAR)] = _T('\0');
            proxy.proxy_address = proxy_ip;
        }
        DEBUG_Message1((_T("proxy.proxy_address = %s"), proxy.proxy_address.c_str()));

        data = 0;
        size = sizeof(DWORD);
        if (RegQueryValueEx( key,
                             _T("ProxyPort"),
                             0,
                             0,
                             (LPBYTE)&data,
                             &size) == ERROR_SUCCESS)
        {
            proxy.port_number = data;
        }
        DEBUG_Message1((_T("proxy.port_number = %d"), proxy.port_number));
    }
    updater.GetUpdateShell()->SetProxySettings(proxy);

    // Update paths
    TCHAR update_dir[MAX_PATH];
    size = MAX_PATH;
    result = RegQueryValueEx( key,
                              _T("UpdateSource"),
                              0,
                              0,
                              (LPBYTE)update_dir,
                              &size);
    if (result == ERROR_SUCCESS)
    {
        update_dir[size / sizeof(TCHAR) - 1] = _T('\0');
        // Trimming spaces
        LPSTR space = &update_dir[size / sizeof(TCHAR) - 2];
        while (*space == _T(' '))
        {
            *space-- = _T('\0');
        }
        LPSTR no_space = &update_dir[0];
        while (*no_space++ == _T(' '));
        source = --no_space;
        DEBUG_Message3((_T("UpdateSource = \"%s\""), source.c_str()));
        updater.GetUpdateShell()->SetUpdatePaths(source, current_dir, temp_dir);
    }
    std::tstring webconsole_path = _T("");
    vfGetProgramDir(webconsole_path);
    webconsole_path += _T("WebConsole");
    updater.GetUpdateShell()->SetUpdateVariable(std::tstring(_T("WEBCONSOLE")), webconsole_path);

    return (result);
}


DWORD DoUpdate()
{
    DEBUG_Message1((_T("DoUpdate(): Start")));
    VbaCC_Update cc_updater;

    HRESULT res = cc_updater.CreateUpdateObject();
    if (FAILED(res))
    {
        // Creating COM object error
        p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_INIT_ERROR), _T("failed creating update object"), res, FormatErrorMessage(res));
        return (res);
    }
    DEBUG_Message1((_T("DoUpdate(): CreateUpdateObject() success")));

    res = cc_updater.BindToConnectionPoint();
    if (FAILED(res))
    {
        // Binding to connection point error
        p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_INIT_ERROR), _T("failed binding to connection point"), res, FormatErrorMessage(res));
        return (res);
    }
    DEBUG_Message1((_T("DoUpdate(): BindToConnectionPoint() success")));

    std::tstring source;
    DWORD init = InitializeUpdate(cc_updater, 0, source);
    if (init != 0)
    {
        p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_INIT_ERROR), _T("failed reading Registry"), init, FormatErrorMessage(init));
        return (init);
    }
    DEBUG_Message1((_T("DoUpdate(): InitializeUpdate() success")));

    HANDLE events[2] = {0};
    events[0] = OpenEvent( SYNCHRONIZE,
                           FALSE,
                           _T("Vba32CC_stop_update_thread"));
    events[1] = CreateEvent( 0,
                             TRUE,
                             FALSE,
                             _T("Vba32CC_update_finished"));
    if (!events[0] || !events[1])
    {
        DWORD err = GetLastError();
        p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_INIT_ERROR), _T("failed to open stop events"), err, FormatErrorMessage(err));
        return (err);
    }

    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_STARTED), source.c_str());
    if (!cc_updater.GetUpdateShell()->Start(_T("VBA32AAW")))
    {
        p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_START_ERROR));
    }
    DEBUG_Message1((_T("DoUpdate(): Start() success, waiting to complete")));

    // Waiting for update to complete
    init = WaitForMultipleObjects( 2,
                                   events,
                                   FALSE,
                                   INFINITE);
    CloseHandle(events[0]);
    CloseHandle(events[1]);
    DEBUG_Message1((_T("DoUpdate(): Wait() complete, result = %d"), init));

    return (0);
}
