#include "StdAfx.h"
#include "thread_pool.h"

//////////////////////////////
/// Thread Pool with requests

BOOL VbaTaskWorker::Initialize(void* p_param)
{
    return TRUE;
}

void VbaTaskWorker::Execute(VbaTaskWorker::RequestType request, void* p_worker_param, OVERLAPPED* p_overlapped)
{
    ATLENSURE(request != NULL);

    VbaThreadPool* p_thread_pool = NULL;
    
    if (p_worker_param)
    {
        p_thread_pool =	reinterpret_cast<VbaThreadPool*>(p_worker_param);
    }

    if (p_thread_pool)
    {
        p_thread_pool->SetTaskStatus(request, STATUS_STARTED);
    }

    IVbaTaskHandler* p_handler	= reinterpret_cast<IVbaTaskHandler*>(request);

    p_handler->ProcessTask();

    if (p_thread_pool)
    {
        p_thread_pool->SetTaskStatus(request, STATUS_DONE);
    }
}

void VbaTaskWorker::Terminate(void* p_param)
{
}

// class VbaThreadPoolWithRequests

VbaThreadPoolWithRequests::VbaThreadPoolWithRequests(void) : m_is_initialized(FALSE)
{
    m_critical_section.Init();
}

VbaThreadPoolWithRequests::~VbaThreadPoolWithRequests(void)
{
    m_thread_pool.Shutdown();
    m_critical_section.Term();
}

HRESULT VbaThreadPoolWithRequests::Initialize(unsigned long threads_number)
{
    IVbaThreadPool* p_thread_pool = static_cast<IVbaThreadPool*>(this);
    
    HRESULT return_value = S_OK;
    
    if (!m_is_initialized)
    {
        return_value = m_thread_pool.Initialize((LPVOID)p_thread_pool, threads_number);
        m_is_initialized = SUCCEEDED(return_value);
    }

    return return_value;
}

BOOL VbaThreadPoolWithRequests::QueueRequest(const VbaTaskWorker::RequestType& request, BOOL track_for_polling)
{
    BOOL retval = FALSE;

    m_critical_section.Lock();
    
    if (track_for_polling)
    {
        m_task_status.SetAt(request, STATUS_NOT_STARTED);
    }

    retval = m_thread_pool.QueueRequest(request);

    if (track_for_polling && !retval)
    {
        m_task_status.RemoveKey(request);
    }

    m_critical_section.Unlock();

    return retval;
}

TaskStatus VbaThreadPoolWithRequests::PollTaskStatus(VbaTaskWorker::RequestType request, BOOL remove_if_done)
{
    TaskStatus task_status;

    bool retval = m_task_status.Lookup(request, task_status);
    
    if (!retval)
    {
        task_status	= STATUS_ERROR;
    }

    return task_status;
}

void VbaThreadPoolWithRequests::Shutdown(DWORD max_timeout)
{
    m_thread_pool.Shutdown(max_timeout);
}

void VbaThreadPoolWithRequests::SetTaskStatus(VbaTaskWorker::RequestType request, TaskStatus status)
{
	m_critical_section.Lock();

    ATLASSERT((status == STATUS_STARTED) || (status == STATUS_DONE));
	
    TaskStatus prev_status;
	
    if (m_task_status.Lookup(request, prev_status))
    {
		// изменять только если уже существует
		m_task_status.SetAt(request, status);
    }

	m_critical_section.Unlock();
}

/////////////////////////////////////////
/// Thread Pool for working with sockets

void VbaThreadPool::AssignDeviceToPool(HANDLE device, DWORD completion_key)
{
    CreateIoCompletionPort(device, m_thread_pool.GetQueueHandle(), completion_key, 0);
}