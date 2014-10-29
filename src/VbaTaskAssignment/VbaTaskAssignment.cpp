// VbaTaskAssignment.cpp : Implementation of WinMain

#include "stdafx.h"
#include "resource.h"
#include "VbaTaskAssignment.h"
#include <stdio.h>

#include "VbaAtlAutoThreadModuleT.h"


#include "task_sender.h"
#include "common/log.h"


#define THREADPOOL_SIZE	20
#define THREADPOOL_SIZE_COM 20


template <class ThreadAllocator = VbaComThreadAllocator, DWORD dwWait = INFINITE>
class CVbaTaskAssignmentModule : public CAtlServiceModuleT< CVbaTaskAssignmentModule<>, IDS_SERVICENAME >,
                                 public VbaAtlAutoThreadModuleT<CVbaTaskAssignmentModule<>, ThreadAllocator, dwWait>
{   
private:
    typedef vba::AutoSingletonWrap<VbaTaskSender> TaskSender;
	TaskSender* mp_task_sender;

public:
	DECLARE_LIBID(LIBID_VbaTaskAssignmentLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_VBATASKASSIGNMENT, "{335CFE6E-7408-466C-9A76-672479542D81}")

	




	HRESULT Run(int nShowCmd) throw()
	{
        LOG_FILE_ENABLE_NAME(L"log_cc_task_assignment.txt");
        LOG() % L"-----------";
        LOG() % L"Run service";
        LOG_DEBUG_VIEW_ENABLE();
        DLOG() % LOG_FUNC % L" Enable log.";
		mp_task_sender = vba::AutoSingleton<TaskSender>::Instance();
        mp_task_sender->_Initialize();		
		return __super::Run(nShowCmd);
	}

    void OnShutdown()
    {
        LOG() % L"Stop service on shutdown";
        mp_task_sender->m_action_q_init = false;
		mp_task_sender->_Uninitialize();
		return __super::OnStop();
    }

	void OnStop()
	{
        LOG() % L"Stop service";
		mp_task_sender->m_action_q_init = false;
		mp_task_sender->_Uninitialize();
		return __super::OnStop();
	}


    HRESULT InitializeSecurity() throw()
	{
        return CoInitializeSecurity(NULL, -1, NULL, NULL, RPC_C_AUTHN_LEVEL_DEFAULT, RPC_C_IMP_LEVEL_IMPERSONATE, NULL, EOAC_NONE, NULL);
	}

    bool ParseCommandLine(LPCTSTR lpCmdLine, HRESULT* pnRetCode);

    //We will change launch permissions here
    HRESULT RegisterAppId(bool is_server = false)
    {
        HRESULT hr = CAtlServiceModuleT< CVbaTaskAssignmentModule, IDS_SERVICENAME >::RegisterAppId(is_server);
        
        if (SUCCEEDED(hr))
        {
            // Open registry key and set launch permission
            CRegKey reg_key;
            LONG last_error = reg_key.Open(HKEY_CLASSES_ROOT, _T("AppID"), KEY_WRITE);
            
            if (last_error != ERROR_SUCCESS)
            {
                return AtlHresultFromWin32(last_error);
            }

            CRegKey app_id_reg_key;
            last_error = app_id_reg_key.Open(reg_key, GetAppIdT());
            
            if (last_error != ERROR_SUCCESS)
            {
                return AtlHresultFromWin32(last_error);
            }

            hr = SetLaunchPermission(app_id_reg_key);
        }

        return hr;
    }

    static DWORD GetDefaultThreads()
    {
        return THREADPOOL_SIZE_COM;
    }
};

// Переопределяем метод ParseCommandLine, чтобы обрабатывать Install/Uninstall
// вместо Service/UnregServer
bool CVbaTaskAssignmentModule<>::ParseCommandLine(LPCTSTR lpCmdLine, HRESULT* pnRetCode)
{
    *pnRetCode = S_OK;

    TCHAR p_delimeters[] = _T("-/");
    LPCTSTR p_token = FindOneOf(lpCmdLine, p_delimeters);
	
    while (p_token)
    {
        if (WordCmpI(p_token, _T("Uninstall")) == 0)
        {
	        *pnRetCode = this->UnregisterServer(TRUE);

	        if (SUCCEEDED(*pnRetCode))
            {
		        *pnRetCode = this->UnregisterAppId();
            }

	        return false;
        }
		
        if (WordCmpI(p_token, _T("Install")) == 0)
        {
	        *pnRetCode = this->RegisterAppId(true);

	        if (SUCCEEDED(*pnRetCode))
            {
		        *pnRetCode = this->RegisterServer(TRUE);
            }

	        return false;
        }

        p_token = FindOneOf(p_token, p_delimeters);
    }

    return true;
}

