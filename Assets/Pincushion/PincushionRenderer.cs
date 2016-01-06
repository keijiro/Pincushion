using UnityEngine;

namespace Pincushion
{
    [ExecuteInEditMode]
    public class PincushionRenderer : MonoBehaviour
    {
        #region Exposed Properties

        [SerializeField] float _radius = 5;
        [SerializeField] float _scale = 0.05f;

        [Space]
        [SerializeField] float _noiseAmplitude = 0.2f;
        [SerializeField] float _noiseFrequency = 1.0f;
        [SerializeField] float _noiseMotion = 0.3f;

        [Space]
        [SerializeField] PincushionMesh _mesh;

        [Space]
        [SerializeField] Color _color = Color.white;
        [SerializeField, Range(0, 1)] float _metallic = 0.5f;
        [SerializeField, Range(0, 1)] float _smoothness = 0.5f;

        [Space]
        [SerializeField] Texture2D _albedoTexture;
        [SerializeField] float _textureScale = 1.0f;

        [Space]
        [SerializeField] Texture2D _normalTexture;
        [SerializeField, Range(0, 2)] float _normalScale = 1.0f;

        [Space]
        [SerializeField] Color _lineColor = Color.white;

        [SerializeField, HideInInspector] Shader _surfaceShader;
        [SerializeField, HideInInspector] Shader _lineShader;

        #endregion

        #region Internal Objects and Variables

        Material _surfaceMaterial;
        Material _lineMaterial;

        MaterialPropertyBlock _materialProps;
        Vector3 _noiseOffset;

        #endregion

        #region MonoBehaviour Functions

        void Start()
        {
            _noiseOffset = Random.insideUnitSphere * Random.Range(-100.0f, 100.0f);
        }

        void OnDestroy()
        {
            if (_surfaceMaterial) DestroyImmediate(_surfaceMaterial);
            if (_lineMaterial) DestroyImmediate(_lineMaterial);
        }

        void Update()
        {
            _noiseOffset += Vector3.forward * (_noiseMotion * Time.deltaTime);

            if (_surfaceMaterial == null)
            {
                _surfaceMaterial = new Material(_surfaceShader);
                _surfaceMaterial.hideFlags = HideFlags.DontSave;
            }

            if (_lineMaterial == null)
            {
                _lineMaterial = new Material(_lineShader);
                _lineMaterial.hideFlags = HideFlags.DontSave;
            }

            if (_materialProps == null)
                _materialProps = new MaterialPropertyBlock();

            _surfaceMaterial.SetColor("_Color", _color);
            _surfaceMaterial.SetTexture("_MainTex", _albedoTexture);
            _surfaceMaterial.SetFloat("_TexScale", _textureScale);
            _surfaceMaterial.SetFloat("_Metallic", _metallic);
            _surfaceMaterial.SetFloat("_Glossiness", _smoothness);
            _surfaceMaterial.SetTexture("_NormalTex", _normalTexture);
            _surfaceMaterial.SetFloat("_NormalScale", _normalScale);

            _lineMaterial.SetColor("_Color", _lineColor);

            _materialProps.SetFloat("_Radius", _radius);
            _materialProps.SetFloat("_Scale", _scale);
            _materialProps.SetFloat("_NoiseAmp", _noiseAmplitude);
            _materialProps.SetFloat("_NoiseFreq", _noiseFrequency);
            _materialProps.SetVector("_NoiseOffs", _noiseOffset);

            var mtx = transform.localToWorldMatrix;

            Graphics.DrawMesh(
                _mesh.sharedMesh, mtx,
                _surfaceMaterial, 0, null, 0, _materialProps);

            Graphics.DrawMesh(
                _mesh.sharedMesh, mtx,
                _lineMaterial, 0, null, 1, _materialProps);
        }

        #endregion
    }
}
