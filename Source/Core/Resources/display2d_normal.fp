
in vec4 Color;
in vec2 UV;

out vec4 FragColor;

// Render settings
// x = texel width
// y = texel height
// z = FSAA blend factor
// w = transparency
uniform vec4 rendersettings;
uniform float desaturation;

uniform sampler2D texture1;
uniform vec4 texturefactor;

vec3 desaturate(vec3 texel)
{
	float gray = (texel.r * 0.3 + texel.g * 0.56 + texel.b * 0.14);	
	return mix(texel, vec3(gray), desaturation);
}

void main()
{
	vec4 c = texture(texture1, UV);
	FragColor = vec4(desaturate(c.rgb), c.a * rendersettings.w) * Color;
	FragColor *= texturefactor;

	#if defined(ALPHA_TEST)
	if (FragColor.a < 0.5) discard;
	#endif
}
