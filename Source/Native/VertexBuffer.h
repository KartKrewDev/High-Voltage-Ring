#pragma once

#include <list>

enum class VertexFormat : int32_t { Flat, World };

class RenderDevice;
class VertexBuffer;

class SharedVertexBuffer
{
public:
	SharedVertexBuffer(VertexFormat format, int size);

	GLuint GetBuffer();
	GLuint GetVAO();

	VertexFormat Format = VertexFormat::Flat;

	int NextPos = 0;
	int Size = 0;

	std::list<VertexBuffer*> VertexBuffers;

	static const int FlatStride = 24;
	static const int WorldStride = 36;

	static void SetupFlatVAO();
	static void SetupWorldVAO();
	
private:
	GLuint mBuffer = 0;
	GLuint mVAO = 0;
};

class VertexBuffer
{
public:
	~VertexBuffer();

	VertexFormat Format = VertexFormat::Flat;

	RenderDevice* Device = nullptr;
	std::list<VertexBuffer*>::iterator ListIt;

	int BufferOffset = 0;
	int BufferStartIndex = 0;
	int Size = 0;
};
