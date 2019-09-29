
in vec4 Color;
in vec2 UV;

out vec4 FragColor;

// Render settings
// x = texel width
// y = texel height
// z = FSAA blend factor
// w = transparency
uniform vec4 rendersettings;

uniform sampler2D texture1;
uniform vec4 texturefactor;

void main()
{
	vec4 c = texture(texture1, UV);
	FragColor = vec4(c.rgb, c.a * rendersettings.w);
	FragColor *= texturefactor;

	#if defined(ALPHA_TEST)
	if (FragColor.a < 0.5) discard;
	#endif
}
