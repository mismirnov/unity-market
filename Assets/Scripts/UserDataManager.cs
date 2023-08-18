using System;
using System.Collections;
using Siccity.GLTFUtility;
using UnityEngine;
using UnityEngine.Networking;

public class UserDataManager : MonoBehaviour
{
    // Start is called before the first frame update

    private readonly string[] _userModels =
    {
        "https://assets.objkt.media/file/assets-003/Qmd3A5LayPoyhoTyAeQkPForCPZsqQtnzHNTGayXRNb28K/artifact",
        "https://assets.objkt.media/file/assets-003/QmZkKyMzwMvM9YrRfWhuAJnaQPD5uHww1SLUwfkL84Hua3/artifact",
        "https://assets.objkt.media/file/assets-003/QmPUmoTb2LRNPhgGHEWDnXVR49tmQANH8woaR22k5dQ6Yv/artifact",
        "https://assets.objkt.media/file/assets-003/QmNMUJ7unj3W42Nj5TAgR99m1NMx19rGAMNVKSp7MtiS4C/artifact",
        "https://assets.objkt.media/file/assets-003/QmcaVeBdMo2mVdi1P2PPY2xDFSQbM7w7Aoo9VqgBuvWTE1/artifact",
        "https://assets.objkt.media/file/assets-003/QmQU1zD2Z3yPGeFfaZA9kUQXQ41png8ZS5WR6szAyS7cm1/artifact",
        "https://assets.objkt.media/file/assets-003/QmemFfu9U9aGBAb2UMfGJhUVXqCL5DmKMs87a2RNKLHunN/artifact",
        "https://assets.objkt.media/file/assets-003/QmVNofJa6GjaoW7BGD1Vsy3ujYeEUVeVcAaW2CmyNCfhdw/artifact"
        // "https://assets.objkt.media/file/assets-003/QmbtByc2D4W9VVXMutVYya1hTVwRwAdEPUP8WgDrGVjfWL/artifact"
    };

    private int _itemPositionIndex;
    private const int ItemsMaxCount = 20;
    
    void Start()
    {
        StartCoroutine(LoadUserModels(ModelLoaded));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void ModelLoaded(byte[] bytes)
    {
        _itemPositionIndex++;
        var model = Importer.LoadFromBytes(bytes);
        var parent = GameObject.Find($"barrel ({_itemPositionIndex})");

        model.transform.parent = parent.transform;
        model.transform.position = parent.transform.position + new Vector3(0, 1.5f, 0);

        float boundsSize = 0;
        var meshes = model.GetComponentsInChildren<Renderer>();

        if (meshes.Length == 0)
        {
            Destroy(model);
            _itemPositionIndex--;
        }
        else
        {
            var modelCamera = model.GetComponentInChildren<Camera>();
            if (modelCamera != null)
                modelCamera.enabled = false;
            
            foreach (var mesh in meshes)
            {
                var size = mesh.bounds.size;
                var maxSize = Math.Max(size.x, Math.Max(size.y, size.z));
            
                if (boundsSize >= maxSize) continue;

                boundsSize = maxSize;
            }

            var scaleRatio = boundsSize switch
            {
                > 1000 => 0.0001f,
                > 200 => 0.002f,
                > 100 => 0.001f,
                > 50 => 0.05f,
                > 10 => 0.01f,
                > 1 => 0.1f,
                _ => 1f
            };
        
            model.transform.localScale = new Vector3(scaleRatio, scaleRatio, scaleRatio);
            parent.GetComponentInChildren<Light>().enabled = true;
        }
    }

    IEnumerator LoadUserModels(Action<byte[]> cb)
    {
        foreach (var url in _userModels)
        {
            if (_itemPositionIndex >= ItemsMaxCount) break;
            
            var www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            cb.Invoke(www.isDone ? www.downloadHandler.data : Array.Empty<byte>());   
        }
    }
}
