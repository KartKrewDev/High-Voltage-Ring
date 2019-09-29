
in vec4 Color;
in vec2 UV;
in vec4 viewpos;

out vec4 FragColor;

uniform vec4 highlightcolor;
uniform vec4 stencilColor;

uniform sampler2D texture1;

uniform vec4 fogsettings;
uniform vec4 fogcolor;

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

	if (fogsettings.x >= 0.0f) FragColor = mix(FragColor, fogcolor, clamp((-viewpos.z - fogsettings.x) / (fogsettings.y - fogsettings.x), 0.0, 1.0));
}
