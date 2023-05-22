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
float4 AmbientColor = float4(1, 1, 1, 1);
float4 AmbienceColor = float4(0.1f, 0.1f, 0.1f, 1.0f);
float3 Attenuation = float3(0.0f, 0.2f, 0.0f);
float3 LightPosition = float3(10.0f, 0.0f, 0.0f);
float LightRange = 10.0f;
texture Texture;
 

sampler2D textureSampler = sampler_state
{
    Texture = (Texture);
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

struct PointLightInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float2 TextureCoordinate : TEXCOORD0;
};

struct PointLightOutput
{
    float4 Position : POSITION0;
    float3 Normal : TEXCOORD0;
    float2 TextureCoordinate : TEXCOORD1;
    float4 WorldPosition : TEXCOORD2;
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



PointLightOutput VertexShaderPointlightt(PointLightInput input)
{
    PointLightOutput output;

    float4x4 WVP = mul(World, View);
    WVP = mul(WVP, Projection);
    output.Position = mul(input.Position, WVP);

    output.WorldPosition = mul(input.Position, World);
    output.Normal = mul(input.Normal, (float3x3) World);
    output.TextureCoordinate = input.TextureCoordinate;

    return output;
}

float4 PixelShaderPointlight(PointLightOutput input) : COLOR0
{
    input.Normal = normalize(input.Normal);
    float4 diffuse = tex2D(textureSampler, input.TextureCoordinate);
    float3 finalColor = float3(0.0f, 0.0f, 0.0f);

    float3 lightToPixelVec = LightPosition - (float3) input.WorldPosition;
    float d = length(lightToPixelVec);

    float3 finalAmbient = diffuse * AmbientColor;
    if (d > LightRange)
       return float4(finalAmbient, diffuse.a);

    lightToPixelVec /= d;
    float howMuchLight = dot(lightToPixelVec, input.Normal);

    if (howMuchLight > 0.0f)
    {
        
        finalColor += howMuchLight * diffuse * DiffuseColor;

        
        finalColor /= (Attenuation[0] + (Attenuation[1] * d) + (Attenuation[2] * (d * d)));
    }


    finalColor = saturate(finalColor + finalAmbient);
    
    
    return float4(finalColor, diffuse.a);
}

VertexToPixel CelVertexShader(AppToVertex input)
{
    VertexToPixel output;
 

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
 

    output.Normal = normalize(mul(input.Normal, (float3x4)WorldInverseTranspose));
 

    output.TextureCoordinate = input.TextureCoordinate;
 
    return output;
}
 

float4 CelPixelShader(VertexToPixel input) : COLOR0
{

    float intensity = dot(normalize(DiffuseLightDirection), input.Normal);
    if (intensity < 0)
        intensity = 0;
 

    float4 color = tex2D(textureSampler, input.TextureCoordinate) * DiffuseColor * DiffuseIntensity;
    color.a = 1;
 

    if (intensity > 0.95)
        color = float4(1.0, 1, 1, 1.0) * color;
    else if (intensity > 0.5)
        color = float4(0.7, 0.7, 0.7, 1.0) * color;
    else if (intensity > 0.05)
        color = float4(0.35, 0.35, 0.35, 1.0) * color;
    else
        color = float4(0.1, 0.1, 0.1, 1.0) * color;
 
    return color;
}
 

VertexToPixel OutlineVertexShader(AppToVertex input)
{
    VertexToPixel output = (VertexToPixel) 0;
 

    float4 original = mul(mul(mul(input.Position, World), View), Projection);
 

    float4 normal = mul(mul(mul(input.Normal, (float3x4)World), View), Projection);
 

    output.Position = original + (mul(LineThickness, normal));
 
    return output;
}
 

float4 OutlinePixelShader(PointLightOutput input) : COLOR0
{
    return LineColor;
}
 


technique BasicColorDrawing
{
   
  
   
   
    
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL CelVertexShader();
        PixelShader = compile PS_SHADERMODEL CelPixelShader();
        

    }
    pass P1
    {
        VertexShader = compile VS_SHADERMODEL VertexShaderPointlightt();
        PixelShader = compile PS_SHADERMODEL PixelShaderPointlight();
        CullMode = CCW;
        
    }
   
    pass P2
    {
        VertexShader = compile VS_SHADERMODEL OutlineVertexShader();
        PixelShader = compile PS_SHADERMODEL OutlinePixelShader();
        CullMode = CW;
    }
    
    
};