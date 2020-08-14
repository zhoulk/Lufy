/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2019/04/22
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Diagnostics;
using System.Security;
using System.Runtime.CompilerServices;

namespace LT
{
    /// <summary>
    /// Represents a builder for asynchronous methods that return a lttask
    /// </summary>
    public struct AsyncLTTaskMethodBuilder
    {
        private LTTaskCompletionSource tcs;
        private Action moveNext;

        /// <summary>Creates an instance of the <see cref="AsyncLTTaskMethodBuilder" /> class.</summary>
		/// <returns>A new instance of the builder.</returns>
        [DebuggerHidden]
        public static AsyncLTTaskMethodBuilder Create()
        {
            return new AsyncLTTaskMethodBuilder();
        }

        /// <summary>
        /// Begins running the builder with the associated state machine.
        /// </summary>
        /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
        /// <param name="stateMachine">The state machine instance, passed by reference.</param>
        [DebuggerHidden]
        [SecuritySafeCritical]
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            if (stateMachine == null)
            {
                throw new ArgumentNullException("stateMachine");
            }

            stateMachine.MoveNext();
        }

        /// <summary>
        /// Associates the builder with the specified state machine.
        /// </summary>
        /// <param name="stateMachine">The state machine instance to associate with the builder.</param>
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        /// <summary>
        /// Schedules the state machine to proceed to the next action when the specified awaiter completes.
        /// </summary>
        /// <typeparam name="TAwaiter">The awaiter.</typeparam>
        /// <typeparam name="TStateMachine">The state machine.</typeparam>
        /// <param name="awaiter">The type of the awaiter.</param>
        /// <param name="stateMachine">The type of the state machine.</param>
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            if (moveNext == null)
            {
                if (this.tcs == null)
                {
                    this.tcs = new LTTaskCompletionSource();
                }

                var runner = new MoveNextRunner<TStateMachine>();
                moveNext = runner.Run;
                runner.StateMachine = stateMachine;
            }

