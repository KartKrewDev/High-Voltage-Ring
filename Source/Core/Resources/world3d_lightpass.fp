
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
