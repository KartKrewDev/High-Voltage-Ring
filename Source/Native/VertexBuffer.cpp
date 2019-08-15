
#include "Precomp.h"
#include "VertexBuffer.h"

VertexBuffer::VertexBuffer(int sizeInBytes) : mSize(sizeInBytes)
{
}

VertexBuffer::~VertexBuffer()
{
	// To do: move mBuffer to a delete list as this might be called by a finalizer in a different thread
}

GLuint VertexBuffer::GetBuffer()
{
	if (mBuffer == 0)
	{
		glGenBuffers(1, &mBuffer);
		glBindBuffer(GL_ARRAY_BUFFER, mBuffer);
		glBufferData(GL_ARRAY_BUFFER, mSize, nullptr, GL_STREAM_DRAW);
	}
	return mBuffer;
}

/////////////////////////////////////////////////////////////////////////////

VertexBuffer* VertexBuffer_New(int sizeInBytes)
{
	return new VertexBuffer(sizeInBytes);
}

void VertexBuffer_Delete(VertexBuffer* buffer)
{
	delete buffer;
}
