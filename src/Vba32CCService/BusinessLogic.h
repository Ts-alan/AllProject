#pragma once


#import "packet_parser.tlb" no_namespace, raw_interfaces_only


#define business_logic BusinessLogic::GetInstance()

class BusinessLogic
{
private:
    _PacketParser* mp_parser;
    CRITICAL_SECTION m_cs;
public:
    // Creates an instance of coclass
    HRESULT Initialize();
    HRESULT SetCallbacks();

    static BusinessLogic& GetInstance();

    bool ParseXmlToDB(BSTR xml_fragment);
protected:
    // Forbidden operations
    BusinessLogic();
    BusinessLogic(const BusinessLogic&);
    BusinessLogic& operator=(const BusinessLogic&);
    virtual ~BusinessLogic();
};
