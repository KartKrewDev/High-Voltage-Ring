#pragma once

#include <cstdint>
#include <vector>
#include <map>
#include <memory>

#include <Windows.h>
#include "gl_load/gl_system.h"

#undef min
#undef max

#define APART(x) (static_cast<uint32_t>(x) >> 24)
#define RPART(x) ((static_cast<uint32_t>(x) >> 16)  & 0xff)
#define GPART(x) ((static_cast<uint32_t>(x) >> 8)  & 0xff)
#define BPART(x) (static_cast<uint32_t>(x) & 0xff)
