uniforms
{
	mat4 world;
	mat4 view;
	mat4 projection;

	vec4 campos;  //w is set to fade factor (distance, at wich fog color completely overrides pixel color)

	vec4 highlightcolor;

	vec4 fogsettings;
	vec4 fogcolor;

	samplerCube texture1;
}

shader world3d_skybox
{
	in
	{
		vec3 Position;
		vec4 Color;
		vec2 TextureCoordinate;
		vec3 Normal;
	}
	
	v2f
	{
		vec3 Tex;
		vec4 viewpos;
	}
	
	out
	{
		vec4 FragColor;
	}

	vertex
	{
		v2f.viewpos = view * world * vec4(in.Position, 1.0);
		gl_Position = projection * v2f.viewpos;
		vec3 worldpos = (world * vec4(in.Position, 1.0)).xyz;
		vec4 skynormal = vec4(0.0, 1.0, 0.0, 0.0);
		vec3 normal = normalize((world * skynormal).xyz);
		v2f.Tex = reflect(worldpos - campos.xyz, normal);
	}

	fragment
	{
		vec4 ncolor = texture(texture1, v2f.Tex);
		out.FragColor = vec4(highlightcolor.rgb * highlightcolor.a + (ncolor.rgb - 0.4 * highlightcolor.a), 1.0);

		#if defined(ALPHA_TEST)
		if (out.FragColor.a < 0.5) discard;
		#endif

		if (fogsettings.x >= 0.0) out.FragColor = mix(out.FragColor, fogcolor, clamp((-v2f.viewpos.z - fogsettings.x) / (fogsettings.y - fogsettings.x), 0.0, 1.0));
	}
}