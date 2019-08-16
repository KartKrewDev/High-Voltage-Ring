
#include "Precomp.h"
#include "IndexBuffer.h"

IndexBuffer::IndexBuffer()
{
}

IndexBuffer::~IndexBuffer()
{
	// To do: move mBuffer to a delete list as this might be called by a finalizer in a different thread
}

GLuint IndexBuffer::GetBuffer()
{
	if (mBuffer == 0)
		glGenBuffers(1, &mBuffer);
	return mBuffer;
}

/////////////////////////////////////////////////////////////////////////////

IndexBuffer* IndexBuffer_New()
{
	return new IndexBuffer();
}

void IndexBuffer_Delete(IndexBuffer* buffer)
{
	delete buffer;
}
