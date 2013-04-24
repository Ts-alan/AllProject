#pragma once

#include "vba_common/vba_strings.h"

#include "socket_pool.h"
#include "thread_pool.h"

#define THREADPOOL_SIZE	5
#define SOCKET_POOL_SIZE 100

class VbaTaskSender
{
public:
    bool Initialize(unsigned short port);
    bool Shutdown();
    bool AddTask(unsigned long address, const utf8_string& packet);

    static DWORD  WINAPI  AddThreadTask(LPVOID p_void); 

    VbaTaskSender();
    ~VbaTaskSender();

private:
    VbaSocketPool                  m_socket_pool;
    VbaThreadPool<VbaSocketWorker> m_thread_pool;
    
    unsigned short m_port;
};

struct TaskContainer
{
    VbaTaskSender* mp_vba_task_sender;
    unsigned long  m_address;
    utf8_string    m_packet;
};
