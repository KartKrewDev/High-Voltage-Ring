
in vec4 Color;
in vec2 UV;
in vec4 viewpos;

out vec4 FragColor;

uniform vec4 highlightcolor;
uniform vec4 stencilColor;
uniform float desaturation;

uniform vec4 fogsettings;
uniform vec4 fogcolor;

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

	if (fogsettings.x >= 0.0f) FragColor = mix(FragColor, fogcolor, clamp((-viewpos.z - fogsettings.x) / (fogsettings.y - fogsettings.x), 0.0, 1.0));
}
