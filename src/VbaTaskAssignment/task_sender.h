#pragma once

#include "common/strings.h"
#include "common/patterns.h"

#include "vba_socket.h"
#include "task_reporter.h"

#include <common/patterns/action_queue.hpp>


struct TaskParam
{
	unsigned long address;
	unsigned short port;
    vba::utf8_string packet;
	INT64 id;
};

class VbaTaskSender : public vba::ActionQueue<TaskParam, 4>
{
public:
    bool _Initialize();
	bool _Uninitialize();
    bool AddTask(TaskParam &task_param);

	void OnActionWork(TaskParam task_param);

    VbaTaskSender();
    ~VbaTaskSender();
    
	typedef vba::AutoSingletonWrap<VbaTaskReporter> ReportTasks;

	bool				m_action_q_init;
private:      
	ReportTasks*		mp_tasks_report;
};

