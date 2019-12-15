#pragma once

static const char* things2D_vs = R"(
	in vec3 AttrPosition;
	in vec4 AttrColor;
	in vec2 AttrUV;

	out vec4 Color;
	out vec2 UV;

	uniform mat4 projection;

	void main()
	{
		gl_Position = projection * vec4(AttrPosition, 1.0);
		Color = AttrColor;
		UV = AttrUV;
	}
)";

static const char* things2D_ps_sprite = R"(
	in vec4 Color;
	in vec2 UV;

	out vec4 FragColor;

	// Render settings
	// w = transparency
	uniform vec4 rendersettings;
	uniform float desaturation;

	uniform sampler2D texture1;
	uniform vec4 texturefactor;

	vec3 desaturate(vec3 texel)
	{
		float gray = (texel.r * 0.3 + texel.g * 0.56 + texel.b * 0.14);	
		return mix(texel, vec3(gray), desaturation);
	}

	void main()
	{
		// Take this pixel's color
		vec4 c = texture(texture1, UV);
	
		// Modulate it by selection color
		if (Color.a > 0.0)
		{
			vec3 cr = desaturate(c.rgb);
			FragColor = vec4((cr.r + Color.r) / 2.0, (cr.g + Color.g) / 2.0, (cr.b + Color.b) / 2.0, c.a * rendersettings.w * Color.a);
		}
		else
		{
			// Or leave it as it is
			FragColor = vec4(desaturate(c.rgb), c.a * rendersettings.w);
		}

		FragColor *= texturefactor;

		#if defined(ALPHA_TEST)
		if (FragColor.a < 0.5) discard;
		#endif
	}
)";

static const char* things2D_ps_thing = R"(
	in vec4 Color;
	in vec2 UV;

	out vec4 FragColor;

	// Render settings
	// w = transparency
	uniform vec4 rendersettings;
	uniform float desaturation;

	uniform sampler2D texture1;
	uniform vec4 texturefactor;

	vec3 desaturate(vec3 texel)
	{
		float gray = (texel.r * 0.3 + texel.g * 0.56 + texel.b * 0.14);	
		return mix(texel, vec3(gray), desaturation);
	}

	void main()
	{
		vec4 c = texture(texture1, UV);
		FragColor = vec4(desaturate(c.rgb), c.a * rendersettings.w) * Color;

		FragColor *= texturefactor;

		#if defined(ALPHA_TEST)
		if (FragColor.a < 0.5) discard;
		#endif
	}
)";

static const char* things2D_ps_fill = R"(
	in vec4 Color;
	in vec2 UV;

	out vec4 FragColor;

	uniform vec4 fillColor;

	void main()
	{
		FragColor = fillColor;

		#if defined(ALPHA_TEST)
		if (FragColor.a < 0.5) discard;
		#endif
	}
)";
