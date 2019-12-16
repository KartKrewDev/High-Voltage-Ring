#pragma once

enum class CubeMapFace : int
{
	PositiveX,
	PositiveY,
	PositiveZ,
	NegativeX,
	NegativeY,
	NegativeZ
};

class Texture
{
public:
	Texture();
	~Texture();

	void Set2DImage(int width, int height);
	void SetCubeImage(int size);

	void SetPixels(const void* data);
	void SetCubePixels(CubeMapFace face, const void* data);

	bool IsCubeTexture() const { return mCubeTexture; }
	int GetWidth() const { return mWidth; }
	int GetHeight() const { return mHeight; }

	bool IsTextureCreated() const { return mTexture; }
	void Invalidate();

	GLuint GetTexture();
	GLuint GetFramebuffer(bool usedepthbuffer);
	GLuint GetPBO();

private:
	int mWidth = 0;
	int mHeight = 0;
	bool mCubeTexture = false;
	bool mPBOTexture = false;
	std::map<int, std::vector<uint32_t>> mPixels;
	GLuint mTexture = 0;
	GLuint mFramebuffer = 0;
	GLuint mDepthRenderbuffer = 0;
	//
	GLuint mPBO = 0;
};
