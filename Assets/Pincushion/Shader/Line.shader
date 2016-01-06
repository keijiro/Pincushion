Shader "Hidden/Pincushion/Line"
{
    Properties
    {
        _Color ("-", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Standard vertex:vert nolightmap
        #pragma target 3.0

        #include "Common.cginc"

        struct Input {
            float2 uv_MainTex;
        };

        half4 _Color;

        void vert(inout appdata_full v)
        {
            float2 uv = v.texcoord1.xy;

            float4 rot = random_rotation(float2(uv.x, 0));
            float l = _Radius * uv.y;

            float3 vp = v.vertex.xyz;
            vp = rotate_vector(float3(0, 0, l), rot);
            vp += get_displacement(float2(uv.x, 0)) * uv.y;

            v.vertex.xyz = vp;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Emission = _Color.rgb;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
