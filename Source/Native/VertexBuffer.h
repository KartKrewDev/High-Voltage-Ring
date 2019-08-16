#pragma once

class VertexBuffer
{
public:
	VertexBuffer();
	~VertexBuffer();

	GLuint GetBuffer();
	
private:
	GLuint mBuffer = 0;
};
