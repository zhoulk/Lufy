// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-10 17:31:38
// ========================================================

using UnityEngine;
using UnityEngine.EventSystems;

namespace LF.UINavi
{
    public delegate void PointerClickHandler(PointerEventData obj);

    public class UIEventLisner : MonoBehaviour, IPointerClickHandler
    {

        public event PointerClickHandler PointerClickHandler;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (PointerClickHandler != null)
            {
                PointerClickHandler(eventData);
            }
        }
    }
}

