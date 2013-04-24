#include "stdafx.h"
#include "BusinessLogic.h"
#include "packet_parser_callbacks.h"

BusinessLogic::BusinessLogic():
    mp_parser(0)
{
    InitializeCriticalSection(&m_cs);
}

BusinessLogic::~BusinessLogic()
{
    if (mp_parser)
    {
        VARIANT_BOOL result = VARIANT_FALSE;
        mp_parser->CloseConnection(&result);
    }
    DeleteCriticalSection(&m_cs);
}

// Creates an instance of class
HRESULT BusinessLogic::Initialize()
{
    return (CoCreateInstance( __uuidof(PacketParser),
                                    0,
                                    CLSCTX_ALL,
                                    __uuidof(_PacketParser),
                                    (LPVOID*) &mp_parser));
}

HRESULT BusinessLogic::SetCallbacks()
{
    PacketParserCallBacks* p_callbacks = new PacketParserCallBacks;
    VARIANT_BOOL result = VARIANT_FALSE;
    return (mp_parser->SetCallbacks(p_callbacks, &result));
}

// Singleton implementation
BusinessLogic& BusinessLogic::GetInstance()
{
	static BusinessLogic business_logic_instance;
	return business_logic_instance;
}

bool BusinessLogic::ParseXmlToDB(BSTR xml_fragment)
{
    VARIANT_BOOL result = VARIANT_FALSE;
    HRESULT hr = S_OK;
    EnterCriticalSection(&m_cs);
    hr = mp_parser->ParseXmlToDB(xml_fragment, &result);
    if (FAILED(hr))
    {
        DEBUG_Message1((_T("ParseXmlToDB() failed with hr = 0x%X"), hr));
        LeaveCriticalSection(&m_cs);
        return false;
    }

    if (result == VARIANT_FALSE)
    {
        BSTR last_error = NULL;
        if (SUCCEEDED(mp_parser->GetLastError(&last_error)))
        {
            DEBUG_Message1((_T("Error parsing XML - %s"), vsBSTRToTString(last_error).c_str()));
            SysFreeString(last_error);
        }
    }
    LeaveCriticalSection(&m_cs);

    return (result != VARIANT_FALSE);
}
