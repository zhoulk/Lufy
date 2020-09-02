// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-09-02 16:17:16
// ========================================================

using LF;
using LF.Net;
using UnityEngine;

namespace Bowling
{
    public class SensorCollection : MonoBehaviour
    {
        bool isClickDown = false;

        Gyroscope go;

        // Start is called before the first frame update
        void Start()
        {
            go = Input.gyro;
            go.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                isClickDown = true;
            }
            else
            {
                if(isClickDown == true)
                {
                    isClickDown = false;
                    Debug.Log("acceleration " + Input.acceleration + "  " + go.attitude.eulerAngles);

                    IMessage msg = MessagesFactory.BowlingBall(1, Input.acceleration, go.attitude.eulerAngles);
                    UDPManager.Instance.Send(msg);
                }
            }
        }
    }
}

