#pragma once

class RenderDevice;

class IndexBuffer
{
public:
	~IndexBuffer();

	GLuint GetBuffer();

	RenderDevice* Device = nullptr;

private:
	GLuint mBuffer = 0;
};
