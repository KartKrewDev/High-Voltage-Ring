#pragma once

#include <cstdint>

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
enum class SamplerState : int { AddressU, AddressV, AddressW };
enum class TextureAddress : int { Wrap, Clamp };
enum class ShaderFlags : int { None, Debug };
enum class PrimitiveType : int { LineList, TriangleList, TriangleStrip };
enum class TextureFilter : int { None, Point, Linear, Anisotropic };

class RenderDevice
{
public:
	RenderDevice();

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
};
