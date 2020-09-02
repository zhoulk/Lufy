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
        private ResourceManager m_ResourceManager = null;

        public void Init()
        {
            m_ResourceManager = new ResourceManager();
        }

        public void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            Log.Debug("Load asset {0}", assetName);
            //uint crc = Crc32.GetCrc32(assetName);
            //ResouceItem resouceItem = m_AssetBundleManager.LoadResouceAssetBundle(crc);
            //AssetBundleRequest bundleRequest = null;
            //if(assetType != null)
            //{
            //    bundleRequest = resouceItem.m_AssetBundle.LoadAssetAsync(assetName, assetType);
            //}
            //else
            //{
            //    bundleRequest = resouceItem.m_AssetBundle.LoadAssetAsync(assetName);
            //}
            //bundleRequest.completed += (op) =>
            //{
            //    if (loadAssetCallbacks.LoadAssetSuccessCallback != null)
            //    {
            //        loadAssetCallbacks.LoadAssetSuccessCallback(assetName, bundleRequest.asset, 0, null);
            //    }
            //};
            UnityEngine.Object obj = m_ResourceManager.LoadResource<UnityEngine.Object>(assetName);
            if (loadAssetCallbacks.LoadAssetSuccessCallback != null)
            {
                loadAssetCallbacks.LoadAssetSuccessCallback(assetName, obj, 0, userData);
            }
        }

        public void UnLoadAsset(object asset)
        {
            
        }
    }
}

