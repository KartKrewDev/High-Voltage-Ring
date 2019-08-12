#pragma once

#include <string>
#include "RenderDevice.h"

class Shader
{
public:
	Shader();
	~Shader();

	bool Compile(const std::string& vertexShader, const std::string& fragmentShader);
	const char* GetErrors() const { return mErrors.c_str(); }

	GLuint GetProgram() const { return mProgram; }

	GLuint TransformLocations[(int)TransformState::NumTransforms] = { 0 };

private:
	GLuint CompileShader(const std::string& code, GLenum type);

	GLuint mProgram = 0;
	GLuint mVertexShader = 0;
	GLuint mFragmentShader = 0;
	std::string mErrors;

	Shader(const Shader&) = delete;
	Shader& operator=(const Shader&) = delete;
};