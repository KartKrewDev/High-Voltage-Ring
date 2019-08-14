#pragma once

#include "OpenGLContext.h"

class VertexBuffer;
class IndexBuffer;
class VertexDeclaration;
class Texture;
class ShaderManager;
class Shader;
enum class CubeMapFace;

enum class Cull : int { None, Counterclockwise };
enum class Blend : int { InverseSourceAlpha, SourceAlpha, One, BlendFactor };
enum class BlendOperation : int { Add, ReverseSubtract };
enum class FillMode : int { Solid, Wireframe };
enum class TransformState : int { World, View, Projection, NumTransforms };
enum class TextureAddress : int { Wrap, Clamp };
enum class ShaderFlags : int { None, Debug };
enum class PrimitiveType : int { LineList, TriangleList, TriangleStrip };
enum class TextureFilter : int { None, Point, Linear, Anisotropic };

enum class ShaderName
{
	basic,
	display2d_fsaa,
	display2d_normal,
	display2d_fullbright,
	things2d_thing,
	things2d_sprite,
	things2d_fill,
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
	world3d_lightpass, // AlphaBlendEnable = true
	count
};

enum class UniformName : int
{
	rendersettings,
	transformsettings,
	desaturation,
	texture1,
	highlightcolor,
	worldviewproj,
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
	NumUniforms
};

class RenderDevice
{
public:
	RenderDevice(HWND hwnd);
	~RenderDevice();

	void SetShader(ShaderName name);
	void SetUniform(UniformName name, const void* values, int count);
	void SetVertexBuffer(int index, VertexBuffer* buffer, long offset, long stride);
	void SetIndexBuffer(IndexBuffer* buffer);
	void SetAlphaBlendEnable(bool value);
	void SetAlphaTestEnable(bool value);
	void SetCullMode(Cull mode);
	void SetBlendOperation(BlendOperation op);
	void SetSourceBlend(Blend blend);
	void SetDestinationBlend(Blend blend);
	void SetFillMode(FillMode mode);
	void SetFogEnable(bool value);
	void SetFogColor(int value);
	void SetFogStart(float value);
	void SetFogEnd(float value);
	void SetMultisampleAntialias(bool value);
	void SetTextureFactor(int factor);
	void SetZEnable(bool value);
	void SetZWriteEnable(bool value);
	void SetTransform(TransformState state, float* matrix);
	void SetSamplerState(int unit, TextureAddress addressU, TextureAddress addressV, TextureAddress addressW);
	void DrawPrimitives(PrimitiveType type, int startIndex, int primitiveCount);
	void DrawUserPrimitives(PrimitiveType type, int startIndex, int primitiveCount, const void* data);
	void SetVertexDeclaration(VertexDeclaration* decl);
	void StartRendering(bool clear, int backcolor, Texture* target, bool usedepthbuffer);
	void FinishRendering();
	void Present();
	void ClearTexture(int backcolor, Texture* texture);
	void CopyTexture(Texture* src, Texture* dst, CubeMapFace face);

	void ApplyChanges();
	void ApplyVertexBuffers();
	void ApplyIndexBuffer();
	void ApplyShader();
	void ApplyMatrices();
	void ApplyUniforms();
	void ApplyTextures();
	void ApplyRasterizerState();
	void ApplyBlendState();
	void ApplyDepthState();
	void ApplyRenderTarget(Texture* target, bool usedepthbuffer);

	void CheckError();

	OpenGLContext Context;

	struct VertexBinding
	{
		VertexBinding() = default;
		VertexBinding(VertexBuffer* buffer, long offset, long stride) : Buffer(buffer), Offset(offset), Stride(stride) { }

		VertexBuffer* Buffer = nullptr;
		long Offset = 0;
		long Stride = 0;
	};

	struct SamplerState
	{
		SamplerState() = default;
		SamplerState(TextureAddress addressU, TextureAddress addressV, TextureAddress addressW) : AddressU(addressU), AddressV(addressV), AddressW(addressW) { }

		TextureAddress AddressU = TextureAddress::Wrap;
		TextureAddress AddressV = TextureAddress::Wrap;
		TextureAddress AddressW = TextureAddress::Wrap;
	};

	enum { NumSlots = 16 };

	VertexDeclaration *mVertexDeclaration = nullptr;
	GLuint mVAO = 0;
	int mEnabledVertexAttributes[NumSlots] = { 0 };
	VertexBinding mVertexBindings[NumSlots];

	SamplerState mSamplerStates[NumSlots];

	IndexBuffer* mIndexBuffer = nullptr;

	std::unique_ptr<ShaderManager> mShaderManager;
	Shader* mShader = nullptr;

	struct Mat4f
	{
		Mat4f() { Values[0] = 1.0f; Values[5] = 1.0f; Values[10] = 1.0f; Values[15] = 1.0f; }
		float Values[16] = { 0.0f };
	};

	union UniformEntry
	{
		float valuef;
		int32_t valuei;
	};

	Mat4f mTransforms[(int)TransformState::NumTransforms];
	UniformEntry mUniforms[4 * 16 + 12 * 4];

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
};
