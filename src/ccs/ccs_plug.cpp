#include "ccs_plug.h"

#include "common/common.h"
#include "common/log.h"
#include "common/process.h"

#include <boost/algorithm/string.hpp>
#include <boost/foreach.hpp>
#include <boost/regex.hpp>

#include "settings/settings_define.h"
#include "interfaces/i_settings.h"
#include "settings/settings_use.h"
#include "version/version.h"
#include <fstream>


extern const wchar_t c_cc_setupdll_name[] = L"ccs.dll";

namespace vba{
namespace cc{


bool GetVersionFromIni(const std::wstring& file_path, std::wstring& version)
{
    std::wstring line;
    std::wifstream ini_file(file_path);    
    if(!ini_file)
    {
        return false;
    }

    while(ini_file)
    {
        std::getline(ini_file, line);
        if(boost::starts_with(line, L"Version"))
        {
            boost::match_results<std::wstring::const_iterator> what;
            boost::regex exp("([0-9]{1,4}\.[0-9]{1,4}\.[0-9]{1,4}\.[0-9]{1,4})");
            if(!boost::regex_search(line.cbegin(), line.cend(), what, exp))
            {
                return false;
            }
            version = std::wstring(what[1].first, what[1].second);
            return true;
        }
    }

    return false;
}


class CCSettings: public vba::settings::SettingsUse<&__uuidof(VBA_CC)>
{
    WStringList c_update_groups;

    PARAMETERS_MAP_BEGIN()
        PARAMETER(UpdateGroups, c_update_groups)
    PARAMETERS_MAP_END()

        GET_SET(WStringList, UpdateGroups)
public:
    CCSettings()
    {
        c_update_groups.push_back(L"REDIST");
        c_update_groups.push_back(L"SERVICE");
        c_update_groups.push_back(L"TASKS");
        c_update_groups.push_back(L"TXT");
        c_update_groups.push_back(L"UPDATE");
    }
};


bool CCPlug::GetVesion(std::wstring& version)
{
    // TODO ???
    return true;
}

bool CCPlug::ReactionOnUpdate(const WStringList& files, const std::wstring& version,
                                uint32_t& update_actions)
{
    update_actions = upd::None;
    if(!files.empty())
    {
        update_actions = upd::NeedReinstall;
    }
    return true;
}

bool CCPlug::Install(const SetupParams& param)
{
    LOG() % "Install";
    WStringMap params = param.parameters;
    std::wstring install_path = params[L"path"];
    if(install_path.empty())
    {
        return false;
    }

    // HACK get old version from settings
    ObjectPtr<ISettings> settings;
    if(!param.p_service->GetInterfaceT<ISettings, &__uuidof(VBA_SETTINGS)>(&settings))
    {
        LOG_ERROR() % L"Fail to get Settings";
        return false;
    }

    Param cur_version_param(L"OldVersion", NormalParam);
    if(!settings->GetParameter(__uuidof(VBA_CC), cur_version_param))
    {
        LOG_WARN() % L"OldVersion Not Finded";
        return true;
    }
    settings->DelParameter(__uuidof(VBA_CC), cur_version_param);

    std::wstring old_version = boost::get<std::wstring>(cur_version_param.m_value);

    // get new version
    std::wstring new_version;
    if(!GetVersionFromIni(install_path + L"\\VBA32AAW.ini", new_version))
    {
        LOG_ERROR() % L"Fail to get version of VBA32AAW";
        return false;
    }

    std::wstring cmdline = install_path + L"\\Vba32ControlCenterUpdate.exe ActionAfterReplaceFiles"; 
    cmdline += L" " + old_version;
    cmdline += L" " + new_version; 
    cmdline += L" " + params[L"files"]; // <file_path>|<file_path>|...

    LOG() % cmdline;
    return vba::LaunchApp(L"", cmdline, true);
}

bool CCPlug::Uninstall(const SetupParams& param)
{
    LOG() % "Uninstall";
    WStringMap params = param.parameters;
    std::wstring install_path = params[L"path"];
    if(install_path.empty())
    {
        return false;
    }


    // HACK set old version to settings
    std::wstring cur_version;
    if(!GetVersionFromIni(install_path + L"\\VBA32AAW.ini", cur_version))
    {
        LOG_ERROR() % L"Fail to get version of VBA32AAW";
        return false;
    }
    ObjectPtr<ISettings> settings;
    if(!param.p_service->GetInterfaceT<ISettings, &__uuidof(VBA_SETTINGS)>(&settings))
    {
        LOG_ERROR() % L"Fail to get Settings";
        return false;
    }

    Param cur_version_param(L"OldVersion", cur_version, NormalParam);
    if(!settings->SetParameter(__uuidof(VBA_CC), cur_version_param))
    {
        LOG_ERROR() % L"Fail to set OldVersion";
        return false;
    }
    //

    std::wstring cmdline = install_path + L"\\Vba32ControlCenterUpdate.exe ActionBeforeReplaceFiles"; 
    cmdline += L" " + params[L"files"]; // <file_path>|<file_path>|...

    LOG() % cmdline;
    return vba::LaunchApp(L"", cmdline, true);
}


bool CCPlug::Initialize(const InterfaceId& interface_id, vba::IObject* p_object)
{
    LOG() % "Initialize";

    if(!__super::Initialize(interface_id, p_object))
    {
        LOG_ERROR() % L"Fail to base initialize";
        return false;
    }
    ObjectPtr<ISettings> settings;
    if(!mp_service->GetInterfaceT<ISettings, &__uuidof(VBA_SETTINGS)>(&settings))
    {
        LOG_ERROR() % L"Fail to get settings";
        return false;
    }

    CCSettings cc_settings;
    IF_NOT_RETURN_AND_LOG(cc_settings.Initialize(settings.get()), false, L"Fail to init setttings");

    return true;
}

bool CCPlug::Uninitialize(const InterfaceId& interface_id, vba::IObject* p_object)
{
    LOG() % L"Uninitialize";
    return __super::Uninitialize(interface_id, p_object);
}

}
}
