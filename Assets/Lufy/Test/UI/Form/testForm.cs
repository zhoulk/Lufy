// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-21 11:01:45
// ========================================================
using LF;
using LF.UI;
using LF.UINavi;
using UnityEngine;
using UnityEngine.UI;

public class testForm : UIFormLogic
{
    public GameObject formBtn;
    public GameObject popBtn;
    public GameObject backBtn;

    protected internal override void OnClose(object userData)
    {
        base.OnClose(userData);

        Log.Debug("test close");
    }

    protected internal override void OnCover()
    {
        base.OnCover();

        Log.Debug("test cover");
    }

    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);
        Lufy.GetManager<UIEventManager>().AddOnClickHandler(formBtn, (GameObject obj) =>
        {
            Lufy.GetManager<UIManager>().OpenUIForm("Assets/Lufy/Test/UI/Prefab/testUI2.prefab");
        });

        Lufy.GetManager<UIEventManager>().AddOnClickHandler(popBtn, (GameObject obj) =>
        {
            Lufy.GetManager<UIManager>().OpenUIForm("Assets/Lufy/Test/UI/Prefab/testPop2.prefab");
        });

        Log.Debug("test init");
    }

    protected internal override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        Log.Debug("test open");
    }

    protected internal override void OnPause()
    {
        base.OnPause();

        Log.Debug("test pause");
    }

    protected internal override void OnRealse(object uiFormAsset, object uiFormInstance)
    {
        base.OnRealse(uiFormAsset, uiFormInstance);

        Log.Debug("test release");
    }

    protected internal override void OnRecycle()
    {
        base.OnRecycle();

        Log.Debug("test recycle");
    }

    protected internal override void OnResume()
    {
        base.OnResume();

        Log.Debug("test resume");
    }

    protected internal override void OnReveal()
    {
        base.OnReveal();

        Log.Debug("test reveal");
    }

    protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
    }
}
