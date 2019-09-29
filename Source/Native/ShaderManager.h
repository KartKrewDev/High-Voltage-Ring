#pragma once

#include "Shader.h"

class ShaderManager
{
public:
	void ReleaseResources();

	void DeclareShader(int index, const char* vs, const char* ps);

	std::vector<Shader> AlphaTestShaders;
	std::vector<Shader> Shaders;
};
