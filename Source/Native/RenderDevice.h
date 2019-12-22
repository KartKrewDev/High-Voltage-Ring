/*
**  BuilderNative Renderer
**  Copyright (c) 2019 Magnus Norddahl
**
**  This software is provided 'as-is', without any express or implied
**  warranty.  In no event will the authors be held liable for any damages
**  arising from the use of this software.
**
**  Permission is granted to anyone to use this software for any purpose,
**  including commercial applications, and to alter it and redistribute it
**  freely, subject to the following restrictions:
**
**  1. The origin of this software must not be misrepresented; you must not
**     claim that you wrote the original software. If you use this software
**     in a product, an acknowledgment in the product documentation would be
**     appreciated but is not required.
**  2. Altered source versions must be plainly marked as such, and must not be
**     misrepresented as being the original software.
**  3. This notice may not be removed or altered from any source distribution.
*/

#pragma once

#include "OpenGLContext.h"
#include <string>
#include <mutex>

class SharedVertexBuffer;
class VertexBuffer;
class IndexBuffer;
class Texture;
class ShaderManager;
class Shader;
enum class CubeMapFace;
enum class VertexFormat;

enum class Cull : int { None, Clockwise };
enum class Blend : int { InverseSourceAlpha, SourceAlpha, One };
enum class BlendOperation : int { Add, ReverseSubtract };
enum class FillMode : int { Solid, Wireframe };
enum class TextureAddress : int { Wrap, Clamp };
enum class ShaderFlags : int { None, Debug };
enum class PrimitiveType : int { LineList, TriangleList, TriangleStrip };
enum class TextureFilter : int { None, Point, Linear, Anisotropic };

typedef int UniformName;
typedef int ShaderName;

enum class UniformType : int
{
	Vec4f,
	Vec3f,
	Vec2f,
	Float,
	Mat4
};

class RenderDevice
{
public:
	RenderDevice(void* disp, void* window);
	~RenderDevice();

	void DeclareUniform(UniformName name, const char* glslname, UniformType type);
	void DeclareShader(ShaderName index, const char* name, const char* vertexshader, const char* fragmentshader);
	void SetShader(ShaderName name);
	void SetUniform(UniformName name, const void* values, int count);
	void SetVertexBuffer(VertexBuffer* buffer);
	void SetIndexBuffer(IndexBuffer* buffer);
	void SetAlphaBlendEnable(bool value);
	void SetAlphaTestEnable(bool value);
	void SetCullMode(Cull mode);
	void SetBlendOperation(BlendOperation op);
	void SetSourceBlend(Blend blend);
	void SetDestinationBlend(Blend blend);
	void SetFillMode(FillMode mode);
	void SetMultisampleAntialias(bool value);
	void SetZEnable(bool value);
	void SetZWriteEnable(bool value);
	void SetTexture(Texture* texture);
	void SetSamplerFilter(TextureFilter minfilter, TextureFilter magfilter, TextureFilter mipfilter, float maxanisotropy);
	void SetSamplerState(TextureAddress address);
	bool Draw(PrimitiveType type, int startIndex, int primitiveCount);
	bool DrawIndexed(PrimitiveType type, int startIndex, int primitiveCount);
	bool DrawData(PrimitiveType type, int startIndex, int primitiveCount, const void* data);
	bool StartRendering(bool clear, int backcolor, Texture* target, bool usedepthbuffer);
	bool FinishRendering();
	bool Present();
	bool ClearTexture(int backcolor, Texture* texture);
	bool CopyTexture(Texture* dst, CubeMapFace face);

	bool SetVertexBufferData(VertexBuffer* buffer, void* data, int64_t size, VertexFormat format);
	bool SetVertexBufferSubdata(VertexBuffer* buffer, int64_t destOffset, void* data, int64_t size);
	bool SetIndexBufferData(IndexBuffer* buffer, void* data, int64_t size);

	bool SetPixels(Texture* texture, const void* data);
	bool SetCubePixels(Texture* texture, CubeMapFace face, const void* data);
	void* MapPBO(Texture* texture);
	bool UnmapPBO(Texture* texture);

