// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-12 18:17:29
// ========================================================

using LF.Fsm;
using UnityEditor;

namespace LF.Editor
{
    [CustomEditor(typeof(FsmManager))]
    internal sealed class FsmManagerInspector : LufyInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            FsmManager t = (FsmManager)target;

            EditorGUILayout.LabelField("FSM Count", t.Count.ToString());

            IFsm[] fsms = t.GetAllFsms();
            foreach (IFsm fsm in fsms)
            {
                DrawFsm(fsm);
            }

            Repaint();
        }

        private void OnEnable()
        {
        }

        private void DrawFsm(IFsm fsm)
        {
            EditorGUILayout.LabelField(fsm.FullName, fsm.IsRunning ? Utility.Text.Format("{0}, {1} s", fsm.CurrentStateName, fsm.CurrentStateTime.ToString("F1")) : (fsm.IsDestroyed ? "Destroyed" : "Not Running"));
        }
    }
}

