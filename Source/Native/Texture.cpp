
#include "Texture.h"

Texture::Texture()
{
}

Texture* Texture_New()
{
	return new Texture();
}

void Texture_Delete(Texture* tex)
{
	delete tex;
}
