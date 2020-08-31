// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-18 15:56:06
// ========================================================

using LF;
using LF.UI;
using System.Collections.Generic;

public static class UIExtention
{
    public static void OpenUIForm(this UIManager ui, UIFormId formId, Dictionary<string, object> param = null)
    {
        string path = Utility.Text.Format("Assets/Game/Res/Prefabs/{0}.prefab", formId.ToString().ToLower());
        GameEntry.UI.OpenUIForm(path, param);
    }

    public static void CloseUIForm(this UIManager ui, UIFormId formId)
    {
        GameEntry.UI.CloseUIForm(formId.ToString().ToLower());
    }
}
