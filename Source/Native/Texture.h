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

	void* Lock();
	void Unlock();

	bool IsCubeTexture() const { return mCubeTexture; }

	GLuint GetTexture();

private:
	int mWidth = 0;
	int mHeight = 0;
	bool mCubeTexture = false;
	std::map<int, std::vector<uint32_t>> mPixels;
	GLuint mTexture = 0;
};
