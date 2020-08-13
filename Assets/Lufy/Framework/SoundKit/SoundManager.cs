// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-12 19:01:48
// ========================================================

using System.Collections.Generic;
using UnityEngine;

namespace LF.Sound
{
    /// <summary>
    /// 声音管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Lufy/Sound")]
    public sealed class SoundManager : LufyManager
    {
        private AudioSource m_MusicSource = null;
        private AudioSource m_SoundSource = null;

        private PlaySoundParams m_MusicSoundParams = null;
        private PlaySoundParams m_SoundSoundParams = null;

        private readonly Dictionary<string, AudioClip> m_AudioClips;

        public SoundManager()
        {
            m_AudioClips = new Dictionary<string, AudioClip>();
            m_MusicSoundParams = PlaySoundParams.Create();
            m_MusicSoundParams.Loop = true;
            m_SoundSoundParams = PlaySoundParams.Create();
        }

        public PlaySoundParams MusicParams
        {
            get
            {
                return m_MusicSoundParams;
            }
            set
            {
                m_MusicSoundParams = value;

                m_MusicSource.mute = m_MusicSoundParams.Mute;
                m_MusicSource.loop = m_MusicSoundParams.Loop;
                m_MusicSource.volume = m_MusicSoundParams.Volume;
            }
        }

        public PlaySoundParams SoundParams
        {
            get
            {
                return m_SoundSoundParams;
            }
            set
            {
                m_SoundSoundParams = value;

                m_SoundSource.mute = m_SoundSoundParams.Mute;
                m_SoundSource.loop = m_SoundSoundParams.Loop;
                m_SoundSource.volume = m_SoundSoundParams.Volume;
            }
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, bool isShort)
        {
            return PlaySound(soundAssetName, null, isShort);
        }

        /// <summary>
        /// 播放声音。
        /// </summary>
        /// <param name="soundAssetName">声音资源名称。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string soundAssetName, PlaySoundParams playSoundParams, bool isShort)
        {
            if (playSoundParams != null)
            {
                if (isShort)
                {
                    m_SoundSoundParams = playSoundParams;
                }
                else
                {
                    m_MusicSoundParams = playSoundParams;
                }
            }

            AudioClip clip = null;
            m_AudioClips.TryGetValue(soundAssetName, out clip);
            if (clip == null)
            {
                clip = Resources.Load<AudioClip>(soundAssetName);
                m_AudioClips.Add(soundAssetName, clip);
            }

            if (isShort)
            {
                m_SoundSource.mute = m_SoundSoundParams.Mute;
                m_SoundSource.loop = m_SoundSoundParams.Loop;
                m_SoundSource.volume = m_SoundSoundParams.Volume;
                m_SoundSource.PlayOneShot(clip);
            }
            else
            {
                m_MusicSource.clip = clip;
                m_MusicSource.mute = m_MusicSoundParams.Mute;
                m_MusicSource.loop = m_MusicSoundParams.Loop;
                m_MusicSource.volume = m_MusicSoundParams.Volume;
                m_MusicSource.Play();
            }

            return 0;
        }

        /// <summary>
        /// 停止播放声音。
        /// </summary>
        public void StopSound(bool isShort)
        {
            if (isShort)
            {
                m_SoundSource.Stop();
            }
            else
            {
                m_MusicSource.Stop();
            }
        }

        /// <summary>
        /// 暂停播放声音。
        /// </summary>
        public void PauseSound(bool isShort)
        {
            if (isShort)
            {
                m_SoundSource.Pause();
            }
            else
            {
                m_MusicSource.Pause();
            }
        }

        /// <summary>
        /// 恢复播放声音。
        /// </summary>
        public void ResumeSound(bool isShort)
        {
            if (isShort)
            {
                m_SoundSource.UnPause();
            }
            else
            {
                m_MusicSource.UnPause();
            }
        }

        public void Volume(float volume, bool isShort)
        {
            if (isShort)
            {
                m_SoundSoundParams.Volume = volume;
                m_SoundSource.volume = volume;
            }
            else
            {
                m_MusicSoundParams.Volume = volume;
                m_MusicSource.volume = volume;
            }
        }

        public void Mute(bool mute, bool isShort)
        {
            if (isShort)
            {
                m_SoundSoundParams.Mute = mute;
                m_SoundSource.mute = mute;
            }
            else
            {
                m_MusicSoundParams.Mute = mute;
                m_MusicSource.mute = mute;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if(m_MusicSource == null)
            {
                
                m_MusicSource = gameObject.AddComponent<AudioSource>();
                m_MusicSource.playOnAwake = false;
                m_MusicSource.rolloffMode = AudioRolloffMode.Custom;
            }

            if(m_SoundSource == null)
            {
                m_SoundSource = gameObject.AddComponent<AudioSource>();
                m_SoundSource.playOnAwake = false;
                m_SoundSource.rolloffMode = AudioRolloffMode.Custom;
            }
        }

        internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {

        }

        internal override void Shutdown()
        {
            m_AudioClips.Clear();
        }
    }
}

