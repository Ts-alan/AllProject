

/* this ALWAYS GENERATED file contains the proxy stub code */


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

#if !defined(_M_IA64) && !defined(_M_AMD64)


#pragma warning( disable: 4049 )  /* more than 64k source lines */
#if _MSC_VER >= 1200
#pragma warning(push)
#endif

#pragma warning( disable: 4211 )  /* redefine extern to static */
#pragma warning( disable: 4232 )  /* dllimport identity*/
#pragma warning( disable: 4024 )  /* array to pointer mapping*/
#pragma warning( disable: 4152 )  /* function/data pointer conversion in expression */
#pragma warning( disable: 4100 ) /* unreferenced arguments in x86 call */

#pragma optimize("", off ) 

#define USE_STUBLESS_PROXY


/* verify that the <rpcproxy.h> version is high enough to compile this file*/
#ifndef __REDQ_RPCPROXY_H_VERSION__
#define __REQUIRED_RPCPROXY_H_VERSION__ 440
#endif


#include "rpcproxy.h"
#ifndef __RPCPROXY_H_VERSION__
#error this stub requires an updated version of <rpcproxy.h>
#endif // __RPCPROXY_H_VERSION__


#include "VbaTaskAssignment.h"

#define TYPE_FORMAT_STRING_SIZE   1031                              
#define PROC_FORMAT_STRING_SIZE   303                               
#define EXPR_FORMAT_STRING_SIZE   1                                 
#define TRANSMIT_AS_TABLE_SIZE    0            
#define WIRE_MARSHAL_TABLE_SIZE   2            

typedef struct _VbaTaskAssignment_MIDL_TYPE_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ TYPE_FORMAT_STRING_SIZE ];
    } VbaTaskAssignment_MIDL_TYPE_FORMAT_STRING;

typedef struct _VbaTaskAssignment_MIDL_PROC_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ PROC_FORMAT_STRING_SIZE ];
    } VbaTaskAssignment_MIDL_PROC_FORMAT_STRING;

typedef struct _VbaTaskAssignment_MIDL_EXPR_FORMAT_STRING
    {
    long          Pad;
    unsigned char  Format[ EXPR_FORMAT_STRING_SIZE ];
    } VbaTaskAssignment_MIDL_EXPR_FORMAT_STRING;


static RPC_SYNTAX_IDENTIFIER  _RpcTransferSyntax = 
{{0x8A885D04,0x1CEB,0x11C9,{0x9F,0xE8,0x08,0x00,0x2B,0x10,0x48,0x60}},{2,0}};


extern const VbaTaskAssignment_MIDL_TYPE_FORMAT_STRING VbaTaskAssignment__MIDL_TypeFormatString;
extern const VbaTaskAssignment_MIDL_PROC_FORMAT_STRING VbaTaskAssignment__MIDL_ProcFormatString;
extern const VbaTaskAssignment_MIDL_EXPR_FORMAT_STRING VbaTaskAssignment__MIDL_ExprFormatString;


extern const MIDL_STUB_DESC Object_StubDesc;


extern const MIDL_SERVER_INFO ITaskService_ServerInfo;
extern const MIDL_STUBLESS_PROXY_INFO ITaskService_ProxyInfo;


extern const USER_MARSHAL_ROUTINE_QUADRUPLE UserMarshalRoutines[ WIRE_MARSHAL_TABLE_SIZE ];

#if !defined(__RPC_WIN32__)
#error  Invalid build platform for this stub.
#endif

#if !(TARGET_IS_NT40_OR_LATER)
#error You need a Windows NT 4.0 or later to run this stub because it uses these features:
#error   -Oif or -Oicf, [wire_marshal] or [user_marshal] attribute.
#error However, your C/C++ compilation flags indicate you intend to run this app on earlier systems.
#error This app will fail with the RPC_X_WRONG_STUB_VERSION error.
#endif


