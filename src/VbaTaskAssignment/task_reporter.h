#pragma once

#define _WINSOCKAPI_ //отмена включения winsock.h в windows.h

#define CONTROL_CENTER_PORT 17001

#include "common/strings.h"
#include "common/patterns.h"
#include "common/critical_section.h"
#include "task_states.h"
#include "thread_pool.h"
#include "vba_socket.h"
#include <list>
#include "windows.h"
#include "common/log.h"

class VbaTaskReporter :	public vba::ActionQueue<std::wstring>
{
public:
	bool VbaTaskReporter::SaveTaskState(INT64 id_task, TASK_STATE task_state)
	{
		wchar_t date[32]={0};
		wchar_t str_state[512]={0};
		SYSTEMTIME st;
		GetLocalTime(&st);	
		
		swprintf(date, L"%04u-%02u-%02u %02u:%02u:%02u", st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond);
		return AddAction(vba::ToWString(vba::FormatString(L"<TaskState><ID>%1%</ID><State>%2%</State><Date>%3%</Date></TaskState>"
			) % static_cast<unsigned int>(id_task) % STATES[task_state] % date));		
	}
private:
	virtual void OnAction(std::wstring action_object)
	{
		VbaSocket socket;
		if (!socket.Initialize())
		{        
			LOG_WARN() % " OnAction failed initializing socket (fail)";
			return;
		}
		std::string service("127.0.0.1");
		if (!socket.ConnectSocket(inet_addr(service.c_str()), CONTROL_CENTER_PORT))
		{		
			socket.Uninitialize();
			LOG_WARN() % " Connection problems with"% service % ":" % CONTROL_CENTER_PORT % "(fail)";
			return;
		}
		std::wstring packet (L"<TaskStates>");
		packet.append(action_object);
		packet.append(L"</TaskStates>");
		if (!socket.Send(vba::conv::WStringToUtf8(packet).c_str(), true))
		{
			LOG_WARN()  % "Sending problems on 127.0.0.1:" % CONTROL_CENTER_PORT % "(fail)";
		}
		socket.Uninitialize();
	}
};