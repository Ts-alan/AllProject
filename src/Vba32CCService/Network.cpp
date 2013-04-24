// JetByte library
#include "stdafx.h"

#include "JetByteTools\Win32Tools\WinsockWrapper.h"
#include "JetByteTools\Win32Tools\Exception.h"
#include "JetByteTools\Win32Tools\Utils.h"
#include "JetByteTools\Win32Tools\NamedIndex.h"
#include "JetByteTools\Win32Tools\SharedCriticalSection.h"

#include "SocketServer.h"
#include "IOPool.h"
#include "ThreadPool.h"
#include "Network.h"

using JetByteTools::Win32::_tstring;
using JetByteTools::Win32::CException;
using JetByteTools::Win32::Output;
using JetByteTools::Win32::CManualResetEvent;
using JetByteTools::Win32::CNamedIndex;
using JetByteTools::Win32::CInstrumentedSharedCriticalSection;
using JetByteTools::Win32::CSharedCriticalSection;

//  Thread function for asynchronous network IO
DWORD WINAPI NetworkActivityThread(LPVOID/* p_parameter*/);

static BOOL s_is_network_started = FALSE;

//////////////////////////////////////////////////////////////////////////////


VOID StartupNetwork()
{
    if (!s_is_network_started)
    {
        s_is_network_started = TRUE;
        p_logfile -> AddToLog(LoadStringFromResource(IDS_LOG_STARTING_NETWORK));

        // Create thread that creates IO and business pools
        CreateThread(   0,
                        0,
                        NetworkActivityThread,
                        0,
                        0,
                        0);
    }
}

DWORD WINAPI NetworkActivityThread(LPVOID/* p_parameter */)
{
    try
    {
        CNamedIndex user_data_slots;

        CThreadPool business_logic_pool(user_data_slots,
                                        5,               // initial number of threads to create
                                        5,               // minimum number of threads to keep in the pool
                                        10,              // maximum number of threads in the pool
                                        5,               // maximum number of "dormant" threads
                                        5000,            // pool maintenance period (millis)
                                        100,             // dispatch timeout (millis)
                                        10000);          // dispatch timeout for when pool is at max threads

        business_logic_pool.Start();

        const size_t number_user_data_slots = user_data_slots.Lock();

        CIOPool io_pool(0);     // number of threads (0 = 2 x processors)
        io_pool.Start();

        CSharedCriticalSection lock_factory(47);

        CSocketServer server( lock_factory,
                              io_pool,
                              INADDR_ANY,              // address to listen on
                              17001,                   // port to listen on
                              100,                     // max number of sockets to keep in the pool
                              100,                     // max number of buffers to keep in the pool
                              65536,                   // buffer size 
                              number_user_data_slots,
                              business_logic_pool);

        server.Start();
        server.StartAcceptingConnections();

        CManualResetEvent shutdown_event(_T("Vba32CC_ServerShutdown"), FALSE);
        HANDLE h_event = shutdown_event.GetEvent();

        bool done = false;

        while (!done)
        {
            DWORD waitResult = ::WaitForSingleObject(h_event, INFINITE);

            if (waitResult == WAIT_OBJECT_0)
            {
                done = true;
            }
            else
            {
                Output(_T("Unexpected result from WaitForMultipleObjects - exiting"));
                done = true;
            }
        }

        business_logic_pool.WaitForShutdownToComplete(5000, true);

        io_pool.WaitForShutdownToComplete();

        server.WaitForShutdownToComplete();  
    }
    catch(const CException &e)
    {
        Output(_T("Exception: ") + e.GetWhere() + _T(" - ") + e.GetMessage());
    }
    catch(...)
    {
        Output(_T("Unexpected exception"));
    }
    return 0;
}

VOID ShutdownNetwork()
{
    if (s_is_network_started)
    {
        s_is_network_started = FALSE;

        p_logfile -> AddToLog(LoadStringFromResource(IDS_LOG_SHUTTING_NETWORK));
        CManualResetEvent shutdown_event(_T("Vba32CC_ServerShutdown"), FALSE);
        try
        {
            shutdown_event.Pulse();
        }
        catch(CException &e)
        {
            Output(_T("Exception: ") + e.GetWhere() + _T(" - ") + e.GetMessage());
        }
    }
}