static const VbaTaskAssignment_MIDL_PROC_FORMAT_STRING VbaTaskAssignment__MIDL_ProcFormatString =
    {
        0,
        {

	/* Procedure PacketSystemInfo */

			0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/*  2 */	NdrFcLong( 0x0 ),	/* 0 */
/*  6 */	NdrFcShort( 0x7 ),	/* 7 */
/*  8 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 10 */	NdrFcShort( 0x0 ),	/* 0 */
/* 12 */	NdrFcShort( 0x8 ),	/* 8 */
/* 14 */	0x6,		/* Oi2 Flags:  clt must size, has return, */
			0x3,		/* 3 */

	/* Parameter p_task_ids */

/* 16 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 18 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 20 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Parameter p_ip_addresses */

/* 22 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 24 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 26 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Return value */

/* 28 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 30 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 32 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure PacketCreateProcess */

/* 34 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 36 */	NdrFcLong( 0x0 ),	/* 0 */
/* 40 */	NdrFcShort( 0x8 ),	/* 8 */
/* 42 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 44 */	NdrFcShort( 0x0 ),	/* 0 */
/* 46 */	NdrFcShort( 0x8 ),	/* 8 */
/* 48 */	0x6,		/* Oi2 Flags:  clt must size, has return, */
			0x4,		/* 4 */

	/* Parameter p_task_ids */

/* 50 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 52 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 54 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Parameter p_ip_addresses */

/* 56 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 58 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 60 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Parameter cmd_line */

/* 62 */	NdrFcShort( 0x8b ),	/* Flags:  must size, must free, in, by val, */
/* 64 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 66 */	NdrFcShort( 0x3fc ),	/* Type Offset=1020 */

	/* Return value */

/* 68 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 70 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 72 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure PacketSendFile */

/* 74 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 76 */	NdrFcLong( 0x0 ),	/* 0 */
/* 80 */	NdrFcShort( 0x9 ),	/* 9 */
/* 82 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 84 */	NdrFcShort( 0x0 ),	/* 0 */
/* 86 */	NdrFcShort( 0x8 ),	/* 8 */
/* 88 */	0x6,		/* Oi2 Flags:  clt must size, has return, */
			0x5,		/* 5 */

	/* Parameter p_task_ids */

/* 90 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 92 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 94 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Parameter p_ip_addresses */

/* 96 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 98 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 100 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Parameter src_path */

/* 102 */	NdrFcShort( 0x8b ),	/* Flags:  must size, must free, in, by val, */
/* 104 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 106 */	NdrFcShort( 0x3fc ),	/* Type Offset=1020 */

	/* Parameter dst_path */

/* 108 */	NdrFcShort( 0x8b ),	/* Flags:  must size, must free, in, by val, */
/* 110 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 112 */	NdrFcShort( 0x3fc ),	/* Type Offset=1020 */

	/* Return value */

/* 114 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 116 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 118 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure PacketConfigureSettings */

/* 120 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 122 */	NdrFcLong( 0x0 ),	/* 0 */
/* 126 */	NdrFcShort( 0xa ),	/* 10 */
/* 128 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 130 */	NdrFcShort( 0x0 ),	/* 0 */
/* 132 */	NdrFcShort( 0x8 ),	/* 8 */
/* 134 */	0x6,		/* Oi2 Flags:  clt must size, has return, */
			0x4,		/* 4 */

	/* Parameter p_task_ids */

/* 136 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 138 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 140 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Parameter p_ip_addresses */

/* 142 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 144 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 146 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Parameter settings */

/* 148 */	NdrFcShort( 0x8b ),	/* Flags:  must size, must free, in, by val, */
/* 150 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 152 */	NdrFcShort( 0x3fc ),	/* Type Offset=1020 */

	/* Return value */

/* 154 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 156 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 158 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure PacketComponentState */

/* 160 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 162 */	NdrFcLong( 0x0 ),	/* 0 */
/* 166 */	NdrFcShort( 0xb ),	/* 11 */
/* 168 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 170 */	NdrFcShort( 0x0 ),	/* 0 */
/* 172 */	NdrFcShort( 0x8 ),	/* 8 */
/* 174 */	0x6,		/* Oi2 Flags:  clt must size, has return, */
			0x3,		/* 3 */

	/* Parameter p_task_ids */

/* 176 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 178 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 180 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Parameter p_ip_addresses */

/* 182 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 184 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 186 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Return value */

/* 188 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 190 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 192 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure PacketCustomAction */

/* 194 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 196 */	NdrFcLong( 0x0 ),	/* 0 */
/* 200 */	NdrFcShort( 0xc ),	/* 12 */
/* 202 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 204 */	NdrFcShort( 0x0 ),	/* 0 */
/* 206 */	NdrFcShort( 0x8 ),	/* 8 */
/* 208 */	0x6,		/* Oi2 Flags:  clt must size, has return, */
			0x4,		/* 4 */

	/* Parameter p_task_ids */

/* 210 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 212 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 214 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Parameter p_ip_addresses */

/* 216 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 218 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 220 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Parameter options */

/* 222 */	NdrFcShort( 0x8b ),	/* Flags:  must size, must free, in, by val, */
/* 224 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 226 */	NdrFcShort( 0x3fc ),	/* Type Offset=1020 */

	/* Return value */

/* 228 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 230 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 232 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure PacketCancelTask */

/* 234 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 236 */	NdrFcLong( 0x0 ),	/* 0 */
/* 240 */	NdrFcShort( 0xd ),	/* 13 */
/* 242 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 244 */	NdrFcShort( 0x0 ),	/* 0 */
/* 246 */	NdrFcShort( 0x8 ),	/* 8 */
/* 248 */	0x6,		/* Oi2 Flags:  clt must size, has return, */
			0x3,		/* 3 */

	/* Parameter p_task_ids */

/* 250 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 252 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 254 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Parameter p_ip_addresses */

/* 256 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 258 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 260 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Return value */

/* 262 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 264 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 266 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure PacketListProcesses */

/* 268 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 270 */	NdrFcLong( 0x0 ),	/* 0 */
/* 274 */	NdrFcShort( 0xe ),	/* 14 */
/* 276 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 278 */	NdrFcShort( 0x0 ),	/* 0 */
/* 280 */	NdrFcShort( 0x8 ),	/* 8 */
/* 282 */	0x6,		/* Oi2 Flags:  clt must size, has return, */
			0x3,		/* 3 */

	/* Parameter p_task_ids */

/* 284 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 286 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 288 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Parameter p_ip_addresses */

/* 290 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 292 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 294 */	NdrFcShort( 0x3f2 ),	/* Type Offset=1010 */

	/* Return value */

/* 296 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 298 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 300 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

			0x0
        }
    };

static const VbaTaskAssignment_MIDL_TYPE_FORMAT_STRING VbaTaskAssignment__MIDL_TypeFormatString =
    {
        0,
        {
			NdrFcShort( 0x0 ),	/* 0 */
/*  2 */	
			0x11, 0x0,	/* FC_RP */
/*  4 */	NdrFcShort( 0x3ee ),	/* Offset= 1006 (1010) */
/*  6 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/*  8 */	NdrFcShort( 0x2 ),	/* Offset= 2 (10) */
/* 10 */	
			0x12, 0x0,	/* FC_UP */
/* 12 */	NdrFcShort( 0x3d4 ),	/* Offset= 980 (992) */
/* 14 */	
			0x2a,		/* FC_ENCAPSULATED_UNION */
			0x49,		/* 73 */
/* 16 */	NdrFcShort( 0x18 ),	/* 24 */
/* 18 */	NdrFcShort( 0xa ),	/* 10 */
/* 20 */	NdrFcLong( 0x8 ),	/* 8 */
/* 24 */	NdrFcShort( 0x6c ),	/* Offset= 108 (132) */
/* 26 */	NdrFcLong( 0xd ),	/* 13 */
/* 30 */	NdrFcShort( 0x9e ),	/* Offset= 158 (188) */
/* 32 */	NdrFcLong( 0x9 ),	/* 9 */
/* 36 */	NdrFcShort( 0xcc ),	/* Offset= 204 (240) */
/* 38 */	NdrFcLong( 0xc ),	/* 12 */
/* 42 */	NdrFcShort( 0x2c4 ),	/* Offset= 708 (750) */
/* 44 */	NdrFcLong( 0x24 ),	/* 36 */
/* 48 */	NdrFcShort( 0x2ec ),	/* Offset= 748 (796) */
/* 50 */	NdrFcLong( 0x800d ),	/* 32781 */
/* 54 */	NdrFcShort( 0x308 ),	/* Offset= 776 (830) */
/* 56 */	NdrFcLong( 0x10 ),	/* 16 */
/* 60 */	NdrFcShort( 0x320 ),	/* Offset= 800 (860) */
/* 62 */	NdrFcLong( 0x2 ),	/* 2 */
/* 66 */	NdrFcShort( 0x338 ),	/* Offset= 824 (890) */
/* 68 */	NdrFcLong( 0x3 ),	/* 3 */
/* 72 */	NdrFcShort( 0x350 ),	/* Offset= 848 (920) */
/* 74 */	NdrFcLong( 0x14 ),	/* 20 */
/* 78 */	NdrFcShort( 0x368 ),	/* Offset= 872 (950) */
/* 80 */	NdrFcShort( 0xffff ),	/* Offset= -1 (79) */
/* 82 */	
			0x1b,		/* FC_CARRAY */
			0x1,		/* 1 */
/* 84 */	NdrFcShort( 0x2 ),	/* 2 */
/* 86 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 88 */	NdrFcShort( 0xfffc ),	/* -4 */
/* 90 */	0x6,		/* FC_SHORT */
			0x5b,		/* FC_END */
/* 92 */	
			0x17,		/* FC_CSTRUCT */
			0x3,		/* 3 */
/* 94 */	NdrFcShort( 0x8 ),	/* 8 */
/* 96 */	NdrFcShort( 0xfff2 ),	/* Offset= -14 (82) */
/* 98 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 100 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 102 */	
			0x1b,		/* FC_CARRAY */
			0x3,		/* 3 */
/* 104 */	NdrFcShort( 0x4 ),	/* 4 */
/* 106 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 108 */	NdrFcShort( 0x0 ),	/* 0 */
/* 110 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 112 */	
			0x48,		/* FC_VARIABLE_REPEAT */
			0x49,		/* FC_FIXED_OFFSET */
/* 114 */	NdrFcShort( 0x4 ),	/* 4 */
/* 116 */	NdrFcShort( 0x0 ),	/* 0 */
/* 118 */	NdrFcShort( 0x1 ),	/* 1 */
/* 120 */	NdrFcShort( 0x0 ),	/* 0 */
/* 122 */	NdrFcShort( 0x0 ),	/* 0 */
/* 124 */	0x12, 0x0,	/* FC_UP */
/* 126 */	NdrFcShort( 0xffde ),	/* Offset= -34 (92) */
/* 128 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 130 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 132 */	
			0x16,		/* FC_PSTRUCT */
			0x3,		/* 3 */
/* 134 */	NdrFcShort( 0x8 ),	/* 8 */
/* 136 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 138 */	
			0x46,		/* FC_NO_REPEAT */
			0x5c,		/* FC_PAD */
/* 140 */	NdrFcShort( 0x4 ),	/* 4 */
/* 142 */	NdrFcShort( 0x4 ),	/* 4 */
/* 144 */	0x11, 0x0,	/* FC_RP */
/* 146 */	NdrFcShort( 0xffd4 ),	/* Offset= -44 (102) */
/* 148 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 150 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 152 */	
			0x2f,		/* FC_IP */
			0x5a,		/* FC_CONSTANT_IID */
/* 154 */	NdrFcLong( 0x0 ),	/* 0 */
/* 158 */	NdrFcShort( 0x0 ),	/* 0 */
/* 160 */	NdrFcShort( 0x0 ),	/* 0 */
/* 162 */	0xc0,		/* 192 */
			0x0,		/* 0 */
/* 164 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 166 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 168 */	0x0,		/* 0 */
			0x46,		/* 70 */
/* 170 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 172 */	NdrFcShort( 0x0 ),	/* 0 */
/* 174 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 176 */	NdrFcShort( 0x0 ),	/* 0 */
/* 178 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 182 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 184 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (152) */
/* 186 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 188 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 190 */	NdrFcShort( 0x8 ),	/* 8 */
/* 192 */	NdrFcShort( 0x0 ),	/* 0 */
/* 194 */	NdrFcShort( 0x6 ),	/* Offset= 6 (200) */
/* 196 */	0x8,		/* FC_LONG */
			0x36,		/* FC_POINTER */
/* 198 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 200 */	
			0x11, 0x0,	/* FC_RP */
/* 202 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (170) */
/* 204 */	
			0x2f,		/* FC_IP */
			0x5a,		/* FC_CONSTANT_IID */
/* 206 */	NdrFcLong( 0x20400 ),	/* 132096 */
/* 210 */	NdrFcShort( 0x0 ),	/* 0 */
/* 212 */	NdrFcShort( 0x0 ),	/* 0 */
/* 214 */	0xc0,		/* 192 */
			0x0,		/* 0 */
/* 216 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 218 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 220 */	0x0,		/* 0 */
			0x46,		/* 70 */
/* 222 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 224 */	NdrFcShort( 0x0 ),	/* 0 */
/* 226 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 228 */	NdrFcShort( 0x0 ),	/* 0 */
/* 230 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 234 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 236 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (204) */
/* 238 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 240 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 242 */	NdrFcShort( 0x8 ),	/* 8 */
/* 244 */	NdrFcShort( 0x0 ),	/* 0 */
/* 246 */	NdrFcShort( 0x6 ),	/* Offset= 6 (252) */
/* 248 */	0x8,		/* FC_LONG */
			0x36,		/* FC_POINTER */
/* 250 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 252 */	
			0x11, 0x0,	/* FC_RP */
/* 254 */	NdrFcShort( 0xffe0 ),	/* Offset= -32 (222) */
/* 256 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 258 */	0x7,		/* Corr desc: FC_USHORT */
			0x0,		/*  */
/* 260 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 262 */	NdrFcShort( 0x2 ),	/* Offset= 2 (264) */
/* 264 */	NdrFcShort( 0x10 ),	/* 16 */
/* 266 */	NdrFcShort( 0x2f ),	/* 47 */
/* 268 */	NdrFcLong( 0x14 ),	/* 20 */
/* 272 */	NdrFcShort( 0x800b ),	/* Simple arm type: FC_HYPER */
/* 274 */	NdrFcLong( 0x3 ),	/* 3 */
/* 278 */	NdrFcShort( 0x8008 ),	/* Simple arm type: FC_LONG */
/* 280 */	NdrFcLong( 0x11 ),	/* 17 */
/* 284 */	NdrFcShort( 0x8001 ),	/* Simple arm type: FC_BYTE */
/* 286 */	NdrFcLong( 0x2 ),	/* 2 */
/* 290 */	NdrFcShort( 0x8006 ),	/* Simple arm type: FC_SHORT */
/* 292 */	NdrFcLong( 0x4 ),	/* 4 */
/* 296 */	NdrFcShort( 0x800a ),	/* Simple arm type: FC_FLOAT */
/* 298 */	NdrFcLong( 0x5 ),	/* 5 */
/* 302 */	NdrFcShort( 0x800c ),	/* Simple arm type: FC_DOUBLE */
/* 304 */	NdrFcLong( 0xb ),	/* 11 */
/* 308 */	NdrFcShort( 0x8006 ),	/* Simple arm type: FC_SHORT */
/* 310 */	NdrFcLong( 0xa ),	/* 10 */
/* 314 */	NdrFcShort( 0x8008 ),	/* Simple arm type: FC_LONG */
/* 316 */	NdrFcLong( 0x6 ),	/* 6 */
/* 320 */	NdrFcShort( 0xe8 ),	/* Offset= 232 (552) */
/* 322 */	NdrFcLong( 0x7 ),	/* 7 */
/* 326 */	NdrFcShort( 0x800c ),	/* Simple arm type: FC_DOUBLE */
/* 328 */	NdrFcLong( 0x8 ),	/* 8 */
/* 332 */	NdrFcShort( 0xe2 ),	/* Offset= 226 (558) */
/* 334 */	NdrFcLong( 0xd ),	/* 13 */
/* 338 */	NdrFcShort( 0xff46 ),	/* Offset= -186 (152) */
/* 340 */	NdrFcLong( 0x9 ),	/* 9 */
/* 344 */	NdrFcShort( 0xff74 ),	/* Offset= -140 (204) */
/* 346 */	NdrFcLong( 0x2000 ),	/* 8192 */
/* 350 */	NdrFcShort( 0xd4 ),	/* Offset= 212 (562) */
/* 352 */	NdrFcLong( 0x24 ),	/* 36 */
/* 356 */	NdrFcShort( 0xd6 ),	/* Offset= 214 (570) */
/* 358 */	NdrFcLong( 0x4024 ),	/* 16420 */
/* 362 */	NdrFcShort( 0xd0 ),	/* Offset= 208 (570) */
/* 364 */	NdrFcLong( 0x4011 ),	/* 16401 */
/* 368 */	NdrFcShort( 0xfe ),	/* Offset= 254 (622) */
/* 370 */	NdrFcLong( 0x4002 ),	/* 16386 */
/* 374 */	NdrFcShort( 0xfc ),	/* Offset= 252 (626) */
/* 376 */	NdrFcLong( 0x4003 ),	/* 16387 */
/* 380 */	NdrFcShort( 0xfa ),	/* Offset= 250 (630) */
/* 382 */	NdrFcLong( 0x4014 ),	/* 16404 */
/* 386 */	NdrFcShort( 0xf8 ),	/* Offset= 248 (634) */
/* 388 */	NdrFcLong( 0x4004 ),	/* 16388 */
/* 392 */	NdrFcShort( 0xf6 ),	/* Offset= 246 (638) */
/* 394 */	NdrFcLong( 0x4005 ),	/* 16389 */
/* 398 */	NdrFcShort( 0xf4 ),	/* Offset= 244 (642) */
/* 400 */	NdrFcLong( 0x400b ),	/* 16395 */
/* 404 */	NdrFcShort( 0xde ),	/* Offset= 222 (626) */
/* 406 */	NdrFcLong( 0x400a ),	/* 16394 */
/* 410 */	NdrFcShort( 0xdc ),	/* Offset= 220 (630) */
/* 412 */	NdrFcLong( 0x4006 ),	/* 16390 */
/* 416 */	NdrFcShort( 0xe6 ),	/* Offset= 230 (646) */
/* 418 */	NdrFcLong( 0x4007 ),	/* 16391 */
/* 422 */	NdrFcShort( 0xdc ),	/* Offset= 220 (642) */
/* 424 */	NdrFcLong( 0x4008 ),	/* 16392 */
/* 428 */	NdrFcShort( 0xde ),	/* Offset= 222 (650) */
/* 430 */	NdrFcLong( 0x400d ),	/* 16397 */
/* 434 */	NdrFcShort( 0xdc ),	/* Offset= 220 (654) */
/* 436 */	NdrFcLong( 0x4009 ),	/* 16393 */
/* 440 */	NdrFcShort( 0xda ),	/* Offset= 218 (658) */
/* 442 */	NdrFcLong( 0x6000 ),	/* 24576 */
/* 446 */	NdrFcShort( 0xd8 ),	/* Offset= 216 (662) */
/* 448 */	NdrFcLong( 0x400c ),	/* 16396 */
/* 452 */	NdrFcShort( 0xde ),	/* Offset= 222 (674) */
/* 454 */	NdrFcLong( 0x10 ),	/* 16 */
/* 458 */	NdrFcShort( 0x8002 ),	/* Simple arm type: FC_CHAR */
/* 460 */	NdrFcLong( 0x12 ),	/* 18 */
/* 464 */	NdrFcShort( 0x8006 ),	/* Simple arm type: FC_SHORT */
/* 466 */	NdrFcLong( 0x13 ),	/* 19 */
/* 470 */	NdrFcShort( 0x8008 ),	/* Simple arm type: FC_LONG */
/* 472 */	NdrFcLong( 0x15 ),	/* 21 */
/* 476 */	NdrFcShort( 0x800b ),	/* Simple arm type: FC_HYPER */
/* 478 */	NdrFcLong( 0x16 ),	/* 22 */
/* 482 */	NdrFcShort( 0x8008 ),	/* Simple arm type: FC_LONG */
/* 484 */	NdrFcLong( 0x17 ),	/* 23 */
/* 488 */	NdrFcShort( 0x8008 ),	/* Simple arm type: FC_LONG */
/* 490 */	NdrFcLong( 0xe ),	/* 14 */
/* 494 */	NdrFcShort( 0xbc ),	/* Offset= 188 (682) */
/* 496 */	NdrFcLong( 0x400e ),	/* 16398 */
/* 500 */	NdrFcShort( 0xc0 ),	/* Offset= 192 (692) */
/* 502 */	NdrFcLong( 0x4010 ),	/* 16400 */
/* 506 */	NdrFcShort( 0xbe ),	/* Offset= 190 (696) */
/* 508 */	NdrFcLong( 0x4012 ),	/* 16402 */
/* 512 */	NdrFcShort( 0x72 ),	/* Offset= 114 (626) */
/* 514 */	NdrFcLong( 0x4013 ),	/* 16403 */
/* 518 */	NdrFcShort( 0x70 ),	/* Offset= 112 (630) */
/* 520 */	NdrFcLong( 0x4015 ),	/* 16405 */
/* 524 */	NdrFcShort( 0x6e ),	/* Offset= 110 (634) */
/* 526 */	NdrFcLong( 0x4016 ),	/* 16406 */
/* 530 */	NdrFcShort( 0x64 ),	/* Offset= 100 (630) */
/* 532 */	NdrFcLong( 0x4017 ),	/* 16407 */
/* 536 */	NdrFcShort( 0x5e ),	/* Offset= 94 (630) */
/* 538 */	NdrFcLong( 0x0 ),	/* 0 */
/* 542 */	NdrFcShort( 0x0 ),	/* Offset= 0 (542) */
/* 544 */	NdrFcLong( 0x1 ),	/* 1 */
/* 548 */	NdrFcShort( 0x0 ),	/* Offset= 0 (548) */
/* 550 */	NdrFcShort( 0xffff ),	/* Offset= -1 (549) */
/* 552 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 554 */	NdrFcShort( 0x8 ),	/* 8 */
/* 556 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 558 */	
			0x12, 0x0,	/* FC_UP */
/* 560 */	NdrFcShort( 0xfe2c ),	/* Offset= -468 (92) */
/* 562 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 564 */	NdrFcShort( 0x2 ),	/* Offset= 2 (566) */
/* 566 */	
			0x12, 0x0,	/* FC_UP */
/* 568 */	NdrFcShort( 0x1a8 ),	/* Offset= 424 (992) */
/* 570 */	
			0x12, 0x0,	/* FC_UP */
/* 572 */	NdrFcShort( 0x1e ),	/* Offset= 30 (602) */
/* 574 */	
			0x2f,		/* FC_IP */
			0x5a,		/* FC_CONSTANT_IID */
/* 576 */	NdrFcLong( 0x2f ),	/* 47 */
/* 580 */	NdrFcShort( 0x0 ),	/* 0 */
/* 582 */	NdrFcShort( 0x0 ),	/* 0 */
/* 584 */	0xc0,		/* 192 */
			0x0,		/* 0 */
/* 586 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 588 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 590 */	0x0,		/* 0 */
			0x46,		/* 70 */
/* 592 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 594 */	NdrFcShort( 0x1 ),	/* 1 */
/* 596 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 598 */	NdrFcShort( 0x4 ),	/* 4 */
/* 600 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 602 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 604 */	NdrFcShort( 0x10 ),	/* 16 */
/* 606 */	NdrFcShort( 0x0 ),	/* 0 */
/* 608 */	NdrFcShort( 0xa ),	/* Offset= 10 (618) */
/* 610 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 612 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 614 */	NdrFcShort( 0xffd8 ),	/* Offset= -40 (574) */
/* 616 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 618 */	
			0x12, 0x0,	/* FC_UP */
/* 620 */	NdrFcShort( 0xffe4 ),	/* Offset= -28 (592) */
/* 622 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 624 */	0x1,		/* FC_BYTE */
			0x5c,		/* FC_PAD */
/* 626 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 628 */	0x6,		/* FC_SHORT */
			0x5c,		/* FC_PAD */
/* 630 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 632 */	0x8,		/* FC_LONG */
			0x5c,		/* FC_PAD */
/* 634 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 636 */	0xb,		/* FC_HYPER */
			0x5c,		/* FC_PAD */
/* 638 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 640 */	0xa,		/* FC_FLOAT */
			0x5c,		/* FC_PAD */
/* 642 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 644 */	0xc,		/* FC_DOUBLE */
			0x5c,		/* FC_PAD */
/* 646 */	
			0x12, 0x0,	/* FC_UP */
/* 648 */	NdrFcShort( 0xffa0 ),	/* Offset= -96 (552) */
/* 650 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 652 */	NdrFcShort( 0xffa2 ),	/* Offset= -94 (558) */
/* 654 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 656 */	NdrFcShort( 0xfe08 ),	/* Offset= -504 (152) */
/* 658 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 660 */	NdrFcShort( 0xfe38 ),	/* Offset= -456 (204) */
/* 662 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 664 */	NdrFcShort( 0x2 ),	/* Offset= 2 (666) */
/* 666 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 668 */	NdrFcShort( 0x2 ),	/* Offset= 2 (670) */
/* 670 */	
			0x12, 0x0,	/* FC_UP */
/* 672 */	NdrFcShort( 0x140 ),	/* Offset= 320 (992) */
/* 674 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 676 */	NdrFcShort( 0x2 ),	/* Offset= 2 (678) */
/* 678 */	
			0x12, 0x0,	/* FC_UP */
/* 680 */	NdrFcShort( 0x14 ),	/* Offset= 20 (700) */
/* 682 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 684 */	NdrFcShort( 0x10 ),	/* 16 */
/* 686 */	0x6,		/* FC_SHORT */
			0x1,		/* FC_BYTE */
/* 688 */	0x1,		/* FC_BYTE */
			0x8,		/* FC_LONG */
/* 690 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 692 */	
			0x12, 0x0,	/* FC_UP */
/* 694 */	NdrFcShort( 0xfff4 ),	/* Offset= -12 (682) */
/* 696 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 698 */	0x2,		/* FC_CHAR */
			0x5c,		/* FC_PAD */
/* 700 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 702 */	NdrFcShort( 0x20 ),	/* 32 */
/* 704 */	NdrFcShort( 0x0 ),	/* 0 */
/* 706 */	NdrFcShort( 0x0 ),	/* Offset= 0 (706) */
/* 708 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 710 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 712 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 714 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 716 */	NdrFcShort( 0xfe34 ),	/* Offset= -460 (256) */
/* 718 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 720 */	
			0x1b,		/* FC_CARRAY */
			0x3,		/* 3 */
/* 722 */	NdrFcShort( 0x4 ),	/* 4 */
/* 724 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 726 */	NdrFcShort( 0x0 ),	/* 0 */
/* 728 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 730 */	
			0x48,		/* FC_VARIABLE_REPEAT */
			0x49,		/* FC_FIXED_OFFSET */
/* 732 */	NdrFcShort( 0x4 ),	/* 4 */
/* 734 */	NdrFcShort( 0x0 ),	/* 0 */
/* 736 */	NdrFcShort( 0x1 ),	/* 1 */
/* 738 */	NdrFcShort( 0x0 ),	/* 0 */
/* 740 */	NdrFcShort( 0x0 ),	/* 0 */
/* 742 */	0x12, 0x0,	/* FC_UP */
/* 744 */	NdrFcShort( 0xffd4 ),	/* Offset= -44 (700) */
/* 746 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 748 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 750 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 752 */	NdrFcShort( 0x8 ),	/* 8 */
/* 754 */	NdrFcShort( 0x0 ),	/* 0 */
/* 756 */	NdrFcShort( 0x6 ),	/* Offset= 6 (762) */
/* 758 */	0x8,		/* FC_LONG */
			0x36,		/* FC_POINTER */
/* 760 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 762 */	
			0x11, 0x0,	/* FC_RP */
/* 764 */	NdrFcShort( 0xffd4 ),	/* Offset= -44 (720) */
/* 766 */	
			0x1b,		/* FC_CARRAY */
			0x3,		/* 3 */
/* 768 */	NdrFcShort( 0x4 ),	/* 4 */
/* 770 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 772 */	NdrFcShort( 0x0 ),	/* 0 */
/* 774 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 776 */	
			0x48,		/* FC_VARIABLE_REPEAT */
			0x49,		/* FC_FIXED_OFFSET */
/* 778 */	NdrFcShort( 0x4 ),	/* 4 */
/* 780 */	NdrFcShort( 0x0 ),	/* 0 */
/* 782 */	NdrFcShort( 0x1 ),	/* 1 */
/* 784 */	NdrFcShort( 0x0 ),	/* 0 */
/* 786 */	NdrFcShort( 0x0 ),	/* 0 */
/* 788 */	0x12, 0x0,	/* FC_UP */
/* 790 */	NdrFcShort( 0xff44 ),	/* Offset= -188 (602) */
/* 792 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 794 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 796 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 798 */	NdrFcShort( 0x8 ),	/* 8 */
/* 800 */	NdrFcShort( 0x0 ),	/* 0 */
/* 802 */	NdrFcShort( 0x6 ),	/* Offset= 6 (808) */
/* 804 */	0x8,		/* FC_LONG */
			0x36,		/* FC_POINTER */
/* 806 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 808 */	
			0x11, 0x0,	/* FC_RP */
/* 810 */	NdrFcShort( 0xffd4 ),	/* Offset= -44 (766) */
/* 812 */	
			0x1d,		/* FC_SMFARRAY */
			0x0,		/* 0 */
/* 814 */	NdrFcShort( 0x8 ),	/* 8 */
/* 816 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 818 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 820 */	NdrFcShort( 0x10 ),	/* 16 */
/* 822 */	0x8,		/* FC_LONG */
			0x6,		/* FC_SHORT */
/* 824 */	0x6,		/* FC_SHORT */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 826 */	0x0,		/* 0 */
			NdrFcShort( 0xfff1 ),	/* Offset= -15 (812) */
			0x5b,		/* FC_END */
/* 830 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 832 */	NdrFcShort( 0x18 ),	/* 24 */
/* 834 */	NdrFcShort( 0x0 ),	/* 0 */
/* 836 */	NdrFcShort( 0xa ),	/* Offset= 10 (846) */
/* 838 */	0x8,		/* FC_LONG */
			0x36,		/* FC_POINTER */
/* 840 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 842 */	NdrFcShort( 0xffe8 ),	/* Offset= -24 (818) */
/* 844 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 846 */	
			0x11, 0x0,	/* FC_RP */
/* 848 */	NdrFcShort( 0xfd5a ),	/* Offset= -678 (170) */
/* 850 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 852 */	NdrFcShort( 0x1 ),	/* 1 */
/* 854 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 856 */	NdrFcShort( 0x0 ),	/* 0 */
/* 858 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 860 */	
			0x16,		/* FC_PSTRUCT */
			0x3,		/* 3 */
/* 862 */	NdrFcShort( 0x8 ),	/* 8 */
/* 864 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 866 */	
			0x46,		/* FC_NO_REPEAT */
			0x5c,		/* FC_PAD */
/* 868 */	NdrFcShort( 0x4 ),	/* 4 */
/* 870 */	NdrFcShort( 0x4 ),	/* 4 */
/* 872 */	0x12, 0x0,	/* FC_UP */
/* 874 */	NdrFcShort( 0xffe8 ),	/* Offset= -24 (850) */
/* 876 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 878 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 880 */	
			0x1b,		/* FC_CARRAY */
			0x1,		/* 1 */
/* 882 */	NdrFcShort( 0x2 ),	/* 2 */
/* 884 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 886 */	NdrFcShort( 0x0 ),	/* 0 */
/* 888 */	0x6,		/* FC_SHORT */
			0x5b,		/* FC_END */
/* 890 */	
			0x16,		/* FC_PSTRUCT */
			0x3,		/* 3 */
/* 892 */	NdrFcShort( 0x8 ),	/* 8 */
/* 894 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 896 */	
			0x46,		/* FC_NO_REPEAT */
			0x5c,		/* FC_PAD */
/* 898 */	NdrFcShort( 0x4 ),	/* 4 */
/* 900 */	NdrFcShort( 0x4 ),	/* 4 */
/* 902 */	0x12, 0x0,	/* FC_UP */
/* 904 */	NdrFcShort( 0xffe8 ),	/* Offset= -24 (880) */
/* 906 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 908 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 910 */	
			0x1b,		/* FC_CARRAY */
			0x3,		/* 3 */
/* 912 */	NdrFcShort( 0x4 ),	/* 4 */
/* 914 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 916 */	NdrFcShort( 0x0 ),	/* 0 */
/* 918 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 920 */	
			0x16,		/* FC_PSTRUCT */
			0x3,		/* 3 */
/* 922 */	NdrFcShort( 0x8 ),	/* 8 */
/* 924 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 926 */	
			0x46,		/* FC_NO_REPEAT */
			0x5c,		/* FC_PAD */
/* 928 */	NdrFcShort( 0x4 ),	/* 4 */
/* 930 */	NdrFcShort( 0x4 ),	/* 4 */
/* 932 */	0x12, 0x0,	/* FC_UP */
/* 934 */	NdrFcShort( 0xffe8 ),	/* Offset= -24 (910) */
/* 936 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 938 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 940 */	
			0x1b,		/* FC_CARRAY */
			0x7,		/* 7 */
/* 942 */	NdrFcShort( 0x8 ),	/* 8 */
/* 944 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 946 */	NdrFcShort( 0x0 ),	/* 0 */
/* 948 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 950 */	
			0x16,		/* FC_PSTRUCT */
			0x3,		/* 3 */
/* 952 */	NdrFcShort( 0x8 ),	/* 8 */
/* 954 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 956 */	
			0x46,		/* FC_NO_REPEAT */
			0x5c,		/* FC_PAD */
/* 958 */	NdrFcShort( 0x4 ),	/* 4 */
/* 960 */	NdrFcShort( 0x4 ),	/* 4 */
/* 962 */	0x12, 0x0,	/* FC_UP */
/* 964 */	NdrFcShort( 0xffe8 ),	/* Offset= -24 (940) */
/* 966 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 968 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 970 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 972 */	NdrFcShort( 0x8 ),	/* 8 */
/* 974 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 976 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 978 */	
			0x1b,		/* FC_CARRAY */
			0x3,		/* 3 */
/* 980 */	NdrFcShort( 0x8 ),	/* 8 */
/* 982 */	0x7,		/* Corr desc: FC_USHORT */
			0x0,		/*  */
/* 984 */	NdrFcShort( 0xffd8 ),	/* -40 */
/* 986 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 988 */	NdrFcShort( 0xffee ),	/* Offset= -18 (970) */
/* 990 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 992 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 994 */	NdrFcShort( 0x28 ),	/* 40 */
/* 996 */	NdrFcShort( 0xffee ),	/* Offset= -18 (978) */
/* 998 */	NdrFcShort( 0x0 ),	/* Offset= 0 (998) */
/* 1000 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 1002 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1004 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1006 */	NdrFcShort( 0xfc20 ),	/* Offset= -992 (14) */
/* 1008 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1010 */	0xb4,		/* FC_USER_MARSHAL */
			0x83,		/* 131 */
/* 1012 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1014 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1016 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1018 */	NdrFcShort( 0xfc0c ),	/* Offset= -1012 (6) */
/* 1020 */	0xb4,		/* FC_USER_MARSHAL */
			0x83,		/* 131 */
/* 1022 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1024 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1026 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1028 */	NdrFcShort( 0xfe2a ),	/* Offset= -470 (558) */

			0x0
        }
    };

