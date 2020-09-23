// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-12 19:48:50
// ========================================================

namespace LF.Sound
{
    public sealed class PlaySoundParams
    {
        private bool m_Mute;
        private bool m_Loop;
        private float m_Volume;
        private float m_Interval;

        /// <summary>
        /// 获取或设置在声音组内是否静音。
        /// </summary>
        public bool Mute
        {
            get
            {
                return m_Mute;
            }
            set
            {
                m_Mute = value;
            }
        }

        /// <summary>
        /// 获取或设置是否循环播放。
        /// </summary>
        public bool Loop
        {
            get
            {
                return m_Loop;
            }
            set
            {
                m_Loop = value;
            }
        }

        /// <summary>
        /// 获取或设置在声音组内音量大小。
        /// </summary>
        public float Volume
        {
            get
            {
                return m_Volume;
            }
            set
            {
                m_Volume = value;
            }
        }

        /// <summary>
        /// 播放间隔
        /// </summary>
        public float Interval
        {
            get
            {
                return m_Interval;
            }
            set
            {
                m_Interval = value;
            }
        }

        /// <summary>
        /// 创建播放声音参数。
        /// </summary>
        /// <returns>创建的播放声音参数。</returns>
        public static PlaySoundParams Create()
        {
            PlaySoundParams playSoundParams = new PlaySoundParams();
            playSoundParams.Mute = false;
            playSoundParams.Loop = false;
            playSoundParams.Volume = 1;
            return playSoundParams;
        }
    }
}

