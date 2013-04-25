#ifndef _WIN32_WINNT		// Allow use of features specific to Windows XP or later.                   
#define _WIN32_WINNT 0x0501	// Change this to the appropriate value to target other versions of Windows.
#endif						

#include <winsock2.h>
#include <stdio.h>
#include <tchar.h>
#include <windows.h>
#define WIN32_LEAN_AND_MEAN
#include <time.h>
#include "../../vba_common/FormatErrorMessage.h"

int _tmain(int argc, _TCHAR* argv[])
{
    if (argc != 3)
    {
        _tprintf(_T("Small packet sender\n"));
        _tprintf(_T("Usage: NetTest_01.exe server_ip server_port\n"));
        return -1;
    }

    LPCTSTR server_ip = argv[1];
    const WORD port = _ttoi(argv[2]);

    int result = NO_ERROR;
    WORD version_requested = MAKEWORD(2, 2);
    WSADATA wsa_data;
     
    result = WSAStartup(version_requested, &wsa_data);
    if (result)
    {
	    // Error initializing Windows Sockets
	    _tprintf(_T("Error initializing Windows Sockets, error code %d (%s)\n"), result, FormatErrorMessage(result));
        return result;
    }

    _tprintf(_T("Sending test packet to %s:%d...\n"), server_ip, port);

    srand((unsigned)time(0));

    // Forming string to send
    const WORD msg_size = rand() % 256 + 1;
    _tprintf(_T("Message size: %d\n"), msg_size);
    LPTSTR msg = 0;
    msg = new TCHAR[msg_size];
    LPTSTR full_msg = 0;
    full_msg = new TCHAR[msg_size + 2];

    for (USHORT i = 0; i < msg_size - 1; i++)
    {
        msg[i] = (char)(rand() % 230 + 25);
    }
    msg[msg_size - 1] = '\0';

    _tprintf(_T("Message: \"%s\"\n"), msg);

    SOCKET s = WSASocket(   AF_INET,
                            SOCK_STREAM,
                            IPPROTO_TCP,
                            0,
                            0,
                            0);
    if (s == INVALID_SOCKET)
    {
        result = WSAGetLastError();
	    _tprintf(_T("WSASocket() failed, error code %d (%s)\n"), result, FormatErrorMessage(result));
        goto cleanup;
    }

    struct sockaddr_in addr;
    addr.sin_addr.s_addr = inet_addr(server_ip);
    addr.sin_family = AF_INET;
    addr.sin_port = htons(port);
    result = WSAConnect(    s,
                            (struct sockaddr*)&addr,
                            sizeof(addr),
                            0,
                            0,
                            0,
                            0);
    if (result)
    {
        result = WSAGetLastError();
	    _tprintf(_T("WSAConnect() failed, error code %d (%s)\n"), result, FormatErrorMessage(result));
        goto cleanup;
    }

    *((WORD*)full_msg) = msg_size + 2;
    ::CopyMemory(full_msg + 2, msg, msg_size);

    WSABUF buf;
    buf.buf = full_msg;
    buf.len = msg_size + 2;
    DWORD sent = 0;
    result = WSASend( s, 
                      &buf,
                      1,
                      &sent,
                      0,
                      0,
                      0);
    if (result)
    {
        result = WSAGetLastError();
	    _tprintf(_T("WSASend() failed, error code %d (%s)\n"), result, FormatErrorMessage(result));
        goto cleanup;
    }
    _tprintf(_T("Sent %d bytes\n"), sent);

    result = shutdown(s, SD_BOTH);
    if (result)
    {
        result = WSAGetLastError();
	    _tprintf(_T("shutdown() failed, error code %d (%s)\n"), result, FormatErrorMessage(result));
        goto cleanup;
    }

    result = closesocket(s);
    if (result)
    {
        result = WSAGetLastError();
	    _tprintf(_T("closesocket() failed, error code %d (%s)\n"), result, FormatErrorMessage(result));
        goto cleanup;
    }


cleanup:
    if (msg)
    {
        delete[] msg;
    }
    if (full_msg)
    {
        delete[] full_msg;
    }
    WSACleanup();
	return 0;
}
