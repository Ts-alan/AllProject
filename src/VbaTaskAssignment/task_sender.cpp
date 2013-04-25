#include "stdafx.h"

#include <winsock2.h>

#include "task_sender.h"

//#include "../dbg_msg/dbg_msg.h"
//#include <TCHAR.H>

int SendPacket(SOCKET sck, const utf8_string& packet)
{
    WSABUF buffer;
    buffer.len = packet.size();
    buffer.buf = const_cast<CHAR*>(packet.c_str());

    DWORD number_of_bytes;
    
    WSAOVERLAPPED overlapped;
    ZeroMemory(&overlapped, sizeof(WSAOVERLAPPED));

    DWORD flags = 0;

	DEBUG_Message3((_T("WSASend start\n")));

    if (WSASend(sck, &buffer, 1, &number_of_bytes, flags, &overlapped, NULL) == SOCKET_ERROR)
    {
		DEBUG_Message3((_T("WSASend failed\n")));
        return WSAGetLastError();
    }

	DEBUG_Message3((_T("WSASend success\n")));

    return 0;
}

VbaTaskSender::VbaTaskSender() : m_port(0), m_socket_pool(SOCKET_POOL_SIZE)
{
}

VbaTaskSender::~VbaTaskSender()
{
}

bool VbaTaskSender::Initialize(unsigned short port)
{
    WSADATA wsaData;
    int res = WSAStartup(MAKEWORD(2,2), &wsaData);

    m_port = port;

    m_thread_pool.Initialize(THREADPOOL_SIZE, (void*)&m_socket_pool);

    return true;
}

bool VbaTaskSender::Shutdown()
{
    m_thread_pool.Shutdown(1000);
    m_socket_pool.Shutdown();

    WSACleanup();

    return true;
}

bool VbaTaskSender::AddTask(unsigned long address, const utf8_string& packet)
{
    DEBUG_Message3((_T("VbaTaskSender::AddTask\n")));

    WaitForSingleObject(m_socket_pool.m_free_sockets_event, INFINITE);
        
    SOCKET sck = m_socket_pool.GetSocket(address, m_port); 

    if (sck == INVALID_SOCKET)
    {
        DEBUG_Message3((_T("FAILED  Invalid socket address:%d port:%d\n"), address, m_port));
        return false;
    }
            
    m_thread_pool.AssignDeviceToPool((HANDLE)sck, (DWORD_PTR)sck);

    int sending_result = SendPacket(sck, packet);

    return (sending_result == 0) || (sending_result == WSA_IO_PENDING); 
}

DWORD  WINAPI  VbaTaskSender::AddThreadTask(LPVOID p_void)
{       
    DEBUG_Message3((_T("TID: %d VbaTaskSender::AddThreadTask   Start\n"), GetCurrentThreadId()));

    TaskContainer* p_task_container = reinterpret_cast<TaskContainer*>(p_void);

    WaitForSingleObject(p_task_container->mp_vba_task_sender->m_socket_pool.m_free_sockets_event, INFINITE);
        
    SOCKET sck = p_task_container->mp_vba_task_sender->m_socket_pool.GetSocket(p_task_container->m_address, p_task_container->mp_vba_task_sender->m_port);

    if (sck == INVALID_SOCKET)
    {
		DEBUG_Message3((_T("FAILED  Invalid socket address:%d port:%d\n"), p_task_container->m_address, p_task_container->mp_vba_task_sender->m_port));

        HeapFree(GetProcessHeap(), 0, p_void);

        DEBUG_Message3((_T("TID: %d VbaTaskSender::AddThreadTask  Finsih Failed\n"), GetCurrentThreadId()));
        
        return false;
    }
            
    p_task_container->mp_vba_task_sender->m_thread_pool.AssignDeviceToPool((HANDLE)sck, (DWORD_PTR)sck);

    int sending_result = SendPacket(sck, p_task_container->m_packet);

    HeapFree(GetProcessHeap(), 0, p_void);

    DEBUG_Message3((_T("TID: %d VbaTaskSender::AddThreadTask  Finsih\n"), GetCurrentThreadId()));
    
    return (sending_result == 0) || (sending_result == WSA_IO_PENDING); 
}


