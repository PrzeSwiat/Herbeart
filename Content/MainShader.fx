#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;
float3 DiffuseLightDirection = float3(1, 0, 0);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 1.0;
float4 LineColor = float4(0, 0, 0, 1);
float LineThickness = .03;
texture Texture;
float hp = 1.0f;

sampler2D textureSampler = sampler_state
{
    Texture = (Texture);
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};



// Dyrektywa dla przezroczystości
BlendState AlphaBlendState
{
    AlphaBlendEnable = true;
    SourceBlend = SRC_ALPHA;
    DestinationBlend = INV_SRC_ALPHA;
    // 
};

struct AppToVertex
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TextureCoordinate : TEXCOORD0;
};
 

struct VertexToPixel
{
    float4 Position : POSITION0;
    float2 TextureCoordinate : TEXCOORD0;
    float3 Normal : TEXCOORD1;
};





VertexToPixel CelVertexShader(AppToVertex input)
{
    VertexToPixel output;
 

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
 

    output.Normal = normalize(mul(input.Normal, (float3x4) WorldInverseTranspose));
 

    output.TextureCoordinate = input.TextureCoordinate;
 
    return output;
}
 

float4 CelPixelShader(VertexToPixel input) : COLOR0
{

    float intensity = dot(normalize(DiffuseLightDirection), input.Normal);
    if (intensity < 0)
        intensity = 0;

    float4 color = tex2D(textureSampler, input.TextureCoordinate) * DiffuseColor * DiffuseIntensity;
    clip(color.a < 0.75f ? -1 : 1);

    // Wartość HealthPercent z zakresu [0, 1]
    float healthIntensity = 0.6f - hp; ///parame

    // Zastosowanie efektu poszarzenia w zależności od wartości HealthPercent
    float4 grayColor = float4(0.5, 0.5, 0.5, 1.0);
    color = lerp(color, grayColor, healthIntensity);

    if (intensity > 0.95)
        color *= float4(1.0, 1.0, 1.0, 1.0);
    else if (intensity > 0.5)
        color *= float4(0.7, 0.7, 0.7, 1.0);
    else if (intensity > 0.05)
        color *= float4(0.35, 0.35, 0.35, 1.0);
    else
        color *= float4(0.1, 0.1, 0.1, 1.0);

    color.a = 0;
    return color;
}
 

VertexToPixel OutlineVertexShader(AppToVertex input)
{
    VertexToPixel output = (VertexToPixel) 0;
 

    float4 original = mul(mul(mul(input.Position, World), View), Projection);
 

    float4 normal = mul(mul(mul(input.Normal, (float3x4) World), View), Projection);
 

    output.Position = original + (mul(LineThickness, normal));
 
    return output;
}
 

float4 OutlinePixelShader(VertexToPixel input) : COLOR0
{
    return LineColor;
}
 


technique BasicColorDrawing
{
   
    
    
    pass P0
    {
  
        VertexShader = compile VS_SHADERMODEL OutlineVertexShader();
        PixelShader = compile PS_SHADERMODEL OutlinePixelShader();
        CullMode = CW;
    }
    pass P1
    {

        VertexShader = compile VS_SHADERMODEL CelVertexShader();
        PixelShader = compile PS_SHADERMODEL CelPixelShader();
        CullMode = CCW;


    }
    
};