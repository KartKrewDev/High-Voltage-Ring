uniforms
{
	mat4 projection;

	// Render settings
	// x = texel width
	// y = texel height
	// z = FSAA blend factor
	// w = transparency
	vec4 rendersettings;
	float desaturation;

	sampler2D texture1;
	vec4 texturefactor;
}

functions
{
	vec3 desaturate(vec3 texel)
	{
		float gray = (texel.r * 0.3 + texel.g * 0.56 + texel.b * 0.14);
		return mix(texel, vec3(gray), desaturation);
	}

	// This blends the max of 2 pixels
	vec4 addcolor(vec4 c1, vec4 c2)
	{
		return vec4(
			max(c1.r, c2.r),
			max(c1.g, c2.g),
			max(c1.b, c2.b),
			clamp(c1.a + c2.a * 0.5, 0.0, 1.0));
	}
}

shader display2d_normal
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

shader display2d_fullbright extends display2d_normal
{
	fragment
	{
		vec4 c = texture(texture1, v2f.UV);
		out.FragColor = vec4(c.rgb, c.a * rendersettings.w);
		out.FragColor *= texturefactor;

		#if defined(ALPHA_TEST)
		if (out.FragColor.a < 0.5) discard;
		#endif
	}
}

shader display2d_fsaa extends display2d_normal
{
	fragment
	{
		vec4 c = texture(texture1, v2f.UV);
	
		// If this pixel is not drawn on...
		if(c.a < 0.1)
		{
			// Mix the colors of nearby pixels
			vec4 n = vec4(0.0);
			n = addcolor(n, texture(texture1, vec2(v2f.UV.x + rendersettings.x, v2f.UV.y)));
			n = addcolor(n, texture(texture1, vec2(v2f.UV.x - rendersettings.x, v2f.UV.y)));
			n = addcolor(n, texture(texture1, vec2(v2f.UV.x, v2f.UV.y + rendersettings.y)));
			n = addcolor(n, texture(texture1, vec2(v2f.UV.x, v2f.UV.y - rendersettings.y)));
		
			out.FragColor = vec4(desaturate(n.rgb), n.a * rendersettings.z * rendersettings.w);
		}
		else
		{
			out.FragColor = vec4(desaturate(c.rgb), c.a * rendersettings.w) * v2f.Color;
		}

		out.FragColor *= texturefactor;

		#if defined(ALPHA_TEST)
		if (out.FragColor.a < 0.5) discard;
		#endif
	}
}