static const USER_MARSHAL_ROUTINE_QUADRUPLE UserMarshalRoutines[ WIRE_MARSHAL_TABLE_SIZE ] = 
        {
            
            {
            LPSAFEARRAY_UserSize
            ,LPSAFEARRAY_UserMarshal
            ,LPSAFEARRAY_UserUnmarshal
            ,LPSAFEARRAY_UserFree
            },
            {
            BSTR_UserSize
            ,BSTR_UserMarshal
            ,BSTR_UserUnmarshal
            ,BSTR_UserFree
            }

        };



/* Object interface: IUnknown, ver. 0.0,
   GUID={0x00000000,0x0000,0x0000,{0xC0,0x00,0x00,0x00,0x00,0x00,0x00,0x46}} */


/* Object interface: IDispatch, ver. 0.0,
   GUID={0x00020400,0x0000,0x0000,{0xC0,0x00,0x00,0x00,0x00,0x00,0x00,0x46}} */


/* Object interface: ITaskService, ver. 0.0,
   GUID={0x4D8F6784,0xB7D3,0x46A4,{0xBC,0x29,0x9F,0x42,0x09,0xAE,0x93,0xEA}} */

#pragma code_seg(".orpc")
static const unsigned short ITaskService_FormatStringOffsetTable[] =
    {
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    (unsigned short) -1,
    0,
    34,
    74,
    120,
    160,
    194,
    234,
    268
    };

