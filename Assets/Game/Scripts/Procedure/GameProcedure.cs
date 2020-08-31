// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-10 16:57:03
// ========================================================
using LF.Procedure;

public class GameProcedure : ProcedureBase
{
    public void Open(UIFormId formId)
    {
        GameEntry.UI.OpenUIForm(formId);
    }

    public void Close(UIFormId formId)
    {
        GameEntry.UI.CloseUIForm(formId);
    }
}
