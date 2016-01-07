using UnityEngine;

namespace Pincushion
{
    static class MaterialSetterExtension
    {
        static public Material Property(this Material m, string name, float x)
        {
            m.SetFloat(name, x);
            return m;
        }

        static public Material Property(this Material m, string name, float x, float y)
        {
            m.SetVector(name, new Vector2(x, y));
            return m;
        }

        static public Material Property(this Material m, string name, float x, float y, float z)
        {
            m.SetVector(name, new Vector3(x, y, z));
            return m;
        }

        static public Material Property(this Material m, string name, float x, float y, float z, float w)
        {
            m.SetVector(name, new Vector4(x, y, z, w));
            return m;
        }

        static public Material Property(this Material m, string name, Vector2 v)
        {
            m.SetVector(name, v);
            return m;
        }

        static public Material Property(this Material m, string name, Vector3 v)
        {
            m.SetVector(name, v);
            return m;
        }

        static public Material Property(this Material m, string name, Vector4 v)
        {
            m.SetVector(name, v);
            return m;
        }

        static public Material Property(this Material m, string name, Color color)
        {
            m.SetColor(name, color);
            return m;
        }

        static public Material Property(this Material m, string name, Texture texture)
        {
            m.SetTexture(name, texture);
            return m;
        }
    }

    static class MaterialPropertySetterExtension
    {
        static public MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, float x)
        {
            m.SetFloat(name, x);
            return m;
        }

        static public MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, float x, float y)
        {
            m.SetVector(name, new Vector2(x, y));
            return m;
        }

        static public MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, float x, float y, float z)
        {
            m.SetVector(name, new Vector3(x, y, z));
            return m;
        }

        static public MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, float x, float y, float z, float w)
        {
            m.SetVector(name, new Vector4(x, y, z, w));
            return m;
        }

        static public MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, Vector2 v)
        {
            m.SetVector(name, v);
            return m;
        }

        static public MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, Vector3 v)
        {
            m.SetVector(name, v);
            return m;
        }

        static public MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, Vector4 v)
        {
            m.SetVector(name, v);
            return m;
        }

        static public MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, Color color)
        {
            m.SetColor(name, color);
            return m;
        }

        static public MaterialPropertyBlock Property(this MaterialPropertyBlock m, string name, Texture texture)
        {
            m.SetTexture(name, texture);
            return m;
        }
    }
}
