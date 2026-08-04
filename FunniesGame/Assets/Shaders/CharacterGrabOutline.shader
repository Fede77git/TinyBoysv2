Shader "Custom/CharacterGrabOutline"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _OutlineColor ("Outline Color", Color) = (1, 1, 0, 1)
        _OutlineThickness ("Outline Thickness", Range(0.0, 100.0)) = 5.0
        [Toggle] _OutlineEnabled ("Outline Enabled", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        // Outline 
        Pass
        {
            Name "Outline"
            Tags { "LightMode"="SRPDefaultUnlit" }
            Cull Front
            ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            float _GlobalBlackoutOutlineEnabled;
            float4 _GlobalBlackoutOutlineColor;

            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float _OutlineThickness;
                float _OutlineEnabled;
                float4 _BaseMap_ST;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                
                float effectiveEnabled = max(_OutlineEnabled, _GlobalBlackoutOutlineEnabled);
                float thickness = _OutlineThickness * effectiveEnabled;
                
                float3 positionOS = input.positionOS.xyz + input.normalOS * thickness;
                output.positionCS = TransformObjectToHClip(positionOS);
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float effectiveEnabled = max(_OutlineEnabled, _GlobalBlackoutOutlineEnabled);
                if (effectiveEnabled < 0.5) discard;
                
                if (_OutlineEnabled > 0.5) 
                {
                    return _OutlineColor;
                }
                return _GlobalBlackoutOutlineColor;
            }
            ENDHLSL
        }

        // Textura Base del Personaje
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            Cull Back
            ZWrite On

            HLSLPROGRAM
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : NORMAL;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float _OutlineThickness;
                float _OutlineEnabled;
                float4 _BaseMap_ST;
                float4 _BaseColor;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.positionWS = positionWS;
                output.positionCS = TransformWorldToHClip(positionWS);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                float3 normalWS = normalize(input.normalWS);
                
                float4 shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                Light mainLight = GetMainLight(shadowCoord);
                
                half shadow = lerp(0.5, 1.0, mainLight.shadowAttenuation);
                half NdotL = saturate(dot(normalWS, mainLight.direction));
                half3 ambientLight = SampleSH(normalWS);
                
                half3 totalLight = ambientLight + (mainLight.color * mainLight.distanceAttenuation * shadow * NdotL);

#if defined(_ADDITIONAL_LIGHTS)
                uint pixelLightCount = GetAdditionalLightsCount();
                for (uint lightIndex = 0u; lightIndex < pixelLightCount; ++lightIndex)
                {
                    Light light = GetAdditionalLight(lightIndex, input.positionWS);
                    half addNdotL = saturate(dot(normalWS, light.direction));
                    totalLight += light.color * light.distanceAttenuation * light.shadowAttenuation * addNdotL;
                }
#endif

                totalLight = saturate(totalLight);
                return texColor * _BaseColor * half4(totalLight, 1.0);
            }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }
            ZWrite On
            ZTest LEqual
            ColorMask 0

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }
    }
}
