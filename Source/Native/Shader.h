#pragma once

#include <string>
#include "RenderDevice.h"

enum class DeclarationUsage : int32_t { Position, Color, TextureCoordinate, Normal };

class Shader
{
public:
	Shader() = default;
	void ReleaseResources();

	void Setup(const std::string& vertexShader, const std::string& fragmentShader, bool alphatest);
	void Bind();

	GLuint UniformLocations[(int)UniformName::NumUniforms] = { 0 };

private:
	void CreateProgram();
	GLuint CompileShader(const std::string& code, GLenum type);

	std::string mVertexText;
	std::string mFragmentText;
	bool mAlphatest = false;
	bool mProgramBuilt = false;

	GLuint mProgram = 0;
	GLuint mVertexShader = 0;
	GLuint mFragmentShader = 0;
	std::string mErrors;

	Shader(const Shader&) = delete;
	Shader& operator=(const Shader&) = delete;
};