////////////////////////////////////////////////////////////////////////////////////////
///We will change launch permissions here
////////////////////////////////////////////////////////////////////////////////////////

//We have to change launch permissions
#ifndef COM_RIGHTS_EXECUTE
#define COM_RIGHTS_EXECUTE 0x1
#endif

BOOL ConstructWellKnownSID(LPCTSTR principal, PSID *p_sid)
{

    BOOL return_value = FALSE;
    PSID p_temp_sid = NULL;
    BOOL use_world_auth = FALSE;
    
    SID_IDENTIFIER_AUTHORITY sid_authority_nt = SECURITY_NT_AUTHORITY;
    SID_IDENTIFIER_AUTHORITY sid_authority_world = SECURITY_WORLD_SID_AUTHORITY;

    DWORD sub_authority;

    // Look for well-known English names
    if (_tcsicmp (principal, _T("administrators")) == 0) 
    {
        sub_authority = DOMAIN_ALIAS_RID_ADMINS;
    } 
    else if (_tcsicmp (principal, _T("system")) == 0) 
    {
        sub_authority = SECURITY_LOCAL_SYSTEM_RID;
    } 
    else if (_tcsicmp (principal, _T("interactive")) == 0) 
    {
        sub_authority = SECURITY_INTERACTIVE_RID;
    } 
    else if (_tcsicmp (principal, _T("networkservice")) == 0)
    {
        sub_authority = SECURITY_NETWORK_SERVICE_RID;
    }
    else 
    {
        return FALSE;
    }

    if(sub_authority == DOMAIN_ALIAS_RID_ADMINS || sub_authority == DOMAIN_ALIAS_RID_POWER_USERS)
    {
        if(!AllocateAndInitializeSid (&sid_authority_nt, 2, SECURITY_BUILTIN_DOMAIN_RID, sub_authority,
            0, 0, 0, 0, 0, 0, &p_temp_sid))
        {
            return FALSE;
        }
    }
    else
    {
        if(!AllocateAndInitializeSid (use_world_auth ? &sid_authority_world : &sid_authority_nt, 
            1, sub_authority, 0, 0, 0, 0, 0, 0, 0, &p_temp_sid )) return FALSE;

    }

    if (IsValidSid(p_temp_sid))
    {
        DWORD sid_bytes_count = GetLengthSid(p_temp_sid);
        *p_sid = (PSID)LocalAlloc(LPTR, sid_bytes_count);
        
        if (p_sid)
        {
            if (!CopySid(sid_bytes_count, *p_sid, p_temp_sid))
            {
                LocalFree(*p_sid);
                *p_sid = NULL;
            }
            else
            {
                return_value = TRUE;
            }
        }
        
        FreeSid(p_temp_sid);
    }

    return return_value;
}

// Get the SID for the given principal
DWORD GetAccountSID(LPCTSTR account_name, PSID* p_sid)
{
    if (ConstructWellKnownSID(account_name, p_sid))
    {
        return ERROR_SUCCESS;
    }

    DWORD sid_size = 0;
    TCHAR domain_name[256] = {0};
    DWORD domain_name_size = 255;
    SID_NAME_USE snu;

    LookupAccountName( NULL, account_name, *p_sid, &sid_size, domain_name, &domain_name_size, &snu );
    DWORD last_error = GetLastError();
    
    if (last_error != ERROR_INSUFFICIENT_BUFFER)
    {
        return last_error;
    }

    *p_sid = (PSID)LocalAlloc(LPTR, sid_size);
    domain_name_size = 255;
    
    if (!LookupAccountName( NULL, account_name, *p_sid, &sid_size, domain_name, &domain_name_size, &snu))
    {
        LocalFree(*p_sid);
        return GetLastError();
    }

    return ERROR_SUCCESS;
}

DWORD CopyACL(PACL p_old_acl, PACL p_new_acl)
{
    ACL_SIZE_INFORMATION acl_size_info;
    LPVOID p_ace = NULL;
    ACE_HEADER *pAceHeader = NULL;
    if( !GetAclInformation (p_old_acl, (LPVOID)&acl_size_info, (DWORD)sizeof(acl_size_info), AclSizeInformation) )
    {
        return GetLastError();
    }
    // Copy all ACE in the new ACL
    for( DWORD i = 0; i < acl_size_info.AceCount; i++ )
    {
        // Get the ACE and header info
        if( !GetAce(p_old_acl, i, &p_ace) )
        {
            return GetLastError();
        }
        pAceHeader = (ACE_HEADER*)p_ace;
        // Add the ACE to the new list
        if( !AddAce (p_new_acl, ACL_REVISION, MAXDWORD, p_ace, pAceHeader->AceSize) )
        {
            return GetLastError();
        }
    }
    return ERROR_SUCCESS;
}

