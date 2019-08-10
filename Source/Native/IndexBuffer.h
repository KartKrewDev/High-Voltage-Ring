#pragma once

class IndexBuffer
{
public:
	IndexBuffer(int sizeInBytes);

	void SetBufferData(const void* data, int64_t size);

private:
	std::vector<uint8_t> mData;
};
