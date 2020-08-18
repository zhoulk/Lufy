﻿// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-18 15:03:29
// ========================================================
using LF.Event;

namespace LF.Scene
{
    /// <summary>
    /// 加载场景成功事件
    /// </summary>
    public class LoadSceneSuccessEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadSceneSuccessEventArgs).GetHashCode();

        public override int Id => EventId;

        /// <summary>
        /// 初始化加载场景成功事件的新实例。
        /// </summary>
        public LoadSceneSuccessEventArgs()
        {
            SceneAssetName = null;
            Duration = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取场景资源名称。
        /// </summary>
        public string SceneAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载持续时间。
        /// </summary>
        public float Duration
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建加载场景成功事件。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="duration">加载持续时间。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载场景成功事件。</returns>
        public static LoadSceneSuccessEventArgs Create(string sceneAssetName, float duration, object userData)
        {
            LoadSceneSuccessEventArgs loadSceneSuccessEventArgs = ReferencePool.Acquire<LoadSceneSuccessEventArgs>();
            loadSceneSuccessEventArgs.SceneAssetName = sceneAssetName;
            loadSceneSuccessEventArgs.Duration = duration;
            loadSceneSuccessEventArgs.UserData = userData;
            return loadSceneSuccessEventArgs;
        }

        /// <summary>
        /// 清理加载场景成功事件。
        /// </summary>
        public override void Clear()
        {
            SceneAssetName = null;
            Duration = 0f;
            UserData = null;
        }
    }
}