static const MIDL_STUBLESS_PROXY_INFO ITaskService_ProxyInfo =
    {
    &Object_StubDesc,
    VbaTaskAssignment__MIDL_ProcFormatString.Format,
    &ITaskService_FormatStringOffsetTable[-3],
    0,
    0,
    0
    };


static const MIDL_SERVER_INFO ITaskService_ServerInfo = 
    {
    &Object_StubDesc,
    0,
    VbaTaskAssignment__MIDL_ProcFormatString.Format,
    &ITaskService_FormatStringOffsetTable[-3],
    0,
    0,
    0,
    0};
CINTERFACE_PROXY_VTABLE(15) _ITaskServiceProxyVtbl = 
{
    &ITaskService_ProxyInfo,
    &IID_ITaskService,
    IUnknown_QueryInterface_Proxy,
    IUnknown_AddRef_Proxy,
    IUnknown_Release_Proxy ,
    0 /* (void *) (INT_PTR) -1 /* IDispatch::GetTypeInfoCount */ ,
    0 /* (void *) (INT_PTR) -1 /* IDispatch::GetTypeInfo */ ,
    0 /* (void *) (INT_PTR) -1 /* IDispatch::GetIDsOfNames */ ,
    0 /* IDispatch_Invoke_Proxy */ ,
    (void *) (INT_PTR) -1 /* ITaskService::PacketSystemInfo */ ,
    (void *) (INT_PTR) -1 /* ITaskService::PacketCreateProcess */ ,
    (void *) (INT_PTR) -1 /* ITaskService::PacketSendFile */ ,
    (void *) (INT_PTR) -1 /* ITaskService::PacketConfigureSettings */ ,
    (void *) (INT_PTR) -1 /* ITaskService::PacketComponentState */ ,
    (void *) (INT_PTR) -1 /* ITaskService::PacketCustomAction */ ,
    (void *) (INT_PTR) -1 /* ITaskService::PacketCancelTask */ ,
    (void *) (INT_PTR) -1 /* ITaskService::PacketListProcesses */
};