// Add the given principal to the given ACL
DWORD AddAccessAllowedACEToACL(PACL* pp_acl, DWORD access_mask, LPCTSTR account_name)
{
    ACL_SIZE_INFORMATION  acl_size_info;
    PACL p_old_acl, p_new_acl;
    p_old_acl = *pp_acl;

    PSID p_account_sid = NULL;
    DWORD last_error = GetAccountSID( account_name, &p_account_sid );
    if( last_error != ERROR_SUCCESS )
    {
        return last_error;
    }

    GetAclInformation( p_old_acl, (LPVOID)&acl_size_info, (DWORD)sizeof(ACL_SIZE_INFORMATION), AclSizeInformation );
    DWORD acl_size = acl_size_info.AclBytesInUse +
                    sizeof(ACL) + sizeof(ACCESS_ALLOWED_ACE) +
                    GetLengthSid(p_account_sid) - sizeof(DWORD);

    p_new_acl = (PACL)new BYTE [acl_size];
    if( !InitializeAcl (p_new_acl, acl_size, ACL_REVISION) )
    {
        LocalFree(p_account_sid);
        return GetLastError();
    }

    last_error = CopyACL( p_old_acl, p_new_acl );
    if( last_error != ERROR_SUCCESS )
    {
        LocalFree(p_account_sid);
        return last_error;
    }

    if( !AddAccessAllowedAce (p_new_acl, ACL_REVISION2, access_mask, p_account_sid) )
    {
        LocalFree(p_account_sid);
        return GetLastError();
    }

    *pp_acl = p_new_acl;
    LocalFree(p_account_sid);

    return ERROR_SUCCESS;
}

DWORD CreateNewSD(SECURITY_DESCRIPTOR** pp_security_desc)
{
    PACL p_acl = NULL;
    DWORD sid_bytes_count = 0;
    PSID p_sid = NULL;
    PSID p_group_sid = NULL;
    PSID p_owner_sid = NULL;
    DWORD return_value = ERROR_SUCCESS;
    SID_IDENTIFIER_AUTHORITY system_sid_authority = SECURITY_NT_AUTHORITY;

    if(!pp_security_desc)
    {
        return ERROR_BAD_ARGUMENTS;
    }
    
    *pp_security_desc = NULL;

    //Create a SID for the owner (BUILTIN\Administrators) & the owner's group
    if (!AllocateAndInitializeSid ( &system_sid_authority, 2, SECURITY_BUILTIN_DOMAIN_RID, 
        DOMAIN_ALIAS_RID_ADMINS, 0, 0, 0, 0, 0, 0, &p_sid))
    {
        if(p_sid)
        {
            FreeSid(p_sid);
        }
        return GetLastError();
    }
    
    sid_bytes_count = GetLengthSid (p_sid);

    *pp_security_desc = (SECURITY_DESCRIPTOR *) LocalAlloc (LPTR, sizeof (ACL) +  (2 * sid_bytes_count) + 
        sizeof (SECURITY_DESCRIPTOR));

    if(!*pp_security_desc)
    {
        if(p_sid)
        {
            FreeSid(p_sid);
        }
        return ERROR_OUTOFMEMORY;
    }

    p_group_sid = (SID *) (*pp_security_desc + 1);
    p_owner_sid = (SID *) (((BYTE *) p_group_sid) + sid_bytes_count);
    p_acl = (ACL *) (((BYTE *) p_owner_sid) + sid_bytes_count);

    if (!InitializeSecurityDescriptor (*pp_security_desc, SECURITY_DESCRIPTOR_REVISION))
    {
        if(*pp_security_desc)
        {
            LocalFree (*pp_security_desc);
        }
        if(p_sid)
        {
            FreeSid(p_sid);
        }
        return GetLastError();
    }

    if (!InitializeAcl (p_acl, sizeof (ACL) + sizeof(ACCESS_ALLOWED_ACE) + sid_bytes_count, ACL_REVISION2))
    {
        if(*pp_security_desc)
        {
            LocalFree (*pp_security_desc);
        }
        if(p_sid)
        {
            FreeSid(p_sid);
        }
        return GetLastError();
    }

    if (!SetSecurityDescriptorDacl (*pp_security_desc, TRUE, p_acl, FALSE))
    {
        if(*pp_security_desc)
        {
            LocalFree (*pp_security_desc);
        }
        if(p_sid)
        {
            FreeSid(p_sid);
        }
        return GetLastError();
    }

    memcpy (p_group_sid, p_sid, sid_bytes_count);
    if (!SetSecurityDescriptorGroup (*pp_security_desc, p_group_sid, FALSE))
    {
        if(*pp_security_desc)
        {
            LocalFree (*pp_security_desc);
        }
        if(p_sid)
        {
            FreeSid(p_sid);
        }
        return GetLastError();
    }

    memcpy (p_owner_sid, p_sid, sid_bytes_count);
    if (!SetSecurityDescriptorOwner (*pp_security_desc, p_owner_sid, FALSE))
    {
        if(*pp_security_desc)
        {
            LocalFree (*pp_security_desc);
        }
        if(p_sid)
        {
            FreeSid(p_sid);
        }
        return GetLastError();
    }

    if(return_value != ERROR_SUCCESS)
    {
        if(*pp_security_desc)
        {
            LocalFree (*pp_security_desc);
        }
    }
    if(p_sid)
    {
        FreeSid(p_sid);
    }

    return return_value;
}

