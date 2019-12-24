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
#include "GLBackend.h"
#include "GLRenderDevice.h"
#include "GLVertexBuffer.h"
#include "GLIndexBuffer.h"
#include "GLTexture.h"

RenderDevice* GLBackend::NewRenderDevice(void* disp, void* window)
{
	GLRenderDevice* device = new GLRenderDevice(disp, window);
	if (!device->Context)
	{
		delete device;
		return nullptr;
	}
	else
	{
		return device;
	}
}

void GLBackend::DeleteRenderDevice(RenderDevice* device)
{
	delete device;
}

VertexBuffer* GLBackend::NewVertexBuffer()
{
	return new GLVertexBuffer();
}

void GLBackend::DeleteVertexBuffer(VertexBuffer* buffer)
{
	GLRenderDevice::DeleteObject(static_cast<GLVertexBuffer*>(buffer));
}

IndexBuffer* GLBackend::NewIndexBuffer()
{
	return new GLIndexBuffer();
}

void GLBackend::DeleteIndexBuffer(IndexBuffer* buffer)
{
	GLRenderDevice::DeleteObject(static_cast<GLIndexBuffer*>(buffer));
}

Texture* GLBackend::NewTexture()
{
	return new GLTexture();
}

void GLBackend::DeleteTexture(Texture* texture)
{
	GLRenderDevice::DeleteObject(static_cast<GLTexture*>(texture));
}
