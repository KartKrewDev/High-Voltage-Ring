
in vec4 Color;
in vec2 UV;
in vec3 PosW;
in vec3 Normal;
in vec4 viewpos;

out vec4 FragColor;

uniform vec4 highlightcolor;
uniform vec4 stencilColor;
uniform vec4 lightColor;
uniform float desaturation;
uniform vec4 campos;  //w is set to fade factor (distance, at wich fog color completely overrides pixel color)

uniform vec4 fogsettings;
uniform vec4 fogcolor;

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

	if (fogsettings.x >= 0.0f) FragColor = mix(FragColor, fogcolor, clamp((-viewpos.z - fogsettings.x) / (fogsettings.y - fogsettings.x), 0.0, 1.0));
}
