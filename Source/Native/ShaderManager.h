#pragma once

#include "Shader.h"

class ShaderManager
{
public:
	ShaderManager();
	void ReleaseResources();

	Shader Shaders[(int)ShaderName::count];

	std::string CompileErrors;
};
