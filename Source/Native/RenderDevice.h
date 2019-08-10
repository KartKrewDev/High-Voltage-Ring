#pragma once

#include "OpenGLContext.h"

class VertexBuffer;
class IndexBuffer;
class VertexDeclaration;
class Texture;
enum class CubeMapFace;

enum class Cull : int { None, Counterclockwise };
enum class Blend : int { InverseSourceAlpha, SourceAlpha, One, BlendFactor };
enum class BlendOperation : int { Add, ReverseSubtract };
enum class FillMode : int { Solid, Wireframe };
enum class TransformState : int { World, View, Projection };
enum class TextureAddress : int { Wrap, Clamp };
enum class ShaderFlags : int { None, Debug };
enum class PrimitiveType : int { LineList, TriangleList, TriangleStrip };
enum class TextureFilter : int { None, Point, Linear, Anisotropic };

class RenderDevice
{
public:
	RenderDevice(HWND hwnd);

	void SetVertexBuffer(int index, VertexBuffer* buffer, long offset, long stride);
	void SetIndexBuffer(IndexBuffer* buffer);
	void SetAlphaBlendEnable(bool value);
	void SetAlphaRef(int value);
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
	void ApplyMatrices();
	void ApplyTextures();
	void ApplyRenderTarget(Texture* target, bool usedepthbuffer);

	OpenGLContext Context;

	struct VertexBinding
	{
		VertexBinding() = default;
		VertexBinding(VertexBuffer* buffer, long offset, long stride) : Buffer(buffer), Offset(offset), Stride(stride) { }

		VertexBuffer* Buffer = nullptr;
		long Offset = 0;
		long Stride = 0;
	};

	struct Mat4f
	{
		Mat4f() { Values[0] = 1.0f; Values[5] = 1.0f; Values[10] = 1.0f; Values[15] = 1.0f; }
		float Values[16] = { 0.0f };
	};

	struct SamplerState
	{
		SamplerState() = default;
		SamplerState(TextureAddress addressU, TextureAddress addressV, TextureAddress addressW) : AddressU(addressU), AddressV(addressV), AddressW(addressW) { }

		TextureAddress AddressU = TextureAddress::Wrap;
		TextureAddress AddressV = TextureAddress::Wrap;
		TextureAddress AddressW = TextureAddress::Wrap;
	};

	enum { NumTransforms = 3, NumSlots = 16 };
	Mat4f mTransforms[NumTransforms];
	VertexDeclaration *mVertexDeclaration = nullptr;
	int mEnabledVertexAttributes[NumSlots] = { 0 };
	VertexBinding mVertexBindings[NumSlots];
	SamplerState mSamplerStates[NumSlots];
	IndexBuffer* mIndexBuffer = nullptr;
	bool mNeedApply = true;
};
