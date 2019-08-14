#pragma once

#include "Shader.h"

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

class ShaderManager
{
public:
	ShaderManager();
	
	void ReleaseResources();

	std::string CompileErrors;

private:
	Shader mShaders[(int)ShaderName::count];
};
