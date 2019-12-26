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

#pragma once

#include <list>

#include "../Backend.h"

class GLRenderDevice;
class GLVertexBuffer;

class GLSharedVertexBuffer
{
public:
	GLSharedVertexBuffer(VertexFormat format, int size) : Format(format), Size(size) { }

	GLuint GetBuffer();
	GLuint GetVAO();

	VertexFormat Format = VertexFormat::Flat;

	int NextPos = 0;
	int Size = 0;

	std::list<GLVertexBuffer*> VertexBuffers;

	static void SetupFlatVAO();
	static void SetupWorldVAO();
	
private:
	GLuint mBuffer = 0;
	GLuint mVAO = 0;
};

class GLVertexBuffer : public VertexBuffer
{
public:
	~GLVertexBuffer();

	void Finalize();

	VertexFormat Format = VertexFormat::Flat;

	GLRenderDevice* Device = nullptr;
	std::list<GLVertexBuffer*>::iterator ListIt;

	int BufferOffset = 0;
	int BufferStartIndex = 0;
	int Size = 0;
};
