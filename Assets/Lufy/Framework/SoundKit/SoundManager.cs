// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-12 19:01:48
// ========================================================

using LF.Event;
using LF.Res;
using LF.Timer;
using System.Collections.Generic;
using UnityEngine;

namespace LF.Sound
{
    /// <summary>
    /// 声音管理器
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Lufy/Sound")]
    public sealed partial class SoundManager : LufyManager
    {
        private AudioSource m_MusicSource = null;
        private AudioSource m_SoundSource = null;

        private PlaySoundParams m_MusicSoundParams = null;
        private PlaySoundParams m_SoundSoundParams = null;

        private readonly Dictionary<string, AudioClip> m_AudioClips;

        private IResManager m_ResManager = null;
        private LoadAssetCallbacks m_LoadAssetCallbacks;

        private EventManager m_EventManager = null;
        private TimerManager m_TimerManager = null;

        // 音乐播放时间计时
        private float m_MusicTimer = 0;
        private string m_MusicAssetName = string.Empty;

        public SoundManager()
        {
            m_AudioClips = new Dictionary<string, AudioClip>();
            m_MusicSoundParams = PlaySoundParams.Create();
            m_MusicSoundParams.Loop = true;
            m_SoundSoundParams = PlaySoundParams.Create();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadAssetSuccess, LoadAssetFail);
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

        public void SetResManager(IResManager resManager)
        {
            m_ResManager = resManager;
        }

        public void SetEventManager(EventManager eventManager)
        {
            m_EventManager = eventManager;
        }

        public void SetTimerManager(TimerManager timerManager)
        {
            m_TimerManager = timerManager;
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
            if (m_ResManager == null)
            {
                throw new LufyException("ResManager is invalid.");
            }

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
                m_ResManager.LoadAsset(soundAssetName, m_LoadAssetCallbacks, PlaySoundInfo.Create(playSoundParams, isShort));
            }
            else
            {
                internalPlaySound(soundAssetName, clip, playSoundParams, isShort);
            }
            return 0;
        }

        private void internalPlaySound(string assetName, AudioClip clip, PlaySoundParams playSoundParams, bool isShort)
        {
            if (isShort)
            {
                m_SoundSource.mute = m_SoundSoundParams.Mute;
                m_SoundSource.loop = m_SoundSoundParams.Loop;
                m_SoundSource.volume = m_SoundSoundParams.Volume;
                m_SoundSource.PlayOneShot(clip);
            }
            else
            {
                if (m_TimerManager)
                {
                    m_TimerManager.clearTimer(DelayPlayMusic);
                }

                m_MusicSource.clip = clip;
                m_MusicSource.mute = m_MusicSoundParams.Mute;
                if(m_MusicSoundParams.Interval > 0)
                {
                    m_MusicSource.loop = false;
                }
                else
                {
                    m_MusicSource.loop = m_MusicSoundParams.Loop;
                }
                m_MusicSource.volume = m_MusicSoundParams.Volume;
                m_MusicSource.Play();

                m_MusicTimer = 0;
                m_MusicAssetName = assetName;
            }
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
            if (m_MusicSource.clip != null)
            {
                if (m_MusicSource.isPlaying)
                {
                    m_MusicTimer += elapseSeconds;
                }

                bool complete = CheckMusicComplete();
                if (complete)
                {
                    if(m_MusicSoundParams.Loop)
                    {
                        if (m_TimerManager)
                        {
                            m_TimerManager.doOnce((int)(m_MusicSoundParams.Interval * 1000), DelayPlayMusic);
                        }
                        else
                        {
                            internalPlaySound(m_MusicAssetName, m_AudioClips[m_MusicAssetName], m_MusicSoundParams, false);
                        }
                    }
                }
            }
        }

        internal override void Shutdown()
        {
            m_AudioClips.Clear();
        }

        void DelayPlayMusic()
        {
            internalPlaySound(m_MusicAssetName, m_AudioClips[m_MusicAssetName], m_MusicSoundParams, false);
        }

        bool CheckMusicComplete()
        {
            bool complete = false;
            if (!m_MusicSource.isPlaying)
            {
                // 音乐播放完毕  计时判断 误差1秒内都算结束
                if (Mathf.Abs(m_MusicTimer - m_MusicSource.clip.length) < 1)
                {
                    m_MusicTimer = 0;
                    if (m_EventManager != null)
                    {
                        m_EventManager.Fire(this, PlayMusicCompleteEventArgs.Create(m_MusicAssetName, m_MusicSource.clip.length, null));
                    }

                    complete = true;
                }
            }
            else
            {
                if (m_MusicSoundParams.Loop)
                {
                    if (m_MusicTimer > m_MusicSource.clip.length)
                    {
                        m_MusicTimer = 0;
                        if (m_EventManager != null)
                        {
                            m_EventManager.Fire(this, PlayMusicCompleteEventArgs.Create(m_MusicAssetName, m_MusicSource.clip.length, null));
                        }

                        complete = true;
                    }
                }
            }
            return complete;
        }

        void LoadAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            PlaySoundInfo playSoundInfo = userData as PlaySoundInfo;
            AudioClip clip  = asset as AudioClip;
            m_AudioClips.Add(assetName, clip);
            internalPlaySound(assetName, clip, playSoundInfo.PlaySoundParams, playSoundInfo.IsShort);
            ReferencePool.Release(playSoundInfo);
        }

        void LoadAssetFail(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {

        }
    }
}

