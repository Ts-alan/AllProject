#pragma once

#include "cc_define.h"

#include "plug/plug.h"
#include "update/base_setup.h"


namespace vba{
namespace cc{

extern const wchar_t c_cc_setupdll_name[];

class CCPlug : public vba::plug::Plug<Object, &__uuidof(VBA_CC), 3000>,
               public upd::BaseSetup<c_cc_setupdll_name>
{
public:
    // ISetup
    virtual bool Install(const SetupParams& param);
    virtual bool Uninstall(const SetupParams& param);

    // IUpdateSetup
    virtual bool GetVesion(std::wstring& version);
    virtual bool ReactionOnUpdate(const WStringList& files, const std::wstring& version,
        uint32_t& update_actions);



    virtual bool Initialize(const InterfaceId& interface_id, vba::IObject* p_object);
};


}
}
