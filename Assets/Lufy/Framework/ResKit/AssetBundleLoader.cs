// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-31 15:24:32
// ========================================================
using LF.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace LF.Res
{
    public class AssetBundleLoader : IResLoader
    {
        private AssetBundleManager m_AssetBundleManager = null;

        public void Init()
        {
            m_AssetBundleManager = new AssetBundleManager();
            m_AssetBundleManager.LoadAssetBundleConfig();
        }

        public void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks)
        {
            uint crc = Crc32.GetCrc32(assetName);
            ResouceItem resouceItem = m_AssetBundleManager.LoadResouceAssetBundle(crc);
            AssetBundleRequest bundleRequest = null;
            if(assetType != null)
            {
                bundleRequest = resouceItem.m_AssetBundle.LoadAssetAsync(assetName, assetType);
            }
            else
            {
                bundleRequest = resouceItem.m_AssetBundle.LoadAssetAsync(assetName);
            }
            bundleRequest.completed += (op) =>
            {
                if (loadAssetCallbacks.LoadAssetSuccessCallback != null)
                {
                    loadAssetCallbacks.LoadAssetSuccessCallback(assetName, bundleRequest.asset, 0, null);
                }
            };
        }

        public void UnLoadAsset(object asset)
        {
            
        }
    }
}

