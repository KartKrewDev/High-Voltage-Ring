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

#include "../Backend.h"
#include <list>

class GLRenderDevice;

class GLTexture : public Texture
{
public:
	GLTexture();
	~GLTexture();

	void Finalize();

	void Set2DImage(int width, int height, PixelFormat format) override;
	void SetCubeImage(int size, PixelFormat format) override;

	bool SetPixels(GLRenderDevice* device, const void* data);
	bool SetCubePixels(GLRenderDevice* device, CubeMapFace face, const void* data);

	bool IsCubeTexture() const { return mCubeTexture; }
	int GetWidth() const { return mWidth; }
	int GetHeight() const { return mHeight; }

	bool IsTextureCreated() const { return mTexture; }
	void Invalidate();

	GLuint GetTexture(GLRenderDevice* device);
	GLuint GetFramebuffer(GLRenderDevice* device, bool usedepthbuffer);
	GLuint GetPBO(GLRenderDevice* device);

	GLRenderDevice* Device = nullptr;
	std::list<GLTexture*>::iterator ItTexture;

private:
	static GLint ToInternalFormat(PixelFormat format);
	static GLenum ToDataFormat(PixelFormat format);
	static GLenum ToDataType(PixelFormat format);

	int mWidth = 0;
	int mHeight = 0;
	PixelFormat mFormat = {};
	bool mCubeTexture = false;
	bool mPBOTexture = false;
	GLuint mTexture = 0;
	GLuint mFramebuffer = 0;
	GLuint mDepthRenderbuffer = 0;
	GLuint mPBO = 0;
};
