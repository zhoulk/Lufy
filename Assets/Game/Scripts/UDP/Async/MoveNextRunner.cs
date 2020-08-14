/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/23
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System.Runtime.CompilerServices;

namespace LT
{
    internal class MoveNextRunner<TStateMachine> where TStateMachine : IAsyncStateMachine
    {
        public TStateMachine StateMachine;

        public void Run()
        {
            StateMachine.MoveNext();
        }
    }
}