#pragma once

#include "common/strings.h"
#include "common/patterns.h"

//#include "socket_pool.h"
#include "thread_pool.h"
#include "vba_socket.h"
#include "task_reporter.h"

struct TaskParam
{
	unsigned long address;
	unsigned short port;
    vba::utf8_string packet;
	INT64 id;
};

class VbaTaskSender : public vba::ActionQueue<TaskParam>
{
protected:
    struct TaskContainer :	public vba::ThreadNonPaged
	{		
		unsigned long	m_address;
		unsigned short	m_port;
        vba::utf8_string		m_packet;
		INT64			m_id_task;

		virtual unsigned ThreadFunction();
	};

public:
    bool _Initialize();
	bool _Uninitialize();
    bool AddTask(TaskParam &task_param);

	virtual void OnAction(TaskParam task_param);

    VbaTaskSender();
    ~VbaTaskSender();

    typedef vba::AutoSingletonWrap<VbaThreadPool<TaskContainer> > SenderThreadPool;
	typedef vba::AutoSingletonWrap<VbaTaskReporter> ReportTasks;

	bool				m_action_q_init;
private:    
	
	SenderThreadPool*	mp_thread_pool;        
	ReportTasks*		mp_tasks_report;
    vba::CriticalSection	m_cs;	
	TaskContainer*		mp_container;
};

