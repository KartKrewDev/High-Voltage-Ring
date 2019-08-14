#pragma once

static const char* default_vs = R"(
	#version 150

	in vec4 AttrPosition;
	in vec4 AttrColor;
	in vec2 AttrUV;
	in vec3 AttrNormal;

	out vec4 Color;
	out vec2 UV;
	out vec3 Normal;

	uniform mat4 World;
	uniform mat4 View;
	uniform mat4 Projection;

	void main()
	{
		Color = AttrColor;
		UV = AttrUV;
		Normal = AttrNormal;
		gl_Position = Projection * View * World * AttrPosition;
	}
)";

static const char* default_ps = R"(
	#version 150

	in vec4 Color;
	in vec2 UV;
	in vec3 Normal;

	out vec4 FragColor;

	void main()
	{
		FragColor = vec4(UV, 1.0, 1.0);
	}
)";
