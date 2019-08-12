
#include "Precomp.h"
#include "IndexBuffer.h"

IndexBuffer::IndexBuffer(int sizeInBytes) : mData(sizeInBytes)
{
}

IndexBuffer::~IndexBuffer()
{
	// To do: move mBuffer to a delete list as this might be called by a finalizer in a different thread
}

void IndexBuffer::SetBufferData(const void* data, int64_t size)
{
	if (size > 0 && size <= (int64_t)mData.size())
		memcpy(mData.data(), data, size);
}

GLuint IndexBuffer::GetBuffer()
{
	if (mBuffer == 0)
	{
		glGenBuffers(1, &mBuffer);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, mBuffer);
		glBufferData(GL_ELEMENT_ARRAY_BUFFER, mData.size(), mData.data(), GL_STATIC_DRAW);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
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

void IndexBuffer_SetBufferData(IndexBuffer* handle, void* data, int64_t size)
{
	handle->SetBufferData(data, size);
}
