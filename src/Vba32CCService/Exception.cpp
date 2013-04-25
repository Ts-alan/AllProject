#include "stdafx.h"
#include "Exception.h"


LONG WINAPI OnProcessException(LPEXCEPTION_POINTERS lpExceptInfo)
{
    DEBUG_Message1((_T("OnProcessException(): creating dump...")));
	p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_EXCEPTION));
	DWORD dwRet = WriteCurrentProcessDump(lpExceptInfo);
	if (dwRet)
    {
		// Error writing dump
		p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_DUMP_ERROR), dwRet, FormatErrorMessage(dwRet));
	}
	else
    {
		// Dump successful
		p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_DUMP_SUCCESS));
	}

	// Restarting service


	return (EXCEPTION_EXECUTE_HANDLER);
}


DWORD WriteCurrentProcessDump(LPEXCEPTION_POINTERS lpExceptInfo)
{
	DWORD dwErr = 0;

	// Loading vba_dump.dll
	HMODULE hDll = LoadLibrary(_T("vba_dump.dll"));
	if (!hDll)
    {
		dwErr = GetLastError();
		return (dwErr);
	}

	// Getting procedure
	VBACREATEDUMPS pVbaCreateDumps = (VBACREATEDUMPS)GetProcAddress(hDll, "VbaCreateDumps");

	if (!pVbaCreateDumps)
    {
		dwErr = GetLastError();
		goto cleanup;
	}

	LONG lResult = 0;
	(pVbaCreateDumps)(lpExceptInfo, ALL_DUMPS, &lResult);
	dwErr = lResult;

cleanup:
	FreeLibrary(hDll);
	return (dwErr);
}

