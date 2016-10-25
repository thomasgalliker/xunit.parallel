﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Parallel.Sdk.Framework.Runners
{
    public class ParallelTestInvoker : XunitTestInvoker
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Xunit.Sdk.XunitTestInvoker"/> class.
        /// </summary>
        /// <param name="test">The test that this invocation belongs to.</param><param name="messageBus">The message bus to report run status to.</param><param name="testClass">The test class that the test method belongs to.</param><param name="constructorArguments">The arguments to be passed to the test class constructor.</param><param name="testMethod">The test method that will be invoked.</param><param name="testMethodArguments">The arguments to be passed to the test method.</param><param name="beforeAfterAttributes">The list of <see cref="T:Xunit.Sdk.BeforeAfterTestAttribute"/>s for this test invocation.</param><param name="aggregator">The exception aggregator used to run code and collect exceptions.</param><param name="cancellationTokenSource">The task cancellation token source, used to cancel the test run.</param>
        public ParallelTestInvoker(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
            : base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, beforeAfterAttributes, aggregator, cancellationTokenSource)
        {
        }

        /// <inheritdoc/>
        protected override async Task<decimal> InvokeTestMethodAsync(object testClassInstance)
        {
            var oldSyncContext = SynchronizationContext.Current;

            try
            {
                var asyncSyncContext = new AsyncTestSyncContext(oldSyncContext);
                SetSynchronizationContext(asyncSyncContext);

                await this.Aggregator.RunAsync(
                    () => this.Timer.AggregateAsync(
                        async () =>
                        {
                            var parameterCount = this.TestMethod.GetParameters().Length;
                            var valueCount = this.TestMethodArguments == null ? 0 : this.TestMethodArguments.Length;
                            if (parameterCount != valueCount)
                            {
                                this.Aggregator.Add(
                                    new InvalidOperationException(
                                        $"The test method expected {parameterCount} parameter value{(parameterCount == 1 ? "" : "s")}," + 
                                        $"but {valueCount} parameter value{(valueCount == 1 ? "" : "s")} {(valueCount == 1 ? "was" : "were")} provided.")
                                );
                            }
                            else
                            {
                                var result = this.TestMethod.Invoke(testClassInstance, this.TestMethodArguments);
                                var task = result as Task;
                                if (task != null)
                                    await task;
                                else
                                {
                                    var ex = await asyncSyncContext.WaitForCompletionAsync();
                                    if (ex != null)
                                        this.Aggregator.Add(ex);
                                }
                            }
                        }
                    )
                );
                
                //if (Aggregator.HasExceptions)
                //{
                //    var handleTestFailure = testClassInstance as INeedToKnowTestFailure;
                //    if (handleTestFailure != null)
                //    {
                //        await
                //            Aggregator.RunAsync(
                //                () => Timer.AggregateAsync(
                //                    () => handleTestFailure.HandleFailureAsync(Test, Aggregator.ToException())));
                //    }
                //}
            }
            finally
            {
                SetSynchronizationContext(oldSyncContext);
            }

            return this.Timer.Total;
        }

        [SecuritySafeCritical]
        static void SetSynchronizationContext(SynchronizationContext context)
        {
            SynchronizationContext.SetSynchronizationContext(context);
        }
    }
}
