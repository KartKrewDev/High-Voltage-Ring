#pragma once

enum class VertexFormat : int32_t { Flat, World };

class SharedVertexBuffer
{
public:
	SharedVertexBuffer(VertexFormat format, int64_t size);

	GLuint GetBuffer();
	GLuint GetVAO();

	VertexFormat Format = VertexFormat::Flat;

	int64_t NextPos = 0;
	int64_t Size = 0;

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
	VertexBuffer();
	~VertexBuffer();

	VertexFormat Format = VertexFormat::Flat;

	int BufferOffset = 0;
	int BufferStartIndex = 0;
};
