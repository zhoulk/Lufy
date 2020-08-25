// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-24 16:01:15
// ========================================================
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataControl : MonoBehaviour
{
    public GameObject cube;

    Tweener tweener;

    // Start is called before the first frame update
    void Start()
    {
        #region static method

        //Debug.Log(DOTween.PausedTweens());
        //Debug.Log(DOTween.PlayingTweens());
        //Debug.Log(DOTween.TweensById("", false));
        //Debug.Log(DOTween.TweensByTarget(null, false));
        //Debug.Log(DOTween.IsTweening(transform));
        //Debug.Log(DOTween.TotalPlayingTweens());

        #endregion

        #region instance mathod

        tweener = cube.transform.DOMove(Vector3.one * 2, 5).SetLoops(-1);

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(tweener.fullPosition);
        //Debug.Log(tweener.CompletedLoops());
        //Debug.Log(tweener.Delay());
        //Debug.Log(tweener.Duration());
        //Debug.Log(tweener.Elapsed());
        //Debug.Log(tweener.ElapsedDirectionalPercentage());
        //Debug.Log(tweener.ElapsedPercentage());
        //Debug.Log(tweener.IsActive());
        //Debug.Log(tweener.IsBackwards());
        //Debug.Log(tweener.IsComplete());
        //Debug.Log(tweener.IsInitialized());
        //Debug.Log(tweener.IsPlaying());
        Debug.Log(tweener.Loops());
    }
}
