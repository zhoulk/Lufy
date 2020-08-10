// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-10 16:35:45
// ========================================================
using LF.UI;

public class GameUILogic : UIFormLogic
{
    public void Open(UIFormId formId)
    {
        GameEntry.UI.OpenUIForm(formId.ToString().ToLower());
    }

    public void Close(UIFormId formId)
    {
        GameEntry.UI.CloseUIForm(formId.ToString().ToLower());
    }

    public void Close()
    {
        GameEntry.UI.CloseUIForm(this);
    }
}
