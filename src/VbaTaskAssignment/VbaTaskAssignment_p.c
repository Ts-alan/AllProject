

/* this ALWAYS GENERATED file contains the proxy stub code */


 /* File created by MIDL compiler version 6.00.0366 */
/* at Mon Sep 29 13:13:58 2008
 */
/* Compiler settings for .\VbaTaskAssignment.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext, robust
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
#pragma warning( disable: 4100 ) /* unreferenced arguments in x86 call */
#pragma warning( disable: 4211 )  /* redefine extent to static */
#pragma warning( disable: 4232 )  /* dllimport identity*/
#pragma optimize("", off ) 

#define USE_STUBLESS_PROXY


/* verify that the <rpcproxy.h> version is high enough to compile this file*/
#ifndef __REDQ_RPCPROXY_H_VERSION__
#define __REQUIRED_RPCPROXY_H_VERSION__ 475
#endif


#include "rpcproxy.h"
#ifndef __RPCPROXY_H_VERSION__
#error this stub requires an updated version of <rpcproxy.h>
#endif // __RPCPROXY_H_VERSION__


#include "VbaTaskAssignment.h"

#define TYPE_FORMAT_STRING_SIZE   1061                              
#define PROC_FORMAT_STRING_SIZE   367                               
#define TRANSMIT_AS_TABLE_SIZE    0            
#define WIRE_MARSHAL_TABLE_SIZE   2            

typedef struct _MIDL_TYPE_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ TYPE_FORMAT_STRING_SIZE ];
    } MIDL_TYPE_FORMAT_STRING;

typedef struct _MIDL_PROC_FORMAT_STRING
    {
    short          Pad;
    unsigned char  Format[ PROC_FORMAT_STRING_SIZE ];
    } MIDL_PROC_FORMAT_STRING;


static RPC_SYNTAX_IDENTIFIER  _RpcTransferSyntax = 
{{0x8A885D04,0x1CEB,0x11C9,{0x9F,0xE8,0x08,0x00,0x2B,0x10,0x48,0x60}},{2,0}};


extern const MIDL_TYPE_FORMAT_STRING __MIDL_TypeFormatString;
extern const MIDL_PROC_FORMAT_STRING __MIDL_ProcFormatString;


extern const MIDL_STUB_DESC Object_StubDesc;


extern const MIDL_SERVER_INFO ITaskService_ServerInfo;
extern const MIDL_STUBLESS_PROXY_INFO ITaskService_ProxyInfo;


extern const USER_MARSHAL_ROUTINE_QUADRUPLE UserMarshalRoutines[ WIRE_MARSHAL_TABLE_SIZE ];

#if !defined(__RPC_WIN32__)
#error  Invalid build platform for this stub.
#endif

#if !(TARGET_IS_NT50_OR_LATER)
#error You need a Windows 2000 or later to run this stub because it uses these features:
#error   /robust command line switch.
#error However, your C/C++ compilation flags indicate you intend to run this app on earlier systems.
#error This app will die there with the RPC_X_WRONG_STUB_VERSION error.
#endif


static const MIDL_PROC_FORMAT_STRING __MIDL_ProcFormatString =
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
/* 14 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x3,		/* 3 */
/* 16 */	0x8,		/* 8 */
			0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 18 */	NdrFcShort( 0x0 ),	/* 0 */
/* 20 */	NdrFcShort( 0x24 ),	/* 36 */
/* 22 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter p_task_ids */

/* 24 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 26 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 28 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Parameter p_ip_addresses */

/* 30 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 32 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 34 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Return value */

/* 36 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 38 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 40 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure PacketCreateProcess */

/* 42 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 44 */	NdrFcLong( 0x0 ),	/* 0 */
/* 48 */	NdrFcShort( 0x8 ),	/* 8 */
/* 50 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 52 */	NdrFcShort( 0x0 ),	/* 0 */
/* 54 */	NdrFcShort( 0x8 ),	/* 8 */
/* 56 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x4,		/* 4 */
/* 58 */	0x8,		/* 8 */
			0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 60 */	NdrFcShort( 0x0 ),	/* 0 */
/* 62 */	NdrFcShort( 0x25 ),	/* 37 */
/* 64 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter p_task_ids */

/* 66 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 68 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 70 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Parameter p_ip_addresses */

/* 72 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 74 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 76 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Parameter cmd_line */

/* 78 */	NdrFcShort( 0x8b ),	/* Flags:  must size, must free, in, by val, */
/* 80 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 82 */	NdrFcShort( 0x41a ),	/* Type Offset=1050 */

	/* Return value */

/* 84 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 86 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 88 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure PacketSendFile */

/* 90 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 92 */	NdrFcLong( 0x0 ),	/* 0 */
/* 96 */	NdrFcShort( 0x9 ),	/* 9 */
/* 98 */	NdrFcShort( 0x18 ),	/* x86 Stack size/offset = 24 */
/* 100 */	NdrFcShort( 0x0 ),	/* 0 */
/* 102 */	NdrFcShort( 0x8 ),	/* 8 */
/* 104 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x5,		/* 5 */
/* 106 */	0x8,		/* 8 */
			0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 108 */	NdrFcShort( 0x0 ),	/* 0 */
/* 110 */	NdrFcShort( 0x26 ),	/* 38 */
/* 112 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter p_task_ids */

/* 114 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 116 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 118 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Parameter p_ip_addresses */

/* 120 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 122 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 124 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Parameter src_path */

/* 126 */	NdrFcShort( 0x8b ),	/* Flags:  must size, must free, in, by val, */
/* 128 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 130 */	NdrFcShort( 0x41a ),	/* Type Offset=1050 */

	/* Parameter dst_path */