            awaiter.OnCompleted(moveNext);
        }

        /// <summary>
        /// Schedules the state machine to proceed to the next action when the specified awaiter completes. This method can be called from partially trusted code.
        /// </summary>
        /// <typeparam name="TAwaiter">The awaiter.</typeparam>
        /// <typeparam name="TStateMachine">The state machine.</typeparam>
        /// <param name="awaiter">The type of the awaiter.</param>
        /// <param name="stateMachine">The type of the state machine.</param>
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            if (moveNext == null)
            {
                if (this.tcs == null)
                {
                    this.tcs = new LTTaskCompletionSource();
                }

                var runner = new MoveNextRunner<TStateMachine>();
                moveNext = runner.Run;
                runner.StateMachine = stateMachine;
            }

            awaiter.UnsafeOnCompleted(moveNext);
        }

        /// <summary>
        /// Gets the task for this builder.
        /// </summary>
        public LTTask Task
        {
            get
            {
                if (this.tcs != null)
                {
                    return this.tcs.Task;
                }

                if (moveNext == null)
                {
                    return new LTTask();
                }

                this.tcs = new LTTaskCompletionSource();
                return this.tcs.Task;
            }
        }

        /// <summary>
        /// Marks the task as successfully completed.
        /// </summary>
        public void SetResult()
        {
            if (moveNext == null)
            {
            }
            else
            {
                if (this.tcs == null)
                {
                    this.tcs = new LTTaskCompletionSource();
                }

                this.tcs.TrySetResult();
            }
        }

        /// <summary>
        /// Marks the task as failed and binds the specified exception to the task.
        /// </summary>
        /// <param name="exception"></param>
        public void SetException(Exception exception)
        {
            if (this.tcs == null)
            {
                this.tcs = new LTTaskCompletionSource();
            }

            if (exception is OperationCanceledException ex)
            {
                this.tcs.TrySetCanceled(ex);
            }
            else
            {
                this.tcs.TrySetException(exception);
            }
        }
    }

    /// <summary>
    /// Represents a builder for asynchronous methods that return a lttask
    /// </summary>
    public struct AsyncLTTaskMethodBuilder<T>
    {
        T result;
        private LTTaskCompletionSource<T> tcs;
        private Action moveNext;

        /// <summary>Creates an instance of the <see cref="AsyncLTTaskMethodBuilder" /> class.</summary>
		/// <returns>A new instance of the builder.</returns>
        [DebuggerHidden]
        public static AsyncLTTaskMethodBuilder<T> Create()
        {
            return new AsyncLTTaskMethodBuilder<T>();
        }

        /// <summary>
        /// Begins running the builder with the associated state machine.
        /// </summary>
        /// <typeparam name="TStateMachine">The type of the state machine.</typeparam>
        /// <param name="stateMachine">The state machine instance, passed by reference.</param>
        [DebuggerHidden]
        [SecuritySafeCritical]
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            if (stateMachine == null)
            {
                throw new ArgumentNullException("stateMachine");
            }

            stateMachine.MoveNext();
        }

        /// <summary>
        /// Associates the builder with the specified state machine.
        /// </summary>
        /// <param name="stateMachine">The state machine instance to associate with the builder.</param>
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }

        /// <summary>
        /// Schedules the state machine to proceed to the next action when the specified awaiter completes.
        /// </summary>
        /// <typeparam name="TAwaiter">The awaiter.</typeparam>
        /// <typeparam name="TStateMachine">The state machine.</typeparam>
        /// <param name="awaiter">The type of the awaiter.</param>
        /// <param name="stateMachine">The type of the state machine.</param>
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            if (moveNext == null)
            {
                if (this.tcs == null)
                {
                    this.tcs = new LTTaskCompletionSource<T>();
                }

                var runner = new MoveNextRunner<TStateMachine>();
                moveNext = runner.Run;
                runner.StateMachine = stateMachine;
            }

            awaiter.OnCompleted(moveNext);
        }

        /// <summary>
        /// Schedules the state machine to proceed to the next action when the specified awaiter completes. This method can be called from partially trusted code.
        /// </summary>
        /// <typeparam name="TAwaiter">The awaiter.</typeparam>
        /// <typeparam name="TStateMachine">The state machine.</typeparam>
        /// <param name="awaiter">The type of the awaiter.</param>
        /// <param name="stateMachine">The type of the state machine.</param>
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            if (moveNext == null)
            {
                if (this.tcs == null)
                {
                    this.tcs = new LTTaskCompletionSource<T>();
                }

                var runner = new MoveNextRunner<TStateMachine>();
                moveNext = runner.Run;
                runner.StateMachine = stateMachine;
            }

            awaiter.UnsafeOnCompleted(moveNext);
        }

        /// <summary>
        /// Gets the task for this builder.
        /// </summary>
        public LTTask<T> Task
        {
            get
            {
                if (this.tcs != null)
                {
                    return new LTTask<T>(this.tcs);
                }

                if (moveNext == null)
                {
                    return new LTTask<T>(result);
                }

                this.tcs = new LTTaskCompletionSource<T>();
                return this.tcs.Task;
            }
        }

        /// <summary>
        /// Marks the task as successfully completed.
        /// </summary>
        public void SetResult(T ret)
        {
            if (moveNext == null)
            {
                this.result = ret;
            }
            else
            {
                if (this.tcs == null)
                {
                    this.tcs = new LTTaskCompletionSource<T>();
                }

                this.tcs.TrySetResult(ret);
            }
        }

        /// <summary>
        /// Marks the task as failed and binds the specified exception to the task.
        /// </summary>
        /// <param name="exception"></param>
        public void SetException(Exception exception)
        {
            if (this.tcs == null)
            {
                this.tcs = new LTTaskCompletionSource<T>();
            }

            if (exception is OperationCanceledException ex)
            {
                this.tcs.TrySetCanceled(ex);
            }
            else
            {
                this.tcs.TrySetException(exception);
            }
        }
    }
}