
#include "Precomp.h"
#include "VertexBuffer.h"
#include "Shader.h"
#include "RenderDevice.h"

SharedVertexBuffer::SharedVertexBuffer(VertexFormat format, int size) : Format(format), Size(size)
{
}

GLuint SharedVertexBuffer::GetBuffer()
{
	if (mBuffer == 0)
		glGenBuffers(1, &mBuffer);
	return mBuffer;
}

GLuint SharedVertexBuffer::GetVAO()
{
	if (!mVAO)
	{
		glGenVertexArrays(1, &mVAO);
		glBindVertexArray(mVAO);
		glBindBuffer(GL_ARRAY_BUFFER, GetBuffer());
		if (Format == VertexFormat::Flat)
			SetupFlatVAO();
		else
			SetupWorldVAO();
		glBindBuffer(GL_ARRAY_BUFFER, 0);
	}
	return mVAO;
}

void SharedVertexBuffer::SetupFlatVAO()
{
	glEnableVertexAttribArray((int)DeclarationUsage::Position);
	glEnableVertexAttribArray((int)DeclarationUsage::Color);
	glEnableVertexAttribArray((int)DeclarationUsage::TextureCoordinate);
	glVertexAttribPointer((int)DeclarationUsage::Position, 3, GL_FLOAT, GL_FALSE, FlatStride, (const void*)0);
	glVertexAttribPointer((int)DeclarationUsage::Color, GL_BGRA, GL_UNSIGNED_BYTE, GL_TRUE, FlatStride, (const void*)12);
	glVertexAttribPointer((int)DeclarationUsage::TextureCoordinate, 2, GL_FLOAT, GL_FALSE, FlatStride, (const void*)16);
}

void SharedVertexBuffer::SetupWorldVAO()
{
	glEnableVertexAttribArray((int)DeclarationUsage::Position);
	glEnableVertexAttribArray((int)DeclarationUsage::Color);
	glEnableVertexAttribArray((int)DeclarationUsage::TextureCoordinate);
	glEnableVertexAttribArray((int)DeclarationUsage::Normal);
	glVertexAttribPointer((int)DeclarationUsage::Position, 3, GL_FLOAT, GL_FALSE, WorldStride, (const void*)0);
	glVertexAttribPointer((int)DeclarationUsage::Color, GL_BGRA, GL_UNSIGNED_BYTE, GL_TRUE, WorldStride, (const void*)12);
	glVertexAttribPointer((int)DeclarationUsage::TextureCoordinate, 2, GL_FLOAT, GL_FALSE, WorldStride, (const void*)16);
	glVertexAttribPointer((int)DeclarationUsage::Normal, 3, GL_FLOAT, GL_FALSE, WorldStride, (const void*)24);
}

/////////////////////////////////////////////////////////////////////////////

VertexBuffer::~VertexBuffer()
{
	if (Device)
	{
		Device->mSharedVertexBuffers[(int)Format]->VertexBuffers.erase(ListIt);
		Device = nullptr;
	}
}

/////////////////////////////////////////////////////////////////////////////

extern "C"
{

VertexBuffer* VertexBuffer_New()
{
	return new VertexBuffer();
}

void VertexBuffer_Delete(VertexBuffer* buffer)
{
	RenderDevice::DeleteObject(buffer);
}

}
