#ifndef _WIN32_WINNT		// Allow use of features specific to Windows XP or later.                   
#define _WIN32_WINNT 0x0501	// Change this to the appropriate value to target other versions of Windows.
#endif						

#include <winsock2.h>
#include <stdio.h>
#include <tchar.h>
#include <windows.h>
#define WIN32_LEAN_AND_MEAN
#include "../../vba_common/FormatErrorMessage.h"
#include "../../vba_common/service/autocleanup.h"
#include "../../libcrypt/vbacrypt.h"


CryptSecretKey g_secret_key;

    
int _tmain(int argc, _TCHAR* argv[])
{
    if (argc != 4)
    {
        _tprintf(_T("Signed packet sender\n"));
        _tprintf(_T("Usage: NetTest_04.exe agent_ip server_port file_name\n"));
        return -1;
    }

    LPCTSTR server_ip = argv[1];
    const WORD port = (WORD)_ttoi(argv[2]);
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

    // Initializing Winsock
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

    CryptSignature Signature;
    SecureZeroMemory(&Signature, sizeof(Signature));
    SecureZeroMemory(&g_secret_key, sizeof(g_secret_key));

    CAutoRegCloseKey key;
    LONG ret = 0;
    ret = RegOpenKeyEx( HKEY_LOCAL_MACHINE,
                        _T("SOFTWARE\\Vba32\\ControlCenter\\Signature"),
                        0,
                        KEY_READ,
                        key.GetPtr());
    if (ret != ERROR_SUCCESS)
    {
        printf(_T("Error opening registry key, error code %d (%s)\n"), ret, FormatErrorMessage(ret));
        return (ret);
    }

    // Reading private key
    DWORD size = sizeof(g_secret_key);
    ret = RegQueryValueEx( key,
                           _T("PrivateKey"),
                           0,
                           0,
                           (LPBYTE)&g_secret_key,
                           &size);
    if (ret != ERROR_SUCCESS)
    {
        printf(_T("Error quering PrivateKey value, error code %d (%s)\n"), ret, FormatErrorMessage(ret));
        return (ret);
    }
    if (size != sizeof(g_secret_key))
    {
        printf(_T("Size of PrivateKey doesn't match\n"));
        return (ret);
    }

    // Reading file
    CAutoCloseFile hFile = CreateFile( file_name,
                                       GENERIC_READ,
                                       FILE_SHARE_READ | FILE_SHARE_WRITE,
                                       0,
                                       OPEN_EXISTING,
                                       0,
                                       0);
    if (hFile.IsInvalid())
    {
        DWORD res = GetLastError();
	    _tprintf(_T("Error opening file, error code %d (%s)\n"), res, FormatErrorMessage(res));
        goto cleanup;
    }

    const DWORD file_size = GetFileSize(hFile, 0);

    if (file_size > 65400)
    {
	    _tprintf(_T("File must be less than 65400 bytes, actual size %d\n"), file_size);
        goto cleanup;
    }

    _tprintf(_T("Sending file %s to %s:%d...\n"), file_name, server_ip, port);
    
    file_data = new TCHAR[file_size + sizeof(Signature) + 1];

    DWORD read = 0;
    if (!ReadFile(  hFile, 
                    file_data,
                    file_size,
                    &read,
                    0))
    {
        DWORD res = GetLastError();
	    _tprintf(_T("Error reading file, error code %d (%s)\n"), res, FormatErrorMessage(res));
        goto cleanup;
    }

    // Signing
    bool bResult = CryptSignDataBlock(file_data, file_size, &g_secret_key, &Signature);
	if (!bResult)
    {
        _tprintf(_T("Block sign failed\n"));
        return -1;
    }

    CopyMemory(file_data + file_size, &Signature, sizeof(Signature));

    size = file_size + sizeof(Signature);
    file_data[size] = '\0';
    _tprintf(_T("Message: \"%s\"\n"), file_data);

    SOCKET s = WSASocket( AF_INET,
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
    result = WSAConnect( s,
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

    WSABUF buf;
    buf.buf = file_data;
    buf.len = size;
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

    _tprintf(_T("Data signed and sent successfully\n"));

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
