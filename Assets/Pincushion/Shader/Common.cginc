#include "SimplexNoiseGrad3D.cginc"

// PRNG function
float nrand(float2 uv, float salt)
{
    uv += float2(salt, 0);
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}

// Quaternion multiplication
// http://mathworld.wolfram.com/Quaternion.html
float4 qmul(float4 q1, float4 q2)
{
    return float4(
        q2.xyz * q1.w + q1.xyz * q2.w + cross(q1.xyz, q2.xyz),
        q1.w * q2.w - dot(q1.xyz, q2.xyz)
    );
}

// Vector rotation with a quaternion
// http://mathworld.wolfram.com/Quaternion.html
float3 rotate_vector(float3 v, float4 r)
{
    float4 r_c = r * float4(-1, -1, -1, 1);
    return qmul(r, qmul(float4(v, 0), r_c)).xyz;
}

// Rotation around the Y axis.
float4 y_rotation(float r)
{
    return float4(0, sin(r * 0.5), 0, cos(r * 0.5));
}

// Uniform random unit quaternion
// http://www.realtimerendering.com/resources/GraphicsGems/gemsiii/urot.c
float4 random_rotation(float2 uv)
{
    float r = nrand(uv, 30);
    float r1 = sqrt(1.0 - r);
    float r2 = sqrt(r);
    float t1 = UNITY_PI * 2 * nrand(uv, 40);
    float t2 = UNITY_PI * 2 * nrand(uv, 50);
    return float4(sin(t1) * r1, cos(t1) * r1, sin(t2) * r2, cos(t2) * r2);
}

// Common uniforms
float _Radius;
float _Scale;
float _NoiseAmp;
float _NoiseFreq;
float3 _NoiseOffs;

float4 get_rotation(float2 uv)
{
    return random_rotation(uv);
}

float3 get_displacement(float2 uv)
{
    return snoise_grad(float3(uv, 0) * _NoiseFreq + _NoiseOffs) * _NoiseAmp;
}
