#include "ccs_plug.h"

vba::cc::CCPlug* cc_plug = NULL;


bool GetInterface(vba::IObject** p_plugin)
{
    *p_plugin = cc_plug;
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
        cc_plug = new vba::cc::CCPlug();
        cc_plug->AddRef(); 
        break;

    case DLL_THREAD_ATTACH:
        break;

    case DLL_THREAD_DETACH:
        break;

    case DLL_PROCESS_DETACH:
        cc_plug->Release();
        break;
    }
    return TRUE;
}