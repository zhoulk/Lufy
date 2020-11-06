// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-11-06 15:03:55
// ========================================================
using LF;
using LF.WebRequest;
using UnityEngine;

public class WebRequestTest : MonoBehaviour
{
    WebRequestManager webRequestManager;

    // Start is called before the first frame update
    void Start()
    {
        webRequestManager = Lufy.GetManager<WebRequestManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
