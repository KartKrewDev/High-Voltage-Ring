#pragma once

#include <cstdint>

class IndexBuffer
{
public:
	IndexBuffer(int sizeInBytes);

	void SetBufferData(const void* data, int64_t size);
};
