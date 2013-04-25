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
    if (argc != 4)
    {
        _tprintf(_T("File packet sender\n"));
        _tprintf(_T("Usage: NetTest_03.exe server_ip server_port file_name\n"));
        return -1;
    }

    LPCTSTR server_ip = argv[1];
    const WORD port = _ttoi(argv[2]);
    LPTSTR file_name = 0;
    file_name = new TCHAR[MAX_PATH];
    LPTSTR file_data = 0;
    LPTSTR full_msg = 0;

    GetModuleFileName(0, file_name, MAX_PATH);
    LPTSTR slash = _tcsrchr(file_name, _T('\\'));
    slash[1] = _T('\0');
    _tcscat(file_name, argv[3]);
    if (GetFileAttributes(file_name) == INVALID_FILE_ATTRIBUTES)
    {
        _tprintf(_T("File %s doesn't exist\n"), file_name);
        return -1;
    }

    int result = NO_ERROR;
    WORD version_requested = MAKEWORD(2, 2);
    WSADATA wsa_data;
    
    result = WSAStartup(version_requested, &wsa_data);
    if (result)
    {
	    // Error initializing Windows Sockets
	    _tprintf(_T("Error initializing Windows Sockets, error code %d (%s)\n"), result, FormatErrorMessage(result));
        goto cleanup;
    }

    HANDLE hFile = CreateFile(   file_name,
                                GENERIC_READ,
                                FILE_SHARE_READ | FILE_SHARE_WRITE,
                                0,
                                OPEN_EXISTING,
                                0,
                                0);
    if (hFile == INVALID_HANDLE_VALUE)
    {
        DWORD res = GetLastError();
	    _tprintf(_T("Error opening file, error code %d (%s)\n"), res, FormatErrorMessage(res));
        goto cleanup;
    }

    const DWORD file_size = GetFileSize(hFile, 0);

    if (file_size > 65533)
    {
        CloseHandle(hFile);
	    _tprintf(_T("File must be less than 65533 bytes, actual size %d\n"), file_size);
        goto cleanup;
    }

    _tprintf(_T("Sending file %s to %s:%d...\n"), file_name, server_ip, port);
    
    file_data = new TCHAR[file_size + 1];

    DWORD read;
    if (!ReadFile(  hFile, 
                    file_data,
                    file_size,
                    &read,
                    0))
    {
        DWORD res = GetLastError();
        CloseHandle(hFile);
	    _tprintf(_T("Error reading file, error code %d (%s)\n"), res, FormatErrorMessage(res));
        goto cleanup;
    }

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

    full_msg = new TCHAR[file_size + 2];
    *((WORD*)full_msg) = file_size + 2;
    ::CopyMemory(full_msg + 2, file_data, file_size);

    WSABUF buf;
    buf.buf = full_msg;
    buf.len = file_size + 2;
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
    if (file_data)
    {
        delete[] file_data;
    }
    if (file_name)
    {
        delete[] file_name;
    }
    if (full_msg)
    {
        delete[] full_msg;
    }
    WSACleanup();
	return 0;
}