/* 132 */	NdrFcShort( 0x8b ),	/* Flags:  must size, must free, in, by val, */
/* 134 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 136 */	NdrFcShort( 0x41a ),	/* Type Offset=1050 */

	/* Return value */

/* 138 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 140 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 142 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure PacketConfigureSettings */

/* 144 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 146 */	NdrFcLong( 0x0 ),	/* 0 */
/* 150 */	NdrFcShort( 0xa ),	/* 10 */
/* 152 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 154 */	NdrFcShort( 0x0 ),	/* 0 */
/* 156 */	NdrFcShort( 0x8 ),	/* 8 */
/* 158 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x4,		/* 4 */
/* 160 */	0x8,		/* 8 */
			0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 162 */	NdrFcShort( 0x0 ),	/* 0 */
/* 164 */	NdrFcShort( 0x25 ),	/* 37 */
/* 166 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter p_task_ids */

/* 168 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 170 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 172 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Parameter p_ip_addresses */

/* 174 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 176 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 178 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Parameter settings */

/* 180 */	NdrFcShort( 0x8b ),	/* Flags:  must size, must free, in, by val, */
/* 182 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 184 */	NdrFcShort( 0x41a ),	/* Type Offset=1050 */

	/* Return value */

/* 186 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 188 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 190 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure PacketComponentState */

/* 192 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 194 */	NdrFcLong( 0x0 ),	/* 0 */
/* 198 */	NdrFcShort( 0xb ),	/* 11 */
/* 200 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 202 */	NdrFcShort( 0x0 ),	/* 0 */
/* 204 */	NdrFcShort( 0x8 ),	/* 8 */
/* 206 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x3,		/* 3 */
/* 208 */	0x8,		/* 8 */
			0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 210 */	NdrFcShort( 0x0 ),	/* 0 */
/* 212 */	NdrFcShort( 0x24 ),	/* 36 */
/* 214 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter p_task_ids */

/* 216 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 218 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 220 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Parameter p_ip_addresses */

/* 222 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 224 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 226 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Return value */

/* 228 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 230 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 232 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure PacketCustomAction */

/* 234 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 236 */	NdrFcLong( 0x0 ),	/* 0 */
/* 240 */	NdrFcShort( 0xc ),	/* 12 */
/* 242 */	NdrFcShort( 0x14 ),	/* x86 Stack size/offset = 20 */
/* 244 */	NdrFcShort( 0x0 ),	/* 0 */
/* 246 */	NdrFcShort( 0x8 ),	/* 8 */
/* 248 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x4,		/* 4 */
/* 250 */	0x8,		/* 8 */
			0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 252 */	NdrFcShort( 0x0 ),	/* 0 */
/* 254 */	NdrFcShort( 0x25 ),	/* 37 */
/* 256 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter p_task_ids */

/* 258 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 260 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 262 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Parameter p_ip_addresses */

/* 264 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 266 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 268 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Parameter options */

/* 270 */	NdrFcShort( 0x8b ),	/* Flags:  must size, must free, in, by val, */
/* 272 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 274 */	NdrFcShort( 0x41a ),	/* Type Offset=1050 */

	/* Return value */

/* 276 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 278 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 280 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure PacketCancelTask */

/* 282 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 284 */	NdrFcLong( 0x0 ),	/* 0 */
/* 288 */	NdrFcShort( 0xd ),	/* 13 */
/* 290 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 292 */	NdrFcShort( 0x0 ),	/* 0 */
/* 294 */	NdrFcShort( 0x8 ),	/* 8 */
/* 296 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x3,		/* 3 */
/* 298 */	0x8,		/* 8 */
			0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 300 */	NdrFcShort( 0x0 ),	/* 0 */
/* 302 */	NdrFcShort( 0x24 ),	/* 36 */
/* 304 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter p_task_ids */

/* 306 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 308 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 310 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Parameter p_ip_addresses */

/* 312 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 314 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 316 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Return value */

/* 318 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 320 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 322 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

	/* Procedure PacketListProcesses */

/* 324 */	0x33,		/* FC_AUTO_HANDLE */
			0x6c,		/* Old Flags:  object, Oi2 */
/* 326 */	NdrFcLong( 0x0 ),	/* 0 */
/* 330 */	NdrFcShort( 0xe ),	/* 14 */
/* 332 */	NdrFcShort( 0x10 ),	/* x86 Stack size/offset = 16 */
/* 334 */	NdrFcShort( 0x0 ),	/* 0 */
/* 336 */	NdrFcShort( 0x8 ),	/* 8 */
/* 338 */	0x46,		/* Oi2 Flags:  clt must size, has return, has ext, */
			0x3,		/* 3 */
/* 340 */	0x8,		/* 8 */
			0x5,		/* Ext Flags:  new corr desc, srv corr check, */
/* 342 */	NdrFcShort( 0x0 ),	/* 0 */
/* 344 */	NdrFcShort( 0x24 ),	/* 36 */
/* 346 */	NdrFcShort( 0x0 ),	/* 0 */

	/* Parameter p_task_ids */

/* 348 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 350 */	NdrFcShort( 0x4 ),	/* x86 Stack size/offset = 4 */
/* 352 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Parameter p_ip_addresses */

/* 354 */	NdrFcShort( 0x10b ),	/* Flags:  must size, must free, in, simple ref, */
/* 356 */	NdrFcShort( 0x8 ),	/* x86 Stack size/offset = 8 */
/* 358 */	NdrFcShort( 0x410 ),	/* Type Offset=1040 */

	/* Return value */

