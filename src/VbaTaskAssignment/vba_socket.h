#pragma once

#include "stdafx.h"
#include <windows.h>
#include <winsock2.h>
#include "common/strings.h"

class VbaSocket
{
public:    
	VbaSocket();
	~VbaSocket();
	bool Initialize();
	bool ConnectSocket(unsigned long address, unsigned short port);
	bool Send(const std::tstring &packet, bool push_front_size = false);
	void Uninitialize();
private:
	SOCKET m_socket;
	SOCKADDR_IN m_addr;
};