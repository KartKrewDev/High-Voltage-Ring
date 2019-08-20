
#include "Precomp.h"
#include "ShaderManager.h"
#include "ShaderDisplay2D.h"
#include "ShaderThings2D.h"
#include "ShaderWorld3D.h"
#include "ShaderPlotter.h"
#include <stdexcept>

struct ShaderPair { const char* vs; const char* ps; };

static const ShaderPair ShaderSources[(int)ShaderName::count] = {
	{ display2D_vs, display2D_ps_fsaa },
	{ display2D_vs, display2D_ps_normal },
	{ display2D_vs, display2D_ps_fullbright },
	{ things2D_vs, things2D_ps_thing },
	{ things2D_vs, things2D_ps_sprite },
	{ things2D_vs, things2D_ps_fill },
	{ plotter_vs, plotter_ps },
	{ world3D_vs_main, world3D_ps_main },
	{ world3D_vs_main, world3D_ps_fullbright },
	{ world3D_vs_main, world3D_ps_main_highlight },
	{ world3D_vs_main, world3D_ps_fullbright_highlight },
	{ world3D_vs_customvertexcolor, world3D_ps_main },
	{ world3D_vs_skybox, world3D_ps_skybox },
	{ world3D_vs_customvertexcolor, world3D_ps_main_highlight },
	{ nullptr, nullptr },
	{ world3D_vs_lightpass, world3D_ps_main_fog },
	{ nullptr, nullptr },
	{ world3D_vs_lightpass, world3D_ps_main_highlight_fog },
	{ nullptr, nullptr },
	{ world3D_vs_customvertexcolor_fog, world3D_ps_main_fog },
	{ nullptr, nullptr },
	{ world3D_vs_customvertexcolor_fog, world3D_ps_main_highlight_fog },
	{ world3D_vs_main, world3D_ps_vertex_color },
	{ world3D_vs_customvertexcolor, world3D_ps_constant_color },
	{ world3D_vs_lightpass, world3D_ps_lightpass }
};

ShaderManager::ShaderManager()
{
	for (int i = 0; i < (int)ShaderName::count; i++)
	{
		if (ShaderSources[i].vs && ShaderSources[i].ps)
		{
			if (!Shaders[i].Compile(ShaderSources[i].vs, ShaderSources[i].ps, false))
			{
				CompileErrors += "Could not compile " + std::to_string(i) + "\r\n";
				CompileErrors += Shaders[i].GetErrors();
			}

			if (!AlphaTestShaders[i].Compile(ShaderSources[i].vs, ShaderSources[i].ps, true))
			{
				CompileErrors += "Could not compile " + std::to_string(i) + "\r\n";
				CompileErrors += Shaders[i].GetErrors();
			}
		}
	}
}

void ShaderManager::ReleaseResources()
{
}
