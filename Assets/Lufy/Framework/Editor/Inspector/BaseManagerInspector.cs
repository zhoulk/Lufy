// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-06 15:14:34
// ========================================================

using UnityEditor;

namespace LF.Editor
{
    [CustomEditor(typeof(BaseManager))]
    sealed class BaseManagerInspector : LufyInspector
    {
        private SerializedProperty m_FrameRate = null;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            BaseManager t = (BaseManager)target;

            int frameRate = EditorGUILayout.IntSlider("Frame Rate", m_FrameRate.intValue, 1, 120);
            if (frameRate != m_FrameRate.intValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.FrameRate = frameRate;
                }
                else
                {
                    m_FrameRate.intValue = frameRate;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            m_FrameRate = serializedObject.FindProperty("m_FrameRate");
        }
    }
}

