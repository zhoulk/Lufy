// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-21 11:01:45
// ========================================================
using LF;
using LF.UI;
using UnityEngine.UI;

public class testUI3Form : UIFormLogic
{
    public Button formBtn;
    public Button backBtn;

    protected internal override void OnClose(object userData)
    {
        base.OnClose(userData);

        Log.Debug("testUI3 close");
    }

    protected internal override void OnCover()
    {
        base.OnCover();

        Log.Debug("testUI3 cover");
    }

    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);
        formBtn.onClick.AddListener(() =>
        {
            Lufy.GetManager<UIManager>().OpenUIForm("Assets/Lufy/Test/UI/Prefab/test.prefab");
        });
        backBtn.onClick.AddListener(() =>
        {
            Lufy.GetManager<UIManager>().CloseUIForm("Assets/Lufy/Test/UI/Prefab/testUI3.prefab");
        });

        Log.Debug("testUI3 init");
    }

    protected internal override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        Log.Debug("testUI3 open");
    }

    protected internal override void OnPause()
    {
        base.OnPause();

        Log.Debug("testUI3 pause");
    }

    protected internal override void OnRealse(object uiFormAsset, object uiFormInstance)
    {
        base.OnRealse(uiFormAsset, uiFormInstance);
    }

    protected internal override void OnRecycle()
    {
        base.OnRecycle();

        Log.Debug("testUI3 recycle");
    }

    protected internal override void OnResume()
    {
        base.OnResume();

        Log.Debug("testUI3 resume");
    }

    protected internal override void OnReveal()
    {
        base.OnReveal();

        Log.Debug("testUI3 reveal");
    }

    protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
    }
}
