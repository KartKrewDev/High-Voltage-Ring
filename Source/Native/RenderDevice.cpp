
#include "Precomp.h"
#include "RenderDevice.h"
#include "VertexBuffer.h"
#include "IndexBuffer.h"
#include "VertexDeclaration.h"
#include "Texture.h"
#include "ShaderManager.h"
#include <stdexcept>

RenderDevice::RenderDevice(HWND hwnd) : Context(hwnd)
{
	memset(mUniforms, 0, sizeof(mUniforms));

	if (Context)
	{
		Context.Begin();
		mShaderManager = std::make_unique<ShaderManager>();
		mShader = &mShaderManager->Shaders[(int)ShaderName::basic];
		Context.End();
	}
}

RenderDevice::~RenderDevice()
{
	if (Context)
	{
		Context.Begin();
		mShaderManager->ReleaseResources();
		Context.End();
	}
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
	mAlphaBlend = value;
	mNeedApply = true;
}

void RenderDevice::SetAlphaTestEnable(bool value)
{
	mAlphaTest = value;
	mNeedApply = true;
}

void RenderDevice::SetCullMode(Cull mode)
{
	mCullMode = mode;
	mNeedApply = true;
}

void RenderDevice::SetBlendOperation(BlendOperation op)
{
	mBlendOperation = op;
	mNeedApply = true;
}

void RenderDevice::SetSourceBlend(Blend blend)
{
	mSourceBlend = blend;
	mNeedApply = true;
}

void RenderDevice::SetDestinationBlend(Blend blend)
{
	mDestinationBlend = blend;
	mNeedApply = true;
}

void RenderDevice::SetFillMode(FillMode mode)
{
	mFillMode = mode;
	mNeedApply = true;
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
	mDepthTest = value;
	mNeedApply = true;
}

