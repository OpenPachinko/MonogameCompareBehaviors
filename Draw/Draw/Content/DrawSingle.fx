float4x4 World;
float4x4 View;
float4x4 Projection;

struct VSInput
{
	float4 Position : POSITION0;
	float4 Color    : COLOR;
};

struct VSOutput
{
	float4 Position : SV_POSITION;
	float4 Color    : COLOR;
};

struct PSOutput
{
	float4 Color : SV_TARGET;
};


VSOutput VSMain(VSInput input)
{
	VSOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition  = mul(worldPosition, View);
	output.Position      = mul(viewPosition, Projection);

	output.Color = input.Color;

	return output;
}

PSOutput PSMain(VSOutput input)
{
	PSOutput output;
	output.Color = input.Color;
	return output;
}

technique Technique0
{
    pass Pass0
    {
        VertexShader = compile vs_3_0 VSMain();
        PixelShader  = compile ps_3_0 PSMain();
    }
}
