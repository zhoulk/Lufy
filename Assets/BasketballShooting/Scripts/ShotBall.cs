/* ShotBall.cs
  version 1.0 - August 7, 2015

  Copyright (C) 2015 Wasabi Applications Inc.
   http://www.wasabi-applications.co.jp/
*/

using UnityEngine;
using System.Collections;

namespace BasketBall
{
    public class ShotBall : MonoBehaviour
    {


        public bool isActive { get; private set; }


        public void ChangeActive()
        {
            isActive = true;
        }

        private void Update()
        {
            if (Define.platForm.Equals(PlatForm.Phone)){
                Vector3 screenPos = Shooter.Instance.cameraForShooter.WorldToScreenPoint(transform.position);
                //Debug.Log(screenPos);
                float h = Screen.height;
                float w = Screen.width;
                if (screenPos.y < 0 || screenPos.y > h || screenPos.x < 0 || screenPos.x > w)
                {
                    if (gameObject != null)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

        public void DestroyDelay()
        {
            GameEntry.Timer.doOnce(5000, () =>
            {
                if (gameObject != null)
                {
                    Destroy(gameObject);
                }
            });
        }
    }
}

