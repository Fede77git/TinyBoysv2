// Shader de niebla/nube para un plano horizontal.
//
// Flujo:
//   Vertex — dos noise (A y B) con speed independiente se combinan,
//            se remapean a [-1,1] y se aplica abs() para eliminar valles.
//            El resultado desplaza el plano a lo largo de su normal.
//
//   Fragment — un tercer noise (C) añade variación de alpha sin afectar
//              la geometría. La profundidad de escena suaviza el borde
//              donde la niebla intersecta con otros objetos.
//
// Setup:
//   - Crear un plano subdividido (≥ 30×30 quads) en Blender para que el
//     displacement se vea fluido.
//   - Activar "Depth Texture" en el URP Asset.
//   - Transparentes: ZWrite Off, Blend SrcAlpha OneMinusSrcAlpha.

Shader "Rollgeon/FogPlane"
{
    Properties
    {
        [Header(Color)]
        _Color          ("Fog Color",                        Color)         = (0.75, 0.82, 0.90, 1)
        _Opacity        ("Opacity",                          Range(0,1))    = 0.65
        // < 1 = nube densa/sólida  |  > 1 = nube wispy/tenue
        _AlphaContrast  ("Edge Contrast  (<1 denso  >1 wispy)", Range(0.1,4)) = 1.2

        [Header(Noise A  Displacement principal)]
        _NoiseScaleA    ("Scale A  (world units)",           Float)         = 3.5
        _NoiseSpeedAX   ("Speed A  X",                       Float)         = 0.04
        _NoiseSpeedAZ   ("Speed A  Z",                       Float)         = 0.02

        [Header(Noise B  Variacion de desplazamiento)]
        _NoiseScaleB    ("Scale B  (world units)",           Float)         = 1.8
        _NoiseSpeedBX   ("Speed B  X",                       Float)         = 0.03
        _NoiseSpeedBZ   ("Speed B  Z",                       Float)         = 0.05
        _NoiseMix       ("Mix AB  (0=solo A  1=solo B)",     Range(0,1))    = 0.40

        [Header(Vertex Displacement)]
        _DisplaceStr    ("Displace Strength",                Float)         = 0.35

        [Header(Noise C  Variacion alpha fragment)]
        _NoiseScaleC    ("Scale C  (world units)",           Float)         = 5.0
        _NoiseSpeedCX   ("Speed C  X",                       Float)         = 0.06
        _NoiseSpeedCZ   ("Speed C  Z",                       Float)         = 0.03
        _NoiseCStrength ("Noise C Strength",                 Range(0,1))    = 0.35

        [Header(Depth Intersection)]
        _DepthFade      ("Depth Fade Distance",              Float)         = 0.8
    }

    SubShader
    {
        Tags
        {
            "RenderType"      = "Transparent"
            "Queue"           = "Transparent+1"
            "RenderPipeline"  = "UniversalPipeline"
            "IgnoreProjector" = "True"
        }

        // ── Forward (único pass necesario, unlit transparent) ─────────────────
        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back           // invisible desde abajo — cambiar a "Off" si se necesita

            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex   Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // Provee SampleSceneDepth() para la intersección suave
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float  _Opacity;
                float  _AlphaContrast;
                float  _NoiseScaleA;
                float  _NoiseSpeedAX;
                float  _NoiseSpeedAZ;
                float  _NoiseScaleB;
                float  _NoiseSpeedBX;
                float  _NoiseSpeedBZ;
                float  _NoiseMix;
                float  _DisplaceStr;
                float  _NoiseScaleC;
                float  _NoiseSpeedCX;
                float  _NoiseSpeedCZ;
                float  _NoiseCStrength;
                float  _DepthFade;
            CBUFFER_END

            // ════════════════════════════════════════════════════════════════════
            // VALUE NOISE — hash bilineal con smooth-step quintico
            // ════════════════════════════════════════════════════════════════════

            float Hash1(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            // Retorna [0, 1]. Interpolación quintica para evitar artefactos en
            // los bordes de celda que darían un look cuadrado.
            float ValueNoise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                // Quintic smooth-step: 6t^5 - 15t^4 + 10t^3
                float2 u = f * f * f * (f * (f * 6.0 - 15.0) + 10.0);

                float a = Hash1(i + float2(0, 0));
                float b = Hash1(i + float2(1, 0));
                float c = Hash1(i + float2(0, 1));
                float d = Hash1(i + float2(1, 1));

                return lerp(lerp(a, b, u.x),
                            lerp(c, d, u.x), u.y);
            }

            // ════════════════════════════════════════════════════════════════════
            // SCENE DEPTH — compatible con perspectiva y ortográfica
            // ════════════════════════════════════════════════════════════════════

            // LinearEyeDepth usa 1/(z*a+b) que es correcta para perspectiva,
            // pero en ortográfica el buffer ya es lineal y hay que remapearlo
            // directamente a [near, far] en world units.
            //
            // unity_OrthoParams.w == 1 → ortográfica
            // unity_OrthoParams.w == 0 → perspectiva
            //
            // _ProjectionParams: x=sign  y=near  z=far  w=1/far
            float SampleLinearSceneDepth(float2 screenUV)
            {
                float raw = SampleSceneDepth(screenUV);

                if (unity_OrthoParams.w > 0.5)
                {
                    // Ortográfica: el buffer almacena profundidad lineal.
                    // D3D usa Z invertido por defecto (UNITY_REVERSED_Z):
                    //   1 = near plane, 0 = far plane → hay que flipear.
                    #if UNITY_REVERSED_Z
                    raw = 1.0 - raw;
                    #endif
                    // Remap [0,1] a [near, far] en world units
                    return lerp(_ProjectionParams.y, _ProjectionParams.z, raw);
                }

                // Perspectiva: conversión estándar URP
                return LinearEyeDepth(raw, _ZBufferParams);
            }

            // ════════════════════════════════════════════════════════════════════
            // STRUCTS
            // ════════════════════════════════════════════════════════════════════

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS  : SV_POSITION;
                float2 worldXZ     : TEXCOORD0;  // XZ en world space (sin desplazar)
                float  cloudShape  : TEXCOORD1;  // shape del displacement → base del alpha
                float  eyeDepth    : TEXCOORD2;  // distancia lineal desde cámara (ortho + persp)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // ════════════════════════════════════════════════════════════════════
            // VERTEX
            // ════════════════════════════════════════════════════════════════════

            Varyings Vert(Attributes IN)
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                Varyings OUT = (Varyings)0;
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                float3 normalWS   = TransformObjectToWorldNormal(IN.normalOS);
                float2 wXZ        = positionWS.xz;

                // ── Noise A ───────────────────────────────────────────────────
                float2 uvA = wXZ / max(_NoiseScaleA, 0.01)
                           + _Time.y * float2(_NoiseSpeedAX, _NoiseSpeedAZ);
                float  nA  = ValueNoise(uvA);

                // ── Noise B ───────────────────────────────────────────────────
                float2 uvB = wXZ / max(_NoiseScaleB, 0.01)
                           + _Time.y * float2(_NoiseSpeedBX, _NoiseSpeedBZ);
                float  nB  = ValueNoise(uvB);

                // ── Combinar A + B ────────────────────────────────────────────
                float combined = lerp(nA, nB, _NoiseMix);   // [0, 1]

                // Remap a [-1, 1] y abs() para eliminar valles:
                //   Valores originalmente negativos (valles) se flipean a positivo,
                //   creando un perfil de "crestas dobles" con mayor frecuencia visual.
                float shaped = abs(combined * 2.0 - 1.0);   // [0, 1], sin valles

                // ── Displacement along normal ─────────────────────────────────
                positionWS += normalWS * shaped * _DisplaceStr;

                OUT.positionCS = TransformWorldToHClip(positionWS);
                OUT.worldXZ    = wXZ;
                OUT.cloudShape = shaped;
                // Eye depth: -Z en view space, válido para perspectiva y ortográfica.
                // positionCS.w = 1 en ortográfica, por eso no se puede usar en Frag.
                OUT.eyeDepth   = -TransformWorldToView(positionWS).z;
                return OUT;
            }

            // ════════════════════════════════════════════════════════════════════
            // FRAGMENT
            // ════════════════════════════════════════════════════════════════════

            half4 Frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

                // ── Noise C — variación de alpha en fragment ──────────────────
                // Escala y speed distintos a A/B para romper la repetición visual.
                float2 uvC = IN.worldXZ / max(_NoiseScaleC, 0.01)
                           + _Time.y * float2(_NoiseSpeedCX, _NoiseSpeedCZ);
                float  nC  = ValueNoise(uvC);

                // Mezcla el cloud shape del vertex con la variación del fragment:
                //   _NoiseCStrength = 0 → solo el shape de vértices
                //   _NoiseCStrength = 1 → variación pura de fragment
                float cloudAlpha = IN.cloudShape * (1.0 - _NoiseCStrength)
                                 + nC             *  _NoiseCStrength;

                // Contrast sobre el alpha:
                //   < 1 → alpha más alto en todas partes (nube densa)
                //   > 1 → el alpha cae rápido hacia los bordes (nube wispy)
                cloudAlpha = pow(saturate(cloudAlpha), _AlphaContrast);

                // ── Depth intersection — fade suave al intersectar objetos ────
                float2 screenUV   = IN.positionCS.xy / _ScaledScreenParams.xy;
                // SampleLinearSceneDepth maneja perspectiva y ortográfica.
                float  sceneDepth = SampleLinearSceneDepth(screenUV);
                // eyeDepth viene del vertex: -TransformWorldToView(pos).z
                // Mismo espacio que sceneDepth (world units desde la cámara).
                float  fragDepth  = IN.eyeDepth;
                float  depthDiff  = sceneDepth - fragDepth;
                // 0 en el borde de intersección → 1 al alejarse _DepthFade unidades
                float  depthFade  = saturate(depthDiff / max(_DepthFade, 0.001));

                float finalAlpha = cloudAlpha * _Opacity * depthFade;

                return half4(_Color.rgb, finalAlpha);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
