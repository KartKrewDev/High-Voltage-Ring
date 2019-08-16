#pragma once

enum class VertexFormat : int32_t { Flat, World };

class VertexBuffer
{
public:
	VertexBuffer();
	~VertexBuffer();

	GLuint GetBuffer();
	GLuint GetVAO();

	VertexFormat Format = VertexFormat::Flat;

	static const int FlatStride = 24;
	static const int WorldStride = 36;

	static void SetupFlatVAO();
	static void SetupWorldVAO();
	
private:
	GLuint mBuffer = 0;
	GLuint mVAO = 0;
};
