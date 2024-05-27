using System;
using GLTFast;
using UnityEngine;

public class GltfLoader : MonoBehaviour
{
    private GltfAsset _gltf;
    private GameObject _sceneGo;

    public void Awake()
    {
        var path = Application.streamingAssetsPath + "/3dModel/MaquetteBEScorpion.glb";
        //LoadGltf(new Uri(path));
    }

    private async void LoadGltf(Uri url)
    {
        var gltf = new GltfImport();

        var settings = new ImportSettings
        {
            GenerateMipMaps = true,
            AnisotropicFilterLevel = 3,
            NodeNameMethod = NameImportMethod.OriginalUnique
        };

        var success = await gltf.Load(url.AbsoluteUri, settings);

        if (success)
        {
            if (_gltf != null) _gltf.ClearScenes();
            if (_sceneGo != null) DestroyImmediate(_sceneGo);

            _sceneGo = new GameObject("glTF");
            await gltf.InstantiateMainSceneAsync(_sceneGo.transform);
        }
    }
}