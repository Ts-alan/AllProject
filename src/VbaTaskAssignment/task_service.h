#pragma once

#include "resource.h"       // main symbols

#include <atlsafe.h>

#include "vba_common/vba_strings.h"

#include "VbaTaskAssignment.h"
#include "task_sender.h"

#include "VbaComThreadAllocator.h"

#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

#define LIBRARY_MAJOR 1
#define LIBRARY_MINOR 0

#define AGENT_PORT 17002

// TaskService

class ATL_NO_VTABLE TaskService :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<TaskService, &CLSID_TaskService>,
	public ISupportErrorInfo,
	public IDispatchImpl<ITaskService, &IID_ITaskService, &LIBID_VbaTaskAssignmentLib, LIBRARY_MAJOR, LIBRARY_MINOR>
{
public:
    TaskService()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_TASKSERVICE)

DECLARE_NOT_AGGREGATABLE(TaskService)


BEGIN_COM_MAP(TaskService)
	COM_INTERFACE_ENTRY(ITaskService)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(ISupportErrorInfo)
END_COM_MAP()

// ISupportsErrorInfo
	STDMETHOD(InterfaceSupportsErrorInfo)(REFIID riid);

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
        mp_allocator = cc::VbaSingleton<VbaComThreadAllocator>::Instance();
        mp_allocator->Lock();

        m_task_sender.Initialize(AGENT_PORT);
        
        return S_OK;
	}

	void FinalRelease()
	{
        m_task_sender.Shutdown();

        mp_allocator->Free();
        cc::VbaSingleton<VbaComThreadAllocator>::FreeInst();
	}

    VbaComThreadAllocator* mp_allocator;

public:
    STDMETHOD(PacketSystemInfo)(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses);
    STDMETHOD(PacketCreateProcess)(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses, BSTR cmd_line);
    STDMETHOD(PacketSendFile)(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses, BSTR src_path, BSTR dst_path);
    STDMETHOD(PacketConfigureSettings)(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses, BSTR settings);
    STDMETHOD(PacketComponentState)(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses);
    STDMETHOD(PacketCustomAction)(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses, BSTR options);
    STDMETHOD(PacketCancelTask)(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses);
    STDMETHOD(PacketListProcesses)(SAFEARRAY** p_task_ids, SAFEARRAY** p_ip_addresses);

private:

    VbaTaskSender m_task_sender;

    typedef enum
    {
	    SystemInformation = 0,
	    CreateProcess,
	    SendFile,		            
	    ConfigureSettings,			
        ComponentState,
        CancelTask,
        ListProcesses,
        CustomAction
    } TaskType; 

    bool BuildTask(TaskType task_type, SAFEARRAY** p_task_ids, SAFEARRAY **p_ip_addresses, BSTR param1 = NULL, BSTR param2 = NULL, DWORD param3 = 0);
    void ManageTasks(const CComSafeArray<DWORD>& p_task_ids, std::vector<utf8_string>& packets);
    bool AddTask(const CComSafeArray<DWORD>& addresses, const std::vector<utf8_string>& packets);

};

OBJECT_ENTRY_AUTO(__uuidof(TaskService), TaskService)
