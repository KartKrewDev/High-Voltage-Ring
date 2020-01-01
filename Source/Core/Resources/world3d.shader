uniforms
{
	mat4 world;
	mat4 view;
	mat4 projection;
	mat4 modelnormal;
	vec4 campos;
	
	vec4 highlightcolor;
	vec4 stencilColor;
	float desaturation;

	vec4 fogsettings;
	vec4 fogcolor;
	vec4 vertexColor;

	sampler2D texture1;

	// dynamic light related
	vec4 lightPosAndRadius;
	vec3 lightOrientation; // this is a vector that points in light's direction
	vec2 light2Radius; // this is used with spotlights
	vec4 lightColor;
	float ignoreNormals; // ignore normals in lighting equation. used for non-attenuated lights on models.
	float spotLight; // use lightOrientation
}

functions
{
	// This adds fog color to current pixel color
	vec4 getFogColor(vec3 PosW, vec4 color)
	{
		float fogdist = max(16.0, distance(PosW, campos.xyz));
		float fogfactor = exp2(campos.w * fogdist);

		color.rgb = mix(lightColor.rgb, color.rgb, fogfactor);
		return color;
	}

	vec4 desaturate(vec4 texel)
	{
		float gray = (texel.r * 0.3 + texel.g * 0.56 + texel.b * 0.14);	
		return vec4(mix(texel.rgb, vec3(gray), desaturation), texel.a);
	}
}

shader world3d_main
{
	in
	{
		vec3 Position;
		vec4 Color;
		vec2 TextureCoordinate;
		vec3 Normal;
	}
	
	v2f
	{
		vec4 Color;
		vec2 UV;
		vec4 viewpos;
	}
	
	out
	{
		vec4 FragColor;
	}
	
	vertex
	{
		v2f.viewpos = view * world * vec4(in.Position, 1.0);
		gl_Position = projection * v2f.viewpos;
		v2f.Color = in.Color;
		v2f.UV = in.TextureCoordinate;
	}
	
	fragment
	{
		vec4 tcolor = texture(texture1, v2f.UV);
		tcolor = mix(tcolor, vec4(stencilColor.rgb, tcolor.a), stencilColor.a);
		out.FragColor = desaturate(tcolor * v2f.Color);

		#if defined(ALPHA_TEST)
		if (out.FragColor.a < 0.5) discard;
		#endif

		if (fogsettings.x >= 0.0) out.FragColor = mix(out.FragColor, fogcolor, clamp((-v2f.viewpos.z - fogsettings.x) / (fogsettings.y - fogsettings.x), 0.0, 1.0));
	}
}

shader world3d_fullbright extends world3d_main
{
	fragment
	{
		vec4 tcolor = texture(texture1, v2f.UV);
		tcolor = mix(tcolor, vec4(stencilColor.rgb, tcolor.a), stencilColor.a);
		tcolor.a *= v2f.Color.a;
		out.FragColor = tcolor;

		#if defined(ALPHA_TEST)
		if (out.FragColor.a < 0.5) discard;
		#endif

		if (fogsettings.x >= 0.0) out.FragColor = mix(out.FragColor, fogcolor, clamp((-v2f.viewpos.z - fogsettings.x) / (fogsettings.y - fogsettings.x), 0.0, 1.0));
	}
}

shader world3d_main_highlight extends world3d_main
{
	fragment
	{
		vec4 tcolor = texture(texture1, v2f.UV);
		tcolor = mix(tcolor, vec4(stencilColor.rgb, tcolor.a), stencilColor.a);
		if (tcolor.a == 0.0)
		{
			out.FragColor = tcolor;
		}
		else
		{
			// Blend texture color and vertex color
			vec4 ncolor = desaturate(tcolor * v2f.Color);

			out.FragColor = vec4(highlightcolor.rgb * highlightcolor.a + (ncolor.rgb - 0.4 * highlightcolor.a), max(v2f.Color.a + 0.25, 0.5));
		}

		#if defined(ALPHA_TEST)
		if (out.FragColor.a < 0.5) discard;
		#endif

		if (fogsettings.x >= 0.0) out.FragColor = mix(out.FragColor, fogcolor, clamp((-v2f.viewpos.z - fogsettings.x) / (fogsettings.y - fogsettings.x), 0.0, 1.0));
	}
}

