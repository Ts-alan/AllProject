#include "stdafx.h"

#include <winsock2.h>
#include "task_sender.h"

#include "common/log.h"

VbaTaskSender::VbaTaskSender()
{
    OnAction = boost::bind(&VbaTaskSender::OnActionWork, this, _1);
}

VbaTaskSender::~VbaTaskSender()
{}

bool VbaTaskSender::_Initialize()
{
	m_action_q_init = Initialize();	

	mp_tasks_report = vba::AutoSingleton<ReportTasks>::Instance();
    mp_tasks_report->AddRef();	
	mp_tasks_report->Initialize();    
    return true;
}

bool VbaTaskSender::_Uninitialize()
{
	m_action_q_init = false;
	Uninitialize();


	mp_tasks_report->Uninitialize();
	mp_tasks_report->Release();

	mp_tasks_report->FreeInst();
	mp_tasks_report = NULL;

	return true;
}

void VbaTaskSender::OnActionWork(TaskParam task_param)
{
	if (!m_action_q_init)
	{
		mp_tasks_report->SaveTaskState(task_param.id, TASK_STATE_CANCELLED);
		return ;
	}

    TASK_STATE state = TASK_STATE_ERROR;
	VbaSocket socket;    	

    if (!socket.Initialize())
    {

        LOG_WARN() % L" socket.Initialize (fail)" % task_param.id; 
        mp_tasks_report->SaveTaskState(task_param.id, TASK_STATE_ERROR);
        return;
    }
    else if (!socket.ConnectSocket(task_param.address, task_param.port))
    {
        LOG_WARN() % L" socket.ConnectSocket (fail)" % task_param.id % " (" % task_param.address % " : " % task_param.port %") " ; 
        mp_tasks_report->SaveTaskState(task_param.id, TASK_STATE_ERROR);
        socket.Uninitialize();
        return;
    }
    else if (!socket.Send(std::tstring(task_param.packet.c_str(), task_param.packet.size())))
    {	
        LOG_WARN() % L" socket.Send (fail)" % task_param.id % " (" % task_param.address % " : " % task_param.port %") " ;
		mp_tasks_report->SaveTaskState(task_param.id, TASK_STATE_ERROR);
        socket.Uninitialize();
		return;
	}    
    else 
    {
        LOG() % L" Sent " % task_param.id % " (" % task_param.address % " : " % task_param.port %") (ok)"; 
        state = TASK_STATE_SENDED;
    }    

    mp_tasks_report->SaveTaskState(task_param.id, state);
    socket.Uninitialize();
	return ;
}

bool VbaTaskSender::AddTask(TaskParam &task_param)
{		
    return AddAction(task_param);
}