///////////////////////////////////////////////////////////////////////////////
// File: SocketServer.cpp
///////////////////////////////////////////////////////////////////////////////

#include "SocketServer.h"
#include "ThreadPool.h"

#include "JetByteTools\Win32Tools\Utils.h"
#include "JetByteTools\Win32Tools\Win32Exception.h"

#include "liblogfile/logfile_wrapper.h"

///////////////////////////////////////////////////////////////////////////////
// Using directives
///////////////////////////////////////////////////////////////////////////////

using JetByteTools::Win32::CIOCompletionPort;
using JetByteTools::Win32::CIOBuffer;
using JetByteTools::Win32::Output;
using JetByteTools::Win32::ToString;
using JetByteTools::Win32::_tstring;
using JetByteTools::Win32::CException;
using JetByteTools::Win32::DumpData;
using JetByteTools::Win32::GetLastErrorMessage;
using JetByteTools::Win32::ICriticalSectionFactory;


typedef JetByteTools::Win32::CSocketServer::IOPool IOPool;

///////////////////////////////////////////////////////////////////////////////
// CSocketServer
///////////////////////////////////////////////////////////////////////////////

CSocketServer::CSocketServer(
   const ICriticalSectionFactory &lockFactory,
   IOPool &ioPool,
   unsigned long addressToListenOn,
   unsigned short portToListenOn,
   size_t maxFreeSockets,
   size_t maxFreeBuffers,
   size_t bufferSize,
   size_t numberOfUserDataSlots,
   CThreadPool &pool)
   :  JetByteTools::Win32::CSocketServer(lockFactory, ioPool, addressToListenOn, portToListenOn, maxFreeSockets, maxFreeBuffers, bufferSize, numberOfUserDataSlots),
      m_pool(pool)
{

}

void CSocketServer::OnStartAcceptingConnections()
{
   Output(_T("OnStartAcceptingConnections"));
}

void CSocketServer::OnStopAcceptingConnections()
{
   Output(_T("OnStopAcceptingConnections"));
}
      
void CSocketServer::OnShutdownInitiated()
{
   Output(_T("OnShutdownInitiated"));
}
      
void CSocketServer::OnShutdownComplete()
{
   Output(_T("OnShutdownComplete"));
}

void CSocketServer::OnConnectionEstablished(
   Socket *pSocket,
   CIOBuffer *pAddress)
{
   Output(_T("OnConnectionEstablished"));

   m_pool.DispatchConnectionEstablished(pSocket, pAddress);
}

void CSocketServer::OnConnectionClientClose(
   Socket * /*pSocket*/)
{
   Output(_T("OnConnectionClientClose"));
}

void CSocketServer::OnConnectionReset(
   Socket * /*pSocket*/,
   DWORD lastError)
{
   Output(_T("OnConnectionReset: ") + GetLastErrorMessage(lastError));
}

bool CSocketServer::OnConnectionClosing(
   Socket *pSocket)
{
   Output(_T("OnConnectionClosing"));

   m_pool.DispatchConnectionClosing(pSocket);

   return true;      // We'll handle the close on a worker thread 
}

void CSocketServer::OnSocketReleased(
   Socket *pSocket)
{
   m_pool.OnSocketReleased(pSocket);
}

void CSocketServer::OnConnectionCreated()
{
   Output(_T("OnConnectionCreated"));
}

void CSocketServer::OnConnectionDestroyed()
{
   Output(_T("OnConnectionDestroyed"));
}

void CSocketServer::OnConnectionError(
   ConnectionErrorSource source,
   Socket *pSocket,
   CIOBuffer *pBuffer,
   DWORD lastError)
{
   const LPCTSTR errorSource = (source == ZeroByteReadError ? _T(" Zero Byte Read Error:") : (source == ReadError ? _T(" Read Error:") : _T(" Write Error:")));

   Output(_T("OnConnectionError - Socket = ") + ToString(pSocket) + _T(" Buffer = ") + ToString(pBuffer) + errorSource + GetLastErrorMessage(lastError));
}

void CSocketServer::OnError(
   const JetByteTools::Win32::_tstring &message)
{
   Output(_T("OnError - ") + message);
}

