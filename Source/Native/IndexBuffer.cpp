
#include "Precomp.h"
#include "IndexBuffer.h"
#include "RenderDevice.h"

IndexBuffer::~IndexBuffer()
{
	if (Device && mBuffer != 0)
	{
		glDeleteBuffers(1, &mBuffer);
		mBuffer = 0;
	}
}

GLuint IndexBuffer::GetBuffer()
{
	if (mBuffer == 0)
		glGenBuffers(1, &mBuffer);
	return mBuffer;
}

/////////////////////////////////////////////////////////////////////////////

extern "C"
{

IndexBuffer* IndexBuffer_New()
{
	return new IndexBuffer();
}

void IndexBuffer_Delete(IndexBuffer* buffer)
{
	RenderDevice::DeleteObject(buffer);
}

}
