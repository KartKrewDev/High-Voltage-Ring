#pragma once

#include <string>
#include "RenderDevice.h"

enum class DeclarationUsage : int32_t { Position, Color, TextureCoordinate, Normal };

class Shader
{
public:
	void ReleaseResources();

	void Setup(const std::string& identifier, const std::string& vertexShader, const std::string& fragmentShader, bool alphatest);
	bool CheckCompile(RenderDevice *device);
	void Bind();

	std::string GetCompileError();

	std::vector<int> UniformLastUpdates;
	std::vector<GLuint> UniformLocations;

private:
	void CreateProgram(RenderDevice* device);
	GLuint CompileShader(const std::string& code, GLenum type);

	std::string mIdentifier;
	std::string mVertexText;
	std::string mFragmentText;
	bool mAlphatest = false;
	bool mProgramBuilt = false;

	GLuint mProgram = 0;
	GLuint mVertexShader = 0;
	GLuint mFragmentShader = 0;
	std::string mErrors;
};
