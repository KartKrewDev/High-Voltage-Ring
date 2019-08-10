#pragma once

class VertexBuffer
{
public:
	VertexBuffer(int sizeInBytes);
	~VertexBuffer();

	void SetBufferData(const void* data, int64_t size);
	void SetBufferSubdata(int64_t destOffset, const void* data, int64_t size);

	GLuint GetBuffer();
	
private:
	std::vector<uint8_t> mData;
	GLuint mBuffer = 0;
};
