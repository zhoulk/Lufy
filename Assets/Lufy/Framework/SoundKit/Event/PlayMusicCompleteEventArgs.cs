// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-23 14:57:38
// ========================================================
using LF.Event;

namespace LF.Sound
{
    public class PlayMusicCompleteEventArgs : GameEventArgs
    {
        public override int Id => PlayMusicCompleteEventArgs.EventId;

        public static int EventId = typeof(PlayMusicCompleteEventArgs).GetHashCode();

        public PlayMusicCompleteEventArgs()
        {
            SoundAssetName = string.Empty;
            SoundLength = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取打开成功的界面。
        /// </summary>
        public string SoundAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载持续时间。
        /// </summary>
        public float SoundLength
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

        public static PlayMusicCompleteEventArgs Create(string assetName, float audioLength, object userData)
        {
            PlayMusicCompleteEventArgs playMusicCompleteEventArgs = ReferencePool.Acquire<PlayMusicCompleteEventArgs>();
            playMusicCompleteEventArgs.SoundAssetName = assetName;
            playMusicCompleteEventArgs.SoundLength = audioLength;
            playMusicCompleteEventArgs.UserData = userData;
            return playMusicCompleteEventArgs;
        }

        public override void Clear()
        {
            SoundAssetName = string.Empty;
            SoundLength = 0;
            UserData = null;
        }
    }
}

