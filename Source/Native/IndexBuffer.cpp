
#include "Precomp.h"
#include "IndexBuffer.h"

IndexBuffer::IndexBuffer(int sizeInBytes) : mData(sizeInBytes)
{
}

void IndexBuffer::SetBufferData(const void* data, int64_t size)
{
	if (size > 0 && size < (int64_t)mData.size())
		memcpy(mData.data(), data, size);
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
