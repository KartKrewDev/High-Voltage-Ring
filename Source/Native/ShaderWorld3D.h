#pragma once

static const char* world3D_vs_main = R"(
	in vec3 AttrPosition;
	in vec4 AttrColor;
	in vec2 AttrUV;
	in vec3 AttrNormal;

	out vec4 Color;
	out vec2 UV;

	uniform mat4 worldviewproj;

	void main()
	{
		gl_Position = worldviewproj * vec4(AttrPosition, 1.0);
		Color = AttrColor;
		UV = AttrUV;
	}
)";

static const char* world3D_vs_customvertexcolor = R"(
	in vec3 AttrPosition;
	in vec4 AttrColor;
	in vec2 AttrUV;
	in vec3 AttrNormal;

	out vec4 Color;
	out vec2 UV;

	uniform mat4 worldviewproj;
	uniform vec4 vertexColor;

	void main()
	{
		gl_Position = worldviewproj * vec4(AttrPosition, 1.0);
		Color = vertexColor;
		UV = AttrUV;
	}
)";

static const char* world3D_vs_customvertexcolor_fog = R"(
	in vec3 AttrPosition;
	in vec4 AttrColor;
	in vec2 AttrUV;
	in vec3 AttrNormal;

	out vec4 Color;
	out vec2 UV;
	out vec3 PosW;
	out vec3 Normal;

	uniform mat4 worldviewproj;
	uniform mat4 world;
	uniform mat4 modelnormal;
	uniform vec4 vertexColor;

	void main()
	{
		gl_Position = worldviewproj * vec4(AttrPosition, 1.0);
		PosW = (world * vec4(AttrPosition, 1.0)).xyz;
		Color = vertexColor;
		UV = AttrUV;
		Normal = normalize((modelnormal * vec4(AttrNormal, 1.0)).xyz);
	}
)";

static const char* world3D_vs_lightpass = R"(
	in vec3 AttrPosition;
	in vec4 AttrColor;
	in vec2 AttrUV;
	in vec3 AttrNormal;

	out vec4 Color;
	out vec2 UV;
	out vec3 PosW;
	out vec3 Normal;

	uniform mat4 worldviewproj;
	uniform mat4 world;
	uniform mat4 modelnormal;

	void main()
	{
		gl_Position = worldviewproj * vec4(AttrPosition, 1.0);
		PosW = (world * vec4(AttrPosition, 1.0)).xyz;
		Color = AttrColor;
		UV = AttrUV;
		Normal = normalize((modelnormal * vec4(AttrNormal, 1.0)).xyz);
	}
)";

static const char* world3D_vs_skybox = R"(
	in vec3 AttrPosition;
	in vec2 AttrUV;

	out vec3 Tex;

	uniform mat4 worldviewproj;
	uniform mat4 world;
	uniform vec4 campos;  //w is set to fade factor (distance, at wich fog color completely overrides pixel color)

	void main()
	{
		gl_Position = worldviewproj * vec4(AttrPosition, 1.0);
		vec3 worldpos = (world * vec4(AttrPosition, 1.0)).xyz;
		vec4 skynormal = vec4(0.0, 1.0, 0.0, 0.0);
		vec3 normal = normalize((world * skynormal).xyz);
		Tex = reflect(worldpos - campos.xyz, normal);
	}
)";

static const char* world3D_ps_main = R"(
	in vec4 Color;
	in vec2 UV;

	out vec4 FragColor;

	uniform vec4 stencilColor;
	uniform float desaturation;

	uniform sampler2D texture1;

	vec4 desaturate(vec4 texel)
	{
		float gray = (texel.r * 0.3 + texel.g * 0.56 + texel.b * 0.14);	
		return vec4(mix(texel.rgb, vec3(gray), desaturation), texel.a);
	}

	void main()
	{
		vec4 tcolor = texture(texture1, UV);
		tcolor = mix(tcolor, vec4(stencilColor.rgb, tcolor.a), stencilColor.a);
		FragColor = desaturate(tcolor * Color);

		#if defined(ALPHA_TEST)
		if (FragColor.a < 0.5) discard;
		#endif
	}
)";

static const char* world3D_ps_fullbright = R"(
	in vec4 Color;
	in vec2 UV;

	out vec4 FragColor;

	uniform vec4 stencilColor;

	uniform sampler2D texture1;

	void main()
	{
		vec4 tcolor = texture(texture1, UV);
		tcolor = mix(tcolor, vec4(stencilColor.rgb, tcolor.a), stencilColor.a);
		tcolor.a *= Color.a;
		FragColor = tcolor;

		#if defined(ALPHA_TEST)
		if (FragColor.a < 0.5) discard;
		#endif
	}
)";

