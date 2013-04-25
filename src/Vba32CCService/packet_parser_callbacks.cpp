/*
�������� ������ ������������� packet_parser.dll
��� ������ ���������� 
1) ������������� packet_parser.tlb (tlbexp packet_parser.dll � VS command promt)
2) ������������� packet_parser.reg (regasm packet_parser.dll /regfile � VS command promt)
3) packet_parser.dll, packet_parser.tlb ��������� � ���� ������� � ���-������
4) � ������� packet_parser.reg ���������������� ������� � �������
5) ���������������� packet_parser.tlb

��� �������� ������������ ������� ������ � �������������� ���������� ���������� 
1) ����������� ������ 
HRESULT STDMETHODCALLTYPE OnConnectionFalse()
HRESULT STDMETHODCALLTYPE OnConnectionTrue()
���������� IPacketParserCallBacks
(�������� ������ -- class PacketParserCallBacks)
2) ������� ������ Callback-������
3) �������� ������ callback-������ ������� _PacketParser-������
*/ 

#include "stdafx.h"
#include "packet_parser_callbacks.h"
#include "network.h"
#include "startup_routines.h"

PacketParserCallBacks::PacketParserCallBacks() : m_ref_count(0)
{
    mp_type_info = NULL;
    ITypeLib* p_type_lib = NULL;
    HRESULT hr = LoadRegTypeLib(__uuidof(__packet_parser), 3, 12, NULL, &p_type_lib);
    if (SUCCEEDED(hr))
    {
        hr = p_type_lib->GetTypeInfoOfGuid(__uuidof(IPacketParserCallBacks), &mp_type_info);
        if (p_type_lib)
        {
            p_type_lib->Release();
        }
    }
}

PacketParserCallBacks::~PacketParserCallBacks()
{
    if (mp_type_info)
    {
        mp_type_info->Release();
    }
}

// ����� ���������� IPacketParserCallBacks
HRESULT STDMETHODCALLTYPE PacketParserCallBacks::OnConnectionFalse()
{
    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_DB_CONNECT_OFF));
    // Shutting down network
    ShutdownNetwork();
    return S_OK;
}

// ����� ���������� IPacketParserCallBacks
HRESULT STDMETHODCALLTYPE PacketParserCallBacks::OnConnectionTrue()
{
    p_logfile->AddToLog(LoadStringFromResource(IDS_LOG_DB_CONNECT_ON));
    // Starting network
    if (CheckKeyFileValidity(FALSE))
    {
        StartupNetwork();
    }
    return S_OK;
}

// ������ ����� ���������� �����������, ��� ��� IPacketParserCallBacks ����������� �� IUnknown
ULONG STDMETHODCALLTYPE PacketParserCallBacks::AddRef()
{
    return InterlockedIncrement(&m_ref_count);
}

// ������ ����� ���������� �����������, ��� ��� IPacketParserCallBacks ����������� �� IUnknown
HRESULT STDMETHODCALLTYPE PacketParserCallBacks::QueryInterface(REFIID iid, void** pp_object)
{   
    if (iid == IID_IUnknown)
    {
        *pp_object = (IPacketParserCallBacks*)this;
    }
    else if (iid == IID_IDispatch)
    {
        *pp_object = (IDispatch*)this;
    }
    else if (iid == __uuidof(IPacketParserCallBacks))
    {
        *pp_object = (IPacketParserCallBacks*)this;
    }
    else
    {
        return E_NOINTERFACE;
    }
    AddRef();
    return S_OK;
}

// ������ ����� ���������� �����������, ��� ��� IPacketParserCallBacks ����������� �� IUnknown
ULONG STDMETHODCALLTYPE PacketParserCallBacks::Release()
{
    if (InterlockedDecrement(&m_ref_count) == 0)
    {
        delete this;
        return 0; 
    } 
    return (m_ref_count);
}

// ������ ����� ���������� �����������, ��� ��� IPacketParserCallBacks ����������� �� IDispatch
HRESULT STDMETHODCALLTYPE PacketParserCallBacks::GetTypeInfoCount(UINT *p_info_count)
{
    if (p_info_count == NULL) 
    {
        return E_POINTER;
    }
    *p_info_count = 1;
    return S_OK;
}

// ������ ����� ���������� �����������, ��� ��� IPacketParserCallBacks ����������� �� IDispatch
HRESULT STDMETHODCALLTYPE PacketParserCallBacks::GetTypeInfo(UINT reserved, LCID lcid, ITypeInfo** pp_type_info)
{
    if (reserved == 0 && pp_type_info != NULL && mp_type_info != NULL)
    {
        *pp_type_info = mp_type_info;
        mp_type_info->AddRef();
        return S_OK;
    }

    return E_INVALIDARG;
}

// ������ ����� ���������� �����������, ��� ��� IPacketParserCallBacks ����������� �� IDispatch
HRESULT STDMETHODCALLTYPE PacketParserCallBacks::GetIDsOfNames(const IID& iid, LPOLESTR* p_ids, UINT count,LCID lcid, DISPID* p_dispid)
{
    if (iid == IID_NULL && mp_type_info != NULL)
    {
        return mp_type_info->GetIDsOfNames(p_ids, count, p_dispid);
    }
    return E_INVALIDARG;
}

// ������ ����� ���������� �����������, ��� ��� IPacketParserCallBacks ����������� �� IDispatch
HRESULT STDMETHODCALLTYPE PacketParserCallBacks::Invoke(DISPID disp_id, const IID& iid, LCID lcid, WORD flags, DISPPARAMS* p_params, 
    VARIANT* p_result, EXCEPINFO* p_exeption_info, UINT* p_error_arguments)
{
    if (iid == IID_NULL && mp_type_info != NULL)
    {
        void* p_this = static_cast<IPacketParserCallBacks*>(this);
        return mp_type_info->Invoke(p_this, disp_id, flags, p_params, p_result, p_exeption_info, p_error_arguments);
    }
    return E_INVALIDARG;
}