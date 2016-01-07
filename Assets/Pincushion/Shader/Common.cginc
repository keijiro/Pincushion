#include "SimplexNoiseGrad3D.cginc"

// Common uniforms
float2 _ScaleParams;  // radius, scale
float2 _RandomParams; // randomness, seed
float2 _NoiseParams;  // amplitude, frequency
float3 _NoiseOffs;

// PRNG function
float nrand(float x, float y)
{
    float2 uv = float2(x, y + _RandomParams.y);
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
float4 random_rotation(float id)
{
    float r = nrand(id, 0);
    float r1 = sqrt(1.0 - r);
    float r2 = sqrt(r);
    float t1 = UNITY_PI * 2 * nrand(id, 1);
    float t2 = UNITY_PI * 2 * nrand(id, 2);
    return float4(sin(t1) * r1, cos(t1) * r1, sin(t2) * r2, cos(t2) * r2);
}

float3 base_transform(float3 vp, float2 uv)
{
    // random factor
    float rnd = 1 - nrand(uv.x, 3) * _RandomParams.x;
    // scaling
    vp *= _ScaleParams.y * rnd;
    // move along the z axis
    vp.z += _ScaleParams.x * rnd * uv.y;
    return vp;
}

float3 displacement(float2 uv)
{
    float3 np = float3(uv, 0) * _NoiseParams.y;
    return snoise_grad(np + _NoiseOffs) * _NoiseParams.x * uv.y;
}
