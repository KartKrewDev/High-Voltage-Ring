
#include "Precomp.h"
#include "IndexBuffer.h"

IndexBuffer::IndexBuffer(int sizeInBytes) : mSize(sizeInBytes)
{
}

IndexBuffer::~IndexBuffer()
{
	// To do: move mBuffer to a delete list as this might be called by a finalizer in a different thread
}

GLuint IndexBuffer::GetBuffer()
{
	if (mBuffer == 0)
	{
		glGenBuffers(1, &mBuffer);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, mBuffer);
		glBufferData(GL_ELEMENT_ARRAY_BUFFER, mSize, nullptr, GL_STREAM_DRAW);
	}
	return mBuffer;
}

/////////////////////////////////////////////////////////////////////////////

IndexBuffer* IndexBuffer_New(int sizeInBytes)
{
	return new IndexBuffer(sizeInBytes);
}

void IndexBuffer_Delete(IndexBuffer* buffer)
{
	delete buffer;
}
