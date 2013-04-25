/// �������� ������ ������������� packet_parser.dll
/// ��� ������ ���������� 
/// 1) ������������� packet_parser.tlb (tlbexp packet_parser.dll � VS command promt)
/// 2) ������������� packet_parser.reg (regasm packet_parser.dll /regfile � VS command promt)
/// 3) packet_parser.dll, packet_parser.tlb ��������� � ���� ������� � ���-������
/// 4) � ������� packet_parser.reg ���������������� ������� � �������
/// 5) ���������������� packet_parser.tlb
///
/// ��� �������� ������������ ������� ������ � �������������� ���������� ���������� 
/// 1) ����������� ������ 
/// virtual HRESULT STDMETHODCALLTYPE OnConnectionFalse()
/// virtual HRESULT STDMETHODCALLTYPE OnConnectionTrue()
/// ���������� IPacketParserCallBacks
/// (�������� ������ -- class PacketParserCallBacks)
/// 2) ������� ������ Callback-������
/// 3) �������� ������ callback-������ ������� _PacketParser-������
/// 

#import "packet_parser.tlb" no_namespace, raw_interfaces_only

class PacketParserCallBacks : public IPacketParserCallBacks
{
    public:
    
    PacketParserCallBacks();
    virtual ~PacketParserCallBacks();

    ///����� ���������� IPacketParserCallBacks
    virtual HRESULT STDMETHODCALLTYPE OnConnectionFalse();

    ///����� ���������� IPacketParserCallBacks
    virtual HRESULT STDMETHODCALLTYPE OnConnectionTrue();

    ///������ ����� ���������� �����������, ��� ��� IPacketParserCallBacks ����������� �� IUnknown
    virtual ULONG STDMETHODCALLTYPE AddRef( void);

    ///������ ����� ���������� �����������, ��� ��� IPacketParserCallBacks ����������� �� IUnknown
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID iid, void **ppvObject);

    ///������ ����� ���������� �����������, ��� ��� IPacketParserCallBacks ����������� �� IUnknown
    virtual ULONG STDMETHODCALLTYPE Release( void);

    ///������ ����� ���������� �����������, ��� ��� IPacketParserCallBacks ����������� �� IDispatch
    virtual HRESULT STDMETHODCALLTYPE GetTypeInfoCount(UINT *p_info_count);

    ///������ ����� ���������� �����������, ��� ��� IPacketParserCallBacks ����������� �� IDispatch
    virtual HRESULT STDMETHODCALLTYPE GetTypeInfo(UINT reserved, LCID lcid, ITypeInfo** pp_type_info);

    ///������ ����� ���������� �����������, ��� ��� IPacketParserCallBacks ����������� �� IDispatch
    virtual HRESULT STDMETHODCALLTYPE GetIDsOfNames(const IID& iid, LPOLESTR* p_ids, UINT count,LCID lcid, DISPID* p_dispid);

    ///������ ����� ���������� �����������, ��� ��� IPacketParserCallBacks ����������� �� IDispatch
    virtual HRESULT STDMETHODCALLTYPE Invoke(DISPID disp_id, const IID& iid, LCID lcid, WORD flags, DISPPARAMS* p_params, 
        VARIANT* p_result, EXCEPINFO* p_exeption_info, UINT* p_error_arguments);

private:
    LONG m_ref_count;
    ITypeInfo* mp_type_info;
};