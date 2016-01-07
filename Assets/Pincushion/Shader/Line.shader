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
            float dummy;
        };

        half4 _Color;

        void vert(inout appdata_full v)
        {
            float2 uv = v.texcoord1.xy;
            float3 vp = v.vertex.xyz;
            float4 rot = random_rotation(uv);

            vp = rotate_vector(base_transform(vp, uv), rot) + displacement(uv);

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
