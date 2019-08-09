
#include "RenderDevice.h"
#include "VertexBuffer.h"
#include "IndexBuffer.h"
#include "VertexDeclaration.h"
#include "Texture.h"

RenderDevice::RenderDevice()
{
}

void RenderDevice::SetVertexBuffer(int index, VertexBuffer* buffer, long offset, long stride)
{
}

void RenderDevice::SetIndexBuffer(IndexBuffer* buffer)
{
}

void RenderDevice::SetAlphaBlendEnable(bool value)
{
}

void RenderDevice::SetAlphaRef(int value)
{
}

void RenderDevice::SetAlphaTestEnable(bool value)
{
}

void RenderDevice::SetCullMode(Cull mode)
{
}

void RenderDevice::SetBlendOperation(BlendOperation op)
{
}

void RenderDevice::SetSourceBlend(Blend blend)
{
}

void RenderDevice::SetDestinationBlend(Blend blend)
{
}

void RenderDevice::SetFillMode(FillMode mode)
{
}

void RenderDevice::SetFogEnable(bool value)
{
}

void RenderDevice::SetFogColor(int value)
{
}

void RenderDevice::SetFogStart(float value)
{
}

void RenderDevice::SetFogEnd(float value)
{
}

void RenderDevice::SetMultisampleAntialias(bool value)
{
}

void RenderDevice::SetTextureFactor(int factor)
{
}

void RenderDevice::SetZEnable(bool value)
{
}

void RenderDevice::SetZWriteEnable(bool value)
{
}

void RenderDevice::SetTransform(TransformState state, float* matrix)
{
}

void RenderDevice::SetSamplerState(int unit, TextureAddress addressU, TextureAddress addressV, TextureAddress addressW)
{
}

void RenderDevice::DrawPrimitives(PrimitiveType type, int startIndex, int primitiveCount)
{
}

void RenderDevice::DrawUserPrimitives(PrimitiveType type, int startIndex, int primitiveCount, const void* data)
{
}

void RenderDevice::SetVertexDeclaration(VertexDeclaration* decl)
{
}

void RenderDevice::StartRendering(bool clear, int backcolor, Texture* target, bool usedepthbuffer)
{
	//if (clear && usedepthbuffer)
	//    Clear(ClearFlags.Target | ClearFlags.ZBuffer, backcolor, 1f, 0);
	//else if (clear)
	//    Clear(ClearFlags.Target, backcolor, 1f, 0);
}

void RenderDevice::FinishRendering()
{
}

void RenderDevice::Present()
{
}

void RenderDevice::ClearTexture(int backcolor, Texture* texture)
{
}

void RenderDevice::CopyTexture(Texture* src, Texture* dst, CubeMapFace face)
{
}

/////////////////////////////////////////////////////////////////////////////

RenderDevice* RenderDevice_New()
{
	return new RenderDevice();
}

void RenderDevice_Delete(RenderDevice* device)
{
	delete device;
}

void RenderDevice_SetVertexBuffer(RenderDevice* device, int index, VertexBuffer* buffer, long offset, long stride)
{
	device->SetVertexBuffer(index, buffer, offset, stride);
}

void RenderDevice_SetIndexBuffer(RenderDevice* device, IndexBuffer* buffer)
{
	device->SetIndexBuffer(buffer);
}

void RenderDevice_SetAlphaBlendEnable(RenderDevice* device, bool value)
{
	device->SetAlphaBlendEnable(value);
}

void RenderDevice_SetAlphaRef(RenderDevice* device, int value)
{
	device->SetAlphaRef(value);
}

void RenderDevice_SetAlphaTestEnable(RenderDevice* device, bool value)
{
	device->SetAlphaTestEnable(value);
}

void RenderDevice_SetCullMode(RenderDevice* device, Cull mode)
{
	device->SetCullMode(mode);
}

void RenderDevice_SetBlendOperation(RenderDevice* device, BlendOperation op)
{
	device->SetBlendOperation(op);
}

void RenderDevice_SetSourceBlend(RenderDevice* device, Blend blend)
{
	device->SetSourceBlend(blend);
}

void RenderDevice_SetDestinationBlend(RenderDevice* device, Blend blend)
{
	device->SetDestinationBlend(blend);
}

void RenderDevice_SetFillMode(RenderDevice* device, FillMode mode)
{
	device->SetFillMode(mode);
}

void RenderDevice_SetFogEnable(RenderDevice* device, bool value)
{
	device->SetFogEnable(value);
}

void RenderDevice_SetFogColor(RenderDevice* device, int value)
{
	device->SetFogColor(value);
}

void RenderDevice_SetFogStart(RenderDevice* device, float value)
{
	device->SetFogStart(value);
}

void RenderDevice_SetFogEnd(RenderDevice* device, float value)
{
	device->SetFogEnd(value);
}

void RenderDevice_SetMultisampleAntialias(RenderDevice* device, bool value)
{
	device->SetMultisampleAntialias(value);
}

void RenderDevice_SetTextureFactor(RenderDevice* device, int factor)
{
	device->SetTextureFactor(factor);
}

void RenderDevice_SetZEnable(RenderDevice* device, bool value)
{
	device->SetZEnable(value);
}

void RenderDevice_SetZWriteEnable(RenderDevice* device, bool value)
{
	device->SetZWriteEnable(value);
}

void RenderDevice_SetTransform(RenderDevice* device, TransformState state, float* matrix)
{
	device->SetTransform(state, matrix);
}

void RenderDevice_SetSamplerState(RenderDevice* device, int unit, TextureAddress addressU, TextureAddress addressV, TextureAddress addressW)
{
	device->SetSamplerState(unit, addressU, addressV, addressW);
}

void RenderDevice_DrawPrimitives(RenderDevice* device, PrimitiveType type, int startIndex, int primitiveCount)
{
	device->DrawPrimitives(type, startIndex, primitiveCount);
}

void RenderDevice_DrawUserPrimitives(RenderDevice* device, PrimitiveType type, int startIndex, int primitiveCount, const void* data)
{
	device->DrawUserPrimitives(type, startIndex, primitiveCount, data);
}

void RenderDevice_SetVertexDeclaration(RenderDevice* device, VertexDeclaration* decl)
{
	device->SetVertexDeclaration(decl);
}

void RenderDevice_StartRendering(RenderDevice* device, bool clear, int backcolor, Texture* target, bool usedepthbuffer)
{
	device->StartRendering(clear, backcolor, target, usedepthbuffer);
}

void RenderDevice_FinishRendering(RenderDevice* device)
{
	device->FinishRendering();
}

void RenderDevice_Present(RenderDevice* device)
{
	device->Present();
}

void RenderDevice_ClearTexture(RenderDevice* device, int backcolor, Texture* texture)
{
	device->ClearTexture(backcolor, texture);
}

void RenderDevice_CopyTexture(RenderDevice* device, Texture* src, Texture* dst, CubeMapFace face)
{
	device->CopyTexture(src, dst, face);
}
