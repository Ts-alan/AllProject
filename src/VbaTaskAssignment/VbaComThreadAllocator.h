////
////   VbaComThreadAllocator.h 
////
////     Класс VbaComThreadAllocator отвечает за предоставление потоков(апартаментов)
////   при создании COM-объектов сервисом. Является одниv из двух классов реализующих
////   потоковую модель сервиса. (Является заменой стандартного класса CComSimpleThreadAllocator)
////
////     Данная замена была вызванная последующим причинам:
////   1) CComSimpleThreadAllocator и CAtlAutoThreadModuleT реализуют модель пула потоков 
////      с фиксированным числом потоков (round-robin threading model)
////   2) При выделении потоков не контролируется занятость их объектами.
////   
////     В классе VbaComThreadAllocator реализованны механизмы
////   1) Динамическое изменение числа потоков в пуле.
////   2) Механизм выделения потока для определенного COM-объекта 
////      (ReservedThread - LockedThread - FreeThread)
////     При обращении GetThread поток получает статус ReservedThread и 
////       не может быть выделен другим Com-объектам. После СОМ-объект вызывает 
////       методы Lock (при создании объекта FinalConstruct) 
////            и Free (при уничтожении объекта FinalRelease)
////     После этого поток становится свободным для использования.   
////
#pragma once
                   
#include <atlbase.h>
#include <map>

#include "vba_common/vba_singleton.h"

enum ThreadState
{
    FreeThread      = 0,    // Свободный поток свободен для использования
    LockedThread    = 1,    // Занятый поток
    ReservedThread  = 2     // Зарезервированный поток.
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

    
    /// Блокировки апартамента COM-объектом
    ///     (вызывается вначале создания объекта)
    void Lock();

    /// Разблокировки апартамента COM-объектом
    ///     (вызывается при уничтожении объекта) 
    void Free();

	int GetThread(CComApartment** pApt);

private:

    CRITICAL_SECTION m_CS;

    static const int  mc_max_threads = 200;        /// Максимальное кол-во потоков

	int m_nThread;                                  /// Текущее кол-во потоков

    ThreadsMap m_threads_map;
};