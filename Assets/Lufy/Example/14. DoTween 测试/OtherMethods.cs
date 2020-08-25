// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-24 18:45:39
// ========================================================
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherMethods : MonoBehaviour
{
    public AnimationCurve m_AnimationCurve;

    // Start is called before the first frame update
    void Start()
    {
        //// Example 1: calling another method after 1 second
        //﻿﻿﻿﻿﻿﻿﻿﻿DOVirtual.DelayedCall(1, ()=>
        //        {
        //        });
        //﻿﻿﻿﻿﻿﻿﻿﻿// Example 2: using a lambda to throw a log after 1 second
        //﻿﻿﻿﻿﻿﻿﻿﻿DOVirtual.DelayedCall(1, () => Debug.Log("Hello world"));

        //float f = DOVirtual.EasedValue(1, 10, 0.5f, m_AnimationCurve);
        //Debug.Log(f);

        DOVirtual.Float(1, 10, 3, (f) =>
        {
            Debug.Log(f);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
