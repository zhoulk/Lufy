// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-10 14:41:20
// ========================================================
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtention
{
    private static readonly List<Transform> s_CachedTransforms = new List<Transform>();

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

    /// <summary>
    /// 递归设置游戏对象的层次。
    /// </summary>
    /// <param name="gameObject"><see cref="UnityEngine.GameObject" /> 对象。</param>
    /// <param name="layer">目标层次的编号。</param>
    public static void SetLayerRecursively(this GameObject gameObject, int layer)
    {
        gameObject.GetComponentsInChildren(true, s_CachedTransforms);
        for (int i = 0; i < s_CachedTransforms.Count; i++)
        {
            s_CachedTransforms[i].gameObject.layer = layer;
        }

        s_CachedTransforms.Clear();
    }
}

