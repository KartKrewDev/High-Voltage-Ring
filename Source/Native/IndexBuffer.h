#pragma once

class IndexBuffer
{
public:
	IndexBuffer(int sizeInBytes);
	~IndexBuffer();

	void SetBufferData(const void* data, int64_t size);

	GLuint GetBuffer();

private:
	std::vector<uint8_t> mData;
	GLuint mBuffer = 0;
};
