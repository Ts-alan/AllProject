#include "stdafx.h"
#include "task_service.h"

#include "task_builder.h"

#include <list>
#include "common/convert.h"
#include "task_reporter.h"

#include "common/log.h"

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
	mp_tasks_report = vba::AutoSingleton<ReportTasks>::Instance();
	if ( (!p_task_ids) || (!p_ip_addresses) )
    {
        DLOG() % LOG_FUNC % L"{" %GetCurrentThreadId() % L"}" % L" Build task start ..."; 
        return false;
    }

	typedef std::list<INT64> IdsList;
	typedef std::list<DWORD> AddressesList;

	IdsList id_list;
	AddressesList addr_list;

    vba::conv::SafeArrayToContainer<INT64, IdsList, false>(*p_task_ids, id_list);
	vba::conv::SafeArrayToContainer<DWORD, AddressesList, false>(*p_ip_addresses, addr_list);

	if (addr_list.size()!=id_list.size())
	{
		return false;
	}


	AddressesList::iterator iter_addr = addr_list.begin();
	for(IdsList::iterator iter_id = id_list.begin(); iter_id!= id_list.end(); ++iter_id)
	{
        LOG() %  L" Build task " % *iter_id % "("% iter_addr %")"; 
        vba::utf8_string packet;
        bool res = false;
		switch (task_type)
		{
		case SystemInformation:
				res =  VbaTaskBuilder::BuildPacketSystemInfo(*iter_id, packet);
				break;

		case CreateProcess:
				if (!param1)
				{
					mp_tasks_report->SaveTaskState(*iter_id, TASK_STATE_ERROR);
					return false;
				}
                res = VbaTaskBuilder::BuildPacketCreateProcess(*iter_id, vba::conv::BSTRToString(param1), packet);
				break;

		case SendFile:
				if (!param1 || !param2)
				{
					mp_tasks_report->SaveTaskState(*iter_id, TASK_STATE_ERROR);
					return false;
				}
				res = VbaTaskBuilder::BuildPacketSendFile(*iter_id, vba::conv::BSTRToString(param1), vba::conv::BSTRToString(param2), packet);
				break;

		case ConfigureSettings:
				if (!param1)
				{
					mp_tasks_report->SaveTaskState(*iter_id, TASK_STATE_ERROR);
					return false;
				}
				res = VbaTaskBuilder::BuildPacketConfigureSettings(*iter_id, vba::conv::BSTRToString(param1), packet);
				break;

		case ComponentState:
				res = VbaTaskBuilder::BuildPacketComponentState(*iter_id, packet);
				break;

		case CancelTask:
				res = VbaTaskBuilder::BuildPacketCancelTask(*iter_id, packet);
				break;

		case ListProcesses:
				res = VbaTaskBuilder::BuildPacketListProcesses(*iter_id, packet);
				break;

		case CustomAction:
				if (!param1)
				{
					mp_tasks_report->SaveTaskState(*iter_id, TASK_STATE_ERROR);
					return false;
				}
				res = VbaTaskBuilder::BuildPacketCustomAction(*iter_id, vba::conv::BSTRToString(param1), packet);
				break;

		default:
                DLOG() % LOG_FUNC % L"{" %GetCurrentThreadId() % L"}" % L" Build task. Unknown task (fail)."; 
				return false;
		}


        if (!res)
        {
            LOG_WARN() % L"{" %GetCurrentThreadId() % L"}" % L" Build task. Crypt failed (fail)."; 
        }

		TaskParam task_param;
		task_param.address = *iter_addr;
		task_param.port = AGENT_PORT;
		task_param.packet = packet;
		task_param.id = *iter_id;

		if (mp_task_sender->AddTask(task_param))
		{
			mp_tasks_report->SaveTaskState(*iter_id, TASK_STATE_IN_QUEUE);
		}
		else
		{
			mp_tasks_report->SaveTaskState(*iter_id, TASK_STATE_ERROR);
		}
		++iter_addr;
	}
	mp_tasks_report->FreeInst();
	mp_tasks_report = NULL;

    DLOG() % LOG_FUNC % L"{" %GetCurrentThreadId() % L"}" % L" Build task finish (ok)"; 

}