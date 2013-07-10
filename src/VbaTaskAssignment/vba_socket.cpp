#include "stdafx.h"
#include "vba_socket.h"
#include "common/log.h"

VbaSocket::VbaSocket()
{}					  

VbaSocket::~VbaSocket()
{}

bool VbaSocket::Initialize()
{
	WSADATA wsaData;
    int res = WSAStartup(MAKEWORD(2,2), &wsaData);
    if (res != 0)
    {
        LOG_WARN() % L" WSAStartup error("
			% res 
			% L") (eror)."; 
    }
	return (res == 0);
}

bool VbaSocket::ConnectSocket(unsigned long address, unsigned short port)
{
    
	m_socket = socket(AF_INET, SOCK_STREAM, 0);
    if (m_socket == INVALID_SOCKET)
    {
        LOG_WARN() % L" Socket(AF_INET, SOCK_STREAM, 0) error ("
			% WSAGetLastError() 
            % L") (fail).";          
        return false;
    }

	SOCKADDR_IN sck_address;
    sck_address.sin_family = AF_INET;
    sck_address.sin_addr.S_un.S_addr = address;
    sck_address.sin_port = htons(port);

    if (connect(m_socket, (const sockaddr*)&sck_address, sizeof(sck_address)) == SOCKET_ERROR)
    {		
		LOG_WARN() % " Connect(" 
			% inet_ntoa(sck_address.sin_addr) 
			% L") error ("
			% WSAGetLastError()  
			% L") (fail)."; 		
        return false;
    }

	m_addr = sck_address;
    DLOG() % " Connect(" 
		% inet_ntoa(m_addr.sin_addr)	 
		% L") (ok)."; 
    return true;
}

bool VbaSocket::Send(const std::tstring &packet, bool push_front_size)
{
	if (!push_front_size) 		
    {
		if(send(m_socket, packet.c_str(), packet.size(), 0) != packet.size())
		{
			LOG_WARN() % L" send packet ("
				% inet_ntoa(m_addr.sin_addr)
				% ") error("
				% WSAGetLastError() 
				% L") (fail)."; 
			return false;
		}
    } 
	else 
	{
		WSABUF buffer;
		buffer.len = packet.size() + sizeof(WORD);
		std::vector<char> buffer_(buffer.len,0);		
		CopyMemory(&buffer_[0 + sizeof(WORD)], packet.c_str(), packet.size());
		CopyMemory(&buffer_[0], &buffer.len, sizeof(WORD));
		buffer.buf = &buffer_[0];

		DWORD sent = 0;
		if (WSASend(m_socket, &buffer, 1, &sent, 0, 0, 0))
		{
			LOG_WARN() % L" WSASend packet ("
				% inet_ntoa(m_addr.sin_addr) 
				% ") error("
				% WSAGetLastError() 
				% L") (fail)."; 
			return false;
		}
	}

    DLOG() % L" WSASend packet ("
				% inet_ntoa(m_addr.sin_addr) 
				% ") error("
				% WSAGetLastError() 
				% L") (ok)."; 
	return true;
}

void VbaSocket::Uninitialize()
{
	if (m_socket != INVALID_SOCKET)
    {
		closesocket(m_socket);   
    }
	if ( WSACleanup()!=0 )
	{		
		LOG_WARN() % L" WSACleanup error("
			% WSAGetLastError() 
			% L") (fail)."; 
	}
}