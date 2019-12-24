
in vec4 Color;
in vec2 UV;

out vec4 FragColor;

uniform vec4 fillColor;

void main()
{
	FragColor = fillColor;

	#if defined(ALPHA_TEST)
	if (FragColor.a < 0.5) discard;
	#endif
}
