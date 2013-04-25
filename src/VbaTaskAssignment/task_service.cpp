#include "stdafx.h"
#include "task_service.h"

#include "task_builder.h"

//#include "../dbg_msg/dbg_msg.h"
//#include <TCHAR.H>

// TaskService

STDMETHODIMP TaskService::InterfaceSupportsErrorInfo(REFIID riid)
{
	static const IID* arr[] = 
	{
		&IID_ITaskService
	};

	for (int i=0; i < sizeof(arr) / sizeof(arr[0]); i++)
	{
		if (InlineIsEqualGUID(*arr[i],riid))
			return S_OK;
	}
	return S_FALSE;
}

STDMETHODIMP TaskService::PacketSystemInfo(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses)
{ 
    return BuildTask(SystemInformation, p_task_ids, p_ip_addresses) ? S_OK : E_POINTER;
}

STDMETHODIMP TaskService::PacketCreateProcess(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses, BSTR cmd_line)
{ 
    return BuildTask(CreateProcess, p_task_ids, p_ip_addresses,cmd_line) ? S_OK : E_POINTER;
}

STDMETHODIMP TaskService::PacketSendFile(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses, BSTR src_path, BSTR dst_path)
{    
    return BuildTask(SendFile, p_task_ids, p_ip_addresses, src_path, dst_path) ? S_OK : E_POINTER;
}

STDMETHODIMP TaskService::PacketConfigureSettings(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses, BSTR settings)
{
    return BuildTask(ConfigureSettings, p_task_ids, p_ip_addresses,settings) ? S_OK : E_POINTER; 
}

STDMETHODIMP TaskService::PacketComponentState(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses)
{
    return BuildTask(ComponentState, p_task_ids, p_ip_addresses) ? S_OK : E_POINTER;
}

STDMETHODIMP TaskService::PacketCustomAction(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses, BSTR options)
{
    return BuildTask(CustomAction, p_task_ids, p_ip_addresses,options) ? S_OK : E_POINTER;
}

STDMETHODIMP TaskService::PacketCancelTask(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses)
{
    return BuildTask(CancelTask, p_task_ids, p_ip_addresses) ? S_OK : E_POINTER;
}

STDMETHODIMP TaskService::PacketListProcesses(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses)
{
    return BuildTask(ListProcesses, p_task_ids, p_ip_addresses) ? S_OK : E_POINTER;
}

////

bool TaskService::BuildTask(TaskType task_type, SAFEARRAY** p_task_ids, SAFEARRAY **p_ip_addresses, BSTR param1, BSTR param2, DWORD param3)
{
    if ( (!p_task_ids) || (!p_ip_addresses) )
    {
        return false;
    }

    CComSafeArray<INT64> task_ids(*p_task_ids);

    std::vector<utf8_string> packets(task_ids.GetCount(), "");

    switch (task_type)
    {
    case SystemInformation:
        for (ULONG i = 0; i < task_ids.GetCount(); ++i)
        {
            VbaTaskBuilder::BuildPacketSystemInfo(task_ids[static_cast<LONG>(i)], packets[i]);
        }

        break;
    
    case CreateProcess:
        if (!param1)
        {
            return false;
        }
        
        for (ULONG i = 0; i < task_ids.GetCount(); ++i)
        {
            VbaTaskBuilder::BuildPacketCreateProcess(task_ids[static_cast<LONG>(i)], vsBSTRToTString(param1), packets[i]);
        }
             
        break;
    
    case SendFile:
        if (!param1 || !param2)
        {
            return false;
        }

        for (ULONG i = 0; i < task_ids.GetCount(); ++i)
        {
            VbaTaskBuilder::BuildPacketSendFile(task_ids[static_cast<LONG>(i)], vsBSTRToTString(param1), vsBSTRToTString(param2), packets[i]);
        }

        break;

    case ConfigureSettings:
        if (!param1)
        {
            return false;
        }

        for (ULONG i = 0; i < task_ids.GetCount(); ++i)
        {
            VbaTaskBuilder::BuildPacketConfigureSettings(task_ids[static_cast<LONG>(i)], vsBSTRToTString(param1), packets[i]);
        }
        
        break;

    case ComponentState:
        for (ULONG i = 0; i < task_ids.GetCount(); ++i)
        {
            VbaTaskBuilder::BuildPacketComponentState(task_ids[static_cast<LONG>(i)], packets[i]);
        }

        break;

    case CancelTask:
        for (ULONG i = 0; i < task_ids.GetCount(); ++i)
        {
            VbaTaskBuilder::BuildPacketCancelTask(task_ids[static_cast<LONG>(i)], packets[i]);
        }

        break;

    case ListProcesses:
        for (ULONG i = 0; i < task_ids.GetCount(); ++i)
        {
            VbaTaskBuilder::BuildPacketListProcesses(task_ids[static_cast<LONG>(i)], packets[i]);
        }

        break;

    case CustomAction:
        if (!param1)
        {
            return false;
        }

        for (ULONG i = 0; i < task_ids.GetCount(); ++i)
        {
            VbaTaskBuilder::BuildPacketCustomAction( task_ids[static_cast<LONG>(i)],
                vsBSTRToTString(param1), packets[i] );
        }

        break;

    default:
        return false;
    }

    return AddTask(CComSafeArray<DWORD>(*p_ip_addresses), packets);
}

bool TaskService::AddTask(const CComSafeArray<DWORD>& addresses, const std::vector<utf8_string>& packets)
{
	DEBUG_Message3((_T(" TaskService::AddTask Start Thread:%d\n"), GetCurrentThreadId()));

    if (addresses.GetCount() != packets.size())
    {
        return false;
    }

    DWORD* p_thread_id = new DWORD [packets.size()];
    HANDLE* p_thread = new HANDLE [packets.size()]; 

    for (ULONG i = 0; i < addresses.GetCount(); ++i)
    {
        DEBUG_Message3((_T(" TaskService::AddTask for i = %d Start\n"),i));

        void* p_data = HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY, sizeof(TaskContainer));
        
        if (!p_data)  
        {
            DEBUG_Message3((_T(" TaskService::AddTask HeapAlloc  ERROR\n")));
            break;
        }

        reinterpret_cast<TaskContainer*>(p_data)->m_address = addresses[static_cast<LONG>(i)];
        reinterpret_cast<TaskContainer*>(p_data)->m_packet = packets[i];
        reinterpret_cast<TaskContainer*>(p_data)->mp_vba_task_sender = &m_task_sender;

        // p_data очищается внутри потока

        p_thread[i] = CreateThread(NULL, 0, m_task_sender.AddThreadTask, p_data, 0, &p_thread_id[i]);   

        if (!p_thread[i]) 
        {
            DEBUG_Message3((_T(" TaskService::AddTask CreateThread  ERROR\n")));
            break;
        }
    }

    WaitForMultipleObjects(addresses.GetCount(), p_thread, TRUE, INFINITE);

    for (ULONG i = 0; i < addresses.GetCount(); ++i)
    {
        CloseHandle(p_thread[i]);
    }

    delete p_thread_id;
    delete p_thread;
    
    DEBUG_Message3((_T(" TaskService::AddTask END Thread:%d\n"), GetCurrentThreadId()));

    return true;
}