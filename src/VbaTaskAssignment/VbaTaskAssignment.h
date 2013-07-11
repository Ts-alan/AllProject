

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0500 */
/* at Fri Jun 28 16:17:58 2013
 */
/* Compiler settings for .\VbaTaskAssignment.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 440
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __VbaTaskAssignment_h__
#define __VbaTaskAssignment_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __ITaskService_FWD_DEFINED__
#define __ITaskService_FWD_DEFINED__
typedef interface ITaskService ITaskService;
#endif 	/* __ITaskService_FWD_DEFINED__ */


#ifndef __TaskService_FWD_DEFINED__
#define __TaskService_FWD_DEFINED__

#ifdef __cplusplus
typedef class TaskService TaskService;
#else
typedef struct TaskService TaskService;
#endif /* __cplusplus */

#endif 	/* __TaskService_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __ITaskService_INTERFACE_DEFINED__
#define __ITaskService_INTERFACE_DEFINED__

/* interface ITaskService */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_ITaskService;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("4D8F6784-B7D3-46A4-BC29-9F4209AE93EA")
    ITaskService : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE PacketSystemInfo( 
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE PacketCreateProcess( 
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses,
            /* [in] */ BSTR cmd_line) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE PacketSendFile( 
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses,
            /* [in] */ BSTR src_path,
            BSTR dst_path) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE PacketConfigureSettings( 
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses,
            /* [in] */ BSTR settings) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE PacketComponentState( 
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE PacketCustomAction( 
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses,
            /* [in] */ BSTR options) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE PacketCancelTask( 
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE PacketListProcesses( 
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct ITaskServiceVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ITaskService * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ITaskService * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ITaskService * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ITaskService * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ITaskService * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ITaskService * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ITaskService * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *PacketSystemInfo )( 
            ITaskService * This,
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *PacketCreateProcess )( 
            ITaskService * This,
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses,
            /* [in] */ BSTR cmd_line);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *PacketSendFile )( 
            ITaskService * This,
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses,
            /* [in] */ BSTR src_path,
            BSTR dst_path);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *PacketConfigureSettings )( 
            ITaskService * This,
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses,
            /* [in] */ BSTR settings);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *PacketComponentState )( 
            ITaskService * This,
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *PacketCustomAction )( 
            ITaskService * This,
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses,
            /* [in] */ BSTR options);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *PacketCancelTask )( 
            ITaskService * This,
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *PacketListProcesses )( 
            ITaskService * This,
            /* [in] */ SAFEARRAY * *p_task_ids,
            /* [in] */ SAFEARRAY * *p_ip_addresses);
        
        END_INTERFACE
    } ITaskServiceVtbl;

    interface ITaskService
    {
        CONST_VTBL struct ITaskServiceVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ITaskService_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ITaskService_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ITaskService_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ITaskService_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ITaskService_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ITaskService_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ITaskService_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define ITaskService_PacketSystemInfo(This,p_task_ids,p_ip_addresses)	\
    ( (This)->lpVtbl -> PacketSystemInfo(This,p_task_ids,p_ip_addresses) ) 

#define ITaskService_PacketCreateProcess(This,p_task_ids,p_ip_addresses,cmd_line)	\
    ( (This)->lpVtbl -> PacketCreateProcess(This,p_task_ids,p_ip_addresses,cmd_line) ) 

#define ITaskService_PacketSendFile(This,p_task_ids,p_ip_addresses,src_path,dst_path)	\
    ( (This)->lpVtbl -> PacketSendFile(This,p_task_ids,p_ip_addresses,src_path,dst_path) ) 

#define ITaskService_PacketConfigureSettings(This,p_task_ids,p_ip_addresses,settings)	\
    ( (This)->lpVtbl -> PacketConfigureSettings(This,p_task_ids,p_ip_addresses,settings) ) 

#define ITaskService_PacketComponentState(This,p_task_ids,p_ip_addresses)	\
    ( (This)->lpVtbl -> PacketComponentState(This,p_task_ids,p_ip_addresses) ) 

#define ITaskService_PacketCustomAction(This,p_task_ids,p_ip_addresses,options)	\
    ( (This)->lpVtbl -> PacketCustomAction(This,p_task_ids,p_ip_addresses,options) ) 

#define ITaskService_PacketCancelTask(This,p_task_ids,p_ip_addresses)	\
    ( (This)->lpVtbl -> PacketCancelTask(This,p_task_ids,p_ip_addresses) ) 

#define ITaskService_PacketListProcesses(This,p_task_ids,p_ip_addresses)	\
    ( (This)->lpVtbl -> PacketListProcesses(This,p_task_ids,p_ip_addresses) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ITaskService_INTERFACE_DEFINED__ */



#ifndef __VbaTaskAssignmentLib_LIBRARY_DEFINED__
#define __VbaTaskAssignmentLib_LIBRARY_DEFINED__

/* library VbaTaskAssignmentLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_VbaTaskAssignmentLib;

EXTERN_C const CLSID CLSID_TaskService;

#ifdef __cplusplus

class DECLSPEC_UUID("DE6C9F17-C542-4B66-B253-ABFB2B4DF397")
TaskService;
#endif
#endif /* __VbaTaskAssignmentLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  BSTR_UserSize(     unsigned long *, unsigned long            , BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserMarshal(  unsigned long *, unsigned char *, BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserUnmarshal(unsigned long *, unsigned char *, BSTR * ); 
void                      __RPC_USER  BSTR_UserFree(     unsigned long *, BSTR * ); 

unsigned long             __RPC_USER  LPSAFEARRAY_UserSize(     unsigned long *, unsigned long            , LPSAFEARRAY * ); 
unsigned char * __RPC_USER  LPSAFEARRAY_UserMarshal(  unsigned long *, unsigned char *, LPSAFEARRAY * ); 
unsigned char * __RPC_USER  LPSAFEARRAY_UserUnmarshal(unsigned long *, unsigned char *, LPSAFEARRAY * ); 
void                      __RPC_USER  LPSAFEARRAY_UserFree(     unsigned long *, LPSAFEARRAY * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


