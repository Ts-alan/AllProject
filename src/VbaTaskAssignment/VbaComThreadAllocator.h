////
////   VbaComThreadAllocator.h 
////
////     ����� VbaComThreadAllocator �������� �� �������������� �������(������������)
////   ��� �������� COM-�������� ��������. �������� ����v �� ���� ������� �����������
////   ��������� ������ �������. (�������� ������� ������������ ������ CComSimpleThreadAllocator)
////
////     ������ ������ ���� ��������� ����������� ��������:
////   1) CComSimpleThreadAllocator � CAtlAutoThreadModuleT ��������� ������ ���� ������� 
////      � ������������� ������ ������� (round-robin threading model)
////   2) ��� ��������� ������� �� �������������� ��������� �� ���������.
////   
////     � ������ VbaComThreadAllocator ������������ ���������
////   1) ������������ ��������� ����� ������� � ����.
////   2) �������� ��������� ������ ��� ������������� COM-������� 
////      (ReservedThread - LockedThread - FreeThread)
////     ��� ��������� GetThread ����� �������� ������ ReservedThread � 
////       �� ����� ���� ������� ������ Com-��������. ����� ���-������ �������� 
////       ������ Lock (��� �������� ������� FinalConstruct) 
////            � Free (��� ����������� ������� FinalRelease)
////     ����� ����� ����� ���������� ��������� ��� �������������.   
////
#pragma once
                   
#include <atlbase.h>
#include <map>

#include "vba_common/vba_singleton.h"

enum ThreadState
{
    FreeThread      = 0,    // ��������� ����� �������� ��� �������������
    LockedThread    = 1,    // ������� �����
    ReservedThread  = 2     // ����������������� �����.
};

struct ThreadInfo
{

    CComApartment* mp_com_apartment;
    ThreadState    m_thread_state;  


    ThreadInfo();
    ~ThreadInfo();

};

typedef std::map<DWORD,ThreadInfo*> ThreadsMap;
typedef std::pair<DWORD,ThreadInfo*> ThreadsPair;

class VbaComThreadAllocator : public cc::VbaSingleton<VbaComThreadAllocator>
{
friend class cc::VbaSingleton<VbaComThreadAllocator>;
    
protected:

    VbaComThreadAllocator();
    ~VbaComThreadAllocator();
                       
public:

    
    /// ���������� ����������� COM-��������
    ///     (���������� ������� �������� �������)
    void Lock();

    /// ������������� ����������� COM-��������
    ///     (���������� ��� ����������� �������) 
    void Free();

	int GetThread(CComApartment** pApt);

private:

    CRITICAL_SECTION m_CS;

    static const int  mc_max_threads = 200;        /// ������������ ���-�� �������

	int m_nThread;                                  /// ������� ���-�� �������

    ThreadsMap m_threads_map;
};