#pragma once

#include "GLShader.h"

class GLShaderManager
{
public:
	void ReleaseResources();

	void DeclareShader(int index, const char* name, const char* vs, const char* ps);

	std::vector<GLShader> AlphaTestShaders;
	std::vector<GLShader> Shaders;
};
