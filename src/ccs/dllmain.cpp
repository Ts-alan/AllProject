#include "ccs_setup.h"

vba::cc::CCUpdateSetup* update_setup = NULL;


bool GetInterface(vba::IObject** p_plugin)
{
    *p_plugin = update_setup;
    (*p_plugin)->AddRef();
    return true;
}


// DLL Entry Point
extern "C" BOOL WINAPI DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID lpReserved)
{

    using namespace vba::upd;
    switch (dwReason)
    {
    case DLL_PROCESS_ATTACH:
        update_setup = new vba::cc::CCUpdateSetup();
        update_setup->AddRef(); 
        break;

    case DLL_THREAD_ATTACH:
        break;

    case DLL_THREAD_DETACH:
        break;

    case DLL_PROCESS_DETACH:
        update_setup->Release();
        break;
    }
    return TRUE;
}