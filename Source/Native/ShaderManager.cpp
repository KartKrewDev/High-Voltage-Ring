
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

static const std::string ShaderNames[(int)ShaderName::count] = {
	"display2d_fsaa",
	"display2d_normal",
	"display2d_fullbright",
	"things2d_thing",
	"things2d_sprite",
	"things2d_fill",
	"plotter",
	"world3d_main",
	"world3d_fullbright",
	"world3d_main_highlight",
	"world3d_fullbright_highlight",
	"world3d_main_vertexcolor",
	"world3d_skybox",
	"world3d_main_highlight_vertexcolor",
	"world3d_p7",
	"world3d_main_fog",
	"world3d_p9",
	"world3d_main_highlight_fog",
	"world3d_p11",
	"world3d_main_fog_vertexcolor",
	"world3d_p13",
	"world3d_main_highlight_fog_vertexcolor",
	"world3d_vertex_color",
	"world3d_constant_color",
	"world3d_lightpass",
};

ShaderManager::ShaderManager()
{
	for (int i = 0; i < (int)ShaderName::count; i++)
	{
		if (ShaderSources[i].vs && ShaderSources[i].ps)
		{
			Shaders[i].Setup(ShaderNames[i], ShaderSources[i].vs, ShaderSources[i].ps, false);
			AlphaTestShaders[i].Setup(ShaderNames[i], ShaderSources[i].vs, ShaderSources[i].ps, true);
		}
	}
}

void ShaderManager::ReleaseResources()
{
	for (int i = 0; i < (int)ShaderName::count; i++)
	{
		Shaders[i].ReleaseResources();
		AlphaTestShaders[i].ReleaseResources();
	}
}
