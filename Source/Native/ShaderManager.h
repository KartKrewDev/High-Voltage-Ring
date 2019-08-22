#pragma once

#include "Shader.h"

class ShaderManager
{
public:
	ShaderManager();
	void ReleaseResources();

	Shader AlphaTestShaders[(int)ShaderName::count];
	Shader Shaders[(int)ShaderName::count];
};
