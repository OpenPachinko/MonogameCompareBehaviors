struct VSInput
{
	float3 Position : POSITION0;
};

struct VSOutput
{
	float4 Position : SV_POSITION;
};

struct PSOutput
{
	float4 Color : COLOR0; // xyz=rgb, w=specular intensity
	float4 Normal: COLOR1; // xyz=normal, w=specular power
	float4 Depth : COLOR2; // xyzw=depth
};

// ----------------------------------------------------------------

VSOutput VSFunc(VSInput input)
{
	VSOutput output;
	output.Position = float4(input.Position, 1);
	return output;
}

PSOutput PSFunc(VSOutput input)
{
	PSOutput output;

	// 
	//output.Color = float4(0.0f);
	//output.Normal= float4(0.5f, 0.5f, 0.5f, 0.0f);
	//output.Depth = 1.0f;

	// debug code
	output.Color  = float4(0.0f, 0.0f, 1.0f, 1.0f);
	output.Normal = float4(0.0f, 1.0f, 0.0f, 1.0f);
	output.Depth  = 1.0f;

	return output;
}

// ----------------------------------------------------------------

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_3_0 VSFunc();
        PixelShader  = compile ps_3_0 PSFunc();
    }
}