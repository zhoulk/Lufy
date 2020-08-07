// ========================================================
// 描述：
// 作者：Lufy 
// 创建时间：2020-08-06 17:33:00
// ========================================================

using LF.Fsm;
using System;
using UnityEngine;

namespace LF.Procedure
{
    /// <summary>
    /// 流程管理类
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Lufy/Procedure")]
    public sealed class ProcedureManager : LufyManager
    {
        private FsmManager m_FsmManager;
        private IFsm<ProcedureManager> m_ProcedureFsm;

        /// <summary>
        /// 初始化流程管理器。
        /// </summary>
        /// <param name="fsmManager">有限状态机管理器。</param>
        /// <param name="procedures">流程管理器包含的流程。</param>
        public void Initialize(FsmManager fsmManager, params ProcedureBase[] procedures)
        {
            if (fsmManager == null)
            {
                throw new LufyException("FSM manager is invalid.");
            }

            m_FsmManager = fsmManager;
            m_ProcedureFsm = m_FsmManager.CreateFsm(this, procedures);
        }

        /// <summary>
        /// 开始流程。
        /// </summary>
        /// <param name="procedureType">要开始的流程类型。</param>
        public void StartProcedure(Type procedureType)
        {
            if (m_ProcedureFsm == null)
            {
                throw new LufyException("You must initialize procedure first.");
            }

            m_ProcedureFsm.Start(procedureType);
        }

        internal override void Shutdown()
        {
            if (m_FsmManager != null)
            {
                if (m_ProcedureFsm != null)
                {
                    m_FsmManager.DestroyFsm(m_ProcedureFsm);
                    m_ProcedureFsm = null;
                }

                m_FsmManager = null;
            }
        }

        internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            
        }
    }
}

