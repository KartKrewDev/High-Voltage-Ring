#pragma once

#include "OpenGLContext.h"
#include <string>

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

enum class ShaderName
{
	display2d_fsaa,
	display2d_normal,
	display2d_fullbright,
	things2d_thing,
	things2d_sprite,
	things2d_fill,
	plotter,
	world3d_main,
	world3d_fullbright,
	world3d_main_highlight,
	world3d_fullbright_highlight,
	world3d_main_vertexcolor,
	world3d_skybox,
	world3d_main_highlight_vertexcolor,
	world3d_p7,
	world3d_main_fog,
	world3d_p9,
	world3d_main_highlight_fog,
	world3d_p11,
	world3d_main_fog_vertexcolor,
	world3d_p13,
	world3d_main_highlight_fog_vertexcolor,
	world3d_vertex_color,
	world3d_constant_color,
	world3d_lightpass,
	count
};

enum class UniformName : int
{
	rendersettings,
	projection,
	desaturation,
	highlightcolor,
	view,
	world,
	modelnormal,
	FillColor,
	vertexColor,
	stencilColor,
	lightPosAndRadius,
	lightOrientation,
	light2Radius,
	lightColor,
	ignoreNormals,
	spotLight,
	campos,
	texturefactor,
	fogsettings,
	fogcolor,
	NumUniforms
};

class RenderDevice
{
public:
	RenderDevice(void* disp, void* window);
	~RenderDevice();

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

	std::unique_ptr<IOpenGLContext> Context;

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
	ShaderName mShaderName = ShaderName::display2d_normal;

	enum class UniformType { Matrix, Vec4f, Vec3f, Vec2f, Float };

	struct UniformInfo
	{
		std::string Name;
		UniformType Type = {};
		int Offset = 0;
		int LastUpdate = 0;
	};

	UniformInfo mUniformInfo[(int)UniformName::NumUniforms];
	std::vector<float> mUniformData;

	void DeclareUniform(UniformName name, const char* glslname, UniformType type);

	union UniformEntry
	{
		float valuef;
		int32_t valuei;
	};

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
