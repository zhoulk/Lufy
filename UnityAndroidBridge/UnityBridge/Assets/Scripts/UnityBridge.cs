using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityBridge : MonoBehaviour
{
    private AndroidJavaObject jo;

    private void Awake()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
    }

    void Start()
    {
        jo.Call("HelloWorld", "lufy");
    }
}
