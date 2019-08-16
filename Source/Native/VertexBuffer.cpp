
#include "Precomp.h"
#include "VertexBuffer.h"

VertexBuffer::VertexBuffer()
{
}

VertexBuffer::~VertexBuffer()
{
	// To do: move mBuffer to a delete list as this might be called by a finalizer in a different thread
}

GLuint VertexBuffer::GetBuffer()
{
	if (mBuffer == 0)
		glGenBuffers(1, &mBuffer);
	return mBuffer;
}

/////////////////////////////////////////////////////////////////////////////

VertexBuffer* VertexBuffer_New()
{
	return new VertexBuffer();
}

void VertexBuffer_Delete(VertexBuffer* buffer)
{
	delete buffer;
}
