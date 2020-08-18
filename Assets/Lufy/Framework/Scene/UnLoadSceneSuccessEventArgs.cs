// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-18 15:10:56
// ========================================================
using LF.Event;

namespace LF.Scene
{
    public class UnLoadSceneSuccessEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(UnLoadSceneSuccessEventArgs).GetHashCode();

        public override int Id => EventId;

        /// <summary>
        /// 初始化加载场景失败事件的新实例。
        /// </summary>
        public UnLoadSceneSuccessEventArgs()
        {
            SceneAssetName = null;
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
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
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
        /// 创建加载场景失败事件。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载场景失败事件。</returns>
        public static UnLoadSceneSuccessEventArgs Create(string sceneAssetName, object userData)
        {
            UnLoadSceneSuccessEventArgs loadSceneFailureEventArgs = ReferencePool.Acquire<UnLoadSceneSuccessEventArgs>();
            loadSceneFailureEventArgs.SceneAssetName = sceneAssetName;
            loadSceneFailureEventArgs.UserData = userData;
            return loadSceneFailureEventArgs;
        }

        /// <summary>
        /// 清理加载场景失败事件。
        /// </summary>
        public override void Clear()
        {
            SceneAssetName = null;
            UserData = null;
        }
    }
}

