
#include "VertexBuffer.h"

VertexBuffer::VertexBuffer(int sizeInBytes) : mData(sizeInBytes)
{
}

void VertexBuffer::SetBufferData(const void* data, int64_t size)
{
	if (size > 0 && size < (int64_t)mData.size())
		memcpy(mData.data(), data, size);
}

void VertexBuffer::SetBufferSubdata(int64_t destOffset, const void* data, int64_t size)
{
	if (destOffset >= 0 && size > 0 && size < (int64_t)mData.size() - destOffset)
		memcpy(mData.data() + destOffset, data, size);
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

void VertexBuffer_SetBufferData(VertexBuffer* handle, void* data, int64_t size)
{
	handle->SetBufferData(data, size);
}

void VertexBuffer_SetBufferSubdata(VertexBuffer* handle, int64_t destOffset, void* data, int64_t size)
{
	handle->SetBufferSubdata(destOffset, data, size);
}
