#pragma once

#include "common/tstring.h"
#include "common/strings.h"

class VbaTaskBuilder
{
private:

	///
	///	\brief ������ ������������ ������������ ������
	///
	/// \param source - �����, ���������� ������ ��� ������
	/// \param ������������� ����� (std::tstring), 
	///         � ������ ���� �� ������� ��������� �� ��������� ������ ������
	///
	/// \return true - ���� ����� ������� ������������, � ��������� ������ - false
    static bool SignPacket(std::tostringstream& source, vba::utf8_string& packet);

public:
	static bool BuildPacketSystemInfo(__int64 task_id,vba::utf8_string& packet);
	static bool BuildPacketListProcesses(__int64 task_id,vba::utf8_string& packet);
	static bool BuildPacketComponentState(__int64 task_id,vba::utf8_string& packet);
	static bool BuildPacketConfigureSettings( __int64 task_id, const std::tstring& settings,vba::utf8_string& packet);
	static bool BuildPacketSendFile(__int64 task_id, const std::tstring& source_path,  const std::tstring& destination_path,vba::utf8_string& packet);
	static bool BuildPacketCreateProcess(__int64 task_id, const std::tstring& cmd_line,vba::utf8_string& packet);
	static bool BuildPacketCustomAction(__int64 task_id, const std::tstring& options,vba::utf8_string& packet);
    static bool BuildPacketCancelTask(__int64 task_id, vba::utf8_string& packet);
};