static const char* world3D_ps_main_highlight = R"(
	in vec4 Color;
	in vec2 UV;

	out vec4 FragColor;

	uniform vec4 highlightcolor;
	uniform vec4 stencilColor;
	uniform float desaturation;

	uniform sampler2D texture1;

	vec4 desaturate(vec4 texel)
	{
		float gray = (texel.r * 0.3 + texel.g * 0.56 + texel.b * 0.14);	
		return vec4(mix(texel.rgb, vec3(gray), desaturation), texel.a);
	}

	void main()
	{
		vec4 tcolor = texture(texture1, UV);
		tcolor = mix(tcolor, vec4(stencilColor.rgb, tcolor.a), stencilColor.a);
		if (tcolor.a == 0.0)
		{
			FragColor = tcolor;
		}
		else
		{
			// Blend texture color and vertex color
			vec4 ncolor = desaturate(tcolor * Color);

			FragColor = vec4(highlightcolor.rgb * highlightcolor.a + (ncolor.rgb - 0.4 * highlightcolor.a), max(Color.a + 0.25, 0.5));
		}

		#if defined(ALPHA_TEST)
		if (FragColor.a < 0.5) discard;
		#endif
	}
)";

static const char* world3D_ps_fullbright_highlight = R"(
	in vec4 Color;
	in vec2 UV;

	out vec4 FragColor;

	uniform vec4 highlightcolor;
	uniform vec4 stencilColor;

	uniform sampler2D texture1;

	void main()
	{
		vec4 tcolor = texture(texture1, UV);
		tcolor = mix(tcolor, vec4(stencilColor.rgb, tcolor.a), stencilColor.a);
		if(tcolor.a == 0.0)
		{
			FragColor = tcolor;
		}
		else
		{
			// Blend texture color and vertex color
			vec4 ncolor = tcolor * Color;

			FragColor = vec4(highlightcolor.rgb * highlightcolor.a + (tcolor.rgb - 0.4 * highlightcolor.a), max(Color.a + 0.25, 0.5));
		}

		#if defined(ALPHA_TEST)
		if (FragColor.a < 0.5) discard;
		#endif
	}
)";

static const char* world3D_ps_main_fog = R"(
	in vec4 Color;
	in vec2 UV;
	in vec3 PosW;
	in vec3 Normal;

	out vec4 FragColor;

	uniform vec4 stencilColor;
	uniform vec4 lightColor;
	uniform float desaturation;
	uniform vec4 campos;  //w is set to fade factor (distance, at wich fog color completely overrides pixel color)

	uniform sampler2D texture1;

	// This adds fog color to current pixel color
	vec4 getFogColor(vec4 color)
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

	void main()
	{
		vec4 tcolor = texture(texture1, UV);
		tcolor = mix(tcolor, vec4(stencilColor.rgb, tcolor.a), stencilColor.a);
		if (tcolor.a == 0.0)
		{
			FragColor = tcolor;
		}
		else
		{
			FragColor = desaturate(getFogColor(tcolor * Color));
		}

		#if defined(ALPHA_TEST)
		if (FragColor.a < 0.5) discard;
		#endif
	}
)";

static const char* world3D_ps_main_highlight_fog = R"(
	in vec4 Color;
	in vec2 UV;
	in vec3 PosW;
	in vec3 Normal;

	out vec4 FragColor;

	uniform vec4 highlightcolor;
	uniform vec4 stencilColor;
	uniform vec4 lightColor;
	uniform float desaturation;
	uniform vec4 campos;  //w is set to fade factor (distance, at wich fog color completely overrides pixel color)

	uniform sampler2D texture1;

	// This adds fog color to current pixel color
	vec4 getFogColor(vec4 color)
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

	void main()
	{
		vec4 tcolor = texture(texture1, UV);
		tcolor = mix(tcolor, vec4(stencilColor.rgb, tcolor.a), stencilColor.a);
		if (tcolor.a == 0.0)
		{
			FragColor = tcolor;
		}
		else
		{
			// Blend texture color and vertex color
			vec4 ncolor = desaturate(getFogColor(tcolor * Color));

			FragColor = vec4(highlightcolor.rgb * highlightcolor.a + (ncolor.rgb - 0.4 * highlightcolor.a), max(ncolor.a + 0.25, 0.5));
		}

		#if defined(ALPHA_TEST)
		if (FragColor.a < 0.5) discard;
		#endif
	}
)";

