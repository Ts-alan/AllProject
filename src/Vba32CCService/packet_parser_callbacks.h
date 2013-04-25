/// Тестовый пример использования packet_parser.dll
/// Для работы необходимо 
/// 1) Сгенерировать packet_parser.tlb (tlbexp packet_parser.dll в VS command promt)
/// 2) Сгенерировать packet_parser.reg (regasm packet_parser.dll /regfile в VS command promt)
/// 3) packet_parser.dll, packet_parser.tlb поместить в один каталог с ехе-файлом
/// 4) с помощью packet_parser.reg зарегистрировать сокласс в реестре
/// 5) Зарегистрировать packet_parser.tlb
///
/// Для проверки обработчиков событий потери и восстановления соединения необходимо 
/// 1) Реализовать методы 
/// virtual HRESULT STDMETHODCALLTYPE OnConnectionFalse()
/// virtual HRESULT STDMETHODCALLTYPE OnConnectionTrue()
/// интерфейса IPacketParserCallBacks
/// (тестовый пример -- class PacketParserCallBacks)
/// 2) Создать объект Callback-класса
/// 3) Передать объект callback-класса объекту _PacketParser-класса
/// 

#import "packet_parser.tlb" no_namespace, raw_interfaces_only

class PacketParserCallBacks : public IPacketParserCallBacks
{
    public:
    
    PacketParserCallBacks();
    virtual ~PacketParserCallBacks();

    ///Метод интерфейса IPacketParserCallBacks
    virtual HRESULT STDMETHODCALLTYPE OnConnectionFalse();

    ///Метод интерфейса IPacketParserCallBacks
    virtual HRESULT STDMETHODCALLTYPE OnConnectionTrue();

    ///Данный метод необходимо реализовать, так как IPacketParserCallBacks унаследован от IUnknown
    virtual ULONG STDMETHODCALLTYPE AddRef( void);

    ///Данный метод необходимо реализовать, так как IPacketParserCallBacks унаследован от IUnknown
    virtual HRESULT STDMETHODCALLTYPE QueryInterface(REFIID iid, void **ppvObject);

    ///Данный метод необходимо реализовать, так как IPacketParserCallBacks унаследован от IUnknown
    virtual ULONG STDMETHODCALLTYPE Release( void);

    ///Данный метод необходимо реализовать, так как IPacketParserCallBacks унаследован от IDispatch
    virtual HRESULT STDMETHODCALLTYPE GetTypeInfoCount(UINT *p_info_count);

    ///Данный метод необходимо реализовать, так как IPacketParserCallBacks унаследован от IDispatch
    virtual HRESULT STDMETHODCALLTYPE GetTypeInfo(UINT reserved, LCID lcid, ITypeInfo** pp_type_info);

    ///Данный метод необходимо реализовать, так как IPacketParserCallBacks унаследован от IDispatch
    virtual HRESULT STDMETHODCALLTYPE GetIDsOfNames(const IID& iid, LPOLESTR* p_ids, UINT count,LCID lcid, DISPID* p_dispid);

    ///Данный метод необходимо реализовать, так как IPacketParserCallBacks унаследован от IDispatch
    virtual HRESULT STDMETHODCALLTYPE Invoke(DISPID disp_id, const IID& iid, LCID lcid, WORD flags, DISPPARAMS* p_params, 
        VARIANT* p_result, EXCEPINFO* p_exeption_info, UINT* p_error_arguments);

private:
    LONG m_ref_count;
    ITypeInfo* mp_type_info;
};