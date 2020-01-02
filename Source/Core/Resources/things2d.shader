uniforms
{
	mat4 projection;

	// Render settings
	// w = transparency
	vec4 rendersettings;
	float desaturation;

	sampler2D texture1;
	vec4 texturefactor;

	vec4 fillColor;
}

functions
{
	vec3 desaturate(vec3 texel)
	{
		float gray = (texel.r * 0.3 + texel.g * 0.56 + texel.b * 0.14);
		return mix(texel, vec3(gray), desaturation);
	}
}

shader things2d
{
	in
	{
		vec3 Position;
		vec4 Color;
		vec2 TextureCoordinate;
	}

	v2f
	{
		vec4 Color;
		vec2 UV;
	}

	out
	{
		vec4 FragColor;
	}

	vertex
	{
		gl_Position = projection * vec4(in.Position, 1.0);
		v2f.Color = in.Color;
		v2f.UV = in.TextureCoordinate;
	}
}

shader things2d_fill extends things2d
{
	fragment
	{
		out.FragColor = fillColor;

		#if defined(ALPHA_TEST)
		if (out.FragColor.a < 0.5) discard;
		#endif
	}
}

shader things2d_thing extends things2d
{
	fragment
	{
		vec4 c = texture(texture1, v2f.UV);
		out.FragColor = vec4(desaturate(c.rgb), c.a * rendersettings.w) * v2f.Color;

		out.FragColor *= texturefactor;

		#if defined(ALPHA_TEST)
		if (out.FragColor.a < 0.5) discard;
		#endif
	}
}

shader things2d_sprite extends things2d
{
	fragment
	{
		// Take this pixel's color
		vec4 c = texture(texture1, v2f.UV);
	
		// Modulate it by selection color
		if (v2f.Color.a > 0.0)
		{
			vec3 cr = desaturate(c.rgb);
			out.FragColor = vec4((cr.r + v2f.Color.r) / 2.0, (cr.g + v2f.Color.g) / 2.0, (cr.b + v2f.Color.b) / 2.0, c.a * rendersettings.w * v2f.Color.a);
		}
		else
		{
			// Or leave it as it is
			out.FragColor = vec4(desaturate(c.rgb), c.a * rendersettings.w);
		}

		out.FragColor *= texturefactor;

		#if defined(ALPHA_TEST)
		if (out.FragColor.a < 0.5) discard;
		#endif
	}
}