#pragma once

class IndexBuffer
{
public:
	IndexBuffer();
	~IndexBuffer();

	GLuint GetBuffer();

private:
	GLuint mBuffer = 0;
};
