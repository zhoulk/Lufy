﻿// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-26 16:16:39
// ========================================================
using System;
using UnityEngine;

namespace LF.Res
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Lufy/Res")]
    public class ResManager : LufyManager, IResManager
    {
        private IResLoader m_ResLoader;

        public bool EditorResource = false;

        //public int MaxCacheCount = 500;

        public ResManager()
        {
            m_ResLoader = null;
        }

        public void SetResLoader(IResLoader resLoader)
        {
            m_ResLoader = resLoader;
            m_ResLoader.Init();
        }

        public void LoadAsset(string assetName, LoadAssetCallbacks loadAssetCallbacks, object userData = null)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new LufyException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new LufyException("Load asset callbacks is invalid.");
            }

            if (m_ResLoader == null)
            {
                throw new LufyException("ResLoader is invalid.");
            }

            m_ResLoader.LoadAsset(assetName, null, loadAssetCallbacks, userData);
        }

        public void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks, object userData = null)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new LufyException("Asset name is invalid.");
            }

            if (loadAssetCallbacks == null)
            {
                throw new LufyException("Load asset callbacks is invalid.");
            }

            if (m_ResLoader == null)
            {
                throw new LufyException("ResLoader is invalid.");
            }

            m_ResLoader.LoadAsset(assetName, assetType, loadAssetCallbacks, userData);
        }

        public void UnloadAsset(object asset)
        {
            if (m_ResLoader == null)
            {
                throw new LufyException("ResLoader is invalid.");
            }

            m_ResLoader.UnLoadAsset(asset);
        }

        internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (m_ResLoader == null)
            {
                throw new LufyException("ResLoader is invalid.");
            }

            m_ResLoader.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        internal override void Shutdown()
        {
            
        }

    }
}

