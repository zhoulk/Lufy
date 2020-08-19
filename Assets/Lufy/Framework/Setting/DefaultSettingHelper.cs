// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-19 18:36:57
// ========================================================

using System;
using UnityEngine;

namespace LF.Setting
{
    public class DefaultSettingHelper : ISettingHelper
    {
        public bool GetBool(string settingName)
        {
            return PlayerPrefs.GetInt(settingName) != 0;
        }

        public bool GetBool(string settingName, bool defaultValue)
        {
            return PlayerPrefs.GetInt(settingName, defaultValue ? 1 : 0) != 0;
        }

        public float GetFloat(string settingName)
        {
            return PlayerPrefs.GetFloat(settingName);
        }

        public float GetFloat(string settingName, float defaultValue)
        {
            return PlayerPrefs.GetFloat(settingName, defaultValue);
        }

        public int GetInt(string settingName)
        {
            return PlayerPrefs.GetInt(settingName);
        }

        public int GetInt(string settingName, int defaultValue)
        {
            return PlayerPrefs.GetInt(settingName, defaultValue);
        }

        public T GetObject<T>(string settingName)
        {
            return Utility.Json.ToObject<T>(PlayerPrefs.GetString(settingName));
        }

        public object GetObject(Type objectType, string settingName)
        {
            return Utility.Json.ToObject(objectType, PlayerPrefs.GetString(settingName));
        }

        public T GetObject<T>(string settingName, T defaultObj)
        {
            string json = PlayerPrefs.GetString(settingName, null);
            if (json == null)
            {
                return defaultObj;
            }

            return Utility.Json.ToObject<T>(json);
        }

        public object GetObject(Type objectType, string settingName, object defaultObj)
        {
            string json = PlayerPrefs.GetString(settingName, null);
            if (json == null)
            {
                return defaultObj;
            }

            return Utility.Json.ToObject(objectType, json);
        }

        public string GetString(string settingName)
        {
            return PlayerPrefs.GetString(settingName);
        }

        public string GetString(string settingName, string defaultValue)
        {
            return PlayerPrefs.GetString(settingName, defaultValue);
        }

        public bool HasSetting(string settingName)
        {
            return PlayerPrefs.HasKey(settingName);
        }

        public string[] Keys()
        {
            return new string[] { };
        }

        public bool Load()
        {
            return true;
        }

        public void RemoveAllSettings()
        {
            PlayerPrefs.DeleteAll();
        }

        public void RemoveSetting(string settingName)
        {
            PlayerPrefs.DeleteKey(settingName);
        }

        public bool Save()
        {
            PlayerPrefs.Save();
            return true;
        }

        public void SetBool(string settingName, bool value)
        {
            PlayerPrefs.SetInt(settingName, value ? 1 : 0);
        }

        public void SetFloat(string settingName, float value)
        {
            PlayerPrefs.SetFloat(settingName, value);
        }

        public void SetInt(string settingName, int value)
        {
            PlayerPrefs.SetInt(settingName, value);
        }

        public void SetObject<T>(string settingName, T obj)
        {
            PlayerPrefs.SetString(settingName, Utility.Json.ToJson(obj));
        }

        public void SetObject(string settingName, object obj)
        {
            PlayerPrefs.SetString(settingName, Utility.Json.ToJson(obj));
        }

        public void SetString(string settingName, string value)
        {
            PlayerPrefs.SetString(settingName, value);
        }
    }
}

