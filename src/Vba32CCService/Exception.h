#include "windows.h"

// Declarations for vba_dump library
typedef BOOL (*VBACREATEDUMPS)(
    EXCEPTION_POINTERS* pep, 
	LONG lType, 
	LONG* plError
    );

#define MINI_DUMP 0x0001
#define MIDI_DUMP 0x0010
#define MAXI_DUMP 0x0100
#define ALL_DUMPS 0x0111


//////////////////////////////////////////////////////////////////////////////


// High-level exception handler
LONG WINAPI OnProcessException(LPEXCEPTION_POINTERS lpExceptInfo);
// Writes memory dump
DWORD WriteCurrentProcessDump(LPEXCEPTION_POINTERS lpExceptInfo);
