// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-24 18:30:32
// ========================================================
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoCoroutine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(CompleteCoroutine());
        //StartCoroutine(ElapsedLoopsCoroutine());
        //StartCoroutine(KillCoroutine());
        //StartCoroutine(PositionCoroutine());
        //StartCoroutine(RewindCoroutine());
        StartCoroutine(StartCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CompleteCoroutine()
    {
        Tween myTween = transform.DOMoveX(45, 1);
        yield return myTween.WaitForCompletion();
        // This log will happen after the tween has completed
        Debug.Log("Tween completed!");
    }

    IEnumerator ElapsedLoopsCoroutine()
    {
        Tween myTween = transform.DOMoveX(45, 1).SetLoops(4);
        yield return myTween.WaitForElapsedLoops(2);
        // This log will happen after the 2nd loop has finished
        Debug.Log("Tween has looped twice!");
        yield return myTween.WaitForCompletion();
        // This log will happen after the tween has completed
        Debug.Log("Tween completed!");
    }

    IEnumerator KillCoroutine()
    {
        Tween myTween = transform.DOMoveX(45, 1);
        yield return myTween.WaitForKill();
        // This log will happen after the tween has been killed
        Debug.Log("Tween killed!");
    }

    IEnumerator PositionCoroutine()
    {
        Tween myTween = transform.DOMoveX(45, 1);
        yield return myTween.WaitForPosition(0.3f);
        // This log will happen after the tween has played for 0.3 seconds
        Debug.Log("Tween has played for 0.3 seconds!");
    }

    IEnumerator RewindCoroutine()
    {
        Tween myTween = null;
        myTween = transform.DOMoveX(45, 1).SetAutoKill(false).OnComplete(()=> {
            myTween.Rewind();
        });
        yield return myTween.WaitForRewind();
        // This log will happen when the tween has been rewinded
        Debug.Log("Tween rewinded!");
    }

    IEnumerator StartCoroutine()
    {
        Tween myTween = transform.DOMoveX(45, 1);
        yield return myTween.WaitForStart();
        // This log will happen when the tween starts
        Debug.Log("Tween started!");
    }
}
