#pragma once

#include "common/tstring.h"
#include "common/strings.h"

class VbaTaskBuilder
{
private:

	///
	///	\brief Функци формирования подписанного пакета
	///
	/// \param source - поток, содержащий данные для пакета
	/// \param сформированый пакет (std::tstring), 
	///         в случае если не удалось подписать то вернеться пустая строка
	///
	/// \return true - если пакет удалось сформировать, в противном случае - false
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