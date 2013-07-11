#include "stdafx.h"
#include <algorithm>



#include "socket_pool.h"
#include "common/log.h"

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
    m_free_sockets_event = CreateEvent(NULL, TRUE, TRUE, NULL);
}

VbaSocketPool::~VbaSocketPool()
{
    CloseHandle(m_free_sockets_event);
}

bool VbaSocketPool::Connect(SOCKET sck, unsigned long address, u_short port)
{
    SOCKADDR_IN sck_address;
    sck_address.sin_family = AF_INET;
    sck_address.sin_addr.S_un.S_addr = address;
    sck_address.sin_port = htons(port);

    int res = WSAConnect(sck, (const sockaddr*)&sck_address, sizeof(sck_address), NULL, NULL, NULL, NULL) ;
    if (res == SOCKET_ERROR)
    {
        DLOG() % LOG_FUNC % L"WSAConnect error (" % WSAGetLastError() % L")";        
    }
    return (res!= SOCKET_ERROR);
}

SOCKET VbaSocketPool::GetSocket(unsigned long address, u_short port)
{
    SOCKET sck = INVALID_SOCKET;
    {
        if (!Connect(sck, address, port))
        {
            DLOG() % LOG_FUNC % L" not conected (fail).";
            
            InternalClose(sck);
            sck = INVALID_SOCKET;
        }
        
        if (sck != INVALID_SOCKET)
        {   
            SocketContext sc;
            sc.m_socket = sck;
            sc.m_address = address;

            vba::AutoLockCS lock(m_critical_section);

            m_busy_sockets.push_back(sc);        
            if (m_busy_sockets.size() == m_max_sockets_count)
            {
                ResetEvent(m_free_sockets_event);
            }

            DLOG() % LOG_FUNC % L"getted(ok).";           
        }
    }                      
    return sck;
}

bool VbaSocketPool::ReleaseSocket(SOCKET socket_to_release)
{
    if (socket_to_release == INVALID_SOCKET)
    {
        DLOG() % LOG_FUNC % L" Try release invalid socket (warning).";        
        return false;
    }

    vba::AutoLockCS lock(m_critical_section);    

    for (std::list<SocketContext>::iterator it = m_busy_sockets.begin(); it != m_busy_sockets.end(); ++ it)
    {
        if (it->m_socket == socket_to_release)
        {
            m_free_sockets.splice(m_free_sockets.end(), m_busy_sockets, it);
            SetEvent(m_free_sockets_event);
            return true;
        }
    }
    
    DLOG() % LOG_FUNC % L" Try release unkonown socket (warning).";      
    return false;
}

bool VbaSocketPool::FreeSockets()
{
    bool res = true;

    vba::AutoLockCS lock(m_critical_section);

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

    DLOG() % LOG_FUNC % ( (res) ? L" (ok)." : L" (warning)."); 
    return res;
}

//SOCKET VbaSocketPool::CreateNewSocket()
//{
//    DLOG() % LOG_FUNC % L" Try release unkonown socket (warning)."; 
//
//    SOCKET res = WSASocket(AF_INET, SOCK_STREAM, 0, NULL, 0, WSA_FLAG_OVERLAPPED);
//    if(SOCKET == INVALID_SOCKET)
//    {
//
//    }
//    return res;
//}

bool VbaSocketPool::Shutdown()
{
    return FreeSockets();
}


