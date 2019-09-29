
in vec3 Tex;
in vec4 viewpos;

out vec4 FragColor;

uniform vec4 highlightcolor;

uniform vec4 fogsettings;
uniform vec4 fogcolor;

uniform samplerCube texture1;

void main()
{
	vec4 ncolor = texture(texture1, Tex);
	FragColor = vec4(highlightcolor.rgb * highlightcolor.a + (ncolor.rgb - 0.4 * highlightcolor.a), 1.0);

	#if defined(ALPHA_TEST)
	if (FragColor.a < 0.5) discard;
	#endif

	if (fogsettings.x >= 0.0f) FragColor = mix(FragColor, fogcolor, clamp((-viewpos.z - fogsettings.x) / (fogsettings.y - fogsettings.x), 0.0, 1.0));
}