shader world3d_fullbright_highlight extends world3d_fullbright
{
	fragment
	{
		vec4 tcolor = texture(texture1, v2f.UV);
		tcolor = mix(tcolor, vec4(stencilColor.rgb, tcolor.a), stencilColor.a);
		if(tcolor.a == 0.0)
		{
			out.FragColor = tcolor;
		}
		else
		{
			// Blend texture color and vertex color
			vec4 ncolor = tcolor * v2f.Color;

			out.FragColor = vec4(highlightcolor.rgb * highlightcolor.a + (tcolor.rgb - 0.4 * highlightcolor.a), max(v2f.Color.a + 0.25, 0.5));
		}

		#if defined(ALPHA_TEST)
		if (out.FragColor.a < 0.5) discard;
		#endif

		if (fogsettings.x >= 0.0) out.FragColor = mix(out.FragColor, fogcolor, clamp((-v2f.viewpos.z - fogsettings.x) / (fogsettings.y - fogsettings.x), 0.0, 1.0));
	}
}

shader world3d_vertex_color extends world3d_main
{
	fragment
	{
		out.FragColor = v2f.Color;

		#if defined(ALPHA_TEST)
		if (out.FragColor.a < 0.5) discard;
		#endif

		if (fogsettings.x >= 0.0) out.FragColor = mix(out.FragColor, fogcolor, clamp((-v2f.viewpos.z - fogsettings.x) / (fogsettings.y - fogsettings.x), 0.0, 1.0));
	}
}

shader world3d_main_vertexcolor extends world3d_main
{
	vertex
	{
		v2f.viewpos = view * world * vec4(in.Position, 1.0);
		gl_Position = projection * v2f.viewpos;
		v2f.Color = vertexColor;
		v2f.UV = in.TextureCoordinate;
	}
}

shader world3d_constant_color extends world3d_main_vertexcolor
{
	fragment
	{
		out.FragColor = vertexColor;

		#if defined(ALPHA_TEST)
		if (out.FragColor.a < 0.5) discard;
		#endif

		if (fogsettings.x >= 0.0f) out.FragColor = mix(out.FragColor, fogcolor, clamp((-v2f.viewpos.z - fogsettings.x) / (fogsettings.y - fogsettings.x), 0.0, 1.0));
	}
}

// world3d_main_highlight with vertex program from world3d_vertexcolor
// to-do: rewrite into a function
shader world3d_highlight_vertexcolor extends world3d_main_highlight
{
	vertex
	{
		v2f.viewpos = view * world * vec4(in.Position, 1.0);
		gl_Position = projection * v2f.viewpos;
		v2f.Color = vertexColor;
		v2f.UV = in.TextureCoordinate;
	}
}

shader world3d_lightpass extends world3d_main
{
	v2f
	{
		vec4 Color;
		vec2 UV;
		vec3 PosW;
		vec3 Normal;
		vec4 viewpos;
	}

	vertex
	{
		v2f.viewpos = view * world * vec4(in.Position, 1.0);
		gl_Position = projection * v2f.viewpos;
		v2f.PosW = (world * vec4(in.Position, 1.0)).xyz;
		v2f.Color = in.Color;
		v2f.UV = in.TextureCoordinate;
		v2f.Normal = normalize((modelnormal * vec4(in.Normal, 1.0)).xyz);
	}

	fragment
	{
		//is face facing away from light source?
		//      update 01.02.2017: offset the equation by 3px back to try to emulate GZDoom's broken visibility check.
		float diffuseContribution = dot(v2f.Normal, normalize(lightPosAndRadius.xyz - v2f.PosW + v2f.Normal * 3.0));
		if (diffuseContribution < 0.0 && ignoreNormals < 0.5)
			discard;
		diffuseContribution = max(diffuseContribution, 0.0); // to make sure

		//is pixel in light range?
		float dist = distance(v2f.PosW, lightPosAndRadius.xyz);
		if (dist > lightPosAndRadius.w)
			discard;

		//is pixel tranparent?
		vec4 tcolor = texture(texture1, v2f.UV);
		tcolor = mix(tcolor, vec4(stencilColor.rgb, tcolor.a), stencilColor.a);
		if (tcolor.a == 0.0)
			discard;

		//if it is - calculate color at current pixel
		vec4 lightColorMod = vec4(0.0, 0.0, 0.0, 1.0);

		lightColorMod.rgb = lightColor.rgb * max(lightPosAndRadius.w - dist, 0.0) / lightPosAndRadius.w;
    
		if (spotLight > 0.5)
		{
			vec3 lightDirection = normalize(lightPosAndRadius.xyz - v2f.PosW);
			float cosDir = dot(lightDirection, lightOrientation);
			float df = smoothstep(light2Radius.y, light2Radius.x, cosDir);
			lightColorMod.rgb *= df;
		}

		if (lightColor.a > 0.979 && lightColor.a < 0.981) // attenuated light 98%
			lightColorMod.rgb *= diffuseContribution;

		if (lightColorMod.r <= 0.0 && lightColorMod.g <= 0.0 && lightColorMod.b <= 0.0)
			discard;

		lightColorMod.rgb *= lightColor.a;
		if (lightColor.a > 0.4) //Normal, vavoom or negative light (or attenuated)
			lightColorMod *= tcolor;
		
		out.FragColor = desaturate(lightColorMod); //Additive light

		#if defined(ALPHA_TEST)
		if (out.FragColor.a < 0.5) discard;
		#endif
	}
}

