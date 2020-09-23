// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-23 10:37:55
// ========================================================
using LF.Event;

namespace LF.UI
{
    public class OpenUIFormFailureEventArgs : GameEventArgs
    {
        public override int Id => OpenUIFormFailureEventArgs.EventId;

        public static int EventId = typeof(OpenUIFormFailureEventArgs).GetHashCode();

        /// <summary>
        /// 初始化打开界面失败事件的新实例。
        /// </summary>
        public OpenUIFormFailureEventArgs()
        {
            UIFormAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }

        /// <summary>
        /// 获取界面资源名称。
        /// </summary>
        public string UIFormAssetName
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
        /// 创建打开界面失败事件。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的打开界面失败事件。</returns>
        public static OpenUIFormFailureEventArgs Create(string uiFormAssetName, string errorMessage, object userData)
        {
            OpenUIFormFailureEventArgs openUIFormFailureEventArgs = ReferencePool.Acquire<OpenUIFormFailureEventArgs>();
            openUIFormFailureEventArgs.UIFormAssetName = uiFormAssetName;
            openUIFormFailureEventArgs.ErrorMessage = errorMessage;
            openUIFormFailureEventArgs.UserData = userData;
            return openUIFormFailureEventArgs;
        }

        /// <summary>
        /// 清理打开界面失败事件。
        /// </summary>
        public override void Clear()
        {
            UIFormAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}

