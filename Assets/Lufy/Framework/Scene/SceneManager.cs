// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-18 14:43:40
// ========================================================

using LF.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LF.Scene
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Lufy/Scene")]
    public class SceneManager : LufyManager
    {
        private readonly List<string> m_LoadedSceneAssetNames;
        private readonly List<string> m_LoadingSceneAssetNames;
        private readonly List<string> m_UnloadingSceneAssetNames;

        public SceneManager()
        {
            m_LoadedSceneAssetNames = new List<string>();
            m_LoadingSceneAssetNames = new List<string>();
            m_UnloadingSceneAssetNames = new List<string>();
        }

        /// <summary>
        /// 获取场景是否已加载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否已加载。</returns>
        public bool SceneIsLoaded(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new LufyException("Scene asset name is invalid.");
            }

            return m_LoadedSceneAssetNames.Contains(sceneAssetName);
        }

        /// <summary>
        /// 获取已加载场景的资源名称。
        /// </summary>
        /// <returns>已加载场景的资源名称。</returns>
        public string[] GetLoadedSceneAssetNames()
        {
            return m_LoadedSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取已加载场景的资源名称。
        /// </summary>
        /// <param name="results">已加载场景的资源名称。</param>
        public void GetLoadedSceneAssetNames(List<string> results)
        {
            if (results == null)
            {
                throw new LufyException("Results is invalid.");
            }

            results.Clear();
            results.AddRange(m_LoadedSceneAssetNames);
        }

        /// <summary>
        /// 获取场景是否正在加载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否正在加载。</returns>
        public bool SceneIsLoading(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new LufyException("Scene asset name is invalid.");
            }

            return m_LoadingSceneAssetNames.Contains(sceneAssetName);
        }

        /// <summary>
        /// 获取正在加载场景的资源名称。
        /// </summary>
        /// <returns>正在加载场景的资源名称。</returns>
        public string[] GetLoadingSceneAssetNames()
        {
            return m_LoadingSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取正在加载场景的资源名称。
        /// </summary>
        /// <param name="results">正在加载场景的资源名称。</param>
        public void GetLoadingSceneAssetNames(List<string> results)
        {
            if (results == null)
            {
                throw new LufyException("Results is invalid.");
            }

            results.Clear();
            results.AddRange(m_LoadingSceneAssetNames);
        }

        /// <summary>
        /// 获取场景是否正在卸载。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <returns>场景是否正在卸载。</returns>
        public bool SceneIsUnloading(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new LufyException("Scene asset name is invalid.");
            }

            return m_UnloadingSceneAssetNames.Contains(sceneAssetName);
        }

        /// <summary>
        /// 获取正在卸载场景的资源名称。
        /// </summary>
        /// <returns>正在卸载场景的资源名称。</returns>
        public string[] GetUnloadingSceneAssetNames()
        {
            return m_UnloadingSceneAssetNames.ToArray();
        }

        /// <summary>
        /// 获取正在卸载场景的资源名称。
        /// </summary>
        /// <param name="results">正在卸载场景的资源名称。</param>
        public void GetUnloadingSceneAssetNames(List<string> results)
        {
            if (results == null)
            {
                throw new LufyException("Results is invalid.");
            }

            results.Clear();
            results.AddRange(m_UnloadingSceneAssetNames);
        }

        /// <summary>
        /// 加载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        public void LoadScene(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new LufyException("Scene asset name is invalid.");
            }

            if (SceneIsUnloading(sceneAssetName))
            {
                throw new LufyException(Utility.Text.Format("Scene asset '{0}' is being unloaded.", sceneAssetName));
            }

            if (SceneIsLoading(sceneAssetName))
            {
                throw new LufyException(Utility.Text.Format("Scene asset '{0}' is being loaded.", sceneAssetName));
            }

            if (SceneIsLoaded(sceneAssetName))
            {
                throw new LufyException(Utility.Text.Format("Scene asset '{0}' is already loaded.", sceneAssetName));
            }

            m_LoadingSceneAssetNames.Add(sceneAssetName);
            StartCoroutine(LoadSceneCo(sceneAssetName, LoadSceneSuccessCallback, LoadSceneUpdateCallback));
        }

        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        public void UnloadScene(string sceneAssetName)
        {
            if (string.IsNullOrEmpty(sceneAssetName))
            {
                throw new LufyException("Scene asset name is invalid.");
            }

            if (SceneIsUnloading(sceneAssetName))
            {
                throw new LufyException(Utility.Text.Format("Scene asset '{0}' is being unloaded.", sceneAssetName));
            }

            if (SceneIsLoading(sceneAssetName))
            {
                throw new LufyException(Utility.Text.Format("Scene asset '{0}' is being loaded.", sceneAssetName));
            }

            if (!SceneIsLoaded(sceneAssetName))
            {
                throw new LufyException(Utility.Text.Format("Scene asset '{0}' is not loaded yet.", sceneAssetName));
            }

            m_UnloadingSceneAssetNames.Add(sceneAssetName);
            StartCoroutine(UnLoadSceneCo(sceneAssetName, UnLoadSceneSuccessCallback));
        }


        internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            
        }

        internal override void Shutdown()
        {
            string[] loadedSceneAssetNames = m_LoadedSceneAssetNames.ToArray();
            foreach (string loadedSceneAssetName in loadedSceneAssetNames)
            {
                if (SceneIsUnloading(loadedSceneAssetName))
                {
                    continue;
                }

                UnloadScene(loadedSceneAssetName);
            }

            m_LoadedSceneAssetNames.Clear();
            m_LoadingSceneAssetNames.Clear();
            m_UnloadingSceneAssetNames.Clear();
        }

        private IEnumerator LoadSceneCo(string strLevelName, Action<string> OnSecenLoaded, Action<string, float> OnSceneProgress)
        {
            AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(strLevelName, LoadSceneMode.Additive);
            if (null == async)
            {
                yield break;
            }

            //*加载进度
            while (!async.isDone)
            {
                float fProgressValue;
                if (async.progress < 0.9f)
                {
                    fProgressValue = async.progress;
                }
                else
                {
                    fProgressValue = 1.0f;
                }
                OnSceneProgress?.Invoke(strLevelName, fProgressValue);
                yield return null;
            }
            OnSecenLoaded?.Invoke(strLevelName);
        }

        private IEnumerator UnLoadSceneCo(string strLevelName, Action<string> OnSecenUnLoaded)
        {
            AsyncOperation async = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(strLevelName);
            if (null == async)
            {
                yield break;
            }
            OnSecenUnLoaded?.Invoke(strLevelName);
        }

        private void LoadSceneSuccessCallback(string sceneAssetName)
        {
            m_LoadingSceneAssetNames.Remove(sceneAssetName);
            m_LoadedSceneAssetNames.Add(sceneAssetName);
            if (Lufy.GetManager<EventManager>())
            {
                LoadSceneSuccessEventArgs loadSceneSuccessEventArgs = LoadSceneSuccessEventArgs.Create(sceneAssetName, 0, null);
                Lufy.GetManager<EventManager>().Fire(this, loadSceneSuccessEventArgs);
            }
        }

        private void UnLoadSceneSuccessCallback(string sceneAssetName)
        {
            m_UnloadingSceneAssetNames.Remove(sceneAssetName);
            m_LoadedSceneAssetNames.Remove(sceneAssetName);
            if (Lufy.GetManager<EventManager>())
            {
                UnLoadSceneSuccessEventArgs unloadSceneSuccessEventArgs = UnLoadSceneSuccessEventArgs.Create(sceneAssetName, null);
                Lufy.GetManager<EventManager>().Fire(this, unloadSceneSuccessEventArgs);
            }
        }

        private void LoadSceneUpdateCallback(string sceneAssetName, float progress)
        {
            if (Lufy.GetManager<EventManager>())
            {
                LoadSceneUpdateEventArgs loadSceneUpdateEventArgs = LoadSceneUpdateEventArgs.Create(sceneAssetName, progress, null);
                Lufy.GetManager<EventManager>().Fire(this, loadSceneUpdateEventArgs);
            }
        }
    }
}

