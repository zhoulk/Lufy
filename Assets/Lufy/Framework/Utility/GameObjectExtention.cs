// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-10 14:41:20
// ========================================================
using UnityEngine;

public static class GameObjectExtention
{
    /// <summary>
    /// 获取或增加组件。
    /// </summary>
    /// <typeparam name="T">要获取或增加的组件。</typeparam>
    /// <param name="gameObject">目标对象。</param>
    /// <returns>获取或增加的组件。</returns>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }
}

