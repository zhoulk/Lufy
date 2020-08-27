// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-26 17:45:37
// ========================================================
using System;

namespace LF.Res
{
    public interface IResLoader
    {
        /// <summary>
        /// 异步加载资源。
        /// </summary>
        /// <param name="assetName">要加载资源的名称。</param>
        /// <param name="assetType">要加载资源的类型。</param>
        /// <param name="loadAssetCallbacks">加载资源回调函数集。</param>
        /// <param name="userData">用户自定义数据。</param>
        void LoadAsset(string assetName, Type assetType, LoadAssetCallbacks loadAssetCallbacks);
    }
}

