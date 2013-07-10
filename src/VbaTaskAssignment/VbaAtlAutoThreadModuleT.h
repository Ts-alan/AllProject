////
////   VbaAtlAutoThreadModuleT.h
////
////      ласс VbaAtlAutoThreadModuleT отвечает за предоставление апартамента
////   при создании COM-объектов сервисом. явл€етс€ одниv из двух классов реализующих
////   потоковую модель сервиса. (явл€етс€ заменой стандартного класса CAtlAutoThreadModuleT)
////

#pragma once

#include <atlbase.h>
#include "VbaComThreadAllocator.h"


                                 

template <class T, class ThreadAllocator = VbaComThreadAllocator, DWORD dwWait = INFINITE>
class ATL_NO_VTABLE VbaAtlAutoThreadModuleT : public IAtlAutoThreadModule
{
// This class is not for use in a DLL.
// If this class were used in a DLL,  there will be a deadlock when the DLL is unloaded.
// because of dwWait's default value of INFINITE

private:

    ThreadAllocator* mp_Allocator;


public:

    VbaAtlAutoThreadModuleT(int nThreads = T::GetDefaultThreads())
    {
        mp_Allocator = vba::AutoSingleton<VbaComThreadAllocator>::Instance();
	    ATLASSERT(_pAtlAutoThreadModule == NULL);
	    _pAtlAutoThreadModule = this;

    }

    ~VbaAtlAutoThreadModuleT()
    {
        vba::AutoSingleton<VbaComThreadAllocator>::FreeInst();
        mp_Allocator = NULL;

    }

    HRESULT CreateInstance(void* pfnCreateInstance, REFIID riid, void** ppvObj)
    {
	    ATLASSERT(ppvObj != NULL);

	    if (ppvObj == NULL)
        {
		    return E_POINTER;
        }

	    *ppvObj = NULL;

	    _ATL_CREATORFUNC* p_func = (_ATL_CREATORFUNC*) pfnCreateInstance;
	    _AtlAptCreateObjData data;

	    data.pfnCreateInstance = p_func;
	    data.piid = &riid;
	    data.hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
	    data.hRes = S_OK;     

        CComApartment* p_apartments = NULL;

	    if (mp_Allocator->GetThread(&p_apartments) != 0)
        {
            if (!CAtlBaseModule::m_bInitFailed)
            {
		        CComApartment::ATL_CREATE_OBJECT = RegisterWindowMessage(_T("ATL_CREATE_OBJECT"));
            }

            if ((mp_Allocator->GetThread(&p_apartments)) == 1)
            {
                return E_OUTOFMEMORY;
            }

            return E_FAIL;
        }

	    int num_iterations = 0;
      
	    while(::PostThreadMessage(p_apartments->m_dwThreadID, CComApartment::ATL_CREATE_OBJECT, 0, (LPARAM)&data) == 0 && ++num_iterations < 100)
	    {
		    Sleep(100);
	    }
	    if (num_iterations < 100)
	    {
		    AtlWaitWithMessageLoop(data.hEvent);
	    }
	    else
	    {
		    data.hRes = AtlHresultFromLastError();
	    }
	    CloseHandle(data.hEvent);

	    if (SUCCEEDED(data.hRes))
		    data.hRes = CoGetInterfaceAndReleaseStream(data.pStream, riid, ppvObj);
	    return data.hRes;
    }

	
};
