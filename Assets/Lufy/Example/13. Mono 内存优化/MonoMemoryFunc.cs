// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-20 09:08:16
// ========================================================
using LF.Timer;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LF.Example
{
    public class MonoMemoryFunc : MonoBehaviour
    {
        int[] m_intArray;
        List<int> m_intList;
        ArrayList m_arryList;

        Dictionary<string, string> m_strDic;

        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder(1024);
        string format = "{0}{1}";

        string aStr = "a";
        string bStr = "b";

        string[] names = new string[100];

        WaitForSeconds oneSecondDelay = new WaitForSeconds(1);

        TimerManager timerManager;
        Handler delayHandler;

        public void Start()
        {
            m_intArray = new int[2];
            m_intList = new List<int>();
            m_arryList = new ArrayList();
            for (int i = 0; i < m_intArray.Length; i++)
            {
                m_intArray[i] = i;
                m_intList.Add(i);
                m_arryList.Add(i);
            }

            m_strDic = new Dictionary<string, string>();
            for(int i=0; i<2; i++)
            {
                m_strDic.Add("key" + i, i.ToString());
            }

            timerManager = Lufy.GetManager<TimerManager>();
            delayHandler = DelayFunc;
        }

        void Update()
        {
            //testIntListForeach();
            //testDictonaryForeach();
            //testObject();
            //testString();
            //testStringEqual();
            //testFunc();

            //timerManager.OnUpdate(Time.deltaTime, Time.unscaledDeltaTime);
            //testCoroutine();

            testStruct();
        }

        void testIntListForeach()
        {
            for (int i = 0; i < 1000; i++)
            {
                // 不产生GC
                //foreach (var iNum in m_intList)
                //{
                //}

                // 不产生GC
                //var iNum = m_intList.GetEnumerator();
                //while (iNum.MoveNext())
                //{
                //}

                // 产生GC  46.9KB
                //foreach (var iNum in m_arryList)
                //{
                //}

                // 产生GC  46.9KB
                //var iNum = m_arryList.GetEnumerator();
                //while (iNum.MoveNext())
                //{
                //}

                // 不产生GC
                //for (int j = 0; j < m_arryList.Count; j++)
                //{
                //}
            }
        }

        void testDictonaryForeach()
        {
            for (int i = 0; i < 1000; i++)
            {
                // 不产生GC
                foreach (var kv in m_strDic)
                {
                    //Debug.Log(kv);
                }
            }
        }

        void testObject()
        {
            // 产生GC 39.1 KB
            //for(int i=0; i<1000; i++)
            //{
            //    Student1 s = new Student1();
            //}

            // 产生GC 39.1 KB
            //for (int i = 0; i < 1000; i++)
            //{
            //    Student2 s = new Student2();
            //}

            // 产生GC 46.9 KB
            //for (int i = 0; i < 1000; i++)
            //{
            //    Student3 s = new Student3();
            //    ReferencePool.Acquire<Student3>();
            //}

            // 第一次产生GC  0.9KB  后面不产生GC
            for (int i = 0; i < 1000; i++)
            {
                Student3 s = ReferencePool.Acquire<Student3>();
                ReferencePool.Release(s);
            }
        }

        void testString()
        {
            // 不产生GC
            //for(int i=0; i<1000; i++)
            //{
            //    string str = "image";
            //}

            // 产生GC   91.3 KB
            //for (int i = 0; i < 1000; i++)
            //{
            //    string str = "image" + i;
            //}

            // 产生GC   31 KB
            //for (int i = 0; i < 1000; i++)
            //{
            //    sb.Remove(0, sb.Length);
            //    sb.Append("image").Append(i);
            //    //string str = "image" + i;
            //}

            // 产生GC   71.8 KB
            //for (int i = 0; i < 1000; i++)
            //{
            //    sb.Remove(0, sb.Length);
            //    sb.Append("image").Append(i);
            //    string str = sb.ToString();
            //}

            //产生GC   91.3 KB
            //for (int i = 0; i < 1000; i++)
            //{
            //    sb.Length = 0;
            //    sb.AppendFormat("{0}{1}", "image", i);
            //    string str = sb.ToString();
            //}

            // 产生GC   282.3 KB
            //for (int i = 0; i < 1000; i++)
            //{
            //    string str = "image" + i + "image" + i;
            //}

            //产生GC   219.8 KB
            //for (int i = 0; i < 1000; i++)
            //{
            //    sb.Length = 0;
            //    sb.AppendFormat("{0}{1}{2}{3}", "image", i, "image", i);
            //    string str = sb.ToString();
            //}

            // 产生GC   91.3 KB
            //for (int i = 0; i < 1000; i++)
            //{
            //    string str = Utility.Text.Format(format, "image", i);
            //}


            // 产生GC   62 KB
            //for (int i = 0; i < 1000; i++)
            //{
            //    sb2.Clear();
            //    sb2.Append("image").Append("asasa").Append(i).Append(i);
            //    //string str = sb.ToString();
            //}
        }

        void testStringEqual()
        {
            // 不产生GC
            //for(int i=0; i<1000; i++)
            //{
            //    if("a" == "b")
            //    {

            //    }
            //}

            // 不产生GC
            //for (int i = 0; i < 1000; i++)
            //{
            //    if(aStr == bStr)
            //    {

            //    }
            //}

            // 不产生GC
            //for (int i = 0; i < 1000; i++)
            //{
            //    if (aStr.Equals(bStr))
            //    {

            //    }
            //}

            // 不产生GC
            //for (int i = 0; i < 1000; i++)
            //{
            //    string a = "a";
            //    string b = "b";
            //    if (a == b)
            //    {

            //    }
            //}

            // 不产生GC
            //for (int i = 0; i < 1000; i++)
            //{
            //    string a = "a";
            //    string b = "b";
            //    if (a.Equals(b))
            //    {

            //    }
            //}

            // 产生GC  41KB
            //for (int i = 0; i < 1000; i++)
            //{
            //    string a = gameObject.tag;
            //    string b = "b";
            //    if (a.Equals(b))
            //    {

            //    }
            //}

            // 不产生 GC
            for (int i = 0; i < 1000; i++)
            {
                string b = "Untagged";
                if (gameObject.CompareTag(b))
                {

                }
            }
        }

        void testFunc()
        {
            //产生GC  3.7KB
            //getNames();

            //产生GC  2.9KB
            getNames2(names);
        }

        string[] getNames()
        {
            string[] names = new string[100];
            for(int i=0; i<names.Length; i++)
            {
                names[i] = i.ToString();
            }
            return names;
        }

        void getNames2(string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = i.ToString();
            }
        }

        #region testCoroutine
        void testCoroutine()
        {
            // 产生GC  84B
            //StartCoroutine(DelayCo());

            // 产生GC   64B
            //StartCoroutine(DelayCo2());

            // 产生GC   84B
            //StartCoroutine(DelayCo3());

            // 产生GC   64B
            //StartCoroutine(DelayCo4());

            // 不产生GC
            //Delay();

            // 产生GC   112B
            //Delay2();

            // 不产生GC 
            //Delay3();

            // 不产生GC 
            //Delay4();

            // 产生GC   60B
            Delay5();

            // 产生GC   32B
            //delayHandler.DynamicInvoke();

            // 不产生GC
            //delayHandler.Invoke();

            // 不产生GC
            // delayHandler();
        }

        IEnumerator DelayCo()
        {
            yield return new WaitForSeconds(1);
        }

        IEnumerator DelayCo2()
        {
            yield return oneSecondDelay;
        }

        IEnumerator DelayCo3()
        {
            yield return 0;
        }

        IEnumerator DelayCo4()
        {
            yield return null;
        }

        void Delay()
        {
            Lufy.GetManager<TimerManager>().doOnce(1, null);
        }

        void Delay2()
        {
            Lufy.GetManager<TimerManager>().doOnce(1, DelayFunc);
        }

        void Delay3()
        {
            Lufy.GetManager<TimerManager>().doOnce(1, ()=>
            {
                //Debug.Log("test");
            });
        }

        void Delay4()
        {
            Lufy.GetManager<TimerManager>().doOnce(1000, delayHandler);
        }

        void Delay5()
        {
            Lufy.GetManager<TimerManager>().doOnce<int>(1, (flag) =>
            {
                //Debug.Log("test");
            }, 1);
        }

        void DelayFunc()
        {
            //Debug.Log("test");
        }

        #endregion

        #region testStruct
        void testStruct()
        {
            // 使用更小的类 减少GC检查时间
            // GC 272B
            testStruct[] tests = new testStruct[10];

            // GC 112B
            //string[] sArr = new string[10];
        }
        #endregion
    }

    struct testStruct
    {
        public string name;
        public int age;
        public Vector3 pos;
    }

    class Student1
    {
        public string name = "";
        public int age = 0;
        public int level = 0;
        public int arg1 = 0;
    }

    class Student2
    {
        public string name = "";
        public int age = 0;
        public int level = 0;
        public int arg1 = 0;
        public int arg2 = 0;
    }

    class Student3 : IReference
    {
        public string name = "";
        public int age = 0;
        public int level = 0;
        public int arg1 = 0;
        public int arg2 = 0;
        public int arg3 = 0;

        public void Clear()
        {
            
        }
    }
}
