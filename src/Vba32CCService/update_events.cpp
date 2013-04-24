#include "stdafx.h"
#include "update_events.h"

#include "vba_update_module/vba_update_common.h"
#include "interfaces/VbaUpdateService.h"


VbaCC_UpdateEvents::VbaCC_UpdateEvents(): m_ref_count(0)
{
}

VbaCC_UpdateEvents::~VbaCC_UpdateEvents()
{
}

HRESULT VbaCC_UpdateEvents::QueryInterface(REFIID iid, void** ppvObject)
{   
	if (iid == IID__IVbaUpdateEvents)
    {
        *ppvObject = (void*)this;
    }
    else if (iid == IID_IUnknown)
    {
        *ppvObject = (void*)this;
    }
    else
    {
        return E_NOINTERFACE;
    }

    AddRef();
    return S_OK;
}

ULONG VbaCC_UpdateEvents::AddRef()
{
    InterlockedIncrement(&m_ref_count);
    return m_ref_count;
}

ULONG VbaCC_UpdateEvents::Release()
{
    if (InterlockedDecrement(&m_ref_count) == 0)
    {
        delete this;
        return 0; 
    } 
    return (m_ref_count);
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnUpdateStarted()
{
    // Updating Registry value - last successful update attempt time
    SYSTEMTIME now;
    SecureZeroMemory(&now, sizeof(now));
    GetLocalTime(&now);

    CAutoRegCloseKey key;
    if (RegCreateKeyEx( HKEY_LOCAL_MACHINE,
                        _T("SOFTWARE\\Vba32\\ControlCenter\\Update"),
                        0,
                        0,
                        REG_OPTION_NON_VOLATILE,
                        KEY_ALL_ACCESS,
                        0,
                        key.GetPtr(),
                        0) != ERROR_SUCCESS)
    {
        return (E_FAIL);
    }

    return 
        ((RegSetValueEx( key,
                         _T("LatestUpdateAttempt"),
                         0,
                         REG_BINARY,
                         (LPBYTE)&now,
                         sizeof(now)) == ERROR_SUCCESS) ? S_OK : E_FAIL);
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnUpdateStopped(void)
{
    CAutoCloseHandle stop_event = OpenEvent( EVENT_MODIFY_STATE,
                                             FALSE,
                                             _T("Vba32CC_update_finished"));
    if (stop_event.IsValid())
    {
        if (!SetEvent(stop_event))
        {
            DEBUG_Message1((_T("VbaCC_UpdateEvents::OnUpdateStopped(): failed to set finish event")));
        }
        else
        {
            DEBUG_Message1((_T("VbaCC_UpdateEvents::OnUpdateStopped(): finish event set successfully")));
        }
    }
    else
    {
        DEBUG_Message1((_T("VbaCC_UpdateEvents::OnUpdateStopped(): failed to set finish event")));
    }
	
    return S_OK;
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnUpdateFailed(long error_code, BSTR error_msg)
{
    UNREFERENCED_PARAMETER(error_code);
    UNREFERENCED_PARAMETER(error_msg);

    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_FAILED));
	
    return S_OK;
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnUpdateCompleted(void)
{
	p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_COMPLETE));

    // Updating Registry value - last successful update time
    SYSTEMTIME now;
    SecureZeroMemory(&now, sizeof(now));
    GetLocalTime(&now);

    CAutoRegCloseKey key;
    if (RegCreateKeyEx( HKEY_LOCAL_MACHINE,
                        _T("SOFTWARE\\Vba32\\ControlCenter\\Update"),
                        0,
                        0,
                        REG_OPTION_NON_VOLATILE,
                        KEY_ALL_ACCESS,
                        0,
                        key.GetPtr(),
                        0) != ERROR_SUCCESS)
    {
        return (E_FAIL);
    }

    return 
        ((RegSetValueEx( key,
                         _T("LatestUpdate"),
                         0,
                         REG_BINARY,
                         (LPBYTE)&now,
                         sizeof(now)) == ERROR_SUCCESS) ? S_OK : E_FAIL);
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnUpdateInitialized(unsigned long update_file_count, unsigned long total_size)
{
	p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_INITIALIZED), update_file_count, total_size);
	
    return S_OK;
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnUpdateMessage(BSTR update_msg)
{
	UNREFERENCED_PARAMETER(update_msg);

    return S_OK;
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnStartFileListDownload(void)
{
	p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_FILELIST_DOWNLOAD_STARTED));
	
    return S_OK;
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnFinishFileListDownload(void)
{	
	p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_FILELIST_RECIEVED));
	
    return S_OK;
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnFileListDownloadFailed(BSTR file_name, long error_code)
{
	UNREFERENCED_PARAMETER(file_name);
	p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_FILELIST_DOWNLOAD_FAILED), error_code);
	
    return S_OK;
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnStartFileDownload(BSTR file_name)
{
    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_FILE_DOWNLOAD_STARTED), vsBSTRToTString(file_name).c_str());
	
    return S_OK;
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnFinishFileDownload(BSTR file_name)
{
    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_FILE_RECIEVED), vsBSTRToTString(file_name).c_str());
	
    return S_OK;
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnFileDownloadFailed(BSTR file_name, BSTR error, long error_code)
{
	UNREFERENCED_PARAMETER(error_code);

    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_FILE_DOWNLOAD_ERROR), vsBSTRToTString(file_name).c_str(), vsBSTRToTString(error).c_str());
	
    return S_OK;
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnUpdateProgress(unsigned long current_size, unsigned long total_size)
{
	UNREFERENCED_PARAMETER(current_size);
    UNREFERENCED_PARAMETER(total_size);
    DEBUG_Message3((_T("VbaCC_UpdateEvents::OnUpdateProgress(): current_size = %d, total_size = %d"), current_size, total_size));

    return S_OK;
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnChangeStatus(long previous_status, long new_status)
{
	UNREFERENCED_PARAMETER(previous_status);
    UNREFERENCED_PARAMETER(new_status);
    DEBUG_Message1((_T("VbaCC_UpdateEvents::OnChangeStatus(): изменения статуса обновления, старый статус %d, новый статус %d"), previous_status, new_status));

    return S_OK;
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnGetFileUpdate(BSTR group_name, BSTR file_name, BOOL *p_update_needed)
{
	UNREFERENCED_PARAMETER(file_name);
    UNREFERENCED_PARAMETER(group_name);
    DEBUG_Message3((_T("VbaCC_UpdateEvents::OnGetFileUpdate(): group_name = %s, file_name = %s"), vsBSTRToTString(group_name).c_str(), vsBSTRToTString(file_name).c_str()));

    if (!p_update_needed)
    {
        return E_POINTER;
    }

    *p_update_needed = TRUE;
	
    return S_OK;
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnGetFileOperation(BSTR group_name, BSTR file_name, BSTR *p_str_param, unsigned long *p_num_param, long *p_operation_to_perform)
{
	UNREFERENCED_PARAMETER(group_name);

    if (!p_str_param || !p_num_param || !p_operation_to_perform)
    {
        return E_POINTER;
    }

    std::tstring tfile_name = vsBSTRToTString(file_name);
    std::tstring current_dir;
    vfGetProgramDir(current_dir);
	
    if (vsToUpper(tfile_name) == vsToUpper(current_dir + _T("Vba32CCService.exe")))
	{
		*p_operation_to_perform = OperationsOnClient | ReplaceWithServiceRestart;
		*p_str_param = SysAllocString(L"VbaControlCenter");
	}
    else if (vsToUpper(tfile_name) == vsToUpper(current_dir + _T("VbaTaskAssignment.exe")))
	{
		*p_operation_to_perform = ReplaceWithServiceRestart;
		*p_str_param = SysAllocString(L"VbaTaskAssignment");
	}
    else if (vsToUpper(tfile_name) == vsToUpper(current_dir + _T("Vba32PMS.exe")))
	{
		*p_operation_to_perform = ReplaceWithServiceRestart;
		*p_str_param = SysAllocString(L"Vba32PMS");
	}
    else if (vsToUpper(tfile_name) == vsToUpper(current_dir + _T("Vba32NS.exe")))
	{
		*p_operation_to_perform = ReplaceWithServiceRestart;
		*p_str_param = SysAllocString(L"Vba32NS");
	}
    else if (vsToUpper(tfile_name) == vsToUpper(current_dir + _T("Vba32SS.exe")))
	{
		*p_operation_to_perform = ReplaceWithServiceRestart;
		*p_str_param = SysAllocString(L"Vba32SS");
	}
    else
	{
		*p_operation_to_perform = ReplaceLockedFile;
	}
    *p_num_param = 0;

    return S_OK;
}   

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnFileOperationResult(BSTR group_name, BSTR file_name, long operation_result)
{
	UNREFERENCED_PARAMETER(group_name);
    UNREFERENCED_PARAMETER(file_name);
    UNREFERENCED_PARAMETER(operation_result);
    DEBUG_Message3((_T("VbaCC_UpdateEvents::OnGetFileUpdate(): group_name = %s, file_name = %s, operation_result = %d"), vsBSTRToTString(group_name).c_str(), vsBSTRToTString(file_name).c_str(), operation_result));

    return S_OK;
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnFileReplaceFailed(BSTR file_name)
{
    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_FILE_REPLACE_ERROR), vsBSTRToTString(file_name).c_str());

    return S_OK;
}

HRESULT STDMETHODCALLTYPE VbaCC_UpdateEvents::OnConnectionFailed(long connection_error)
{
    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_UPDATE_CONNECTION_ERROR), connection_error, FormatErrorMessage(connection_error));

    return S_OK;
}
