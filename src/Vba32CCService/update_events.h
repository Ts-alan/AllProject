#pragma once

// interface
#include "interfaces/VbaUpdateService.h"

class VbaCC_UpdateEvents : public _IVbaUpdateEvents
{
public:
    VbaCC_UpdateEvents(void);
    ~VbaCC_UpdateEvents(void);

    HRESULT STDMETHODCALLTYPE QueryInterface(REFIID iid, void **ppvObject);
	ULONG STDMETHODCALLTYPE AddRef();
	ULONG STDMETHODCALLTYPE Release();

    virtual HRESULT STDMETHODCALLTYPE OnUpdateStarted(void);
	virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnUpdateStopped(void);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnUpdateFailed( 
            /* [in] */ long error_code,
            /* [in] */ BSTR error_msg);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnUpdateCompleted( void);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnUpdateInitialized( 
            unsigned long update_file_count,
            unsigned long total_size);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnUpdateMessage( 
            /* [in] */ BSTR update_msg);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnStartFileListDownload(void);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnFinishFileListDownload(void);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnFileListDownloadFailed( 
            /* [in] */ BSTR file_name,
            /* [in] */ long error_code);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnStartFileDownload( 
            BSTR file_name);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnFinishFileDownload( 
            BSTR file_name);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnFileDownloadFailed( 
            /* [in] */ BSTR file_name,
            /* [in] */ BSTR error,
            /* [in] */ long error_code);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnUpdateProgress( 
            /* [in] */ unsigned long current_size,
            /* [in] */ unsigned long total_size);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnChangeStatus( 
            /* [in] */ long previous_status,
            /* [in] */ long new_status);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnGetFileUpdate( 
            /* [in] */ BSTR group_name,
            /* [in] */ BSTR file_name,
            /* [out] */ BOOL *p_update_needed);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnGetFileOperation( 
            /* [in] */ BSTR group_name,
            /* [in] */ BSTR file_name,
            /* [out] */ BSTR *p_str_param,
            /* [out] */ unsigned long *p_num_param,
            /* [out] */ long *p_operation_to_perform);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnFileOperationResult( 
            /* [in] */ BSTR group_name,
            /* [in] */ BSTR file_name,
            /* [in] */ long operation_result);

        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnFileReplaceFailed( 
            /* [in] */ BSTR file_name);
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE OnConnectionFailed( 
            /* [in] */ long connection_error);

private:
	LONG m_ref_count;
};
