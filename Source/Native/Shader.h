#pragma once

#include <string>
#include "RenderDevice.h"

enum class DeclarationUsage : int32_t { Position, Color, TextureCoordinate, Normal };

class Shader
{
public:
	Shader() = default;
	void ReleaseResources();

	bool Compile(const std::string& vertexShader, const std::string& fragmentShader, bool alphatest);
	const char* GetErrors() const { return mErrors.c_str(); }

	GLuint GetProgram() const { return mProgram; }

	GLuint UniformLocations[(int)UniformName::NumUniforms] = { 0 };

	enum { MaxSamplers = 4 };
	GLuint SamplerLocations[MaxSamplers] = { };

private:
	GLuint CompileShader(const std::string& code, GLenum type);

	GLuint mProgram = 0;
	GLuint mVertexShader = 0;
	GLuint mFragmentShader = 0;
	std::string mErrors;

	Shader(const Shader&) = delete;
	Shader& operator=(const Shader&) = delete;
};