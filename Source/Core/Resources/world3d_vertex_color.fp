
in vec4 Color;
in vec2 UV;
in vec4 viewpos;

out vec4 FragColor;

uniform vec4 fogsettings;
uniform vec4 fogcolor;

void main()
{
	FragColor = Color;

	#if defined(ALPHA_TEST)
	if (FragColor.a < 0.5) discard;
	#endif

	if (fogsettings.x >= 0.0f) FragColor = mix(FragColor, fogcolor, clamp((-viewpos.z - fogsettings.x) / (fogsettings.y - fogsettings.x), 0.0, 1.0));
}
