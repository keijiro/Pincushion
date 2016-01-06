using UnityEngine;
using UnityEditor;
using System.IO;

namespace Pincushion
{
    [CustomEditor(typeof(PincushionMesh))]
    public class PincushionMeshEditor : Editor
    {
        SerializedProperty _sourceMesh;
        SerializedProperty _pinCount;

        void OnEnable()
        {
            _sourceMesh = serializedObject.FindProperty("_sourceMesh");
            _pinCount = serializedObject.FindProperty("_pinCount");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_sourceMesh);
            EditorGUILayout.PropertyField(_pinCount);
            var rebuild = EditorGUI.EndChangeCheck();

            serializedObject.ApplyModifiedProperties();

            if (rebuild)
                foreach (var t in targets)
                    ((PincushionMesh)t).RebuildMesh();
        }

        [MenuItem("Assets/Create/PincushionMesh")]
        public static void CreatePincushionMeshAsset()
        {
            // Make a proper path from the current selection.
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
                path = "Assets";
            else if (Path.GetExtension(path) != "")
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            var assetPathName = AssetDatabase.GenerateUniqueAssetPath(path + "/PincushionMesh.asset");

            // Create an asset.
            var asset = ScriptableObject.CreateInstance<PincushionMesh>();
            AssetDatabase.CreateAsset(asset, assetPathName);
            AssetDatabase.AddObjectToAsset(asset.sharedMesh, asset);

            // Build an initial mesh for the asset.
            asset.RebuildMesh();

            // Save the generated mesh asset.
            AssetDatabase.SaveAssets();

            // Tweak the selection.
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}
