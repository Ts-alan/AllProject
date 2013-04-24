///////////////////////////////////////////////////////////////////////////////
// File: ThreadPoolWorkerThread.cpp
/////////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#include "ThreadPool.h"
#include "ThreadPoolWorkerThread.h"

#include "JetByteTools\Win32Tools\IOBuffer.h"
#include "JetByteTools\Win32Tools\SocketServer.h"
#include "JetByteTools\Win32Tools\Exception.h"
#include "JetByteTools\Win32Tools\Utils.h"
#include "JetByteTools\Win32Tools\tstring.h"
#include "JetByteTools\Win32Tools\SocketAddress.h"

#include <atlbase.h>       // USES_CONVERSION

#include "BusinessLogic.h"

///////////////////////////////////////////////////////////////////////////////
// Using directives
///////////////////////////////////////////////////////////////////////////////

using JetByteTools::Win32::CIOBuffer;
using JetByteTools::Win32::CIOCompletionPort;
using JetByteTools::Win32::CEvent;
using JetByteTools::Win32::CSocketServer;
using JetByteTools::Win32::CException;
using JetByteTools::Win32::Output;
using JetByteTools::Win32::_tstring;
using JetByteTools::Win32::CSocketAddress;

typedef JetByteTools::Win32::CSocketServer::Socket Socket;

///////////////////////////////////////////////////////////////////////////////
// CThreadPoolWorkerThread
///////////////////////////////////////////////////////////////////////////////

CThreadPoolWorkerThread::CThreadPoolWorkerThread(
   CIOCompletionPort &iocp,
   CEvent &messageReceivedEvent,
   CThreadPool &pool)
   :  JetByteTools::Win32::CThreadPool::WorkerThread(iocp, messageReceivedEvent, pool),
      m_pool(pool)
{
}
      
bool CThreadPoolWorkerThread::Initialise()
{
   Output(_T("CThreadPoolWorkerThread::Initialise"));

   return true;
}

void CThreadPoolWorkerThread::Process(
   ULONG_PTR completionKey,
   DWORD operation,
   OVERLAPPED *pOverlapped)
{
   Socket *pSocket = reinterpret_cast<Socket *>(completionKey);
   CIOBuffer *pBuffer = static_cast<CIOBuffer *>(pOverlapped);
   
   try
   {
      switch(operation)
      {
         case CThreadPool::ConnectionEstablished :    
      
            OnConnectionEstablished(pSocket, pBuffer);

         break;

         case CThreadPool::ReadCompleted :    

            ProcessMessage(pSocket, pBuffer);

         break;

         case CThreadPool::ConnectionClosing :    

            OnConnectionClosing(pSocket);

         break;

         default :

            // do nothing

         break;
      }
   }
   catch(const CException &e)
   {
      Output(_T("Process - Exception - ") + e.GetWhere() + _T(" - ") + e.GetMessage());
      pSocket->Shutdown();
   }
   catch(...)
   {
      Output(_T("Process - Unexpected exception"));
      pSocket->Shutdown();
   }

   pSocket->Release();

   if (pBuffer)
   {
      pBuffer->Release();
   }
}

void CThreadPoolWorkerThread::Shutdown()
{
   Output(_T("CThreadPoolWorkerThread::Shutdown()"));
}

void CThreadPoolWorkerThread::OnConnectionEstablished(
   Socket *pSocket,
   CIOBuffer *pAddress)
{
   CThreadPool::CPerConnectionData *pData = m_pool.GetPerConnectionData(pSocket);

   //lint -e{826} Suspicious pointer-to-pointer conversion (area too small)
   CSocketAddress address(reinterpret_cast<const sockaddr*>(pAddress->GetBuffer()));

   pData->SetAddress(address.AsString());

   // ÒÓÒÀ ÍÀÏÈÑÀÒÜ ÑÂÎÅ

   Output(_T("CThreadPoolWorkerThread::OnConnectionEstablished(): client connected: ") + pData->GetConnectionDetails());

   pSocket->Read();

   //lint -e{1762} Member function could be made const

   //lint -e{818} Pointer parameter 'pAddress' (line 215) could be declared as pointing to const
}

void CThreadPoolWorkerThread::OnConnectionClosing(
   Socket *pSocket)
{
   CThreadPool::CPerConnectionData *pData = m_pool.GetPerConnectionData(pSocket);
   
   // We'll perform a lingering close on this thread
   Output(_T("CThreadPoolWorkerThread::OnConnectionClosing(): closing connection with ") + pData->GetConnectionDetails());

   pSocket->Close();

   //lint -e{1762} Member function could be made const
}

void CThreadPoolWorkerThread::ProcessMessage(
   Socket *pSocket,
   const CIOBuffer *pBuffer) const
{
    Output(_T("CThreadPoolWorkerThread::ProcessMessage()"));
    CThreadPool::CPerConnectionData *p_data = m_pool.GetPerConnectionData(pSocket);

    // ÒÓÒÀ ÍÀÏÈÑÀÒÜ ÑÂÎÅ
    size_t data_length = pBuffer->GetUsed();
    std::string data_utf8((LPSTR)(pBuffer->GetBuffer() + sizeof(WORD)), data_length - sizeof(WORD));

    // Getting packet data
	CAutoFreeBSTR packet = SysAllocString(vsUtf8ToWString(data_utf8).c_str());
    Output(packet);
    std::tstring packet_for_log(vsUtf8ToTString(data_utf8).c_str(), 20);
    if (!business_logic.ParseXmlToDB(packet))
    {
        p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_XML_PARSING_ERROR), packet_for_log.c_str(), p_data->GetConnectionDetails().c_str());
    }
    else
    {
        p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_XML_PARSED_OK), packet_for_log.c_str(), p_data->GetConnectionDetails().c_str());
    }
}

///////////////////////////////////////////////////////////////////////////////
// End of file: ThreadPoolWorkerThread.cpp
///////////////////////////////////////////////////////////////////////////////
