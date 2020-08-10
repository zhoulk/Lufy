// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-10 17:31:38
// ========================================================

using LF;
using LF.UINavi;
using System;
using UnityEngine;

public static class GameObjectExtentionNavi {

    public static GameObject AddNaviLeft(this GameObject obj, GameObject toObj)
    {
        Lufy.GetManager<UINaviManager>().AddNavi(obj, toObj, UINaviNodeRelation.Left);
        return obj;
    }

    public static GameObject AddNaviRight(this GameObject obj, GameObject toObj)
    {
        Lufy.GetManager<UINaviManager>().AddNavi(obj, toObj, UINaviNodeRelation.Right);
        return obj;
    }

    public static GameObject AddNaviUp(this GameObject obj, GameObject toObj)
    {
        Lufy.GetManager<UINaviManager>().AddNavi(obj, toObj, UINaviNodeRelation.Up);
        return obj;
    }

    public static GameObject AddNaviDown(this GameObject obj, GameObject toObj)
    {
        Lufy.GetManager<UINaviManager>().AddNavi(obj, toObj, UINaviNodeRelation.Down);
        return obj;
    }

    public static GameObject AddSelected(this GameObject obj, Action<GameObject> action)
    {
        Lufy.GetManager<UINaviManager>().AddEvent(obj, action, UINaviNodeEvent.Selected);
        return obj;
    }

    public static GameObject AddUnSelected(this GameObject obj, Action<GameObject> action)
    {
        Lufy.GetManager<UINaviManager>().AddEvent(obj, action, UINaviNodeEvent.UnSelected);
        return obj;
    }

    public static GameObject GetLeftNavi(this GameObject obj)
    {
        return Lufy.GetManager<UINaviManager>().GetNavi(obj, UINaviNodeRelation.Left);
    }

    public static GameObject GetRightNavi(this GameObject obj)
    {
        return Lufy.GetManager<UINaviManager>().GetNavi(obj, UINaviNodeRelation.Right);
    }

    public static GameObject GetUpNavi(this GameObject obj)
    {
        return Lufy.GetManager<UINaviManager>().GetNavi(obj, UINaviNodeRelation.Up);
    }

    public static GameObject GetDownNavi(this GameObject obj)
    {
        return Lufy.GetManager<UINaviManager>().GetNavi(obj, UINaviNodeRelation.Down);
    }

    public static Action<GameObject> GetSelectedAction(this GameObject obj)
    {
        return Lufy.GetManager<UINaviManager>().GetEvent(obj, UINaviNodeEvent.Selected);
    }

    public static Action<GameObject> GetUnSelectedAction(this GameObject obj)
    {
        return Lufy.GetManager<UINaviManager>().GetEvent(obj, UINaviNodeEvent.UnSelected);
    }

    public static void SetAsDefaultNavi(this GameObject obj)
    {
        Lufy.GetManager<UINaviManager>().DefaultObject = obj;
        if (obj.GetSelectedAction() != null)
        {
            obj.GetSelectedAction()(obj);
        }
    }
}
