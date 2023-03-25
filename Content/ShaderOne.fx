float4x4 World;
float4x4 View;
float4x4 Projection;

float4 AmbientColor = float4(1.0f, 1.0f, 1.0f, 1.0f);
float4 DiffuseColor = float4(1.0f, 1.0f, 1.0f, 1.0f);
float AmbientIntensity = 0.1f;

float3 LightPosition[2];
float3 Attenuation[2];
float LightRange[2];

texture ModelTexture;

sampler2D textureSampler = sampler_state
{
    Texture = (ModelTexture);
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float3 Normal : TEXCOORD0;
    float2 TextureCoordinate : TEXCOORD1;
    float4 WorldPosition : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4x4 WVP = mul(World, View);
    WVP = mul(WVP, Projection);
    output.Position = mul(input.Position, WVP);

    output.WorldPosition = mul(input.Position, World);
    output.Normal = mul(input.Normal, World);
    output.TextureCoordinate = input.TextureCoordinate;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    input.Normal = normalize(input.Normal);
    float4 diffuse = tex2D(textureSampler, input.TextureCoordinate);
    float3 finalColor = float3(0.0f, 0.0f, 0.0f);
    float3 singleColor;
    bool outOfRange;
    bool anyInRange = false;
    float3 finalAmbient = diffuse * AmbientColor * AmbientIntensity;

    for (int i = 0; i < 2; i++) {
        singleColor = float3(0.0f, 0.0f, 0.0f);
        float3 lightToPixelVec = LightPosition[i] - input.WorldPosition;
        float d = length(lightToPixelVec);
        
        if (d > LightRange[i]) {
            outOfRange = true;
        }
        else {
            outOfRange = false;
            anyInRange = true;
        }

        if (!outOfRange) {
            lightToPixelVec /= d;
            float howMuchLight = dot(lightToPixelVec, input.Normal);

            if (howMuchLight > 0.0f)
            {
                //Add light to the finalColor of the pixel
                singleColor += howMuchLight * diffuse * DiffuseColor;

                //Calculate Light's Falloff factor
                singleColor /= (Attenuation[i][0] + (Attenuation[i][1] * d) + (Attenuation[i][2] * (d * d)));
            }


            finalColor += saturate(singleColor + finalAmbient);
        }
    }

    if (!anyInRange) {
        return float4(finalAmbient, diffuse.a);
    }

    //Return Final Color
    return float4(finalColor, diffuse.a);
}

technique Ambient
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}