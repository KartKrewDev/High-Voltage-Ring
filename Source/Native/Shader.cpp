
#include "Precomp.h"
#include "Shader.h"
#include "RenderDevice.h"
#include <stdexcept>

void Shader::Setup(const std::string& identifier, const std::string& vertexShader, const std::string& fragmentShader, bool alphatest)
{
	mIdentifier = identifier;
	mVertexText = vertexShader;
	mFragmentText = fragmentShader;
	mAlphatest = alphatest;
}

bool Shader::CheckCompile()
{
	bool firstCall = !mProgramBuilt;
	if (firstCall)
	{
		mProgramBuilt = true;
		CreateProgram();
		glUseProgram(mProgram);
		glUniform1i(glGetUniformLocation(mProgram, "texture1"), 0);
		glUseProgram(0);
	}

	return !mErrors.size();
}

std::string Shader::GetCompileError()
{
	std::string lines = "Error compiling ";
	if (!mVertexShader)
		lines += "vertex ";
	else if (!mFragmentShader)
		lines += "fragment ";
	lines += "shader \"" + mIdentifier + "\":\r\n";
	for (auto c : mErrors)
	{
		if (c == '\r')
			continue;
		if (c == '\n')
			lines += "\r\n";
		else lines += c;
	}
	return lines;
}

void Shader::Bind()
{
	if (!mProgram || !mProgramBuilt || mErrors.size())
		return;

	glUseProgram(mProgram);
}

void Shader::CreateProgram()
{
	const char* prefixNAT = R"(
		#version 150
		#line 1
	)";
	const char* prefixAT = R"(
		#version 150
		#define ALPHA_TEST
		#line 1
	)";

	const char* prefix = mAlphatest ? prefixAT : prefixNAT;

	mVertexShader = CompileShader(prefix + mVertexText, GL_VERTEX_SHADER);
	if (!mVertexShader)
		return;

	mFragmentShader = CompileShader(prefix + mFragmentText, GL_FRAGMENT_SHADER);
	if (!mFragmentShader)
		return;

	mProgram = glCreateProgram();
	glAttachShader(mProgram, mVertexShader);
	glAttachShader(mProgram, mFragmentShader);
	glBindAttribLocation(mProgram, (GLuint)DeclarationUsage::Position, "AttrPosition");
	glBindAttribLocation(mProgram, (GLuint)DeclarationUsage::Color, "AttrColor");
	glBindAttribLocation(mProgram, (GLuint)DeclarationUsage::TextureCoordinate, "AttrUV");
	glBindAttribLocation(mProgram, (GLuint)DeclarationUsage::Normal, "AttrNormal");
	glLinkProgram(mProgram);

	GLint status = 0;
	glGetProgramiv(mProgram, GL_LINK_STATUS, &status);
	if (status != GL_TRUE)
	{
		GLsizei length = 0;
		glGetProgramiv(mProgram, GL_INFO_LOG_LENGTH, &length);
		std::vector<GLchar> errors(length + (size_t)1);
		glGetProgramInfoLog(mProgram, (GLsizei)errors.size(), &length, errors.data());
		mErrors = { errors.begin(), errors.begin() + length };

		glDeleteProgram(mProgram);
		glDeleteShader(mVertexShader);
		glDeleteShader(mFragmentShader);
		mProgram = 0;
		mVertexShader = 0;
		mFragmentShader = 0;
		return;
	}

	static const char* names[(int)UniformName::NumUniforms] = {
		"rendersettings",
		"projection",
		"desaturation",
		"highlightcolor",
		"view",
		"world",
		"modelnormal",
		"fillColor",
		"vertexColor",
		"stencilColor",
		"lightPosAndRadius",
		"lightOrientation",
		"light2Radius",
		"lightColor",
		"ignoreNormals",
		"spotLight",
		"campos",
		"texturefactor",
		"fogsettings",
		"fogcolor"
	};

	for (int i = 0; i < (int)UniformName::NumUniforms; i++)
	{
		UniformLocations[i] = glGetUniformLocation(mProgram, names[i]);
	}
}

GLuint Shader::CompileShader(const std::string& code, GLenum type)
{
	GLuint shader = glCreateShader(type);
	const GLchar* sources[] = { (GLchar*)code.data() };
	const GLint lengths[] = { (GLint)code.size() };
	glShaderSource(shader, 1, sources, lengths);
	glCompileShader(shader);

	GLint status = 0;
	glGetShaderiv(shader, GL_COMPILE_STATUS, &status);
	if (status != GL_TRUE)
	{
		GLsizei length = 0;
		glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &length);
		std::vector<GLchar> errors(length + (size_t)1);
		glGetShaderInfoLog(shader, (GLsizei)errors.size(), &length, errors.data());
		mErrors = { errors.begin(), errors.begin() + length };
		glDeleteShader(shader);
		return 0;
	}
	return shader;
}

void Shader::ReleaseResources()
{
	if (mProgram)
		glDeleteProgram(mProgram);
	if (mVertexShader)
		glDeleteShader(mVertexShader);
	if (mFragmentShader)
		glDeleteShader(mFragmentShader);
	mProgram = 0;
	mVertexShader = 0;
	mFragmentShader = 0;
}
