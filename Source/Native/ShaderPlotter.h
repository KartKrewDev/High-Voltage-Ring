#pragma once

static const char* plotter_vs = R"(
	in vec3 AttrPosition;
	in vec4 AttrColor;
	in vec2 AttrUV;

	out vec4 Color;
	out vec2 UV;
	out vec2 Pos;

	uniform mat4 projection;

	void main()
	{
		gl_Position = projection * vec4(AttrPosition, 1.0f);
		Color = AttrColor;
		UV = AttrUV;
		Pos = AttrPosition.xy;
	}
)";

const char* plotter_ps = R"(
	in vec4 Color;
	in vec2 UV;
	in vec2 Pos;

	out vec4 FragColor;

	uniform vec4 rendersettings;

	void main()
	{
		if (UV.x < 0)
		{
			float yFrac = -(UV.x + 1);

			vec2 tPos = vec2(
				gl_FragCoord.x,
				gl_FragCoord.y
			);

			// line stipple
			if (mod(floor(mix(tPos.x, tPos.y, yFrac)), 2.0) > 0f)
				discard;
		}

		// line smoothing
		float linewidth = 3.0;
		float falloff = 1.8; //1.5..2.5
		float centerdist = abs(UV.y);
		float a = pow(clamp((linewidth - centerdist) / linewidth, 0.0, 1.0), falloff);

		FragColor = vec4(Color.rgb, Color.a * a);
	}
)";
