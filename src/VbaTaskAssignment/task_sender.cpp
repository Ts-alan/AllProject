#include "stdafx.h"

#include <winsock2.h>
#include "task_sender.h"

#include "common/log.h"

VbaTaskSender::VbaTaskSender() : mp_container(0)
{
}

VbaTaskSender::~VbaTaskSender()
{
}

bool VbaTaskSender::_Initialize()
{
	m_action_q_init = vba::deprecated::ActionQueue<TaskParam>::Initialize();
	
	mp_thread_pool = vba::AutoSingleton<SenderThreadPool>::Instance();
	mp_tasks_report = vba::AutoSingleton<ReportTasks>::Instance();

	mp_tasks_report->AddRef();	
	mp_tasks_report->Initialize();
    
    return true;
}

bool VbaTaskSender::_Uninitialize()
{
	m_action_q_init = false;
	vba::deprecated::ActionQueue<TaskParam>::Uninitialize();
	
	mp_thread_pool->FreeInst();
	mp_thread_pool = NULL;

	if (mp_container)
	{
		mp_container->StopThread();
		mp_container = NULL;
	}

	mp_tasks_report->Uninitialize();
	mp_tasks_report->Release();

	mp_tasks_report->FreeInst();
	mp_tasks_report = NULL;

	return true;
}

void VbaTaskSender::OnAction(TaskParam task_param)
{
	mp_container = NULL; 
	if (!m_action_q_init)
	{
		mp_tasks_report->SaveTaskState(task_param.id, TASK_STATE_CANCELLED);
		return ;
	}

	if (mp_thread_pool->getObjectThread(&mp_container))
	{		
		mp_container->m_address = task_param.address;
		mp_container->m_port = task_param.port;
		mp_container->m_packet = task_param.packet;
		mp_container->m_id_task = task_param.id;
		mp_container->StartThread();
	}
	return ;
}

bool VbaTaskSender::AddTask(TaskParam &task_param)
{		
    return AddAction(task_param);
}

unsigned VbaTaskSender::TaskContainer::ThreadFunction()
{
	typedef vba::AutoSingletonWrap<VbaTaskReporter> TaskRep;
	TaskRep *mp_tasks_report = vba::AutoSingleton<TaskRep>::Instance();

	VbaSocket socket;
	socket.Initialize();

    	
	if (!socket.ConnectSocket(m_address, m_port))
	{		
		mp_tasks_report->SaveTaskState(m_id_task, TASK_STATE_ERROR);
		mp_tasks_report->FreeInst();		
		LOG_WARN() % L" socket.ConnectSocket (fail)" % m_id_task; 
		
        socket.Uninitialize();
        this->Release();
		return 0;
	}
	
    bool sending_result = socket.Send(std::tstring(m_packet.c_str(), m_packet.size()));
	
    mp_tasks_report->SaveTaskState(m_id_task, (sending_result) ? TASK_STATE_SENDED : TASK_STATE_ERROR);
	
	mp_tasks_report->FreeInst();

    socket.Uninitialize();
	this->Release();

	return 0;		
}