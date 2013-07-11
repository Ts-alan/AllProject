#pragma once

#include <winsock2.h>

#include <list>

#include "common/critical_section.h"

class VbaSocketPool
{
private:
    struct SocketContext
	{
        SocketContext() : m_socket(INVALID_SOCKET), m_address(0)
        {
        }

        ~SocketContext()
        {
        }

        SOCKET m_socket;
        unsigned long m_address;
	};

public:
    HANDLE m_free_sockets_event;

    SOCKET GetSocket(unsigned long address, u_short port);
    bool ReleaseSocket(SOCKET socket_to_release);
    bool Shutdown();
    
    VbaSocketPool(size_t max_sockets_count);
    virtual ~VbaSocketPool(void);

protected:
    bool Connect(SOCKET sck, unsigned long address, u_short port);
    bool FreeSockets();

    //SOCKET CreateNewSocket();

    std::list<SocketContext> m_free_sockets;
    std::list<SocketContext> m_busy_sockets;

    vba::CriticalSection m_critical_section; 

    size_t m_max_sockets_count;
};
