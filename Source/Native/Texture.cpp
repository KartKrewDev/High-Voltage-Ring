
#include "Precomp.h"
#include "Texture.h"

Texture::Texture()
{
}

Texture::~Texture()
{
	// To do: move mTexture to a delete list as this might be called by a finalizer in a different thread
}

void Texture::Set2DImage(int width, int height)
{
	mCubeTexture = false;
	mWidth = width;
	mHeight = height;
}

void Texture::SetCubeImage(int size)
{
	mCubeTexture = true;
	mWidth = size;
	mHeight = size;
}

void Texture::SetPixels(const void* data)
{
	mPixels[0].resize(mWidth * (size_t)mHeight);
	memcpy(mPixels[0].data(), data, sizeof(uint32_t) * mWidth * mHeight);
}

void Texture::SetCubePixels(CubeMapFace face, const void* data)
{
	mPixels[(int)face].resize(mWidth * (size_t)mHeight);
	memcpy(mPixels[(int)face].data(), data, sizeof(uint32_t) * mWidth * mHeight);
}

void* Texture::Lock()
{
	mPixels[0].resize(mWidth * (size_t)mHeight);
	return mPixels[0].data();
}

void Texture::Unlock()
{
}

GLuint Texture::GetTexture()
{
	if (mTexture == 0)
	{
		if (!IsCubeTexture())
		{
			GLint oldBinding = 0;
			glActiveTexture(GL_TEXTURE0);
			glGetIntegerv(GL_TEXTURE_BINDING_2D, &oldBinding);

			glGenTextures(1, &mTexture);
			glBindTexture(GL_TEXTURE_2D, mTexture);
			glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA8, mWidth, mHeight, 0, GL_RGBA, GL_UNSIGNED_BYTE, !mPixels[0].empty() ? mPixels[0].data() : nullptr);
			glGenerateMipmap(GL_TEXTURE_2D);

			glActiveTexture(GL_TEXTURE0);
			glBindTexture(GL_TEXTURE_2D, oldBinding);
		}
		else
		{
			GLint oldBinding = 0;
			glActiveTexture(GL_TEXTURE0);
			glGetIntegerv(GL_TEXTURE_BINDING_CUBE_MAP, &oldBinding);

			glGenTextures(1, &mTexture);
			glBindTexture(GL_TEXTURE_CUBE_MAP, mTexture);
			glTexImage2D(GL_TEXTURE_CUBE_MAP_POSITIVE_X, 0, GL_RGBA8, mWidth, mHeight, 0, GL_BGRA, GL_UNSIGNED_BYTE, !mPixels[0].empty() ? mPixels[0].data() : nullptr);
			glTexImage2D(GL_TEXTURE_CUBE_MAP_POSITIVE_Y, 0, GL_RGBA8, mWidth, mHeight, 0, GL_BGRA, GL_UNSIGNED_BYTE, !mPixels[1].empty() ? mPixels[1].data() : nullptr);
			glTexImage2D(GL_TEXTURE_CUBE_MAP_POSITIVE_Z, 0, GL_RGBA8, mWidth, mHeight, 0, GL_BGRA, GL_UNSIGNED_BYTE, !mPixels[2].empty() ? mPixels[2].data() : nullptr);
			glTexImage2D(GL_TEXTURE_CUBE_MAP_NEGATIVE_X, 0, GL_RGBA8, mWidth, mHeight, 0, GL_BGRA, GL_UNSIGNED_BYTE, !mPixels[3].empty() ? mPixels[3].data() : nullptr);
			glTexImage2D(GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, 0, GL_RGBA8, mWidth, mHeight, 0, GL_BGRA, GL_UNSIGNED_BYTE, !mPixels[4].empty() ? mPixels[4].data() : nullptr);
			glTexImage2D(GL_TEXTURE_CUBE_MAP_NEGATIVE_Z, 0, GL_RGBA8, mWidth, mHeight, 0, GL_BGRA, GL_UNSIGNED_BYTE, !mPixels[5].empty() ? mPixels[5].data() : nullptr);
			glGenerateMipmap(GL_TEXTURE_CUBE_MAP);

			glActiveTexture(GL_TEXTURE0);
			glBindTexture(GL_TEXTURE_CUBE_MAP, oldBinding);
		}
	}
	return mTexture;
}

/////////////////////////////////////////////////////////////////////////////

Texture* Texture_New()
{
	return new Texture();
}

void Texture_Delete(Texture* tex)
{
	delete tex;
}

void Texture_Set2DImage(Texture* handle, int width, int height)
{
	handle->Set2DImage(width, height);
}

void Texture_SetPixels(Texture* handle, const void* data)
{
	handle->SetPixels(data);
}

void* Texture_Lock(Texture* handle)
{
	return handle->Lock();
}

void Texture_Unlock(Texture* handle)
{
	handle->Unlock();
}

void Texture_SetCubeImage(Texture* handle, int size)
{
	handle->SetCubeImage(size);
}

void Texture_SetCubePixels(Texture* handle, CubeMapFace face, const void *data)
{
	handle->SetCubePixels(face, data);
}
