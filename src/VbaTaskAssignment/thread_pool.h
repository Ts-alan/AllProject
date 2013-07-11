#pragma once
#include "common/patterns/object_pool.h"


typedef enum _TaskStatus {
	StatusError,		// задание в карте не найдено
	StatusNotStarted,	// задание есть в карте, но ещё не начато его выполнение
	StatusStarted,		// задание выполняется
	StatusDone,			// задание завершено

	StatusInQueue,		// задание в очереди
	StatusSended,		// задание отправлено
	StatusCanceled		// задание отменено
} TaskStatus;



template<class Thread = VbaThread>
class ThreadWrap : public Thread,
    public vba::PoolObject
{
public:
	virtual void PoolDestroy()
	{
		Thread::StopThread();
		return;
	}
};




template<class T>
class VbaThreadPool
{
	typedef ThreadWrap<T> ThreadPoolObject;
public:	
	bool getObjectThread(T** pp_result)
	{
		ThreadPoolObject* p_object = NULL;
		
		if (m_pool_obj.GetObjectAddRef(&p_object))
		{
			*pp_result = p_object;
			return true;
		}
		return false;
	}

    bool Init(unsigned long threads_number)
	{
		m_pool_obj.SetObjectCounts(threads_number);
		bool res = m_pool_obj.Initialize();	
		return res;
	}
	
	bool Uninitialize()
	{
		return m_pool_obj.Uninitialize();
	}
private:
    vba::ObjectPool<ThreadPoolObject>	m_pool_obj;
};