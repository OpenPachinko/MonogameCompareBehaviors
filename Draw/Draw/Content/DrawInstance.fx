
float4x4 View;
float4x4 Projection;

struct VSInput
{
	float4 Position : POSITION0;
	float4 Color    : COLOR;
};

struct VSInstance
{
	float4 w1 : BLENDWEIGHT0;
	float4 w2 : BLENDWEIGHT1;
	float4 w3 : BLENDWEIGHT2;
	float4 w4 : BLENDWEIGHT3;
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

VSOutput VSMain(VSInput input, VSInstance instance)
{
	VSOutput output;

    float4x4 world = float4x4(instance.w1, instance.w2, instance.w3, instance.w4);

	float4 worldPosition = mul(input.Position, world);
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
