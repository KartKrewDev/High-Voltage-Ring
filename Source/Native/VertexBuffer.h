#pragma once

class VertexBuffer
{
public:
	VertexBuffer(int sizeInBytes);
	~VertexBuffer();

	GLuint GetBuffer();
	
private:
	int64_t mSize = 0;
	GLuint mBuffer = 0;
};
