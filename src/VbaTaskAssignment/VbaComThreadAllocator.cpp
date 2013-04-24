#include "stdafx.h"
#include "VbaComThreadAllocator.h"

ThreadInfo:: ThreadInfo()
{
    m_thread_state = ReservedThread;
    mp_com_apartment = new CComApartment;

#if !defined(_ATL_MIN_CRT) && defined(_MT)

	typedef unsigned ( __stdcall *pfnThread )( void * );
	mp_com_apartment->m_hThread = (HANDLE)_beginthreadex(NULL, 0, (pfnThread)CComApartment::_Apartment, mp_com_apartment, 0, reinterpret_cast<UINT*>(&(mp_com_apartment->m_dwThreadID)));
	if (mp_com_apartment->m_hThread == NULL)
	{
		HRESULT hr = E_FAIL;
		switch (Checked::get_errno())
		{
		case EAGAIN:
			hr = HRESULT_FROM_WIN32(ERROR_TOO_MANY_TCBS);
			break;
		case EINVAL:
			hr = E_INVALIDARG;
			break;
		}
		ATLASSERT(0);
		CAtlBaseModule::m_bInitFailed = true;
    }

#else
    mp_com_apartment->m_hThread = ::CreateThread(NULL, 0, CComApartment::_Apartment, reinterpret_cast<void*>(mp_com_apartment), 0, &(mp_com_apartment->m_dwThreadID));
	// clean up allocated threads
	if (mp_com_apartment->m_hThread == NULL)
	{
		ATLASSERT(0);
		CAtlBaseModule::m_bInitFailed = true;		
	}
#endif

}

ThreadInfo::~ThreadInfo()
{
    
	if (mp_com_apartment->m_hThread == NULL)
        return ;

    while ( ::PostThreadMessage(mp_com_apartment->m_dwThreadID, WM_QUIT, 0, 0) == 0)
	{
		if (GetLastError() == ERROR_INVALID_THREAD_ID)
		{
			ATLASSERT(FALSE);
			break;
		}
		::Sleep(100);
	}
	::WaitForSingleObject(mp_com_apartment->m_hThread, INFINITE);
	CloseHandle(mp_com_apartment->m_hThread);
    delete mp_com_apartment;                
}


VbaComThreadAllocator::VbaComThreadAllocator()
{
    InitializeCriticalSection(&m_CS);
    m_nThread = mc_max_threads;
}


VbaComThreadAllocator::~VbaComThreadAllocator()
{
    EnterCriticalSection(&m_CS);        //// CS

    for(ThreadsMap::iterator iter = m_threads_map.begin(); iter!= m_threads_map.end();++iter)
    {
        if (iter->second != NULL)
        {                
            delete iter->second;
        }
    }

    LeaveCriticalSection(&m_CS);        //// CS
}


void VbaComThreadAllocator::Lock()
{
    EnterCriticalSection(&m_CS);        //// CS

    DWORD thread_id = GetCurrentThreadId();
    ThreadsMap::iterator iter = m_threads_map.find(thread_id);
    if (iter != m_threads_map.end())
    {
        iter->second->m_thread_state = LockedThread;
    }

    LeaveCriticalSection(&m_CS);        //// CS
}


void VbaComThreadAllocator::Free()
{
    EnterCriticalSection(&m_CS);        //// CS

    DWORD thread_id = GetCurrentThreadId();
    ThreadsMap::iterator iter = m_threads_map.find(thread_id);
    if (iter != m_threads_map.end())
    {
        iter->second->m_thread_state = FreeThread;
    }

    LeaveCriticalSection(&m_CS);        //// CS
}


int VbaComThreadAllocator::GetThread(CComApartment** pApt)
{
    EnterCriticalSection(&m_CS);        //// CS

    for(ThreadsMap::iterator iter = m_threads_map.begin(); iter!= m_threads_map.end();++iter)
    {
        if (iter->second->m_thread_state == FreeThread)
        {
            iter->second->m_thread_state = ReservedThread;
            *pApt = iter->second->mp_com_apartment;
            LeaveCriticalSection(&m_CS);//// CS
            return 0;
        }
    }
    
    // ѕроверка на количество уже созданных потоков
    if(m_threads_map.size() >= mc_max_threads)
    {
        pApt = NULL;                    
        LeaveCriticalSection(&m_CS);    //// CS
        return 1;
    }

    /// —оздаем поток и апартамент
    ThreadInfo* thread_info = new ThreadInfo();
    *pApt = thread_info->mp_com_apartment;

    m_threads_map.insert(ThreadsPair(thread_info->mp_com_apartment->m_dwThreadID, thread_info));

    LeaveCriticalSection(&m_CS);        //// CS
    return 0;
}