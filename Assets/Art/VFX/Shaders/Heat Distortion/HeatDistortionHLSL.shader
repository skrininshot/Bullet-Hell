Shader "Unlit/Heat Distortion HLSL"
{
    Properties
    {
        [MainTexture]
        _MainTex ("Texture", 2D) = "white" {}
        
        [Header(Heat Distortion)]
        _DistortionSpeed ("Distortion", Range(0, 10)) = 0.5
        _DistortionStrength ("Strength", Range(0, 10)) = 0.5
        _RemapParameter ("Remap Parameter", Float) = 0.5
    }
    SubShader
    {
        Tags 
        {
            "RenderType"="Transparent"
            "Queue"="Transparent" 
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        
        HLSLINCLUDE

        #ifndef NODE_ROTATE
            #define NODE_ROTATE(uv, center, rotation, Out) node_rotate(uv, center, rotation, Out)
            void node_rotate(float2 uv, float2 center, float rotation, out float2 uv_out)
            {
                rotation = rotation * (3.1415926f/180.0f);
                uv -= center;
                float s = sin(rotation);
                float c = cos(rotation);
                float2x2 rMatrix = float2x2(c, -s, s, c);
                rMatrix *= 0.5;
                rMatrix += 0.5;
                rMatrix = rMatrix * 2 - 1;
                uv.xy = mul(uv.xy, rMatrix);
                uv += center;
                uv_out = uv;
            }
            #endif        

        #ifndef NODE_REMAP
            #define NODE_REMAP(in, inMinMax, outMinMax, out) node_remap(in, inMinMax, outMinMax, out)
            void node_remap(float In, float2 InMinMax, float2 OutMinMax, out float4 Out)
            {
                Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }
        #endif

        #ifndef NODE_VORONOI
            #define NODE_VORONOI(UV, AngleOffset, CellDensity, Out, Cells) node_voronoi(UV, AngleOffset, CellDensity, Out, Cells)
            inline float2 voronoi_noise_random_vector(float2 UV, float offset)
            {
                float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
                UV = frac(sin(mul(UV, m)) * 46839.32);
                return float2(sin(UV.y*+offset)*0.5+0.5, cos(UV.x*offset)*0.5+0.5);
            }
            
            void node_voronoi(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
            {
                float2 g = floor(UV * CellDensity);
                float2 f = frac(UV * CellDensity);
                float3 res = float3(8.0, 0.0, 0.0);
            
                for(int y=-1; y<=1; y++)
                {
                    for(int x=-1; x<=1; x++)
                    {
                        float2 lattice = float2(x,y);
                        float2 offset = voronoi_noise_random_vector(lattice + g, AngleOffset);
                        float d = distance(lattice + offset, f);
                        if(d < res.x)
                        {
                            res = float3(d, offset.x, offset.y);
                            Out = res.x;
                            Cells = res.y;
                        }
                    }
                }
            }
        #endif
        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            TEXTURE2D(_CameraColorTexture); SAMPLER(sampler_CameraColorTexture);
            
            CBUFFER_START(UnityPerMaterial)

            float4 _MainTex_ST;
            float _DistortionSpeed;
            float _DistortionStrength;
            float _RemapParameter;
            
            CBUFFER_END
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screen_pos : TEXCOORD1;
            };           

            float get_voronoi_value(const interpolators i, const float texture_value)
            {
                float voronoi; float cells;         
                const float2 oldMinMax = float2(0, 1);
                const float2 newMinMax = float2(0, 0.1);
                float4 distortion_str;
                
                NODE_REMAP(_DistortionStrength, oldMinMax, newMinMax, distortion_str);
                NODE_VORONOI(i.uv, _Time.y * _DistortionSpeed, 12, voronoi, cells);
                return voronoi * distortion_str.x * texture_value;
            }
            
            interpolators vert (appdata i)
            {
                interpolators o;
                o.vertex = TransformObjectToHClip(i.vertex.xyz);
                o.uv = TRANSFORM_TEX(i.uv, _MainTex);

                const VertexPositionInputs vertex_positions = GetVertexPositionInputs(i.vertex.xyz);
                o.screen_pos = vertex_positions.positionNDC;
                return o;
            }

            float4 frag (interpolators input) : SV_Target
            {
                float4 texture_value = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv).r;
                float2 left_rotated_UVs;
                float2 right_rotated_Uvs;

                NODE_ROTATE(input.uv, float2(0.5, 0.5), 90, left_rotated_UVs);
                NODE_ROTATE(input.uv, float2(0.5, 0.5), -90, right_rotated_Uvs);
                
                texture_value *= SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, left_rotated_UVs);
                texture_value *= SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, right_rotated_Uvs);
                
                NODE_REMAP(texture_value.x, float2(0,1), float2(0, _RemapParameter), texture_value);
                
                const float voronoi = get_voronoi_value(input, texture_value.x);

                float4 uv = input.screen_pos / input.screen_pos.w;
                uv += voronoi;
                
                float4 col = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, uv.xy);
                return col;
            }
            ENDHLSL
        }
    }
}
