// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-19 18:57:09
// ========================================================
using System;
using LF;
using LF.Setting;

public class ESSettingHelper : ISettingHelper
{
    public string[] Keys()
    {
        return ES3.GetKeys();
    }

    public bool GetBool(string settingName)
    {
        return GetBool(settingName, false);
    }

    public bool GetBool(string settingName, bool defaultValue)
    {
        if (ES3.KeyExists(settingName))
        {
            return ES3.Load<bool>(settingName);
        }
        return defaultValue;
    }

    public float GetFloat(string settingName)
    {
        return GetFloat(settingName, 0);
    }

    public float GetFloat(string settingName, float defaultValue)
    {
        if (ES3.KeyExists(settingName))
        {
            return ES3.Load<float>(settingName);
        }
        return defaultValue;
    }

    public int GetInt(string settingName)
    {
        return GetInt(settingName, 0);
    }

    public int GetInt(string settingName, int defaultValue)
    {
        if (ES3.KeyExists(settingName))
        {
            return ES3.Load<int>(settingName);
        }
        return defaultValue;
    }

    public T GetObject<T>(string settingName)
    {
        return GetObject<T>(settingName, default(T));
    }

    public object GetObject(Type objectType, string settingName)
    {
        return GetObject(objectType, settingName, null);
    }

    public T GetObject<T>(string settingName, T defaultObj)
    {
        if (ES3.KeyExists(settingName))
        {
            string json = ES3.Load<string>(settingName);
            if (json == null)
            {
                return defaultObj;
            }
            return Utility.Json.ToObject<T>(json);
        }
        else
        {
            return defaultObj;
        }
    }

    public object GetObject(Type objectType, string settingName, object defaultObj)
    {
        if (ES3.KeyExists(settingName))
        {
            string json = ES3.Load<string>(settingName);
            if (json == null)
            {
                return defaultObj;
            }
            return Utility.Json.ToObject(objectType, json);
        }
        else
        {
            return defaultObj;
        }
    }

    public string GetString(string settingName)
    {
        return GetString(settingName, "");
    }

    public string GetString(string settingName, string defaultValue)
    {
        if (ES3.KeyExists(settingName))
        {
            return ES3.Load<string>(settingName);
        }
        return defaultValue;
    }

    public bool HasSetting(string settingName)
    {
        return ES3.KeyExists(settingName);
    }

    public bool Load()
    {
        return true;
    }

    public void RemoveAllSettings()
    {
        string[] keys = ES3.GetKeys();
        for(int i=0; i<keys.Length; i++)
        {
            ES3.DeleteKey(keys[i]);
        }
    }

    public void RemoveSetting(string settingName)
    {
        ES3.DeleteKey(settingName);
    }

    public bool Save()
    {
        return true;
    }

    public void SetBool(string settingName, bool value)
    {
        ES3.Save<bool>(settingName, value);
    }

    public void SetFloat(string settingName, float value)
    {
        ES3.Save<float>(settingName, value);
    }

    public void SetInt(string settingName, int value)
    {
        ES3.Save<int>(settingName, value);
    }

    public void SetObject<T>(string settingName, T obj)
    {
        ES3.Save<string>(settingName, Utility.Json.ToJson(obj));
    }

    public void SetObject(string settingName, object obj)
    {
        ES3.Save<string>(settingName, Utility.Json.ToJson(obj));
    }

    public void SetString(string settingName, string value)
    {
        ES3.Save<string>(settingName, value);
    }
}
