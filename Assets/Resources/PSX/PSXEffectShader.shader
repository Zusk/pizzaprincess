Shader "Hidden/PSXEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // Main texture
        _pixels("Resolution", float) = 512 // Resolution for pixel effect
        _pw("Pixel Width", float) = 64 // Pixel width for pixel effect
        _ph("Pixel Height", float) = 64 // Pixel height for pixel effect
        _DitherTex ("Dither Texture", 2D) = "white" {} // Dithering texture
        _DitherIntensity ("Dither Intensity", Range(0,1)) = 0.5 // Intensity of dithering effect
        _VertexJitterIntensity ("Vertex Jitter Intensity", float) = 0.1 // Intensity of vertex jitter
        _VertexJitterScale ("Vertex Jitter Scale", float) = 1.0 // Scale of vertex jitter
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert // Specify vertex shader
            #pragma fragment frag // Specify fragment shader

            #include "UnityCG.cginc" // Include Unity's common shader functions

            // Global declarations
            float _pixels;
            float _pw;
            float _ph;
            sampler2D _MainTex; // Sampler for main texture
            sampler2D _DitherTex; // Sampler for dither texture
            float _DitherIntensity; // Dither intensity uniform
            float _VertexJitterIntensity; // Vertex jitter intensity
            float _VertexJitterScale; // Vertex jitter scale

            struct appdata
            {
                float4 vertex : POSITION; // Vertex position
                float2 uv : TEXCOORD0; // Texture coordinates
            };

            struct v2f
            {
                float2 uv : TEXCOORD0; // Texture coordinates
                float4 vertex : SV_POSITION; // Vertex position in screen space
            };

            v2f vert (appdata v)
            {
                v2f o;
                // Apply vertex jitter for PS1 style effect
                float3 noise = frac(sin(dot(v.vertex.xyz ,float3(12.9898,78.233,45.5432))) * 43758.5453);
                float3 jitter = noise * _VertexJitterIntensity - (_VertexJitterIntensity / 2.0);
                o.vertex = UnityObjectToClipPos(v.vertex + jitter * _VertexJitterScale);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Calculate pixelated UV coordinates
                float _dx = _pw * (1 / _pixels);
                float _dy = _ph * (1 / _pixels);                
                float2 coord = float2(_dx * floor(i.uv.x / _dx), _dy * floor(i.uv.y / _dy));

                // Sample texture at pixelated coordinates
                fixed4 col = tex2D(_MainTex, coord);

                // Sample the dither texture
                float ditherValue = tex2D(_DitherTex, i.uv).r;

                // Apply the dithering effect
                if (col.r < ditherValue * _DitherIntensity)
                    col.rgb *= (1.0 - _DitherIntensity);
                else
                    col.rgb *= (1.0 + _DitherIntensity);

                return col; // Return final color
            }
            ENDCG
        }
    }
}