shader world3d_main_fog extends world3d_lightpass
{
	fragment
	{
		vec4 tcolor = texture(texture1, v2f.UV);
		tcolor = mix(tcolor, vec4(stencilColor.rgb, tcolor.a), stencilColor.a);
		if (tcolor.a == 0.0)
		{
			out.FragColor = tcolor;
		}
		else
		{
			out.FragColor = desaturate(getFogColor(v2f.PosW, tcolor * v2f.Color));
		}

		#if defined(ALPHA_TEST)
		if (out.FragColor.a < 0.5) discard;
		#endif

		if (fogsettings.x >= 0.0) out.FragColor = mix(out.FragColor, fogcolor, clamp((-v2f.viewpos.z - fogsettings.x) / (fogsettings.y - fogsettings.x), 0.0, 1.0));
	}
}

shader world3d_main_highlight_fog extends world3d_main_fog
{
	fragment
	{
		vec4 tcolor = texture(texture1, v2f.UV);
		tcolor = mix(tcolor, vec4(stencilColor.rgb, tcolor.a), stencilColor.a);
		if (tcolor.a == 0.0)
		{
			out.FragColor = tcolor;
		}
		else
		{
			// Blend texture color and vertex color
			vec4 ncolor = desaturate(getFogColor(v2f.PosW, tcolor * v2f.Color));

			out.FragColor = vec4(highlightcolor.rgb * highlightcolor.a + (ncolor.rgb - 0.4 * highlightcolor.a), max(ncolor.a + 0.25, 0.5));
		}

		#if defined(ALPHA_TEST)
		if (out.FragColor.a < 0.5) discard;
		#endif

		if (fogsettings.x >= 0.0) out.FragColor = mix(out.FragColor, fogcolor, clamp((-v2f.viewpos.z - fogsettings.x) / (fogsettings.y - fogsettings.x), 0.0, 1.0));
	}
}

// world3d_fog, but with vertex shader from customvertexcolor_fog
// to-do: rewrite into a function
shader world3d_main_fog_vertexcolor extends world3d_main_fog
{
	vertex
	{
		v2f.viewpos = view * world * vec4(in.Position, 1.0);
		gl_Position = projection * v2f.viewpos;
		v2f.PosW = (world * vec4(in.Position, 1.0)).xyz;
		v2f.Color = vertexColor;
		v2f.UV = in.TextureCoordinate;
		v2f.Normal = normalize((modelnormal * vec4(in.Normal, 1.0)).xyz);
	}
}

shader world3d_main_highlight_fog_vertexcolor extends world3d_main_highlight_fog
{
	vertex
	{
		v2f.viewpos = view * world * vec4(in.Position, 1.0);
		gl_Position = projection * v2f.viewpos;
		v2f.PosW = (world * vec4(in.Position, 1.0)).xyz;
		v2f.Color = vertexColor;
		v2f.UV = in.TextureCoordinate;
		v2f.Normal = normalize((modelnormal * vec4(in.Normal, 1.0)).xyz);
	}
}