#pragma once

#define _CRT_SECURE_NO_WARNINGS

#include <cstdint>
#include <vector>
#include <map>
#include <memory>

#ifdef WIN32
#include <Windows.h>
#undef min
#undef max
#endif

#include "gl_load/gl_system.h"

#define APART(x) (static_cast<uint32_t>(x) >> 24)
#define RPART(x) ((static_cast<uint32_t>(x) >> 16)  & 0xff)
#define GPART(x) ((static_cast<uint32_t>(x) >> 8)  & 0xff)
#define BPART(x) (static_cast<uint32_t>(x) & 0xff)
