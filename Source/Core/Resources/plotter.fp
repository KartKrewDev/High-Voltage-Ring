
in vec4 Color;
in vec2 UV;

out vec4 FragColor;

void main()
{
	// line stipple
	if (mod(UV.x, 2.0) > 1.0)
		discard;

	// line smoothing
	float linewidth = 3.0;
	float falloff = 1.8; //1.5..2.5
	float centerdist = abs(UV.y);
	float a = pow(clamp((linewidth - centerdist) / linewidth, 0.0, 1.0), falloff);

	FragColor = vec4(Color.rgb, Color.a * a);
}
