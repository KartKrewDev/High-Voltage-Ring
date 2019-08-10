
#include "Precomp.h"
#include "RenderDevice.h"
#include "VertexBuffer.h"
#include "IndexBuffer.h"
#include "VertexDeclaration.h"
#include "Texture.h"

RenderDevice::RenderDevice(HWND hwnd) : Context(hwnd)
{
}

void RenderDevice::SetVertexBuffer(int index, VertexBuffer* buffer, long offset, long stride)
{
	mVertexBindings[index] = { buffer, offset, stride };
	mNeedApply = true;
}

void RenderDevice::SetIndexBuffer(IndexBuffer* buffer)
{
	mIndexBuffer = buffer;
	mNeedApply = true;
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
	memcpy(mTransforms[(int)state].Values, matrix, 16 * sizeof(float));
	mNeedApply = true;
}

void RenderDevice::SetSamplerState(int index, TextureAddress addressU, TextureAddress addressV, TextureAddress addressW)
{
	mSamplerStates[index] = { addressU, addressV, addressW };
	mNeedApply = true;
}

void RenderDevice::DrawPrimitives(PrimitiveType type, int startIndex, int primitiveCount)
{
	static const int modes[] = { GL_LINES, GL_TRIANGLES, GL_TRIANGLE_STRIP };
	static const int toVertexCount[] = { 2, 3, 1 };
	static const int toVertexStart[] = { 0, 0, 2 };

	Context.Begin();
	if (mNeedApply) ApplyChanges();
	glDrawArrays(modes[(int)type], startIndex, toVertexStart[(int)type] + primitiveCount * toVertexCount[(int)type]);
	Context.End();
}

void RenderDevice::DrawUserPrimitives(PrimitiveType type, int startIndex, int primitiveCount, const void* data)
{
}

void RenderDevice::SetVertexDeclaration(VertexDeclaration* decl)
{
	mVertexDeclaration = decl;
	mNeedApply = true;
}

void RenderDevice::StartRendering(bool clear, int backcolor, Texture* target, bool usedepthbuffer)
{
	Context.Begin();
	ApplyRenderTarget(target, usedepthbuffer);
	if (clear && usedepthbuffer)
	{
		glClearColor(RPART(backcolor) / 255.0f, GPART(backcolor) / 255.0f, BPART(backcolor) / 255.0f, APART(backcolor) / 255.0f);
		glClearDepthf(1.0f);
		glClear(GL_COLOR | GL_DEPTH);
	}
	else if (clear)
	{
		glClearColor(RPART(backcolor) / 255.0f, GPART(backcolor) / 255.0f, BPART(backcolor) / 255.0f, APART(backcolor) / 255.0f);
		glClear(GL_COLOR);
	}
	Context.End();
}

void RenderDevice::FinishRendering()
{
}

void RenderDevice::Present()
{
	Context.SwapBuffers();
}

void RenderDevice::ClearTexture(int backcolor, Texture* texture)
{
}

void RenderDevice::CopyTexture(Texture* src, Texture* dst, CubeMapFace face)
{
}

void RenderDevice::ApplyChanges()
{
	ApplyVertexBuffers();
	ApplyIndexBuffer();
	ApplyMatrices();
	ApplyTextures();

	mNeedApply = false;
}

void RenderDevice::ApplyIndexBuffer()
{
	if (mIndexBuffer)
	{
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, mIndexBuffer->GetBuffer());
	}
	else
	{
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	}
}

void RenderDevice::ApplyVertexBuffers()
{
	static const int typeSize[] = { 2, 3, 4 };
	static const int type[] = { GL_FLOAT, GL_FLOAT, GL_UNSIGNED_BYTE };
	static const int typeNormalized[] = { GL_FALSE, GL_FALSE, GL_TRUE };

	if (mVertexDeclaration)
	{
		for (size_t i = 0; i < mVertexDeclaration->Elements.size(); i++)
		{
			const auto& element = mVertexDeclaration->Elements[i];
			auto& vertBinding = mVertexBindings[element.Stream];
			GLuint location = (int)element.Usage;
			if (vertBinding.Buffer)
			{
				GLuint vertexbuffer = vertBinding.Buffer->GetBuffer();
				glEnableVertexAttribArray(location);
				glBindBuffer(GL_ARRAY_BUFFER, vertexbuffer);
				glVertexAttribPointer(location, typeSize[(int)element.Type], type[(int)element.Type], typeNormalized[(int)element.Type], vertBinding.Stride, (const void*)(element.Offset + (ptrdiff_t)vertBinding.Offset));

				mEnabledVertexAttributes[location] = 2;
			}
		}
		glBindBuffer(GL_ARRAY_BUFFER, 0);
	}

	for (size_t i = 0; i < NumSlots; i++)
	{
		if (mEnabledVertexAttributes[i] == 2)
		{
			mEnabledVertexAttributes[i]--;
		}
		else if (mEnabledVertexAttributes[i] == 1)
		{
			glDisableVertexAttribArray((GLuint)i);
			mEnabledVertexAttributes[i] = 0;
		}
	}
}

void RenderDevice::ApplyMatrices()
{
	for (size_t i = 0; i < NumTransforms; i++)
	{
		auto& binding = mTransforms[i];
		glUniformMatrix4fv((GLuint)i, 1, GL_FALSE, binding.Values);
	}
}

void RenderDevice::ApplyTextures()
{
	static const int wrapMode[] = { GL_REPEAT, GL_CLAMP_TO_EDGE };

	for (size_t i = 0; i < NumSlots; i++)
	{
		auto& binding = mSamplerStates[i];
		glActiveTexture(GL_TEXTURE0 + (GLenum)i);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, wrapMode[(int)binding.AddressU]);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, wrapMode[(int)binding.AddressV]);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_R, wrapMode[(int)binding.AddressW]);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	}
}

void RenderDevice::ApplyRenderTarget(Texture* target, bool usedepthbuffer)
{
}

/////////////////////////////////////////////////////////////////////////////

RenderDevice* RenderDevice_New(HWND hwnd)
{
	RenderDevice *device = new RenderDevice(hwnd);
	if (!device->Context)
	{
		delete device;
		return nullptr;
	}
	else
	{
		return device;
	}
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
