// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-12 15:27:21
// ========================================================
using LF.Event;
using UnityEditor;

namespace LF.Editor
{
    [CustomEditor(typeof(EventManager))]
    public class EventManagerInspector : LufyInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            EventManager t = (EventManager)target;

            EditorGUILayout.LabelField("Event Handler Count", t.EventHandlerCount.ToString());
            EditorGUILayout.LabelField("Event Count", t.EventCount.ToString());

            Repaint();
        }

        private void OnEnable()
        {
        }
    }
}

