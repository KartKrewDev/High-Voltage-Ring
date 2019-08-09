#pragma once

#include <cstdint>

class VertexBuffer
{
public:
	VertexBuffer(int sizeInBytes);

	void SetBufferData(const void* data, int64_t size);
	void SetBufferSubdata(int64_t destOffset, const void* data, int64_t size);
};
