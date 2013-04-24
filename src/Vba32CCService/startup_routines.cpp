//////////////////////////////////////////////////
//	Implementation for service startup routines	//
//	(c) 2008 VirusBlokAda Ltd.				    //
//////////////////////////////////////////////////

#include "stdafx.h"
#include "startup_routines.h"

// Keyfile library (wrapper)
#include "keyfile_wrapper/keyfile_wrapper.h"
#include "keyfile_wrapper/common.h"

// Update library
#include "libupdate/vbaupdate.h"

//////////////////////////////////////////////////////////////////

struct KEY_INFO
{
    DWORD key_state;
    UINT computer_limit;
    SYSTEMTIME expiration_date;
    UINT license_number;
    TCHAR customer_name[128];
    KEY_INFO(): key_state(0), computer_limit(0), license_number(0)
    {
        SecureZeroMemory(&expiration_date, sizeof(expiration_date));
        SecureZeroMemory(&customer_name, 128 * sizeof(TCHAR));
    }
};


BOOL CheckKeyFileValidity(BOOL write_to_log)
{
    // Data for web console
    KEY_INFO key_info;

    BOOL was_valid = keyfile.IsValid();

    keyfile.LoadKey(_T("VBA32AAW"));
	// Checking key file validity
    key_info.key_state = keyfile.CheckKey();
    if (key_info.key_state == VBA_KEY_ERROR_SUCCESS)
    {
        key_info.license_number = keyfile.GetLicenseNumber();
        keyfile.GetExpirationDate(&key_info.expiration_date);
        keyfile.GetCustomerName(key_info.customer_name, 128);
        key_info.computer_limit = keyfile.GetComputerLicenseLimit();
        if (!was_valid || write_to_log)
        {
		    // Key file now valid, logging license info
            p_logfile->AddToLog( LoadStringFromResource(IDS_LOG_KEY_LICENSE),
                                 key_info.license_number,
                                 key_info.expiration_date.wYear,
                                 key_info.expiration_date.wMonth, 
                                 key_info.expiration_date.wDay);
            p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_KEY_USER), key_info.customer_name);
        }
	}
	else
    {
        // Logging key error
        TCHAR error_reason[128];
        switch (key_info.key_state)
        {
            case VBA_KEY_ERROR_NOT_FOUND:
                _tcscpy(error_reason, LoadStringFromResource(IDS_LOG_KEY_ERROR_NOT_FOUND));
                break;
            case VBA_KEY_ERROR_BAD_KEY:
                _tcscpy(error_reason, LoadStringFromResource(IDS_LOG_KEY_ERROR_BAD_KEY));
                break;
            case VBA_KEY_ERROR_NO_SECTION:
                _tcscpy(error_reason, LoadStringFromResource(IDS_LOG_KEY_ERROR_NO_SECTION));
                break;
            case VBA_KEY_ERROR_EXPIRED:
                _tcscpy(error_reason, LoadStringFromResource(IDS_LOG_KEY_ERROR_EXPIRED));
                break;
            case VBA_KEY_ERROR_INVALID_TIME:
                _tcscpy(error_reason, LoadStringFromResource(IDS_LOG_KEY_ERROR_INVALID_TIME));
                break;
            case VBA_KEY_ERROR_ROLLED_BACK_TIME:
                _tcscpy(error_reason, LoadStringFromResource(IDS_LOG_KEY_ERROR_ROLLED_BACK_TIME));
                break;                
        }
        // Logging only if recently keyfile was valid
        if (was_valid || write_to_log)
        {
    	    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_KEY_ERROR), error_reason);
        }
	}

    // Signalling key state to web interface
    LONG ret = 0;
    CAutoRegCloseKey key;
    ret = RegCreateKeyEx( HKEY_LOCAL_MACHINE,
                          _T("SOFTWARE\\Vba32\\ControlCenter\\Signature"),
                          0,
                          0,
                          REG_OPTION_NON_VOLATILE,
                          KEY_READ | KEY_WRITE,
                          0,
                          key.GetPtr(),
                          0);
    if (ret == ERROR_SUCCESS)
    {
        ret = RegSetValueEx( key,
                             _T("AuthInfo"),
                             0,
                             REG_BINARY,
                             (LPBYTE)&key_info,
                             sizeof(key_info));
        if (ret != ERROR_SUCCESS)
        {
            DEBUG_Message1((_T("CheckKeyFileValidity(): Error setting value, error code %d (%s)"), ret, FormatErrorMessage(ret)));
        }
    }
    else
    {
        DEBUG_Message1((_T("CheckKeyFileValidity(): Error opening registry key, error code %d (%s)"), ret, FormatErrorMessage(ret)));
    }

    return (key_info.key_state == VBA_KEY_ERROR_SUCCESS);
}

BOOL CheckProgramIntegrity()
{
    vbaUpdate int_checker;
    std::tstring program_dir_t;
    vfGetProgramDir(program_dir_t);
    // vbaUpdate is ANSI shit, so conversion needed
    std::string program_dir = vsTStringToString(program_dir_t);
    int_checker.SetVariable("WEBCONSOLE", (program_dir + "WebConsole").c_str());

    LONG corrupted_files = 0;
    bool ini_ok = int_checker.FindCorruptedFiles("VBA32AAW", program_dir.c_str(), corrupted_files);
	
    if (!ini_ok)
    {
        p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_INTEGRITY_FAILED));
        return FALSE;
    }

    // To prevent rolling system time back
	time_t tCurrentVersion = int_checker.GetCurrentVersionDate();
    keyfile.SetMostRecentMoment(tCurrentVersion);

    for (LONG i = 0; i < corrupted_files; i++)
    {
        LPCSTR p_corrupted_filename;
        LPCSTR p_group_name;
        bool dummy_flag;
        LONG file_size;
        int_checker.GetFileInfo(i, p_group_name, p_corrupted_filename, file_size, dummy_flag);
        // Проверка установленности того или иного компонента

        TCHAR file_name[MAX_PATH];
        TcharFromANSI(p_corrupted_filename, file_name, sizeof(file_name));
        p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_INTEGRITY_CORRUPTED_FILE), file_name);
    }
    
    if (corrupted_files)
    {
        return FALSE;
    }

    // Запись в реестр установленных компонент
/*
    // setup и добавление компонент запускается только когда все хорошо
    if ()
    {
        // Загрузка и выполнение setup.dll
    }
*/
    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_INTEGRITY_OK));

    return TRUE;
}
