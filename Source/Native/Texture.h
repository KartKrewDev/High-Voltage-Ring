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

	void Set2DImage(int width, int height);
	void SetCubeImage(int size);

	void SetPixels(const void* data);
	void SetCubePixels(CubeMapFace face, const void* data);

	void* Lock();
	void Unlock();

private:
	int mWidth = 0;
	int mHeight = 0;
	bool mCubeTexture = false;
	std::map<int, std::vector<uint32_t>> mPixels;
};
