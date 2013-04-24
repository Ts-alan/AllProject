//////////////////////////////////////////////////
//	Header file for service startup routines	//
//	(c) 2008 VirusBlokAda Ltd.				    //
//////////////////////////////////////////////////

#pragma once

#include <windows.h>

//////////////////////////////////////////////////
// Checks key file validity and puts it into log
// Input parameters: recent_time
// Return value: indicates whether keyfile is valid or not
BOOL CheckKeyFileValidity(BOOL write_to_log);


//////////////////////////////////////////////////
// Checks program integrity and puts it into log
// Input parameters: none
// Return value: indicates whether program is integral
BOOL CheckProgramIntegrity();
