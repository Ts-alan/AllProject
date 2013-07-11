#pragma once

#include "common/strings.h"

enum TASK_STATE
{
    TASK_STATE_IN_QUEUE,
	TASK_STATE_SENDED,
	TASK_STATE_ERROR,
	TASK_STATE_CANCELLED
};

std::wstring STATES[];