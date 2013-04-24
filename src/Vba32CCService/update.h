#pragma once

#include "windows.h"

// COM interface wrapper
#include "interface_wrapper/intrwrp.h"

// Update service interface description
#include "interfaces/VbaUpdateService.h"

// Update events and actions implementation
#include "update_events.h"

// Update COM shell
#include "vba_update_module/update_shell.h"

class VbaCC_Update;

// Initializes all data needed for update
DWORD InitializeUpdate(VbaCC_Update& updater, DWORD source_index, std::tstring& source);
DWORD DoUpdate();

class VbaCC_Update
{
private:
    IVbaUpdate* mp_update;
    InterfaceWrap<IVbaUpdate>* mp_wrapped_update;
    VbaCC_UpdateEvents* mp_update_events;
    IConnectionPointContainer* mp_CPC;
    IConnectionPoint* mp_CP;
    DWORD m_advise_cookie;
    COMUpdateShell* mp_update_shell;

public:
    VbaCC_Update();
    virtual ~VbaCC_Update();
    HRESULT CreateUpdateObject();
    HRESULT BindToConnectionPoint();
    COMUpdateShell* GetUpdateShell() {return mp_update_shell;}
};