void CSocketServer::OnBufferCreated()
{
   Output(_T("OnBufferCreated"));
}

void CSocketServer::OnBufferAllocated()
{
   Output(_T("OnBufferAllocated"));
}

void CSocketServer::OnBufferReleased()
{
   Output(_T("OnBufferReleased"));
}

void CSocketServer::OnBufferDestroyed()
{
   Output(_T("OnBufferDestroyed"));
}

void CSocketServer::ReadCompleted(
   Socket *pSocket,
   CIOBuffer *pBuffer)
{
   try
   {
      pBuffer = ProcessDataStream(pSocket, pBuffer);

      pSocket->Read(pBuffer);
   }
   catch(const CException &e)
   {
      Output(_T("ReadCompleted - Exception - ") + e.GetWhere() + _T(" - ") + e.GetMessage());
      pSocket->Shutdown();
   }
   catch(...)
   {
      Output(_T("ReadCompleted - Unexpected exception"));
      pSocket->Shutdown();
   }
}


CIOBuffer *CSocketServer::ProcessDataStream(
   Socket *pSocket,
   CIOBuffer *pBuffer) const
{
    bool done;

    // ÒÓÒÀ ÍÀÏÈÑÀÒÜ ÑÂÎÅ

    do
    {
        done = true;

        const size_t used = pBuffer->GetUsed();
        
        if (used >= GetMinimumMessageSize())
        {
            const size_t messageSize = GetMessageSize(pBuffer);
            _tstring msg1 = _T("CSocketServer::ProcessDataStream(): Got message, used = ") + 
                            ToString(used) + _T(", messageSize = ") + ToString(messageSize);
            Output(msg1);

            if (used == messageSize)
            {
                // we have a whole, distinct, message
                Output(_T("CSocketServer::ProcessDataStream(): Got complete, distinct message"));

                ProcessCommand(pSocket, pBuffer);

                pBuffer = 0;

                done = true;
            }
            else if (used > messageSize)
            {
                Output(_T("CSocketServer::ProcessDataStream(): Got message plus extra data"));
                // we have a message, plus some more data

                // allocate a new buffer, copy the extra data into it and try again...

                CIOBuffer *pMessage = pBuffer->SplitBuffer(messageSize);

                ProcessCommand(pSocket, pMessage);

                pMessage->Release();

                // loop again, we may have another complete message in there...

                done = false;
            }
            else if (messageSize > pBuffer->GetSize())
            {
                _tstring msg = _T("CSocketServer::ProcessDataStream(): error: buffer too small\nExpecting: ") + ToString(messageSize) +
                    _T("\nGot: ") + ToString(pBuffer->GetUsed()) + _T("\nBuffer size = ") + 
                    ToString(pBuffer->GetSize()) + _T("\nData = ") + 
                    DumpData(pBuffer->GetBuffer(), pBuffer->GetUsed());
                Output(msg);

                pSocket->Shutdown();
                Output(_T("CSocketServer::ProcessDataStream(): Disconnecting client..."));

                // throw the rubbish away
                pBuffer->Empty();

                done = true;
            }
        }
    }
    while (!done);

    // not enough data in the buffer, reissue a read into the same buffer to collect more data
    return pBuffer;
}

size_t CSocketServer::GetMinimumMessageSize() const
{
   // ÒÓÒÀ ÍÀÏÈÑÀÒÜ ÑÂÎÅ

   // The smallest possible packet we accept is 2-byte header
   return 2;
}

size_t CSocketServer::GetMessageSize(const CIOBuffer *pBuffer) const
{
    // ÒÓÒÀ ÍÀÏÈÑÀÒÜ ÑÂÎÅ

    size_t messageSize = *((const WORD*)pBuffer->GetBuffer());

    return (messageSize > 1) ? messageSize : 2;
}

void CSocketServer::ProcessCommand(
   Socket *pSocket,
   CIOBuffer *pBuffer) const
{
    Output(_T("CSocketServer::ProcessCommand()"));
    m_pool.DispatchReadCompleted(pSocket, pBuffer);
}

///////////////////////////////////////////////////////////////////////////////
// End of file: SocketServer.cpp
///////////////////////////////////////////////////////////////////////////////
