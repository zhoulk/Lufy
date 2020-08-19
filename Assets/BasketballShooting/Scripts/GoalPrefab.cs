// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-18 16:27:49
// ========================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPrefab : MonoBehaviour
{
    int direct = 1;
    float speed = 0.4f;

    public Transform goalArea;

    bool isPause = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 暂停
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isPause = !isPause;
        }
        // 居中
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Vector3 _pos = transform.position;
            Vector3 _goalPos = goalArea.transform.position;
            _pos.x = 0;
            _goalPos.x = 0;
            transform.position = _pos;
            goalArea.position = _goalPos;
            return;
        }

        if (isPause)
        {
            return;
        }

        Vector3 pos = transform.position;
        Vector3 goalPos = goalArea.transform.position;
        pos.x += direct * speed * Time.deltaTime;
        goalPos.x = pos.x;
        if (pos.x >= 3)
        {
            direct = -1;
        }
        if (pos.x <= -3)
        {
            direct = 1;
        }
        transform.position = pos;
        goalArea.position = goalPos;
    }
}
