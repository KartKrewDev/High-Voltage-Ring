
#include "Texture.h"

Texture::Texture()
{
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
