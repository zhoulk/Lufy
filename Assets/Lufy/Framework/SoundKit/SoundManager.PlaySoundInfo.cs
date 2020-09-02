// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-02 11:29:48
// ========================================================

namespace LF.Sound
{
    public sealed partial class SoundManager : LufyManager
    {
        public class PlaySoundInfo : IReference
        {
            public PlaySoundParams PlaySoundParams
            {
                get;
                private set;
            }

            public bool IsShort
            {
                get;
                private set;
            }

            public static PlaySoundInfo Create(PlaySoundParams playSoundParams, bool isShort)
            {
                PlaySoundInfo playSoundInfo = ReferencePool.Acquire<PlaySoundInfo>();
                playSoundInfo.PlaySoundParams = playSoundParams;
                playSoundInfo.IsShort = isShort;
                return playSoundInfo;
            }

            public void Clear()
            {
                PlaySoundParams = null;
                IsShort = false;
            }
        }
    }
}

