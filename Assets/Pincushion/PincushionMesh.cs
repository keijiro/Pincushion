using UnityEngine;
using System.Collections.Generic;

namespace Pincushion
{
    public class PincushionMesh : ScriptableObject
    {
        #region Public Properties

        [SerializeField]
        Mesh _sourceMesh;

        [SerializeField]
        int _pinCount = 10;

        public int pinCount {
            get { return _pinCount; }
        }

        [SerializeField, HideInInspector]
        Mesh _mesh;

        public Mesh sharedMesh {
            get { return _mesh; }
        }

        #endregion

        #region Public Methods

        public void RebuildMesh()
        {
            if (_mesh == null)
            {
                Debug.LogError("Mesh asset is missing.");
                return;
            }

            _mesh.Clear();

            var vtx_src = _sourceMesh.vertices;
            var nrm_src = _sourceMesh.normals;
            var tan_src = _sourceMesh.tangents;
            var uv0_src = _sourceMesh.uv;

            var vcount = _pinCount * (vtx_src.Length + 2);
            var vtx_tmp = new List<Vector3>(vcount);
            var nrm_tmp = new List<Vector3>(vcount);
            var tan_tmp = new List<Vector4>(vcount);
            var uv0_tmp = new List<Vector2>(vcount);
            var uv1_tmp = new List<Vector2>(vcount);

            for (var i_pin = 0; i_pin < _pinCount; i_pin++)
            {
                var uv1 = new Vector2((float)i_pin / _pinCount, 0);

                for (var i_src = 0; i_src < vtx_src.Length; i_src++)
                {
                    vtx_tmp.Add(vtx_src[i_src]);
                    nrm_tmp.Add(nrm_src[i_src]);
                    tan_tmp.Add(tan_src[i_src]);
                    uv0_tmp.Add(uv0_src[i_src]);
                    uv1_tmp.Add(uv1);
                }

                // line
                vtx_tmp.Add(Vector3.zero);
                vtx_tmp.Add(Vector3.zero);

                nrm_tmp.Add(Vector3.zero);
                nrm_tmp.Add(Vector3.zero);

                tan_tmp.Add(Vector4.zero);
                tan_tmp.Add(Vector4.zero);

                uv0_tmp.Add(Vector2.zero);
                uv0_tmp.Add(Vector2.zero);

                uv1_tmp.Add(uv1);
                uv1_tmp.Add(new Vector2(uv1.x, 1));
            }

            var idx_src = _sourceMesh.GetIndices(0);
            var idx_tmp = new List<int>(idx_src.Length * _pinCount);

            for (var i_pin = 0; i_pin < _pinCount; i_pin++)
            {
                var i_0 = i_pin * (vtx_src.Length + 2);
                for (var i_src = 0; i_src < idx_src.Length; i_src++)
                    idx_tmp.Add(idx_src[i_src] + i_0);
            }

            var idx_tmp2 = new List<int>(_pinCount * 2);
            for (var i_pin = 0; i_pin < _pinCount; i_pin++)
            {
                var i_0 = (i_pin + 1) * (vtx_src.Length + 2) - 2;
                idx_tmp2.Add(i_0);
                idx_tmp2.Add(i_0 + 1);
            }

            _mesh.subMeshCount = 2;
            _mesh.SetVertices(vtx_tmp);
            _mesh.SetNormals (nrm_tmp);
            _mesh.SetTangents(tan_tmp);
            _mesh.SetUVs(0, uv0_tmp);
            _mesh.SetUVs(1, uv1_tmp);
            _mesh.SetIndices(idx_tmp.ToArray(), MeshTopology.Triangles, 0);
            _mesh.SetIndices(idx_tmp2.ToArray(), MeshTopology.Lines, 1);
            _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1000);

            _mesh.Optimize();
            _mesh.UploadMeshData(true);
        }

        #endregion

        #region ScriptableObject Functions

        void OnEnable()
        {
            if (_mesh == null)
            {
                _mesh = new Mesh();
                _mesh.name = "PinCushion";
            }
        }

        #endregion
    }
}
