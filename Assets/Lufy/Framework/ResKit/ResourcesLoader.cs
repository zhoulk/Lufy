// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-26 17:23:45
// ========================================================
using System;
using UnityEngine;

namespace LF.Res
{
    public class ResourcesLoader : IResLoader
    {
        public void Init()
        {

        }

        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks, object userData)
        {
            ResourceRequest resourceRequest = null;
            if (assetType == null)
            {
                resourceRequest = Resources.LoadAsync(assetName);
            }
            else
            {
                resourceRequest = Resources.LoadAsync(assetName, assetType);
            }
            resourceRequest.completed += operation =>
            {
                if (loadAssetCallbacks.LoadAssetSuccessCallback != null)
                {
                    loadAssetCallbacks.LoadAssetSuccessCallback(assetName, resourceRequest.asset, 0, null);
                }
            };
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            
        }

        public void UnLoadAsset(object asset)
        {
            
        }
    }
}

