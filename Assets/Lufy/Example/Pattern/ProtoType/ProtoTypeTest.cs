// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-24 14:29:13
// ========================================================
using LF.Pattern.ProtoType;
using System;
using UnityEngine;

[Serializable]
class A
{
    public string name;
    public B b;
}

[Serializable]
class B
{
    public string name;
}

public class ProtoTypeTest : MonoBehaviour
{
    ProtoTypeManager m_ProtoTypeManager = null;

    // Start is called before the first frame update
    void Start()
    {
        m_ProtoTypeManager = GetComponent<ProtoTypeManager>();

        A a1b1 = new A();
        a1b1.name = "A1";
        a1b1.b = new B();
        a1b1.b.name = "B1";

        A a2b1 = new A();
        a2b1.name = "A2";
        a2b1.b = new B();
        a2b1.b.name = "B1";

        m_ProtoTypeManager.RegisteProtoType("a1b1", a1b1);
        m_ProtoTypeManager.RegisteProtoType("a2b1", a2b1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            A a1b1 = m_ProtoTypeManager.CloneFrom("a1b1") as A;
            Debug.LogFormat("{0} {1}", a1b1.name, a1b1.b.name);
            a1b1.name = "a1111";
            Debug.LogFormat("{0} {1}", a1b1.name, a1b1.b.name);
            A a1b1_1 = m_ProtoTypeManager.CloneFrom("a1b1") as A;
            Debug.LogFormat("{0} {1}", a1b1_1.name, a1b1_1.b.name);
            A a2b1 = m_ProtoTypeManager.CloneFrom("a2b1") as A;
            Debug.LogFormat("{0} {1}", a2b1.name, a2b1.b.name);
        }
    }
}
