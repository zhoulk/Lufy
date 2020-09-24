// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-24 17:53:59
// ========================================================
using LF.Fsm;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Hero
{

}

class HeroRun : FsmState<Hero>
{

}

public class FsmTest : MonoBehaviour
{
    FsmManager m_FsmManager = null;

    // Start is called before the first frame update
    void Start()
    {
        m_FsmManager = GetComponent<FsmManager>();

        FsmState<Hero>[] states = new FsmState<Hero>[3];
        states[0] = new HeroRun();
        m_FsmManager.CreateFsm("Hero", states);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