HRESULT SetLaunchPermission(CRegKey app_id_reg_key)
{
    HRESULT hr = S_OK;
    DWORD last_error = ERROR_SUCCESS;

    SECURITY_DESCRIPTOR* p_new_sd = NULL;
    last_error = CreateNewSD (&p_new_sd);
    if ( last_error != ERROR_SUCCESS )
    {
        hr = AtlHresultFromWin32(GetLastError());
        return hr;
    }

    PACL p_new_acl = NULL;    
    BOOL is_present         = FALSE;
    BOOL dacl_is_default     = FALSE;
    if (!GetSecurityDescriptorDacl (p_new_sd, &is_present, &p_new_acl, &dacl_is_default))
    {
        hr = AtlHresultFromWin32(GetLastError());
        LocalFree (p_new_sd);
        return hr;
    }

    // Allow SYSTEM, INTERACTIVE, Administrators and ASPNET (IIS 5.1) or(and) Network Service (IIS 6.0)
    AddAccessAllowedACEToACL (&p_new_acl, COM_RIGHTS_EXECUTE, _T("administrators"));
    AddAccessAllowedACEToACL (&p_new_acl, COM_RIGHTS_EXECUTE, _T("system"));
    AddAccessAllowedACEToACL (&p_new_acl, COM_RIGHTS_EXECUTE, _T("interactive"));
    AddAccessAllowedACEToACL (&p_new_acl, COM_RIGHTS_EXECUTE, _T("ASPNET"));
    AddAccessAllowedACEToACL (&p_new_acl, COM_RIGHTS_EXECUTE, _T("networkservice"));
    
    PSECURITY_DESCRIPTOR p_relative_sd = NULL;
    DWORD new_sd_bytes_count = 0;

    // Set the discretionary ACL on the security descriptor
    if ( !SetSecurityDescriptorDacl(p_new_sd, TRUE, p_new_acl, FALSE) )
    {
        delete [] p_new_acl;
        LocalFree (p_new_sd);
        hr = AtlHresultFromWin32(GetLastError());
        return hr;
    }

    // This call HAS to fail the first time giving the size of the SD...
    if (!MakeSelfRelativeSD(p_new_sd, p_relative_sd, &new_sd_bytes_count))
    {
        last_error = GetLastError();
        if ( last_error != ERROR_INSUFFICIENT_BUFFER )
        {
            delete [] p_new_acl;
            LocalFree (p_new_sd);
            hr = AtlHresultFromWin32(GetLastError());
            return hr;
        }
    }

    // Allocate the SD which will be in self relative form
    p_relative_sd = (PSECURITY_DESCRIPTOR)LocalAlloc(LPTR, new_sd_bytes_count);
    if( !p_relative_sd )
    {
        delete [] p_new_acl;
        hr = E_OUTOFMEMORY;
        LocalFree (p_new_sd);
        return hr;
    }

    // Store the Security Descriptor with no permissions for anyone in the new SD
    // which is in self relative format...
    if ( !MakeSelfRelativeSD(p_new_sd, p_relative_sd, &new_sd_bytes_count) )
    {
        LocalFree(p_relative_sd);
        LocalFree (p_new_sd);
        delete [] p_new_acl;
        hr = AtlHresultFromWin32(GetLastError());
        return hr;
    }

    // Set the regkey which refuses LaunchPermission to everyone...
    last_error = app_id_reg_key.SetValue(_T("LaunchPermission"), REG_BINARY, (PBYTE)p_relative_sd, new_sd_bytes_count);
    if (last_error != ERROR_SUCCESS)
    {
        hr = AtlHresultFromWin32(GetLastError());
    }
    LocalFree(p_relative_sd);
    LocalFree (p_new_sd);
    delete [] p_new_acl;
    return hr;    
}


CVbaTaskAssignmentModule<> _AtlModule;

//
extern "C" int WINAPI _tWinMain(HINSTANCE /*hInstance*/, HINSTANCE /*hPrevInstance*/, 
                                LPTSTR /*lpCmdLine*/, int nShowCmd)
{
    return _AtlModule.WinMain(nShowCmd);
}