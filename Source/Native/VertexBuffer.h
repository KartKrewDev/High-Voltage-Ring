#pragma once

#include <cstdint>
#include <vector>

class VertexBuffer
{
public:
	VertexBuffer(int sizeInBytes);

	void SetBufferData(const void* data, int64_t size);
	void SetBufferSubdata(int64_t destOffset, const void* data, int64_t size);
	
private:
	std::vector<uint8_t> mData;
};
