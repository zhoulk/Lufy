// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-19 17:30:03
// ========================================================
using LF.Setting;
using UnityEditor;
using UnityEngine;

namespace LF.Editor
{
    [CustomEditor(typeof(SettingManager))]
    public class SettingManagerInspector : LufyInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SettingManager t = (SettingManager)target;

            //EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            //{
            //    m_SettingHelperInfo.Draw();
            //}
            //EditorGUI.EndDisabledGroup();

            string[] keys = t.Keys();
            DrawSetting(keys);

            if (EditorApplication.isPlaying)
            {
                if (GUILayout.Button("Save Settings"))
                {
                    t.Save();
                }
                if (GUILayout.Button("Remove All Settings"))
                {
                    t.RemoveAllSettings();
                }
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        private void DrawSetting(string[] keys)
        {
            EditorGUILayout.BeginVertical("box");
            {
                if (keys.Length > 0)
                {
                    EditorGUILayout.LabelField("Name");
                    foreach (string key in keys)
                    {
                        EditorGUILayout.LabelField(string.IsNullOrEmpty(key) ? "<None>" : key);
                    }
                }
                else
                {
                    GUILayout.Label("Setting is Empty ...");
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();
        }
    }
}

