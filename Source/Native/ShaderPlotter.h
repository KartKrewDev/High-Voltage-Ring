#pragma once

static const char* plotter_vs = R"(
	in vec3 AttrPosition;
	in vec4 AttrColor;
	in vec2 AttrUV;

	out vec4 Color;
	out vec2 UV;

	uniform mat4 transformsettings;

	void main()
	{
		gl_Position = transformsettings * vec4(AttrPosition, 1.0f);
		Color = AttrColor;
		UV = AttrUV;
	}
)";

const char* plotter_ps = R"(
	in vec4 Color;
	in vec2 UV;

	out vec4 FragColor;

	void main()
	{
		// line stipple
		int visible = int(UV.x) & 1;
		if (visible == 1)
			discard;

		// line smoothing
		float linewidth = 2.0;
		float falloff = 1.5; //1.5..2.5
		float centerdist = abs(UV.y);
		float a = pow(clamp((linewidth - centerdist) / linewidth, 0.0, 1.0), falloff);

		FragColor = vec4(Color.rgb, Color.a * a);
	}
)";
