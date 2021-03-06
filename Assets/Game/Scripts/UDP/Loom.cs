﻿// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-13 15:58:03
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Threading;
using System.Linq;

public class Loom : MonoBehaviour
{
    public static int maxThreads = 8;
    static int numThreads;

    private static Loom _current;
    public static Loom Current
    {
        get
        {
            Initialize();
            return _current;
        }
    }

    static bool initialized;

    //####��Ϊ��ʼ�������Լ����ã����ڳ�ʼ����������һ�μ���
    public static void Initialize()
    {
        if (!initialized)
        {

            if (!Application.isPlaying)
                return;
            initialized = true;
            GameObject g = new GameObject("Loom");
            //####��������
            DontDestroyOnLoad(g);
            _current = g.AddComponent<Loom>();
        }

    }

    private List<Action> _actions = new List<Action>();
    public struct DelayedQueueItem
    {
        public float time;
        public Action action;
    }
    private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

    List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

    public static void QueueOnMainThread(Action action)
    {
        QueueOnMainThread(action, 0f);
    }
    public static void QueueOnMainThread(Action action, float time)
    {
        if (time != 0)
        {
            if (Current != null)
            {
                lock (Current._delayed)
                {
                    Current._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
                }
            }
        }
        else
        {
            if (Current != null)
            {
                lock (Current._actions)
                {
                    Current._actions.Add(action);
                }
            }
        }
    }

    public static Thread RunAsync(Action a)
    {
        Initialize();
        while (numThreads >= maxThreads)
        {
            Thread.Sleep(1);
        }
        Interlocked.Increment(ref numThreads);
        ThreadPool.QueueUserWorkItem(RunAction, a);
        return null;
    }

    private static void RunAction(object action)
    {
        try
        {
            ((Action)action)();
        }
        catch
        {
        }
        finally
        {
            Interlocked.Decrement(ref numThreads);
        }

    }


    void OnDisable()
    {
        if (_current == this)
        {

            _current = null;
        }
    }



    // Use this for initialization  
    void Start()
    {

    }

    List<Action> _currentActions = new List<Action>();

    // Update is called once per frame  
    public void Update()
    {
        lock (_actions)
        {
            _currentActions.Clear();
            _currentActions.AddRange(_actions);
            _actions.Clear();
        }
        for (int i = 0; i < _currentActions.Count; i++)
        {
            _currentActions[i]();
        }
        lock (_delayed)
        {
            _currentDelayed.Clear();
            for (int i = 0; i < _delayed.Count; i++)
            {
                if(_delayed[i].time <= Time.time)
                {
                    _currentDelayed.Add(_delayed[i]);
                }
            }

            for (int i = 0; i < _currentDelayed.Count; i++)
            {
                _delayed.Remove(_currentDelayed[i]);
            }
        }
        for(int i=0; i< _currentDelayed.Count; i++)
        {
            _currentDelayed[i].action();
        }
    }
}