static const PRPC_STUB_FUNCTION ITaskService_table[] =
{
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    STUB_FORWARDING_FUNCTION,
    NdrStubCall2,
    NdrStubCall2,
    NdrStubCall2,
    NdrStubCall2,
    NdrStubCall2,
    NdrStubCall2,
    NdrStubCall2,
    NdrStubCall2
};

CInterfaceStubVtbl _ITaskServiceStubVtbl =
{
    &IID_ITaskService,
    &ITaskService_ServerInfo,
    15,
    &ITaskService_table[-3],
    CStdStubBuffer_DELEGATING_METHODS
};

static const MIDL_STUB_DESC Object_StubDesc = 
    {
    0,
    NdrOleAllocate,
    NdrOleFree,
    0,
    0,
    0,
    0,
    0,
    VbaTaskAssignment__MIDL_TypeFormatString.Format,
    1, /* -error bounds_check flag */
    0x20000, /* Ndr library version */
    0,
    0x70001f4, /* MIDL Version 7.0.500 */
    0,
    UserMarshalRoutines,
    0,  /* notify & notify_flag routine table */
    0x1, /* MIDL flag */
    0, /* cs routines */
    0,   /* proxy/server info */
    0
    };

const CInterfaceProxyVtbl * _VbaTaskAssignment_ProxyVtblList[] = 
{
    ( CInterfaceProxyVtbl *) &_ITaskServiceProxyVtbl,
    0
};

