using UnityEngine;

namespace Pincushion
{
    [ExecuteInEditMode]
    public class PincushionRenderer : MonoBehaviour
    {
        #region Editable Properties

        [SerializeField] float _radius = 5.0f;
        [SerializeField] float _scale = 1.0f;
        [SerializeField, Range(0, 1)] float _randomness = 0.1f;

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

        [Space]
        [SerializeField] int _randomSeed;

        [SerializeField, HideInInspector] Shader _surfaceShader;
        [SerializeField, HideInInspector] Shader _lineShader;

        #endregion

        #region Public Properties

        public float radius {
            get { return _radius; }
            set { _radius = value; }
        }

        public float scale {
            get { return _scale; }
            set { _scale = value; }
        }

        public float randomness {
            get { return _randomness; }
            set { _randomness = value; }
        }

        public float noiseAmplitude {
            get { return _noiseAmplitude; }
            set { _noiseAmplitude = value; }
        }

        public float noiseFrequency {
            get { return _noiseFrequency; }
            set { _noiseFrequency = value; }
        }

        public float noiseMotion {
            get { return _noiseMotion; }
            set { _noiseMotion = value; }
        }

        public Color color {
            get { return _color; }
            set { _color = value; }
        }

        public Color lineColor {
            get { return _lineColor; }
            set { _lineColor = value; }
        }

        #endregion

        #region Internal Objects and Variables

        Material _surfaceMaterial;
        Material _lineMaterial;
        MaterialPropertyBlock _materialProps;
        Vector3 _noiseOffset;

        Vector3 RandomNoiseOffset {
            get { return Vector3.one * (_randomSeed * 11.1f); }
        }

        #endregion

        #region MonoBehaviour Functions

        void OnEnable()
        {
            // delay initialization when the assets are not ready yet
            if (_surfaceShader == null) return;

            _surfaceMaterial = new Material(_surfaceShader);
            _surfaceMaterial.hideFlags = HideFlags.DontSave;

            _lineMaterial = new Material(_lineShader);
            _lineMaterial.hideFlags = HideFlags.DontSave;

            if (_materialProps == null)
                _materialProps = new MaterialPropertyBlock();
        }

        void OnDisable()
        {
            DestroyImmediate(_surfaceMaterial);
            DestroyImmediate(_lineMaterial);
            _materialProps = null;
        }

        void Update()
        {
            if (_surfaceMaterial == null) OnEnable(); // delayed initialization

            _noiseOffset += Vector3.forward * (_noiseMotion * Time.deltaTime);

            _surfaceMaterial
                .Property("_Color", _color)
                .Property("_MainTex", _albedoTexture)
                .Property("_TexScale", _textureScale)
                .Property("_Metallic", _metallic)
                .Property("_Glossiness", _smoothness)
                .Property("_NormalTex", _normalTexture)
                .Property("_NormalScale", _normalScale);

            _lineMaterial.Property("_Color", _lineColor);

            _materialProps
                .Property("_ScaleParams", _radius, _scale)
                .Property("_RandomParams", _randomness, _randomSeed)
                .Property("_NoiseParams", _noiseAmplitude, _noiseFrequency)
                .Property("_NoiseOffs", _noiseOffset + RandomNoiseOffset);

            Graphics.DrawMesh(
                _mesh.sharedMesh, transform.localToWorldMatrix,
                _surfaceMaterial, 0, null, 0, _materialProps);

            Graphics.DrawMesh(
                _mesh.sharedMesh, transform.localToWorldMatrix,
                _lineMaterial, 0, null, 1, _materialProps);
        }

        #endregion
    }
}
