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
#include "GLTexture.h"
#include "GLRenderDevice.h"
#include <stdexcept>

GLTexture::GLTexture()
{
}

GLTexture::~GLTexture()
{
	if (Device)
		Invalidate();
}

void GLTexture::Set2DImage(int width, int height)
{
	if (width < 1) width = 1;
	if (height < 1) height = 1;
	mCubeTexture = false;
	mWidth = width;
	mHeight = height;
}

void GLTexture::SetCubeImage(int size)
{
	mCubeTexture = true;
	mWidth = size;
	mHeight = size;
}

void GLTexture::SetPixels(const void* data)
{
	mPixels[0].resize(mWidth * (size_t)mHeight);
	memcpy(mPixels[0].data(), data, sizeof(uint32_t) * mWidth * mHeight);
}

void GLTexture::SetCubePixels(CubeMapFace face, const void* data)
{
	mPixels[(int)face].resize(mWidth * (size_t)mHeight);
	memcpy(mPixels[(int)face].data(), data, sizeof(uint32_t) * mWidth * mHeight);
}

void GLTexture::Invalidate()
{
	if (mDepthRenderbuffer) glDeleteRenderbuffers(1, &mDepthRenderbuffer);
	if (mFramebuffer) glDeleteFramebuffers(1, &mFramebuffer);
	if (mTexture) glDeleteTextures(1, &mTexture);
	if (mPBO) glDeleteBuffers(1, &mPBO);
	mDepthRenderbuffer = 0;
	mFramebuffer = 0;
	mTexture = 0;
	mPBO = 0;
	Device = nullptr;
}

GLuint GLTexture::GetTexture(GLRenderDevice* device)
{
	if (mTexture == 0)
	{
		Device = device;

		GLint oldActiveTex = GL_TEXTURE0;
		glGetIntegerv(GL_ACTIVE_TEXTURE, &oldActiveTex);
		glActiveTexture(GL_TEXTURE0);

		glGenTextures(1, &mTexture);

		if (!IsCubeTexture())
		{
			GLint oldBinding = 0;
			glGetIntegerv(GL_TEXTURE_BINDING_2D, &oldBinding);

			glBindBuffer(GL_PIXEL_UNPACK_BUFFER, 0);
			glBindTexture(GL_TEXTURE_2D, mTexture);
			glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA8, mWidth, mHeight, 0, GL_BGRA, GL_UNSIGNED_BYTE, !mPixels[0].empty() ? mPixels[0].data() : nullptr);
			if (!mPixels[0].empty())
				glGenerateMipmap(GL_TEXTURE_2D);

			glBindTexture(GL_TEXTURE_2D, oldBinding);
		}
		else
		{
			GLint oldBinding = 0;
			glGetIntegerv(GL_TEXTURE_BINDING_CUBE_MAP, &oldBinding);

			glBindBuffer(GL_PIXEL_UNPACK_BUFFER, 0);
			glBindTexture(GL_TEXTURE_CUBE_MAP, mTexture);
			glTexImage2D(GL_TEXTURE_CUBE_MAP_POSITIVE_X, 0, GL_RGBA8, mWidth, mHeight, 0, GL_BGRA, GL_UNSIGNED_BYTE, !mPixels[0].empty() ? mPixels[0].data() : nullptr);
			glTexImage2D(GL_TEXTURE_CUBE_MAP_POSITIVE_Y, 0, GL_RGBA8, mWidth, mHeight, 0, GL_BGRA, GL_UNSIGNED_BYTE, !mPixels[1].empty() ? mPixels[1].data() : nullptr);
			glTexImage2D(GL_TEXTURE_CUBE_MAP_POSITIVE_Z, 0, GL_RGBA8, mWidth, mHeight, 0, GL_BGRA, GL_UNSIGNED_BYTE, !mPixels[2].empty() ? mPixels[2].data() : nullptr);
			glTexImage2D(GL_TEXTURE_CUBE_MAP_NEGATIVE_X, 0, GL_RGBA8, mWidth, mHeight, 0, GL_BGRA, GL_UNSIGNED_BYTE, !mPixels[3].empty() ? mPixels[3].data() : nullptr);
			glTexImage2D(GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, 0, GL_RGBA8, mWidth, mHeight, 0, GL_BGRA, GL_UNSIGNED_BYTE, !mPixels[4].empty() ? mPixels[4].data() : nullptr);
			glTexImage2D(GL_TEXTURE_CUBE_MAP_NEGATIVE_Z, 0, GL_RGBA8, mWidth, mHeight, 0, GL_BGRA, GL_UNSIGNED_BYTE, !mPixels[5].empty() ? mPixels[5].data() : nullptr);
			if (!mPixels[0].empty())
				glGenerateMipmap(GL_TEXTURE_CUBE_MAP);

			glBindTexture(GL_TEXTURE_CUBE_MAP, oldBinding);
		}

		glActiveTexture(oldActiveTex);
	}
	return mTexture;
}

GLuint GLTexture::GetFramebuffer(GLRenderDevice* device, bool usedepthbuffer)
{
	if (!usedepthbuffer)
	{
		if (mFramebuffer == 0)
		{
			GLuint texture = GetTexture(Device);
			glGenFramebuffers(1, &mFramebuffer);
			glBindFramebuffer(GL_FRAMEBUFFER, mFramebuffer);
			glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, texture, 0);
			if (glCheckFramebufferStatus(GL_FRAMEBUFFER) != GL_FRAMEBUFFER_COMPLETE)
				throw std::runtime_error("glCheckFramebufferStatus did not return GL_FRAMEBUFFER_COMPLETE");
		}
		return mFramebuffer;
	}
	else
	{
		if (mDepthRenderbuffer == 0)
		{
			Device = device;

			glGenRenderbuffers(1, &mDepthRenderbuffer);
			glBindRenderbuffer(GL_RENDERBUFFER, mDepthRenderbuffer);
			glRenderbufferStorage(GL_RENDERBUFFER, GL_DEPTH24_STENCIL8, mWidth, mHeight);
			glBindRenderbuffer(GL_RENDERBUFFER, 0);
		}

		if (mFramebuffer == 0)
		{
			GLuint texture = GetTexture(Device);
			glGenFramebuffers(1, &mFramebuffer);
			glBindFramebuffer(GL_FRAMEBUFFER, mFramebuffer);
			glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, texture, 0);
			glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_STENCIL_ATTACHMENT, GL_RENDERBUFFER, mDepthRenderbuffer);
			if (glCheckFramebufferStatus(GL_FRAMEBUFFER) != GL_FRAMEBUFFER_COMPLETE)
				throw std::runtime_error("glCheckFramebufferStatus did not return GL_FRAMEBUFFER_COMPLETE");
			glBindFramebuffer(GL_FRAMEBUFFER, 0);
		}
		return mFramebuffer;
	}
}

GLuint GLTexture::GetPBO(GLRenderDevice* device)
{
	if (mPBO == 0)
	{
		Device = device;

		glGenBuffers(1, &mPBO);
		glBindBuffer(GL_PIXEL_UNPACK_BUFFER, mPBO);
		glBufferData(GL_PIXEL_UNPACK_BUFFER, mWidth*mHeight * 4, NULL, GL_STREAM_DRAW);
	}

	return mPBO;
}
