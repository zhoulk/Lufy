// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-12 18:42:58
// ========================================================

using LF.UI;
using UnityEditor;

namespace LF.Editor
{
    [CustomEditor(typeof(UIManager))]
    public class UIManagerInspector : LufyInspector
    {
        private SerializedProperty m_InstanceRoot = null;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            UIManager t = (UIManager)target;
            
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(m_InstanceRoot);
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.LabelField("UI Count", t.Count.ToString());

            Repaint();
        }

        private void OnEnable()
        {
            m_InstanceRoot = serializedObject.FindProperty("m_InstanceRoot");
        }
    }
}