const CInterfaceStubVtbl * _VbaTaskAssignment_StubVtblList[] = 
{
    ( CInterfaceStubVtbl *) &_ITaskServiceStubVtbl,
    0
};

PCInterfaceName const _VbaTaskAssignment_InterfaceNamesList[] = 
{
    "ITaskService",
    0
};

const IID *  _VbaTaskAssignment_BaseIIDList[] = 
{
    &IID_IDispatch,
    0
};


#define _VbaTaskAssignment_CHECK_IID(n)	IID_GENERIC_CHECK_IID( _VbaTaskAssignment, pIID, n)

int __stdcall _VbaTaskAssignment_IID_Lookup( const IID * pIID, int * pIndex )
{
    
    if(!_VbaTaskAssignment_CHECK_IID(0))
        {
        *pIndex = 0;
        return 1;
        }

    return 0;
}

const ExtendedProxyFileInfo VbaTaskAssignment_ProxyFileInfo = 
{
    (PCInterfaceProxyVtblList *) & _VbaTaskAssignment_ProxyVtblList,
    (PCInterfaceStubVtblList *) & _VbaTaskAssignment_StubVtblList,
    (const PCInterfaceName * ) & _VbaTaskAssignment_InterfaceNamesList,
    (const IID ** ) & _VbaTaskAssignment_BaseIIDList,
    & _VbaTaskAssignment_IID_Lookup, 
    1,
    2,
    0, /* table of [async_uuid] interfaces */
    0, /* Filler1 */
    0, /* Filler2 */
    0  /* Filler3 */
};
#pragma optimize("", on )
#if _MSC_VER >= 1200
#pragma warning(pop)
#endif


#endif /* !defined(_M_IA64) && !defined(_M_AMD64)*/

