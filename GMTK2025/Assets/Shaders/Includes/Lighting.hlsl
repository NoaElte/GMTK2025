#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

void CalculateMainLight_float(float3 WorldPos, out float3 Direction, out float3 Color) {
#if SHADERGRAPH_PREVIEW
    Direction = float3(0.5, 0.5, 0);
    Color = 1;
#else
    Light mainLight = GetMainLight(0);
    Direction = mainLight.direction;
    Color = mainLight.color;
#endif
}

void TexelSnap_float(float3 WorldPos, float4 UV0, float4 TexelSize, out float3 SnappedWorldPos)
{
    // 1.) Calculate how much the texture UV coords need to
    //     shift to be at the center of the nearest texel.
    float2 originalUV = UV0.xy;
    float2 centerUV = floor(originalUV * (TexelSize.zw))/TexelSize.zw + (TexelSize.xy/2.0);
    float2 dUV = (centerUV - originalUV);
 
    // 2b.) Calculate how much the texture coords vary over fragment space.
    //      This essentially defines a 2x2 matrix that gets
    //      texture space (UV) deltas from fragment space (ST) deltas
    // Note: I call fragment space "ST" to disambiguate from world space "XY".
    float2 dUVdS = ddx( originalUV );
    float2 dUVdT = ddy( originalUV );
 
    // 2c.) Invert the texture delta from fragment delta matrix
    float2x2 dSTdUV = float2x2(dUVdT[1], -dUVdT[0], -dUVdS[1], dUVdS[0])*(1.0f/(dUVdS[0]*dUVdT[1]-dUVdT[0]*dUVdS[1]));
 
    // 2d.) Convert the texture delta to fragment delta
    float2 dST = mul(dSTdUV , dUV);
 
    // 2e.) Calculate how much the world coords vary over fragment space.
    float3 dXYZdS = ddx(WorldPos);
    float3 dXYZdT = ddy(WorldPos);
 
    // 2f.) Finally, convert our fragment space delta to a world space delta
    // And be sure to clamp it in case the derivative calc went insane
    float3 dXYZ = dXYZdS * dST[0] + dXYZdT * dST[1];
    dXYZ = clamp (dXYZ, -1, 1);
 
    // 3a.) Transform the snapped UV back to world space
    SnappedWorldPos = (WorldPos + dXYZ);
}

void GetAmbient_float(out float3 Ambient)
{
   Ambient = half3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
}

void StandardPBR_float(float3 ViewDirection, float3 WorldPos, float3 Normal, float3 Ambient, float3 Albedo, float Steps, float GradientMidLevel, float GradientSize, float LightAttenStrength, out float3 Color)
{
    #if SHADERGRAPH_PREVIEW
       float3 Direction = half3(0.5, 0.5, 0);
       float3 LightColor = 1;
       float DistanceAtten = 1;
       float ShadowAtten = 1;
    #else
    #if SHADOWS_SCREEN
       float4 clipPos = TransformWorldToHClip(WorldPos);
       float4 shadowCoord = ComputeScreenPos(clipPos);
    #else
       float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    #endif
       Light light = GetMainLight(shadowCoord);
       float3 Direction = light.direction;
       float3 LightColor = light.color;
       float DistanceAtten = light.distanceAttenuation;
       float ShadowAtten = light.shadowAttenuation;
    #endif
 
    Color = Albedo * saturate(LightColor * DistanceAtten) * ShadowAtten * saturate(dot(Normal, Direction));
 
    #ifndef SHADERGRAPH_PREVIEW
       int pixelLightCount = GetAdditionalLightsCount();
       for (int i = 0; i < pixelLightCount; ++i)
       {
           light = GetAdditionalLight(i, WorldPos, half4(1, 1, 1, 1));
           Direction = light.direction;
           float NL = dot(Normal, Direction) * .5 + .5; // remap
           LightColor = light.color;
           DistanceAtten = saturate(light.distanceAttenuation * LightAttenStrength);
           
           float SmoothSteppedNL = smoothstep(GradientMidLevel - (GradientSize * .5), GradientMidLevel + (GradientSize * .5), NL); // smoothstep
           float SteppedNL = floor(SmoothSteppedNL * DistanceAtten * Steps) / Steps; // posterize
           ShadowAtten = light.shadowAttenuation;
 
           Color += Albedo * LightColor * saturate(ShadowAtten * SteppedNL);
       }
    #endif
 
    Color += (Albedo * Ambient);
}

void GetTexelSize_float(out float4 TexelSize)
{
    TexelSize = _MainTex_TexelSize;
}

#endif