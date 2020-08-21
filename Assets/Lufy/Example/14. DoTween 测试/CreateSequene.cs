// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-21 18:36:12
// ========================================================
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSequene : MonoBehaviour
{
    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(cube.transform.DOMoveX(2, 1));
        mySequence.Join(cube.transform.DORotate(new Vector3(0, 180, 0), 1));

        mySequence.AppendCallback(MyCallback);
        mySequence.AppendInterval(10);
        mySequence.Insert(2, cube.transform.DOMoveY(2, 1));
        mySequence.InsertCallback(2, MiddleCallback);

        mySequence.Prepend(cube.transform.DOMoveX(-2, 1));
        mySequence.PrependCallback(StartCallback);
        mySequence.PrependInterval(10);

    }

    void MyCallback()
    {
        Debug.Log("complete");
    }

    void MiddleCallback()
    {
        Debug.Log("middle");
    }

    void StartCallback()
    {
        Debug.Log("start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
