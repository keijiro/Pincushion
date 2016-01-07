Shader "Hidden/Pincushion/Surface"
{
    Properties
    {
        _Color ("-", Color) = (1,1,1,1)
        _MainTex ("-", 2D) = "white"{}
        _NormalTex ("-", 2D) = "bump"{}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Standard vertex:vert nolightmap addshadow
        #pragma target 3.0

        #include "Common.cginc"

        struct Input {
            float2 uv_MainTex;
        };

        half4 _Color;
        half _Glossiness;
        half _Metallic;

        sampler2D _MainTex;
        half _TexScale;

        sampler2D _NormalTex;
        half _NormalScale;

        void vert(inout appdata_full v)
        {
            float2 uv = v.texcoord1.xy;
            float3 vp = v.vertex.xyz;
            float4 rot = random_rotation(uv);

            vp = rotate_vector(base_transform(vp, uv), rot) + displacement(uv);

            v.vertex.xyz = vp;
            v.normal = rotate_vector(v.normal, rot);
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            half4 tex_c = tex2D(_MainTex, IN.uv_MainTex);
            half4 tex_n = tex2D(_NormalTex, IN.uv_MainTex);
            o.Albedo = _Color.rgb * tex_c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Normal = UnpackScaleNormal(tex_n, _NormalScale);
        }

        ENDCG
    }
    FallBack "Diffuse"
}