void RenderDevice::SetZWriteEnable(bool value)
{
	mDepthWrite = value;
	mNeedApply = true;
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
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	}
	else if (clear)
	{
		glClearColor(RPART(backcolor) / 255.0f, GPART(backcolor) / 255.0f, BPART(backcolor) / 255.0f, APART(backcolor) / 255.0f);
		glClear(GL_COLOR_BUFFER_BIT);
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

void RenderDevice::CheckError()
{
	GLenum error = glGetError();
	if (error != GL_NO_ERROR)
		throw std::runtime_error("OpenGL error!");
}

void RenderDevice::ApplyChanges()
{
	ApplyShader();
	ApplyVertexBuffers();
	ApplyIndexBuffer();
	ApplyMatrices();
	ApplyUniforms();
	ApplyTextures();
	ApplyRasterizerState();
	ApplyBlendState();
	ApplyDepthState();

	CheckError();

	mNeedApply = false;
}

void RenderDevice::ApplyShader()
{
	glUseProgram(mShader->GetProgram());
}

void RenderDevice::ApplyRasterizerState()
{
	if (mCullMode == Cull::None)
	{
		glDisable(GL_CULL_FACE);
	}
	else
	{
		glEnable(GL_CULL_FACE);
		glFrontFace(GL_CCW);
	}

	GLenum fillMode2GL[] = { GL_FILL, GL_LINE };
	glPolygonMode(GL_FRONT_AND_BACK, fillMode2GL[(int)mFillMode]);
}

void RenderDevice::ApplyBlendState()
{
	if (mAlphaBlend)
	{
		static const GLenum blendOp2GL[] = { GL_FUNC_ADD, GL_FUNC_REVERSE_SUBTRACT };
		static const GLenum blendFunc2GL[] = { GL_ONE_MINUS_SRC_ALPHA, GL_SRC_ALPHA, GL_ONE, GL_CONSTANT_COLOR };

		glEnable(GL_BLEND);
		glBlendEquation(blendOp2GL[(int)mBlendOperation]);
		glBlendFunc(blendFunc2GL[(int)mSourceBlend], blendFunc2GL[(int)mDestinationBlend]);
	}
	else
	{
		glDisable(GL_BLEND);
	}
}

void RenderDevice::ApplyDepthState()
{
	if (mDepthTest)
	{
		glEnable(GL_DEPTH_TEST);
		glDepthMask(mDepthWrite ? GL_TRUE : GL_FALSE);
	}
	else
	{
		glDisable(GL_DEPTH_TEST);
	}
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
	static const int type[] = { GL_FLOAT, GL_FLOAT, GL_BGRA };
	static const int typeNormalized[] = { GL_FALSE, GL_FALSE, GL_TRUE };

	if (mVertexDeclaration)
	{
		if (!mVAO)
		{
			glGenVertexArrays(1, &mVAO);
			glBindVertexArray(mVAO);
		}

		for (size_t i = 0; i < mVertexDeclaration->Elements.size(); i++)
		{
			const auto& element = mVertexDeclaration->Elements[i];
			auto& vertBinding = mVertexBindings[element.Stream];
			GLuint location = (int)element.Usage;
			if (vertBinding.Buffer)
			{
				GLuint vertexbuffer = vertBinding.Buffer->GetBuffer();
				glBindBuffer(GL_ARRAY_BUFFER, vertexbuffer);
				glEnableVertexAttribArray(location);
				glVertexAttribPointer(location, typeSize[(int)element.Type], type[(int)element.Type], typeNormalized[(int)element.Type], vertBinding.Stride, (const void*)(element.Offset + (ptrdiff_t)vertBinding.Offset));

				mEnabledVertexAttributes[location] = 2;
				break;
			}
		}
		glBindBuffer(GL_ARRAY_BUFFER, 0);
	}

	for (size_t i = 0; i < NumSlots; i++)
	{
		if (mEnabledVertexAttributes[i] == 2)
		{
			mEnabledVertexAttributes[i] = 1;
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
	for (size_t i = 0; i < (size_t)TransformState::NumTransforms; i++)
	{
		auto& binding = mTransforms[i];
		glUniformMatrix4fv(mShader->TransformLocations[i], 1, GL_FALSE, binding.Values);
	}
}

void RenderDevice::SetShader(ShaderName name)
{
	mShader = &mShaderManager->Shaders[(int)name];
	mNeedApply = true;
}

static const int uniformLocations[(int)UniformName::NumUniforms] = {
	64, // rendersettings
	0, // transformsettings
	108, // desaturation
	-1, // texture1,
	80, // highlightcolor
	16, // worldviewproj
	32, // world
	48, // modelnormal
	68, // FillColor
	72, // vertexColor
	84, // stencilColor
	92, // lightPosAndRadius
	96, // lightOrientation
	100, // light2Radius
	104, // lightColor
	109, // ignoreNormals
	110, // spotLight
	76, // campos
};

void RenderDevice::SetUniform(UniformName name, const void* values, int count)
{
	memcpy(&mUniforms[uniformLocations[(int)name]], values, sizeof(float) * count);
	mNeedApply = true;
}

void RenderDevice::ApplyUniforms()
{
	auto& locations = mShader->UniformLocations;

	glUniformMatrix4fv(locations[(int)UniformName::transformsettings], 1, GL_FALSE, &mUniforms[0].valuef);
	glUniformMatrix4fv(locations[(int)UniformName::worldviewproj], 1, GL_FALSE, &mUniforms[16].valuef);
	glUniformMatrix4fv(locations[(int)UniformName::world], 1, GL_FALSE, &mUniforms[32].valuef);
	glUniformMatrix4fv(locations[(int)UniformName::modelnormal], 1, GL_FALSE, &mUniforms[48].valuef);

	glUniform4fv(locations[(int)UniformName::rendersettings], 1, &mUniforms[64].valuef);
	glUniform4fv(locations[(int)UniformName::FillColor], 1, &mUniforms[68].valuef);
	glUniform4fv(locations[(int)UniformName::vertexColor], 1, &mUniforms[72].valuef);
	glUniform4fv(locations[(int)UniformName::campos], 1, &mUniforms[76].valuef);
	glUniform4fv(locations[(int)UniformName::highlightcolor], 1, &mUniforms[80].valuef);
	glUniform4fv(locations[(int)UniformName::stencilColor], 1, &mUniforms[84].valuef);
	glUniform4fv(locations[(int)UniformName::lightColor], 1, &mUniforms[88].valuef);
	glUniform4fv(locations[(int)UniformName::lightPosAndRadius], 1, &mUniforms[92].valuef);
	glUniform3fv(locations[(int)UniformName::lightOrientation], 1, &mUniforms[96].valuef);
	glUniform2fv(locations[(int)UniformName::light2Radius], 1, &mUniforms[100].valuef);
	glUniform4fv(locations[(int)UniformName::lightColor], 1, &mUniforms[104].valuef);

	glUniform1fv(locations[(int)UniformName::desaturation], 1, &mUniforms[108].valuef);
	glUniform1fv(locations[(int)UniformName::ignoreNormals], 1, &mUniforms[109].valuef);
	glUniform1fv(locations[(int)UniformName::spotLight], 1, &mUniforms[110].valuef);

	glUniform1i(locations[(int)UniformName::texture1], 0);
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
	glViewport(0, 0, Context.GetWidth(), Context.GetHeight());
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

void RenderDevice_SetShader(RenderDevice* device, ShaderName name)
{
	device->SetShader(name);
}

void RenderDevice_SetUniform(RenderDevice* device, UniformName name, const void* values, int count)
{
	device->SetUniform(name, values, count);
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
