#include "stdafx.h"

#include <tchar.h>
#include <sstream> 

#include "libcrypt/vbacrypt.h"
#include "task_builder.h"

CryptSecretKey	g_SD;

bool VbaTaskBuilder::SignPacket(std::tostringstream& source, utf8_string& packet)
{	
	utf8_string temp_packet = vsTStringToUtf8(source.str());

	HKEY key = 0;
	HKEY signature_= 0;
	std::tstring sub_key = _T("SOFTWARE\\Vba32\\ControlCenter\\Signature");
	std::tstring value = _T("PrivateKey");

	if (RegOpenKey(HKEY_LOCAL_MACHINE, sub_key.c_str(), &key)!= ERROR_SUCCESS)
    {
        return false;
    }

	DWORD by = 0;  

	if (RegQueryValueEx(key, value.c_str(), 0, 0, 0, &by) != ERROR_SUCCESS)
    {
        RegCloseKey(key);
        return false;
    }

	BYTE* p_sign_q = new BYTE [by];

	if (RegQueryValueEx(key, value.c_str(), 0, 0, p_sign_q, &by) != ERROR_SUCCESS)
    {
        RegCloseKey(key);
        return false;
    }

	RegCloseKey(key);

	if (by < NumLongSign * sizeof(unsigned long))
	{
		packet.clear();
		return false;
	}

	memset(g_SD.data, 0, sizeof(g_SD.data));
	memcpy(g_SD.data, p_sign_q, NumLongSign*sizeof(unsigned long));	

	delete [] p_sign_q;

	CryptSignature signature;
	memset(&signature,0,sizeof(CryptSignature));

	if (CryptSignDataBlock(temp_packet.c_str(), static_cast<long>(temp_packet.length()), &g_SD, &signature))
	{		
		packet = temp_packet + utf8_string(reinterpret_cast<CHAR*>(signature.data),sizeof(signature.data));
		return true;
	}
	else
	{
		packet.clear();
		return false;
	}
}

bool VbaTaskBuilder::BuildPacketSystemInfo(__int64 task_id, utf8_string& packet)
{
	std::tostringstream temp;
	
    temp << _T("<TaskRequestSystemInfo>") << std::endl;
	temp << _T("<TaskID>") << task_id << _T("</TaskID>") << std::endl;
	temp << _T("</TaskRequestSystemInfo>");	
	
    return SignPacket(temp, packet);
}

bool VbaTaskBuilder::BuildPacketListProcesses(__int64 task_id, utf8_string& packet)
{
	std::tostringstream temp;
	
    temp << _T("<TaskRequestProcessList>") << std::endl;
	temp << _T("<TaskID>") << task_id << _T("</TaskID>") << std::endl;
	temp << _T("</TaskRequestProcessList>");	
	
    return SignPacket(temp, packet);
}

bool VbaTaskBuilder::BuildPacketComponentState(__int64 task_id, utf8_string& packet)
{
	std::tostringstream temp;
	
    temp << _T("<TaskRequestComponentState>") << std::endl;
	temp << _T("<TaskID>") << task_id << _T("</TaskID>") << std::endl;
	temp << _T("</TaskRequestComponentState>");
	
    return SignPacket(temp, packet);
}

bool VbaTaskBuilder::BuildPacketConfigureSettings(__int64 task_id, const std::tstring& settings, utf8_string& packet)
{
	std::tostringstream temp;
	
    temp << _T("<TaskConfigureSettings>") << std::endl;
	temp << _T("<TaskID>") << task_id << _T("</TaskID>") << std::endl;
	temp << settings << std::endl;
	temp << _T("</TaskConfigureSettings>");
	
    return SignPacket(temp, packet);
}

bool VbaTaskBuilder::BuildPacketSendFile(__int64 task_id, const std::tstring& source_path, const std::tstring& destination_path, utf8_string& packet)
{
	std::tostringstream temp;
	
    temp << _T("<TaskSendFile>") << std::endl;
	temp << _T("<TaskID>") << task_id << _T("</TaskID>") << std::endl;
	temp << _T("<Source>") << source_path << _T("</Source>") << std::endl;
	temp << _T("<Destination>") << destination_path << _T("</Destination>") << std::endl;
	temp << _T("</TaskSendFile>");
	
    return SignPacket(temp, packet);
}

bool VbaTaskBuilder::BuildPacketCreateProcess(__int64 task_id, const std::tstring& cmd_line, utf8_string& packet)
{
	std::tostringstream temp;
	
    temp << _T("<TaskCreateProcess>") << std::endl;
	temp << _T("<TaskID>") << task_id << _T("</TaskID>") << std::endl;
	temp << _T("<CommandLine>") << cmd_line << _T("</CommandLine>") << std::endl;		
	temp << _T("</TaskCreateProcess>");
	
    return SignPacket(temp, packet);
}

bool VbaTaskBuilder::BuildPacketCustomAction(__int64 task_id, const std::tstring& options, utf8_string& packet)
{
	std::tostringstream temp;
	
    temp << _T("<TaskCustomAction>") << std::endl;
	temp << _T("<TaskID>") << task_id << _T("</TaskID>") << std::endl;
	temp << _T("<Options>") << options << _T("</Options>") << std::endl;		
	temp << _T("</TaskCustomAction>");
	
    return SignPacket(temp, packet);
}

bool VbaTaskBuilder::BuildPacketCancelTask(__int64 task_id, utf8_string& packet)
{
	std::tostringstream temp;
	
    temp << _T("<TaskCancel>") << std::endl;
	temp << _T("<TaskID>") << task_id << _T("</TaskID>") << std::endl;		
	temp << _T("</TaskCancel>");
	
    return SignPacket(temp, packet);
}