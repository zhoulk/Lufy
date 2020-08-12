// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-12 10:58:10
// ========================================================
using LF;

public class TestManager : MonoSingleton<TestManager>
{
    public void Step1()
    {
        Log.Debug("step1");
    }
}