	bool InvalidateTexture(Texture* texture);

	void GarbageCollectBuffer(int size, VertexFormat format);

	bool ApplyViewport();
	bool ApplyChanges();
	bool ApplyVertexBuffer();
	bool ApplyIndexBuffer();
	bool ApplyShader();
	bool ApplyUniforms();
	bool ApplyTextures();
	bool ApplyRasterizerState();
	bool ApplyBlendState();
	bool ApplyDepthState();

	bool CheckGLError();
	void SetError(const char* fmt, ...);
	const char* GetError();

	Shader* GetActiveShader();

	GLint GetGLMinFilter(TextureFilter filter, TextureFilter mipfilter);

	static std::mutex& GetMutex();
	static void DeleteObject(VertexBuffer* buffer);
	static void DeleteObject(IndexBuffer* buffer);
	static void DeleteObject(Texture* texture);

	void ProcessDeleteList();

	std::unique_ptr<IOpenGLContext> Context;

	struct DeleteList
	{
		std::vector<VertexBuffer*> VertexBuffers;
		std::vector<IndexBuffer*> IndexBuffers;
		std::vector<Texture*> Textures;
	} mDeleteList;

	struct TextureUnit
	{
		Texture* Tex = nullptr;
		TextureAddress WrapMode = TextureAddress::Wrap;
		GLuint SamplerHandle = 0;
	} mTextureUnit;

	struct SamplerFilterKey
	{
		GLuint MinFilter = 0;
		GLuint MagFilter = 0;
		float MaxAnisotropy = 0.0f;

		bool operator<(const SamplerFilterKey& b) const { return memcmp(this, &b, sizeof(SamplerFilterKey)) < 0; }
		bool operator==(const SamplerFilterKey& b) const { return memcmp(this, &b, sizeof(SamplerFilterKey)) == 0; }
		bool operator!=(const SamplerFilterKey& b) const { return memcmp(this, &b, sizeof(SamplerFilterKey)) != 0; }
	};

	struct SamplerFilter
	{
		GLuint WrapModes[2] = { 0, 0 };
	};

	std::map<SamplerFilterKey, SamplerFilter> mSamplers;
	SamplerFilterKey mSamplerFilterKey;
	SamplerFilter* mSamplerFilter = nullptr;

	int mVertexBuffer = -1;
	int64_t mVertexBufferStartIndex = 0;

	IndexBuffer* mIndexBuffer = nullptr;

	std::unique_ptr<SharedVertexBuffer> mSharedVertexBuffers[2];

	std::unique_ptr<ShaderManager> mShaderManager;
	ShaderName mShaderName = {};

	struct UniformInfo
	{
		std::string Name;
		UniformType Type = {};
		int Offset = 0;
		int LastUpdate = 0;
	};

	std::vector<UniformInfo> mUniformInfo;
	std::vector<float> mUniformData;

	GLuint mStreamVertexBuffer = 0;
	GLuint mStreamVAO = 0;

	Cull mCullMode = Cull::None;
	FillMode mFillMode = FillMode::Solid;
	bool mAlphaTest = false;

	bool mAlphaBlend = false;
	BlendOperation mBlendOperation = BlendOperation::Add;
	Blend mSourceBlend = Blend::SourceAlpha;
	Blend mDestinationBlend = Blend::InverseSourceAlpha;

	bool mDepthTest = false;
	bool mDepthWrite = false;

	bool mNeedApply = true;
	bool mShaderChanged = true;
	bool mUniformsChanged = true;
	bool mTexturesChanged = true;
	bool mIndexBufferChanged = true;
	bool mVertexBufferChanged = true;
	bool mDepthStateChanged = true;
	bool mBlendStateChanged = true;
	bool mRasterizerStateChanged = true;

	bool mContextIsCurrent = false;

	std::string mLastError;
	std::string mReturnError;
	char mSetErrorBuffer[4096];

	int mViewportWidth = 0;
	int mViewportHeight = 0;
};