static const char* world3D_ps_constant_color = R"(
	in vec4 Color;
	in vec2 UV;

	out vec4 FragColor;

	uniform vec4 vertexColor;

	void main()
	{
		FragColor = vertexColor;

		#if defined(ALPHA_TEST)
		if (FragColor.a < 0.5) discard;
		#endif
	}
)";

static const char* world3D_ps_vertex_color = R"(
	in vec4 Color;
	in vec2 UV;

	out vec4 FragColor;

	void main()
	{
		FragColor = Color;

		#if defined(ALPHA_TEST)
		if (FragColor.a < 0.5) discard;
		#endif
	}
)";

static const char* world3D_ps_lightpass = R"(
	in vec4 Color;
	in vec2 UV;
	in vec3 PosW;
	in vec3 Normal;

	out vec4 FragColor;

	uniform vec4 stencilColor;
	uniform vec4 lightPosAndRadius;
	uniform vec3 lightOrientation; // this is a vector that points in light's direction
	uniform vec2 light2Radius; // this is used with spotlights
	uniform vec4 lightColor;
	uniform float ignoreNormals; // ignore normals in lighting equation. used for non-attenuated lights on models.
	uniform float spotLight; // use lightOrientation
	uniform float desaturation;

	uniform sampler2D texture1;

	vec4 desaturate(vec4 texel)
	{
		float gray = (texel.r * 0.3 + texel.g * 0.56 + texel.b * 0.14);	
		return vec4(mix(texel.rgb, vec3(gray), desaturation), texel.a);
	}

	void main()
	{
		//is face facing away from light source?
		//      update 01.02.2017: offset the equation by 3px back to try to emulate GZDoom's broken visibility check.
		float diffuseContribution = dot(Normal, normalize(lightPosAndRadius.xyz - PosW + Normal * 3.0));
		if (diffuseContribution < 0.0 && ignoreNormals < 0.5)
			discard;
		diffuseContribution = max(diffuseContribution, 0.0); // to make sure

		//is pixel in light range?
		float dist = distance(PosW, lightPosAndRadius.xyz);
		if (dist > lightPosAndRadius.w)
			discard;

		//is pixel tranparent?
		vec4 tcolor = texture(texture1, UV);
		tcolor = mix(tcolor, vec4(stencilColor.rgb, tcolor.a), stencilColor.a);
		if (tcolor.a == 0.0)
			discard;

		//if it is - calculate color at current pixel
		vec4 lightColorMod = vec4(0.0, 0.0, 0.0, 1.0);

		lightColorMod.rgb = lightColor.rgb * max(lightPosAndRadius.w - dist, 0.0) / lightPosAndRadius.w;
    
		if (spotLight > 0.5)
		{
			vec3 lightDirection = normalize(lightPosAndRadius.xyz - PosW);
			float cosDir = dot(lightDirection, lightOrientation);
			float df = smoothstep(light2Radius.y, light2Radius.x, cosDir);
			lightColorMod.rgb *= df;
		}

		if (lightColor.a > 0.979f && lightColor.a < 0.981f) // attenuated light 98%
			lightColorMod.rgb *= diffuseContribution;

		if (lightColorMod.r <= 0.0 && lightColorMod.g <= 0.0 && lightColorMod.b <= 0.0)
			discard;

		lightColorMod.rgb *= lightColor.a;
		if (lightColor.a > 0.4) //Normal, vavoom or negative light (or attenuated)
			lightColorMod *= tcolor;
		
		FragColor = desaturate(lightColorMod); //Additive light

		#if defined(ALPHA_TEST)
		if (FragColor.a < 0.5) discard;
		#endif
	}
)";

static const char* world3D_ps_skybox = R"(
	in vec3 Tex;

	out vec4 FragColor;

	uniform vec4 highlightcolor;

	uniform samplerCube texture1;

	void main()
	{
		vec4 ncolor = texture(texture1, Tex);
		FragColor = vec4(highlightcolor.rgb * highlightcolor.a + (ncolor.rgb - 0.4 * highlightcolor.a), 1.0);

		#if defined(ALPHA_TEST)
		if (FragColor.a < 0.5) discard;
		#endif
	}
)";
