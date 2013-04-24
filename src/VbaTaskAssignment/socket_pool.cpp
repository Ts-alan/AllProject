#include "stdafx.h"
#include <algorithm>



#include "socket_pool.h"

bool InternalClose(SOCKET sck)
{   
    return (shutdown(sck, SD_BOTH) == 0) && (closesocket(sck) == 0);
}

bool AbortiveClose(SOCKET sck)
{
   LINGER linger;

   linger.l_onoff = 1;
   linger.l_linger = 0;

   return (setsockopt(sck, SOL_SOCKET, SO_LINGER, (char *)&linger, sizeof(linger)) != SOCKET_ERROR ? InternalClose(sck) : false);
}

VbaSocketPool::VbaSocketPool(size_t max_sockets_count) : m_max_sockets_count(max_sockets_count)
{
    InitializeCriticalSection(&m_critical_section);

    m_free_sockets_event = CreateEvent(NULL, TRUE, TRUE, NULL);
}

VbaSocketPool::~VbaSocketPool()
{
    DeleteCriticalSection(&m_critical_section);

    CloseHandle(m_free_sockets_event);
}

bool VbaSocketPool::Connect(SOCKET sck, unsigned long address, u_short port)
{
    SOCKADDR_IN sck_address;
    sck_address.sin_family = AF_INET;
    sck_address.sin_addr.S_un.S_addr = address;
    sck_address.sin_port = htons(port);

    return (WSAConnect(sck, (const sockaddr*)&sck_address, sizeof(sck_address), NULL, NULL, NULL, NULL) != SOCKET_ERROR);
}

SOCKET VbaSocketPool::GetSocket(unsigned long address, u_short port)
{

    DEBUG_Message3((_T("VbaSocketPool::GetSocket\n")));

    SOCKET sck = INVALID_SOCKET;

    EnterCriticalSection(&m_critical_section);

    DEBUG_Message3((_T("VbaSocketPool::GetSocket Enter CS\n")));

    for (std::list<SocketContext>::iterator it = m_free_sockets.begin(); it != m_free_sockets.end(); ++it)
    {
        if (it->m_address == address)
        {
            sck = it->m_socket;
            m_busy_sockets.splice(m_busy_sockets.end(), m_free_sockets, it);
            break;
        }
    }

    DEBUG_Message3((_T("VbaSocketPool::GetSocket Leave CS\n")));
    
    LeaveCriticalSection(&m_critical_section);

    if ( (sck == INVALID_SOCKET) && (m_busy_sockets.size() < m_max_sockets_count) )
    {
        sck = CreateNewSocket();

        if (!Connect(sck, address, port))
        {
            DEBUG_Message3((_T("VbaSocketPool::GetSocket NOT conected\n")));
            
            InternalClose(sck);
            sck = INVALID_SOCKET;
        }
        
        if (sck != INVALID_SOCKET)
        {
            DEBUG_Message3((_T("VbaSocketPool::GetSocket not invaleded\n")));
            
            SocketContext sc;
            sc.m_socket = sck;
            sc.m_address = address;

            EnterCriticalSection(&m_critical_section);
            m_busy_sockets.push_back(sc);
        
            if (m_busy_sockets.size() == m_max_sockets_count)
            {
                ResetEvent(m_free_sockets_event);
            }

            LeaveCriticalSection(&m_critical_section);
        }
    }                      

    DEBUG_Message3((_T("VbaSocketPool::GetSocket Ended\n")));

    return sck;
}

bool VbaSocketPool::ReleaseSocket(SOCKET socket_to_release)
{
    DEBUG_Message3((_T("VbaSocketPool::ReleaseSocket\n")));

    bool ret = false;
    
    if (socket_to_release == INVALID_SOCKET)
    {
        return ret;
    }

    EnterCriticalSection(&m_critical_section);

    DEBUG_Message3((_T("VbaSocketPool::ReleaseSocket Enter CS\n")));

    for (std::list<SocketContext>::iterator it = m_busy_sockets.begin(); it != m_busy_sockets.end(); ++ it)
    {
        if (it->m_socket == socket_to_release)
        {
            m_free_sockets.splice(m_free_sockets.end(), m_busy_sockets, it);
            SetEvent(m_free_sockets_event);
            ret = true;
            break;
        }
    }

    LeaveCriticalSection(&m_critical_section);

    DEBUG_Message3((_T("VbaSocketPool::ReleaseSocket Leave CS\n")));

    return ret;
}

bool VbaSocketPool::FreeSockets()
{
    bool res = true;

    EnterCriticalSection(&m_critical_section);

    for (std::list<SocketContext>::iterator it = m_free_sockets.begin(); it != m_free_sockets.end(); ++ it)
    {
        if (!InternalClose(it->m_socket))
        {
            res = false;
        }
    }

    for (std::list<SocketContext>::iterator it = m_busy_sockets.begin(); it != m_busy_sockets.end(); ++ it)
    {
        if (!AbortiveClose(it->m_socket))
        {
            res = false;
        }
    }

    LeaveCriticalSection(&m_critical_section);

    return res;
}

SOCKET VbaSocketPool::CreateNewSocket()
{
    return WSASocket(AF_INET, SOCK_STREAM, 0, NULL, 0, WSA_FLAG_OVERLAPPED);
}

bool VbaSocketPool::Shutdown()
{
    return FreeSockets();
}


