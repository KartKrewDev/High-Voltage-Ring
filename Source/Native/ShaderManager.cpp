
#include "Precomp.h"
#include "ShaderManager.h"

void ShaderManager::DeclareShader(int i, const char* vs, const char* ps)
{
	if (Shaders.size() <= (size_t)i)
	{
		Shaders.resize((size_t)i + 1);
		AlphaTestShaders.resize((size_t)i + 1);
	}

	Shaders[i].Setup(vs, ps, false);
	AlphaTestShaders[i].Setup(vs, ps, true);
}

void ShaderManager::ReleaseResources()
{
	for (size_t i = 0; i < Shaders.size(); i++)
	{
		Shaders[i].ReleaseResources();
		AlphaTestShaders[i].ReleaseResources();
	}
}
