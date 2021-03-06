﻿// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-27 10:15:58
// ========================================================
using LF.Res;
using UnityEngine;

public class ResTest : MonoBehaviour
{
    public ResManager m_ResManager;

    LoadAssetCallbacks m_LoadAssetCallbacks;

    object cacheObj;

    // Start is called before the first frame update
    void Start()
    {
        m_ResManager.SetResLoader(new AssetBundleLoader());

        m_LoadAssetCallbacks = new LoadAssetCallbacks((string assetName, object asset, float duration, object userData) =>
        {
            cacheObj = asset;
            Debug.Log(asset);
        });
        m_ResManager.LoadAsset("Assets/Game/Res/Sounds/Music/bgm2.mp3", m_LoadAssetCallbacks);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_ResManager.LoadAsset("Assets/Game/Res/Sounds/Music/bgm2.mp3", m_LoadAssetCallbacks);
        }

        if (Input.GetMouseButtonDown(1))
        {
            m_ResManager.UnloadAsset(cacheObj);
        }
    }
}
