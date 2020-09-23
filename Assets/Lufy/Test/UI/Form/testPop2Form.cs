// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-21 11:01:45
// ========================================================
using LF;
using LF.UI;
using UnityEngine.UI;

public class testPop2Form : UIFormLogic
{
    public Button form2Btn;
    public Button backBtn;
    public Button pop3Btn;

    protected internal override void OnClose(object userData)
    {
        base.OnClose(userData);

        Log.Debug("testPop2 close");
    }

    protected internal override void OnCover()
    {
        base.OnCover();

        Log.Debug("testPop2 cover");
    }

    protected internal override void OnInit(object userData)
    {
        base.OnInit(userData);
        form2Btn.onClick.AddListener(() =>
        {
            Lufy.GetManager<UIManager>().OpenUIForm("Assets/Lufy/Test/UI/Prefab/testUI2.prefab");
        });
        backBtn.onClick.AddListener(() =>
        {
            Lufy.GetManager<UIManager>().CloseUIForm("Assets/Lufy/Test/UI/Prefab/testPop2.prefab");
        });
        pop3Btn.onClick.AddListener(() =>
        {
            Lufy.GetManager<UIManager>().OpenUIForm("Assets/Lufy/Test/UI/Prefab/testPop3.prefab");
        });

        Log.Debug("testPop2 init");
    }

    protected internal override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        Log.Debug("testPop2 open");
    }

    protected internal override void OnPause()
    {
        base.OnPause();

        Log.Debug("testPop2 pause");
    }

    protected internal override void OnRealse(object uiFormAsset, object uiFormInstance)
    {
        base.OnRealse(uiFormAsset, uiFormInstance);
    }

    protected internal override void OnRecycle()
    {
        base.OnRecycle();

        Log.Debug("testPop2 recycle");
    }

    protected internal override void OnResume()
    {
        base.OnResume();

        Log.Debug("testPop2 resume");
    }

    protected internal override void OnReveal()
    {
        base.OnReveal();

        Log.Debug("testPop2 reveal");
    }

    protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
    }
}
