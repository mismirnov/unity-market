using System;
using System.Collections;
using Item;
using Siccity.GLTFUtility;
using UnityEngine;
using UnityEngine.Networking;

public class UserDataManager : MonoBehaviour
{
    private readonly string[] _userModels =
    {
        "https://assets.objkt.media/file/assets-003/Qmd3A5LayPoyhoTyAeQkPForCPZsqQtnzHNTGayXRNb28K/artifact",
        "https://assets.objkt.media/file/assets-003/QmZkKyMzwMvM9YrRfWhuAJnaQPD5uHww1SLUwfkL84Hua3/artifact",
        "https://assets.objkt.media/file/assets-003/QmPUmoTb2LRNPhgGHEWDnXVR49tmQANH8woaR22k5dQ6Yv/artifact",
        "https://assets.objkt.media/file/assets-003/QmNMUJ7unj3W42Nj5TAgR99m1NMx19rGAMNVKSp7MtiS4C/artifact",
        "https://assets.objkt.media/file/assets-003/QmcaVeBdMo2mVdi1P2PPY2xDFSQbM7w7Aoo9VqgBuvWTE1/artifact",
        // // "https://assets.objkt.media/file/assets-003/QmQU1zD2Z3yPGeFfaZA9kUQXQ41png8ZS5WR6szAyS7cm1/artifact",
        "https://assets.objkt.media/file/assets-003/QmemFfu9U9aGBAb2UMfGJhUVXqCL5DmKMs87a2RNKLHunN/artifact",
        "https://assets.objkt.media/file/assets-003/QmVNofJa6GjaoW7BGD1Vsy3ujYeEUVeVcAaW2CmyNCfhdw/artifact",
        // // "https://assets.objkt.media/file/assets-003/QmbtByc2D4W9VVXMutVYya1hTVwRwAdEPUP8WgDrGVjfWL/artifact", 
        // // //"https://assets.objkt.media/file/assets-003/QmT6vWb53dz93B3Gg65UH3AwjkU87GbuNvVX78gNcMhyLC/artifact",
        // // "https://assets.objkt.media/file/assets-003/QmeakFtQXSve3dT2qXMboPmqENEHJ93cQU1awNqdVkoXL5/artifact",
        // // "https://assets.objkt.media/file/assets-003/QmSMa55rf6gCTtrd4VAyS9QqvsqL8bPd3J6NgBBBbCeYGz/artifact",
        // // "https://assets.objkt.media/file/assets-003/QmZ5QkR2paZtLpS9K4GvFb9dvVv2iDC2QhsEMNhYzSgPEx/artifact",
        // // "https://assets.objkt.media/file/assets-003/QmZueUZre3H3fH5PTamebTgQpFjeoVFUk3pafsPaEQdXbS/artifact/index.html",
        "https://assets.objkt.media/file/assets-003/QmVdsiNo1geanFDeDodknMKkXCBpPGUcVyLH5GTWqJPL6G/artifact",
        "https://assets.objkt.media/file/assets-003/QmfXDcK11JavqDLdnRwKJgtwk3DWvAZd2VgSabT95JzRwU/artifact",
        "https://assets.objkt.media/file/assets-003/QmRYsvwDbQApetmdGsezXGRfcYjqkHVCwCJPXdcNLxu3mq/artifact",
        "https://assets.objkt.media/file/assets-003/QmQ9TDpXtYZcnx1KnGUz3fDJQQLRSa5d2N91ns8QH5Z2fR/artifact"
    };

    private int _itemPositionIndex;
    private int _itemsMaxCount;
    
    void Start()
    {
        var itemPlaces = GameObject.Find("Barrels");
        _itemsMaxCount = itemPlaces != null
            ? GameObject.Find("Barrels").transform.childCount
            : 0;
        
        StartCoroutine(LoadUserModels(ModelLoaded));
    }

    void ModelLoaded(byte[] bytes)
    {
        _itemPositionIndex++;
        var model = Importer.LoadFromBytes(bytes);
        var parent = GameObject.Find($"barrel ({_itemPositionIndex})");

        var item = parent.transform.Find("Item");
        parent.TryGetComponent<Renderer>(out var parentSize);
        var parentLocalBounds = parentSize.bounds;
        var maxParentSize = Math.Max(parentLocalBounds.size.x, Math.Max(parentLocalBounds.size.y, parentLocalBounds.size.z));
        const float scaleRatio = 1/2f;

        model.transform.parent = item.transform;
        model.transform.position = item.transform.position;

        parent.TryGetComponent<ItemSelection>(out var script);
        if (script != null)
            script.InitModel(model);

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

            model.transform.localScale = new Vector3(
                maxParentSize * scaleRatio / boundsSize,
                maxParentSize * scaleRatio / boundsSize,
                maxParentSize * scaleRatio / boundsSize);

            foreach (Transform child in model.transform)
            {
                child.position = new Vector3(
                    model.transform.position.x,
                    model.transform.position.y,
                    model.transform.position.z);
            }
            
            parent.GetComponentInChildren<Light>().enabled = true;
        }
    }

    IEnumerator LoadUserModels(Action<byte[]> cb)
    {
        foreach (var url in _userModels)
        {
            if (_itemPositionIndex >= _itemsMaxCount) break;
            
            var www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            cb.Invoke(www.isDone ? www.downloadHandler.data : Array.Empty<byte>());   
        }
    }
}
