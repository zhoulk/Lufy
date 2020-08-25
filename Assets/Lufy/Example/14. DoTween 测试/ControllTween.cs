// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-24 15:34:06
// ========================================================
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllTween : MonoBehaviour
{
    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        Tweener tweener = cube.transform.DOMove(Vector3.one * 2, 5);
        tweener.SetId("one");
        //tweener.Pause();
        //DOTween.Pause(cube.transform);
        //DOTween.Pause("one");
        //cube.transform.DOPause();

        //DOTween.Complete("one");
        //DOTween.Flip("one");

        //DOTween.Goto("one", 2);
        //DOTween.Goto("one", 2, true);

        //DOTween.Kill("one", true);
        //DOTween.Kill("one", false);

        //DOTween.Play("one");
        //DOTween.PlayBackwards("one");
        //DOTween.PlayForward("one");

        //DOTween.Restart("one");
        //DOTween.Rewind("one");

        //DOTween.TogglePause("one");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