/* 360 */	NdrFcShort( 0x70 ),	/* Flags:  out, return, base type, */
/* 362 */	NdrFcShort( 0xc ),	/* x86 Stack size/offset = 12 */
/* 364 */	0x8,		/* FC_LONG */
			0x0,		/* 0 */

			0x0
        }
    };

static const MIDL_TYPE_FORMAT_STRING __MIDL_TypeFormatString =
    {
        0,
        {
			NdrFcShort( 0x0 ),	/* 0 */
/*  2 */	
			0x11, 0x0,	/* FC_RP */
/*  4 */	NdrFcShort( 0x40c ),	/* Offset= 1036 (1040) */
/*  6 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/*  8 */	NdrFcShort( 0x2 ),	/* Offset= 2 (10) */
/* 10 */	
			0x12, 0x0,	/* FC_UP */
/* 12 */	NdrFcShort( 0x3f2 ),	/* Offset= 1010 (1022) */
/* 14 */	
			0x2a,		/* FC_ENCAPSULATED_UNION */
			0x49,		/* 73 */
/* 16 */	NdrFcShort( 0x18 ),	/* 24 */
/* 18 */	NdrFcShort( 0xa ),	/* 10 */
/* 20 */	NdrFcLong( 0x8 ),	/* 8 */
/* 24 */	NdrFcShort( 0x70 ),	/* Offset= 112 (136) */
/* 26 */	NdrFcLong( 0xd ),	/* 13 */
/* 30 */	NdrFcShort( 0xa6 ),	/* Offset= 166 (196) */
/* 32 */	NdrFcLong( 0x9 ),	/* 9 */
/* 36 */	NdrFcShort( 0xd8 ),	/* Offset= 216 (252) */
/* 38 */	NdrFcLong( 0xc ),	/* 12 */
/* 42 */	NdrFcShort( 0x2d6 ),	/* Offset= 726 (768) */
/* 44 */	NdrFcLong( 0x24 ),	/* 36 */
/* 48 */	NdrFcShort( 0x300 ),	/* Offset= 768 (816) */
/* 50 */	NdrFcLong( 0x800d ),	/* 32781 */
/* 54 */	NdrFcShort( 0x31c ),	/* Offset= 796 (850) */
/* 56 */	NdrFcLong( 0x10 ),	/* 16 */
/* 60 */	NdrFcShort( 0x336 ),	/* Offset= 822 (882) */
/* 62 */	NdrFcLong( 0x2 ),	/* 2 */
/* 66 */	NdrFcShort( 0x350 ),	/* Offset= 848 (914) */
/* 68 */	NdrFcLong( 0x3 ),	/* 3 */
/* 72 */	NdrFcShort( 0x36a ),	/* Offset= 874 (946) */
/* 74 */	NdrFcLong( 0x14 ),	/* 20 */
/* 78 */	NdrFcShort( 0x384 ),	/* Offset= 900 (978) */
/* 80 */	NdrFcShort( 0xffff ),	/* Offset= -1 (79) */
/* 82 */	
			0x1b,		/* FC_CARRAY */
			0x1,		/* 1 */
/* 84 */	NdrFcShort( 0x2 ),	/* 2 */
/* 86 */	0x9,		/* Corr desc: FC_ULONG */
			0x0,		/*  */
/* 88 */	NdrFcShort( 0xfffc ),	/* -4 */
/* 90 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 92 */	0x6,		/* FC_SHORT */
			0x5b,		/* FC_END */
/* 94 */	
			0x17,		/* FC_CSTRUCT */
			0x3,		/* 3 */
/* 96 */	NdrFcShort( 0x8 ),	/* 8 */
/* 98 */	NdrFcShort( 0xfff0 ),	/* Offset= -16 (82) */
/* 100 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 102 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 104 */	
			0x1b,		/* FC_CARRAY */
			0x3,		/* 3 */
/* 106 */	NdrFcShort( 0x4 ),	/* 4 */
/* 108 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 110 */	NdrFcShort( 0x0 ),	/* 0 */
/* 112 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 114 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 116 */	
			0x48,		/* FC_VARIABLE_REPEAT */
			0x49,		/* FC_FIXED_OFFSET */
/* 118 */	NdrFcShort( 0x4 ),	/* 4 */
/* 120 */	NdrFcShort( 0x0 ),	/* 0 */
/* 122 */	NdrFcShort( 0x1 ),	/* 1 */
/* 124 */	NdrFcShort( 0x0 ),	/* 0 */
/* 126 */	NdrFcShort( 0x0 ),	/* 0 */
/* 128 */	0x12, 0x0,	/* FC_UP */
/* 130 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (94) */
/* 132 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 134 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 136 */	
			0x16,		/* FC_PSTRUCT */
			0x3,		/* 3 */
/* 138 */	NdrFcShort( 0x8 ),	/* 8 */
/* 140 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 142 */	
			0x46,		/* FC_NO_REPEAT */
			0x5c,		/* FC_PAD */
/* 144 */	NdrFcShort( 0x4 ),	/* 4 */
/* 146 */	NdrFcShort( 0x4 ),	/* 4 */
/* 148 */	0x11, 0x0,	/* FC_RP */
/* 150 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (104) */
/* 152 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 154 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 156 */	
			0x2f,		/* FC_IP */
			0x5a,		/* FC_CONSTANT_IID */
/* 158 */	NdrFcLong( 0x0 ),	/* 0 */
/* 162 */	NdrFcShort( 0x0 ),	/* 0 */
/* 164 */	NdrFcShort( 0x0 ),	/* 0 */
/* 166 */	0xc0,		/* 192 */
			0x0,		/* 0 */
/* 168 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 170 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 172 */	0x0,		/* 0 */
			0x46,		/* 70 */
/* 174 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 176 */	NdrFcShort( 0x0 ),	/* 0 */
/* 178 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 180 */	NdrFcShort( 0x0 ),	/* 0 */
/* 182 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 184 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 188 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 190 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 192 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (156) */
/* 194 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 196 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 198 */	NdrFcShort( 0x8 ),	/* 8 */
/* 200 */	NdrFcShort( 0x0 ),	/* 0 */
/* 202 */	NdrFcShort( 0x6 ),	/* Offset= 6 (208) */
/* 204 */	0x8,		/* FC_LONG */
			0x36,		/* FC_POINTER */
/* 206 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 208 */	
			0x11, 0x0,	/* FC_RP */
/* 210 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (174) */
/* 212 */	
			0x2f,		/* FC_IP */
			0x5a,		/* FC_CONSTANT_IID */
/* 214 */	NdrFcLong( 0x20400 ),	/* 132096 */
/* 218 */	NdrFcShort( 0x0 ),	/* 0 */
/* 220 */	NdrFcShort( 0x0 ),	/* 0 */
/* 222 */	0xc0,		/* 192 */
			0x0,		/* 0 */
/* 224 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 226 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 228 */	0x0,		/* 0 */
			0x46,		/* 70 */
/* 230 */	
			0x21,		/* FC_BOGUS_ARRAY */
			0x3,		/* 3 */
/* 232 */	NdrFcShort( 0x0 ),	/* 0 */
/* 234 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 236 */	NdrFcShort( 0x0 ),	/* 0 */
/* 238 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 240 */	NdrFcLong( 0xffffffff ),	/* -1 */
/* 244 */	NdrFcShort( 0x0 ),	/* Corr flags:  */
/* 246 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 248 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (212) */
/* 250 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 252 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 254 */	NdrFcShort( 0x8 ),	/* 8 */
/* 256 */	NdrFcShort( 0x0 ),	/* 0 */
/* 258 */	NdrFcShort( 0x6 ),	/* Offset= 6 (264) */
/* 260 */	0x8,		/* FC_LONG */
			0x36,		/* FC_POINTER */
/* 262 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 264 */	
			0x11, 0x0,	/* FC_RP */
/* 266 */	NdrFcShort( 0xffdc ),	/* Offset= -36 (230) */
/* 268 */	
			0x2b,		/* FC_NON_ENCAPSULATED_UNION */
			0x9,		/* FC_ULONG */
/* 270 */	0x7,		/* Corr desc: FC_USHORT */
			0x0,		/*  */
/* 272 */	NdrFcShort( 0xfff8 ),	/* -8 */
/* 274 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 276 */	NdrFcShort( 0x2 ),	/* Offset= 2 (278) */
/* 278 */	NdrFcShort( 0x10 ),	/* 16 */
/* 280 */	NdrFcShort( 0x2f ),	/* 47 */
/* 282 */	NdrFcLong( 0x14 ),	/* 20 */
/* 286 */	NdrFcShort( 0x800b ),	/* Simple arm type: FC_HYPER */
/* 288 */	NdrFcLong( 0x3 ),	/* 3 */
/* 292 */	NdrFcShort( 0x8008 ),	/* Simple arm type: FC_LONG */
/* 294 */	NdrFcLong( 0x11 ),	/* 17 */
/* 298 */	NdrFcShort( 0x8001 ),	/* Simple arm type: FC_BYTE */
/* 300 */	NdrFcLong( 0x2 ),	/* 2 */
/* 304 */	NdrFcShort( 0x8006 ),	/* Simple arm type: FC_SHORT */
/* 306 */	NdrFcLong( 0x4 ),	/* 4 */
/* 310 */	NdrFcShort( 0x800a ),	/* Simple arm type: FC_FLOAT */
/* 312 */	NdrFcLong( 0x5 ),	/* 5 */
/* 316 */	NdrFcShort( 0x800c ),	/* Simple arm type: FC_DOUBLE */
/* 318 */	NdrFcLong( 0xb ),	/* 11 */
/* 322 */	NdrFcShort( 0x8006 ),	/* Simple arm type: FC_SHORT */
/* 324 */	NdrFcLong( 0xa ),	/* 10 */
/* 328 */	NdrFcShort( 0x8008 ),	/* Simple arm type: FC_LONG */
/* 330 */	NdrFcLong( 0x6 ),	/* 6 */
/* 334 */	NdrFcShort( 0xe8 ),	/* Offset= 232 (566) */
/* 336 */	NdrFcLong( 0x7 ),	/* 7 */
/* 340 */	NdrFcShort( 0x800c ),	/* Simple arm type: FC_DOUBLE */
/* 342 */	NdrFcLong( 0x8 ),	/* 8 */
/* 346 */	NdrFcShort( 0xe2 ),	/* Offset= 226 (572) */
/* 348 */	NdrFcLong( 0xd ),	/* 13 */
/* 352 */	NdrFcShort( 0xff3c ),	/* Offset= -196 (156) */
/* 354 */	NdrFcLong( 0x9 ),	/* 9 */
/* 358 */	NdrFcShort( 0xff6e ),	/* Offset= -146 (212) */
/* 360 */	NdrFcLong( 0x2000 ),	/* 8192 */
/* 364 */	NdrFcShort( 0xd4 ),	/* Offset= 212 (576) */
/* 366 */	NdrFcLong( 0x24 ),	/* 36 */
/* 370 */	NdrFcShort( 0xd6 ),	/* Offset= 214 (584) */
/* 372 */	NdrFcLong( 0x4024 ),	/* 16420 */
/* 376 */	NdrFcShort( 0xd0 ),	/* Offset= 208 (584) */
/* 378 */	NdrFcLong( 0x4011 ),	/* 16401 */
/* 382 */	NdrFcShort( 0x100 ),	/* Offset= 256 (638) */
/* 384 */	NdrFcLong( 0x4002 ),	/* 16386 */
/* 388 */	NdrFcShort( 0xfe ),	/* Offset= 254 (642) */
/* 390 */	NdrFcLong( 0x4003 ),	/* 16387 */
/* 394 */	NdrFcShort( 0xfc ),	/* Offset= 252 (646) */
/* 396 */	NdrFcLong( 0x4014 ),	/* 16404 */
/* 400 */	NdrFcShort( 0xfa ),	/* Offset= 250 (650) */
/* 402 */	NdrFcLong( 0x4004 ),	/* 16388 */
/* 406 */	NdrFcShort( 0xf8 ),	/* Offset= 248 (654) */
/* 408 */	NdrFcLong( 0x4005 ),	/* 16389 */
/* 412 */	NdrFcShort( 0xf6 ),	/* Offset= 246 (658) */
/* 414 */	NdrFcLong( 0x400b ),	/* 16395 */
/* 418 */	NdrFcShort( 0xe0 ),	/* Offset= 224 (642) */
/* 420 */	NdrFcLong( 0x400a ),	/* 16394 */
/* 424 */	NdrFcShort( 0xde ),	/* Offset= 222 (646) */
/* 426 */	NdrFcLong( 0x4006 ),	/* 16390 */
/* 430 */	NdrFcShort( 0xe8 ),	/* Offset= 232 (662) */
/* 432 */	NdrFcLong( 0x4007 ),	/* 16391 */
/* 436 */	NdrFcShort( 0xde ),	/* Offset= 222 (658) */
/* 438 */	NdrFcLong( 0x4008 ),	/* 16392 */
/* 442 */	NdrFcShort( 0xe0 ),	/* Offset= 224 (666) */
/* 444 */	NdrFcLong( 0x400d ),	/* 16397 */
/* 448 */	NdrFcShort( 0xde ),	/* Offset= 222 (670) */
/* 450 */	NdrFcLong( 0x4009 ),	/* 16393 */
/* 454 */	NdrFcShort( 0xdc ),	/* Offset= 220 (674) */
/* 456 */	NdrFcLong( 0x6000 ),	/* 24576 */
/* 460 */	NdrFcShort( 0xda ),	/* Offset= 218 (678) */
/* 462 */	NdrFcLong( 0x400c ),	/* 16396 */
/* 466 */	NdrFcShort( 0xe0 ),	/* Offset= 224 (690) */
/* 468 */	NdrFcLong( 0x10 ),	/* 16 */
/* 472 */	NdrFcShort( 0x8002 ),	/* Simple arm type: FC_CHAR */
/* 474 */	NdrFcLong( 0x12 ),	/* 18 */
/* 478 */	NdrFcShort( 0x8006 ),	/* Simple arm type: FC_SHORT */
/* 480 */	NdrFcLong( 0x13 ),	/* 19 */
/* 484 */	NdrFcShort( 0x8008 ),	/* Simple arm type: FC_LONG */
/* 486 */	NdrFcLong( 0x15 ),	/* 21 */
/* 490 */	NdrFcShort( 0x800b ),	/* Simple arm type: FC_HYPER */
/* 492 */	NdrFcLong( 0x16 ),	/* 22 */
/* 496 */	NdrFcShort( 0x8008 ),	/* Simple arm type: FC_LONG */
/* 498 */	NdrFcLong( 0x17 ),	/* 23 */
/* 502 */	NdrFcShort( 0x8008 ),	/* Simple arm type: FC_LONG */
/* 504 */	NdrFcLong( 0xe ),	/* 14 */
/* 508 */	NdrFcShort( 0xbe ),	/* Offset= 190 (698) */
/* 510 */	NdrFcLong( 0x400e ),	/* 16398 */
/* 514 */	NdrFcShort( 0xc2 ),	/* Offset= 194 (708) */
/* 516 */	NdrFcLong( 0x4010 ),	/* 16400 */
/* 520 */	NdrFcShort( 0xc0 ),	/* Offset= 192 (712) */
/* 522 */	NdrFcLong( 0x4012 ),	/* 16402 */
/* 526 */	NdrFcShort( 0x74 ),	/* Offset= 116 (642) */
/* 528 */	NdrFcLong( 0x4013 ),	/* 16403 */
/* 532 */	NdrFcShort( 0x72 ),	/* Offset= 114 (646) */
/* 534 */	NdrFcLong( 0x4015 ),	/* 16405 */
/* 538 */	NdrFcShort( 0x70 ),	/* Offset= 112 (650) */
/* 540 */	NdrFcLong( 0x4016 ),	/* 16406 */
/* 544 */	NdrFcShort( 0x66 ),	/* Offset= 102 (646) */
/* 546 */	NdrFcLong( 0x4017 ),	/* 16407 */
/* 550 */	NdrFcShort( 0x60 ),	/* Offset= 96 (646) */
/* 552 */	NdrFcLong( 0x0 ),	/* 0 */
/* 556 */	NdrFcShort( 0x0 ),	/* Offset= 0 (556) */
/* 558 */	NdrFcLong( 0x1 ),	/* 1 */
/* 562 */	NdrFcShort( 0x0 ),	/* Offset= 0 (562) */
/* 564 */	NdrFcShort( 0xffff ),	/* Offset= -1 (563) */
/* 566 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 568 */	NdrFcShort( 0x8 ),	/* 8 */
/* 570 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 572 */	
			0x12, 0x0,	/* FC_UP */
/* 574 */	NdrFcShort( 0xfe20 ),	/* Offset= -480 (94) */
/* 576 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 578 */	NdrFcShort( 0x2 ),	/* Offset= 2 (580) */
/* 580 */	
			0x12, 0x0,	/* FC_UP */
/* 582 */	NdrFcShort( 0x1b8 ),	/* Offset= 440 (1022) */
/* 584 */	
			0x12, 0x0,	/* FC_UP */
/* 586 */	NdrFcShort( 0x20 ),	/* Offset= 32 (618) */
/* 588 */	
			0x2f,		/* FC_IP */
			0x5a,		/* FC_CONSTANT_IID */
/* 590 */	NdrFcLong( 0x2f ),	/* 47 */
/* 594 */	NdrFcShort( 0x0 ),	/* 0 */
/* 596 */	NdrFcShort( 0x0 ),	/* 0 */
/* 598 */	0xc0,		/* 192 */
			0x0,		/* 0 */
/* 600 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 602 */	0x0,		/* 0 */
			0x0,		/* 0 */
/* 604 */	0x0,		/* 0 */
			0x46,		/* 70 */
/* 606 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 608 */	NdrFcShort( 0x1 ),	/* 1 */
/* 610 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 612 */	NdrFcShort( 0x4 ),	/* 4 */
/* 614 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 616 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 618 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 620 */	NdrFcShort( 0x10 ),	/* 16 */
/* 622 */	NdrFcShort( 0x0 ),	/* 0 */
/* 624 */	NdrFcShort( 0xa ),	/* Offset= 10 (634) */
/* 626 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 628 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 630 */	NdrFcShort( 0xffd6 ),	/* Offset= -42 (588) */
/* 632 */	0x36,		/* FC_POINTER */
			0x5b,		/* FC_END */
/* 634 */	
			0x12, 0x0,	/* FC_UP */
/* 636 */	NdrFcShort( 0xffe2 ),	/* Offset= -30 (606) */
/* 638 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 640 */	0x1,		/* FC_BYTE */
			0x5c,		/* FC_PAD */
/* 642 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 644 */	0x6,		/* FC_SHORT */
			0x5c,		/* FC_PAD */
/* 646 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 648 */	0x8,		/* FC_LONG */
			0x5c,		/* FC_PAD */
/* 650 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 652 */	0xb,		/* FC_HYPER */
			0x5c,		/* FC_PAD */
/* 654 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 656 */	0xa,		/* FC_FLOAT */
			0x5c,		/* FC_PAD */
/* 658 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 660 */	0xc,		/* FC_DOUBLE */
			0x5c,		/* FC_PAD */
/* 662 */	
			0x12, 0x0,	/* FC_UP */
/* 664 */	NdrFcShort( 0xff9e ),	/* Offset= -98 (566) */
/* 666 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 668 */	NdrFcShort( 0xffa0 ),	/* Offset= -96 (572) */
/* 670 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 672 */	NdrFcShort( 0xfdfc ),	/* Offset= -516 (156) */
/* 674 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 676 */	NdrFcShort( 0xfe30 ),	/* Offset= -464 (212) */
/* 678 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 680 */	NdrFcShort( 0x2 ),	/* Offset= 2 (682) */
/* 682 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 684 */	NdrFcShort( 0x2 ),	/* Offset= 2 (686) */
/* 686 */	
			0x12, 0x0,	/* FC_UP */
/* 688 */	NdrFcShort( 0x14e ),	/* Offset= 334 (1022) */
/* 690 */	
			0x12, 0x10,	/* FC_UP [pointer_deref] */
/* 692 */	NdrFcShort( 0x2 ),	/* Offset= 2 (694) */
/* 694 */	
			0x12, 0x0,	/* FC_UP */
/* 696 */	NdrFcShort( 0x14 ),	/* Offset= 20 (716) */
/* 698 */	
			0x15,		/* FC_STRUCT */
			0x7,		/* 7 */
/* 700 */	NdrFcShort( 0x10 ),	/* 16 */
/* 702 */	0x6,		/* FC_SHORT */
			0x1,		/* FC_BYTE */
/* 704 */	0x1,		/* FC_BYTE */
			0x8,		/* FC_LONG */
/* 706 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 708 */	
			0x12, 0x0,	/* FC_UP */
/* 710 */	NdrFcShort( 0xfff4 ),	/* Offset= -12 (698) */
/* 712 */	
			0x12, 0x8,	/* FC_UP [simple_pointer] */
/* 714 */	0x2,		/* FC_CHAR */
			0x5c,		/* FC_PAD */
/* 716 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x7,		/* 7 */
/* 718 */	NdrFcShort( 0x20 ),	/* 32 */
/* 720 */	NdrFcShort( 0x0 ),	/* 0 */
/* 722 */	NdrFcShort( 0x0 ),	/* Offset= 0 (722) */
/* 724 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 726 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 728 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 730 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 732 */	NdrFcShort( 0xfe30 ),	/* Offset= -464 (268) */
/* 734 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 736 */	
			0x1b,		/* FC_CARRAY */
			0x3,		/* 3 */
/* 738 */	NdrFcShort( 0x4 ),	/* 4 */
/* 740 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 742 */	NdrFcShort( 0x0 ),	/* 0 */
/* 744 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 746 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 748 */	
			0x48,		/* FC_VARIABLE_REPEAT */
			0x49,		/* FC_FIXED_OFFSET */
/* 750 */	NdrFcShort( 0x4 ),	/* 4 */
/* 752 */	NdrFcShort( 0x0 ),	/* 0 */
/* 754 */	NdrFcShort( 0x1 ),	/* 1 */
/* 756 */	NdrFcShort( 0x0 ),	/* 0 */
/* 758 */	NdrFcShort( 0x0 ),	/* 0 */
/* 760 */	0x12, 0x0,	/* FC_UP */
/* 762 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (716) */
/* 764 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 766 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 768 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 770 */	NdrFcShort( 0x8 ),	/* 8 */
/* 772 */	NdrFcShort( 0x0 ),	/* 0 */
/* 774 */	NdrFcShort( 0x6 ),	/* Offset= 6 (780) */
/* 776 */	0x8,		/* FC_LONG */
			0x36,		/* FC_POINTER */
/* 778 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 780 */	
			0x11, 0x0,	/* FC_RP */
/* 782 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (736) */
/* 784 */	
			0x1b,		/* FC_CARRAY */
			0x3,		/* 3 */
/* 786 */	NdrFcShort( 0x4 ),	/* 4 */
/* 788 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 790 */	NdrFcShort( 0x0 ),	/* 0 */
/* 792 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 794 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 796 */	
			0x48,		/* FC_VARIABLE_REPEAT */
			0x49,		/* FC_FIXED_OFFSET */
/* 798 */	NdrFcShort( 0x4 ),	/* 4 */
/* 800 */	NdrFcShort( 0x0 ),	/* 0 */
/* 802 */	NdrFcShort( 0x1 ),	/* 1 */
/* 804 */	NdrFcShort( 0x0 ),	/* 0 */
/* 806 */	NdrFcShort( 0x0 ),	/* 0 */
/* 808 */	0x12, 0x0,	/* FC_UP */
/* 810 */	NdrFcShort( 0xff40 ),	/* Offset= -192 (618) */
/* 812 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 814 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 816 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 818 */	NdrFcShort( 0x8 ),	/* 8 */
/* 820 */	NdrFcShort( 0x0 ),	/* 0 */
/* 822 */	NdrFcShort( 0x6 ),	/* Offset= 6 (828) */
/* 824 */	0x8,		/* FC_LONG */
			0x36,		/* FC_POINTER */
/* 826 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 828 */	
			0x11, 0x0,	/* FC_RP */
/* 830 */	NdrFcShort( 0xffd2 ),	/* Offset= -46 (784) */
/* 832 */	
			0x1d,		/* FC_SMFARRAY */
			0x0,		/* 0 */
/* 834 */	NdrFcShort( 0x8 ),	/* 8 */
/* 836 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 838 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 840 */	NdrFcShort( 0x10 ),	/* 16 */
/* 842 */	0x8,		/* FC_LONG */
			0x6,		/* FC_SHORT */
/* 844 */	0x6,		/* FC_SHORT */
			0x4c,		/* FC_EMBEDDED_COMPLEX */
/* 846 */	0x0,		/* 0 */
			NdrFcShort( 0xfff1 ),	/* Offset= -15 (832) */
			0x5b,		/* FC_END */
/* 850 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 852 */	NdrFcShort( 0x18 ),	/* 24 */
/* 854 */	NdrFcShort( 0x0 ),	/* 0 */
/* 856 */	NdrFcShort( 0xa ),	/* Offset= 10 (866) */
/* 858 */	0x8,		/* FC_LONG */
			0x36,		/* FC_POINTER */
/* 860 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 862 */	NdrFcShort( 0xffe8 ),	/* Offset= -24 (838) */
/* 864 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 866 */	
			0x11, 0x0,	/* FC_RP */
/* 868 */	NdrFcShort( 0xfd4a ),	/* Offset= -694 (174) */
/* 870 */	
			0x1b,		/* FC_CARRAY */
			0x0,		/* 0 */
/* 872 */	NdrFcShort( 0x1 ),	/* 1 */
/* 874 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 876 */	NdrFcShort( 0x0 ),	/* 0 */
/* 878 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 880 */	0x1,		/* FC_BYTE */
			0x5b,		/* FC_END */
/* 882 */	
			0x16,		/* FC_PSTRUCT */
			0x3,		/* 3 */
/* 884 */	NdrFcShort( 0x8 ),	/* 8 */
/* 886 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 888 */	
			0x46,		/* FC_NO_REPEAT */
			0x5c,		/* FC_PAD */
/* 890 */	NdrFcShort( 0x4 ),	/* 4 */
/* 892 */	NdrFcShort( 0x4 ),	/* 4 */
/* 894 */	0x12, 0x0,	/* FC_UP */
/* 896 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (870) */
/* 898 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 900 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 902 */	
			0x1b,		/* FC_CARRAY */
			0x1,		/* 1 */
/* 904 */	NdrFcShort( 0x2 ),	/* 2 */
/* 906 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 908 */	NdrFcShort( 0x0 ),	/* 0 */
/* 910 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 912 */	0x6,		/* FC_SHORT */
			0x5b,		/* FC_END */
/* 914 */	
			0x16,		/* FC_PSTRUCT */
			0x3,		/* 3 */
/* 916 */	NdrFcShort( 0x8 ),	/* 8 */
/* 918 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 920 */	
			0x46,		/* FC_NO_REPEAT */
			0x5c,		/* FC_PAD */
/* 922 */	NdrFcShort( 0x4 ),	/* 4 */
/* 924 */	NdrFcShort( 0x4 ),	/* 4 */
/* 926 */	0x12, 0x0,	/* FC_UP */
/* 928 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (902) */
/* 930 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 932 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 934 */	
			0x1b,		/* FC_CARRAY */
			0x3,		/* 3 */
/* 936 */	NdrFcShort( 0x4 ),	/* 4 */
/* 938 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 940 */	NdrFcShort( 0x0 ),	/* 0 */
/* 942 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 944 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 946 */	
			0x16,		/* FC_PSTRUCT */
			0x3,		/* 3 */
/* 948 */	NdrFcShort( 0x8 ),	/* 8 */
/* 950 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 952 */	
			0x46,		/* FC_NO_REPEAT */
			0x5c,		/* FC_PAD */
/* 954 */	NdrFcShort( 0x4 ),	/* 4 */
/* 956 */	NdrFcShort( 0x4 ),	/* 4 */
/* 958 */	0x12, 0x0,	/* FC_UP */
/* 960 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (934) */
/* 962 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 964 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 966 */	
			0x1b,		/* FC_CARRAY */
			0x7,		/* 7 */
/* 968 */	NdrFcShort( 0x8 ),	/* 8 */
/* 970 */	0x19,		/* Corr desc:  field pointer, FC_ULONG */
			0x0,		/*  */
/* 972 */	NdrFcShort( 0x0 ),	/* 0 */
/* 974 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 976 */	0xb,		/* FC_HYPER */
			0x5b,		/* FC_END */
/* 978 */	
			0x16,		/* FC_PSTRUCT */
			0x3,		/* 3 */
/* 980 */	NdrFcShort( 0x8 ),	/* 8 */
/* 982 */	
			0x4b,		/* FC_PP */
			0x5c,		/* FC_PAD */
/* 984 */	
			0x46,		/* FC_NO_REPEAT */
			0x5c,		/* FC_PAD */
/* 986 */	NdrFcShort( 0x4 ),	/* 4 */
/* 988 */	NdrFcShort( 0x4 ),	/* 4 */
/* 990 */	0x12, 0x0,	/* FC_UP */
/* 992 */	NdrFcShort( 0xffe6 ),	/* Offset= -26 (966) */
/* 994 */	
			0x5b,		/* FC_END */

			0x8,		/* FC_LONG */
/* 996 */	0x8,		/* FC_LONG */
			0x5b,		/* FC_END */
/* 998 */	
			0x15,		/* FC_STRUCT */
			0x3,		/* 3 */
/* 1000 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1002 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1004 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1006 */	
			0x1b,		/* FC_CARRAY */
			0x3,		/* 3 */
/* 1008 */	NdrFcShort( 0x8 ),	/* 8 */
/* 1010 */	0x7,		/* Corr desc: FC_USHORT */
			0x0,		/*  */
/* 1012 */	NdrFcShort( 0xffd8 ),	/* -40 */
/* 1014 */	NdrFcShort( 0x1 ),	/* Corr flags:  early, */
/* 1016 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1018 */	NdrFcShort( 0xffec ),	/* Offset= -20 (998) */
/* 1020 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1022 */	
			0x1a,		/* FC_BOGUS_STRUCT */
			0x3,		/* 3 */
/* 1024 */	NdrFcShort( 0x28 ),	/* 40 */
/* 1026 */	NdrFcShort( 0xffec ),	/* Offset= -20 (1006) */
/* 1028 */	NdrFcShort( 0x0 ),	/* Offset= 0 (1028) */
/* 1030 */	0x6,		/* FC_SHORT */
			0x6,		/* FC_SHORT */
/* 1032 */	0x8,		/* FC_LONG */
			0x8,		/* FC_LONG */
/* 1034 */	0x4c,		/* FC_EMBEDDED_COMPLEX */
			0x0,		/* 0 */
/* 1036 */	NdrFcShort( 0xfc02 ),	/* Offset= -1022 (14) */
/* 1038 */	0x5c,		/* FC_PAD */
			0x5b,		/* FC_END */
/* 1040 */	0xb4,		/* FC_USER_MARSHAL */
			0x83,		/* 131 */
/* 1042 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1044 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1046 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1048 */	NdrFcShort( 0xfbee ),	/* Offset= -1042 (6) */
/* 1050 */	0xb4,		/* FC_USER_MARSHAL */
			0x83,		/* 131 */
/* 1052 */	NdrFcShort( 0x1 ),	/* 1 */
/* 1054 */	NdrFcShort( 0x4 ),	/* 4 */
/* 1056 */	NdrFcShort( 0x0 ),	/* 0 */
/* 1058 */	NdrFcShort( 0xfe1a ),	/* Offset= -486 (572) */

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
    42,
    90,
    144,
    192,
    234,
    282,
    324
    };

static const MIDL_STUBLESS_PROXY_INFO ITaskService_ProxyInfo =
    {
    &Object_StubDesc,
    __MIDL_ProcFormatString.Format,
    &ITaskService_FormatStringOffsetTable[-3],
    0,
    0,
    0
    };


static const MIDL_SERVER_INFO ITaskService_ServerInfo = 
    {
    &Object_StubDesc,
    0,
    __MIDL_ProcFormatString.Format,
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
    __MIDL_TypeFormatString.Format,
    1, /* -error bounds_check flag */
    0x50002, /* Ndr library version */
    0,
    0x600016e, /* MIDL Version 6.0.366 */
    0,
    UserMarshalRoutines,
    0,  /* notify & notify_flag routine table */
    0x1, /* MIDL flag */
    0, /* cs routines */
    0,   /* proxy/server info */
    0   /* Reserved5 */
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

