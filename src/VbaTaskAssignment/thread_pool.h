#pragma once

#include <atlutil.h>
#include <atlcoll.h>

#include "socket_pool.h"

typedef enum _TaskStatus {
	StatusError,		// задание в карте не найлено
	StatusNotStarted,	// задание есть в карте, но ещё не начато его выполнение
	StatusStarted,		// задание выполняется
	StatusDone			// задание завершено
} TaskStatus;

__interface IVbaTaskHandler
{
    virtual void ProcessTask() = 0;
};

__interface IVbaThreadPool
{
	virtual HRESULT		Initialize(unsigned long threads_number, void* p_param) = 0;
	virtual BOOL		QueueRequest(const DWORD_PTR& request, BOOL track_for_polling) = 0;
	virtual TaskStatus	PollTaskStatus(DWORD_PTR request, BOOL remove_if_done) = 0;
	virtual void		Shutdown(DWORD max_timeout) = 0;
};

template <class Worker>
class VbaThreadPool : public IVbaThreadPool
{
public:
    
    VbaThreadPool() : m_is_initialized(false)
    {
    }

    ~VbaThreadPool()
    {
    }

    virtual HRESULT Initialize(unsigned long threads_number, void* p_param)
    {
        HRESULT return_value = S_OK;
    
        if (!m_is_initialized)
        {
            return_value = m_thread_pool.Initialize(p_param, threads_number);
            m_is_initialized = SUCCEEDED(return_value);
        }

        return return_value;
    }

	virtual BOOL QueueRequest(const DWORD_PTR& request, BOOL track_for_polling)
    {
        BOOL retval = FALSE;

        m_critical_section.Lock();

        if (track_for_polling)
        {
            m_task_status.SetAt(request, StatusNotStarted);
        }

        retval = m_thread_pool.QueueRequest(request);

        if (track_for_polling && !retval)
        {
            m_task_status.RemoveKey(request);
        }

        m_critical_section.Unlock();

        return retval;
    }

	virtual TaskStatus	PollTaskStatus(DWORD_PTR request, BOOL remove_if_done)
    {
        TaskStatus task_status;

        bool retval = m_task_status.Lookup(request, task_status);
        
        if (!retval)
        {
            task_status	= StatusError;
        }

        return task_status;
    }

	virtual void Shutdown(DWORD max_timeout)
    {
         m_thread_pool.Shutdown(max_timeout);
    }

    void SetTaskStatus(DWORD_PTR request, TaskStatus status)
    {
	    m_critical_section.Lock();

        TaskStatus prev_status;
	
        if (m_task_status.Lookup(request, prev_status))
        {
		    // изменять только если уже существует
		    m_task_status.SetAt(request, status);
        }

	    m_critical_section.Unlock();
    }

    void AssignDeviceToPool(HANDLE device, DWORD completion_key)
    {
        CreateIoCompletionPort(device, m_thread_pool.GetQueueHandle(), completion_key, 0);
    }

protected:
    CThreadPool<Worker>            m_thread_pool;
	CComCriticalSection	           m_critical_section;
    CAtlMap<DWORD_PTR, TaskStatus> m_task_status;
	BOOL                           m_is_initialized;
};

class VbaTaskWorker
{
public:

    typedef DWORD_PTR RequestType;

    BOOL Initialize(void* /*p_param*/)
    {
        return TRUE;
    }

    void Execute(RequestType request, void* p_worker_param, OVERLAPPED* p_overlapped)
    {
        ATLENSURE(request != NULL);

        VbaThreadPool<VbaTaskWorker>* p_thread_pool = NULL;

        if (p_worker_param)
        {
            p_thread_pool =	reinterpret_cast<VbaThreadPool<VbaTaskWorker>*>(p_worker_param);
        }

        if (p_thread_pool)
        {
            p_thread_pool->SetTaskStatus(request, StatusStarted);
        }

        IVbaTaskHandler* p_handler	= reinterpret_cast<IVbaTaskHandler*>(request);

        p_handler->ProcessTask();

        if (p_thread_pool)
        {
            p_thread_pool->SetTaskStatus(request, StatusDone);
        }
    }
    
    void Terminate(void* /*p_param*/)
    {
        return;
    }
};

class VbaSocketWorker
{
public:
    
    typedef DWORD_PTR RequestType;

    BOOL Initialize(void* /*p_param*/)
    {
        return TRUE;
    }

    void Execute(RequestType request, void* p_worker_param, OVERLAPPED* p_overlapped)
    {
        // если организовывать контроль целостности переданного пакета, то
        // необходимо переписывать класс CThreadPool, так чтобы в Worker::Execute
        // передавалось и значение dwBytesTransfered, возвращаемое функцией
        // GetQueuedCompletionStatus

        ATLENSURE(request != NULL);

        SOCKET sck = static_cast<SOCKET>(request);
        
        VbaSocketPool* p_socket_pool = static_cast<VbaSocketPool*>(p_worker_param);
        
        if (p_socket_pool)
        {
            p_socket_pool->ReleaseSocket(sck);
        }
    }
    
    void Terminate(void* /*p_param*/)
    {
        return;
    }

};