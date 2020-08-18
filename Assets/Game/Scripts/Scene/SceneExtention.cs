// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-18 15:52:23
// ========================================================
using LF.Scene;

public static class SceneExtention 
{
    public static void LoadScene(this SceneManager scene, SceneId sceneId)
    {
        scene.LoadScene(sceneId.ToString());
    }

    public static void UnloadScene(this SceneManager scene, SceneId sceneId)
    {
        scene.UnloadScene(sceneId.ToString());
    }
}
