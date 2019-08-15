#pragma once

class IndexBuffer
{
public:
	IndexBuffer(int sizeInBytes);
	~IndexBuffer();

	GLuint GetBuffer();

private:
	int64_t mSize = 0;
	GLuint mBuffer = 0;
};
