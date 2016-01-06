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
            float2 uv = float2(v.texcoord1.x, 0);
            float sw = v.texcoord1.y;
            float4 rot = get_rotation(uv);

            float3 vp = v.vertex.xyz * _Scale;
            vp += float3(0, 0, _Radius * sw);
            vp = rotate_vector(vp, rot);
            vp += get_displacement(uv) * sw;

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
