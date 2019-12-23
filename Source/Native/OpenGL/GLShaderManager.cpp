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

#include "Precomp.h"
#include "GLShaderManager.h"

void GLShaderManager::DeclareShader(int i, const char* name, const char* vs, const char* ps)
{
	if (Shaders.size() <= (size_t)i)
	{
		Shaders.resize((size_t)i + 1);
		AlphaTestShaders.resize((size_t)i + 1);
	}

	Shaders[i].Setup(name, vs, ps, false);
	AlphaTestShaders[i].Setup(name, vs, ps, true);
}

void GLShaderManager::ReleaseResources()
{
	for (size_t i = 0; i < Shaders.size(); i++)
	{
		Shaders[i].ReleaseResources();
		AlphaTestShaders[i].ReleaseResources();
	}
}
