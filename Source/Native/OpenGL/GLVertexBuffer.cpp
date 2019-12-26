/*
**  BuilderNative Renderer
**  Copyright (c) 2019 Magnus Norddahl
**
**  This software is provided 'as-is', without any express or implied
**  warranty.  In no event will the authors be held liable for any damages
**  arising from the use of this software.
**
**  Permission is granted to anyone to use this software for any purpose,
**  including commercial applications, and to alter it and redistribute it
**  freely, subject to the following restrictions:
**
**  1. The origin of this software must not be misrepresented; you must not
**     claim that you wrote the original software. If you use this software
**     in a product, an acknowledgment in the product documentation would be
**     appreciated but is not required.
**  2. Altered source versions must be plainly marked as such, and must not be
**     misrepresented as being the original software.
**  3. This notice may not be removed or altered from any source distribution.
*/

#include "Precomp.h"
#include "GLVertexBuffer.h"
#include "GLShader.h"
#include "GLRenderDevice.h"

GLuint GLSharedVertexBuffer::GetBuffer()
{
	if (mBuffer == 0)
		glGenBuffers(1, &mBuffer);
	return mBuffer;
}

GLuint GLSharedVertexBuffer::GetVAO()
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

void GLSharedVertexBuffer::SetupFlatVAO()
{
	glEnableVertexAttribArray((int)DeclarationUsage::Position);
	glEnableVertexAttribArray((int)DeclarationUsage::Color);
	glEnableVertexAttribArray((int)DeclarationUsage::TextureCoordinate);
	glVertexAttribPointer((int)DeclarationUsage::Position, 3, GL_FLOAT, GL_FALSE, VertexBuffer::FlatStride, (const void*)0);
	glVertexAttribPointer((int)DeclarationUsage::Color, GL_BGRA, GL_UNSIGNED_BYTE, GL_TRUE, VertexBuffer::FlatStride, (const void*)12);
	glVertexAttribPointer((int)DeclarationUsage::TextureCoordinate, 2, GL_FLOAT, GL_FALSE, VertexBuffer::FlatStride, (const void*)16);
}

void GLSharedVertexBuffer::SetupWorldVAO()
{
	glEnableVertexAttribArray((int)DeclarationUsage::Position);
	glEnableVertexAttribArray((int)DeclarationUsage::Color);
	glEnableVertexAttribArray((int)DeclarationUsage::TextureCoordinate);
	glEnableVertexAttribArray((int)DeclarationUsage::Normal);
	glVertexAttribPointer((int)DeclarationUsage::Position, 3, GL_FLOAT, GL_FALSE, VertexBuffer::WorldStride, (const void*)0);
	glVertexAttribPointer((int)DeclarationUsage::Color, GL_BGRA, GL_UNSIGNED_BYTE, GL_TRUE, VertexBuffer::WorldStride, (const void*)12);
	glVertexAttribPointer((int)DeclarationUsage::TextureCoordinate, 2, GL_FLOAT, GL_FALSE, VertexBuffer::WorldStride, (const void*)16);
	glVertexAttribPointer((int)DeclarationUsage::Normal, 3, GL_FLOAT, GL_FALSE, VertexBuffer::WorldStride, (const void*)24);
}

/////////////////////////////////////////////////////////////////////////////

GLVertexBuffer::~GLVertexBuffer()
{
	Finalize();
}

void GLVertexBuffer::Finalize()
{
	if (Device)
	{
		Device->mSharedVertexBuffers[(int)Format]->VertexBuffers.erase(ListIt);
		Device = nullptr;
	}
}
