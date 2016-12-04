using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Parallel.Sdk.Framework.Runners
{
    public class ParallelTestCollectionRunner : XunitTestCollectionRunner
    {
        protected IMessageSink DiagnosticMessageSink { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Xunit.Sdk.XunitTestCollectionRunner" /> class.
        /// </summary>
        /// <param name="testCollection">The test collection that contains the tests to be run.</param>
        /// <param name="testCases">The test cases to be run.</param>
        /// <param name="diagnosticMessageSink">The message sink used to send diagnostic messages</param>
        /// <param name="messageBus">The message bus to report run status to.</param>
        /// <param name="testCaseOrderer">The test case orderer that will be used to decide how to order the test.</param>
        /// <param name="aggregator">The exception aggregator used to run code and collect exceptions.</param>
        /// <param name="cancellationTokenSource">The task cancellation token source, used to cancel the test run.</param>
        public ParallelTestCollectionRunner(
            ITestCollection testCollection,
            IEnumerable<IXunitTestCase> testCases,
            IMessageSink diagnosticMessageSink,
            IMessageBus messageBus,
            ITestCaseOrderer testCaseOrderer,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
            : base(testCollection, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource)
        {
            this.DiagnosticMessageSink = diagnosticMessageSink;
        }

        /// <inheritdoc />
        protected override async Task<RunSummary> RunTestClassAsync(ITestClass testClass, IReflectionTypeInfo @class, IEnumerable<IXunitTestCase> testCases)
        {
            Func<Func<Task<RunSummary>>, Task<RunSummary>> taskRunner;
            var cancellationTokenSource = this.CancellationTokenSource;
            if (SynchronizationContext.Current != null)
            {
                var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
                taskRunner = code => Task.Factory.StartNew(code, cancellationTokenSource.Token, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler, scheduler).Unwrap();
            }
            else
            {
                taskRunner = code => Task.Run(code, cancellationTokenSource.Token);
            }

            Task<RunSummary>[] tasks;
            if (this.TestCollection is DefaultTestCollection)
            {
                // If TestCollection is a DefaultTestCollection,
                // we parallelize each test case:
                tasks =
                    this.TestCases.Select(
                        testCase =>
                        taskRunner(
                            () =>
                            new ParallelTestClassRunner(
                                testClass,
                                @class,
                                new List<IXunitTestCase> { testCase },
                                this.DiagnosticMessageSink,
                                this.MessageBus,
                                this.TestCaseOrderer,
                                new ExceptionAggregator(this.Aggregator),
                                this.CancellationTokenSource,
                                this.CollectionFixtureMappings).RunAsync())).ToArray();
            }
            else
            {
                // If TestCollection is an intentionally defined collection
                // we must not parallelize the affected test cases:
                tasks = new[]
                            {
                                taskRunner(
                                    () =>
                                    new ParallelTestClassRunner(
                                        testClass,
                                        @class,
                                        this.TestCases,
                                        this.DiagnosticMessageSink,
                                        this.MessageBus,
                                        this.TestCaseOrderer,
                                        new ExceptionAggregator(this.Aggregator),
                                        this.CancellationTokenSource,
                                        this.CollectionFixtureMappings).RunAsync())
                            };
            }

            var summaries = new List<RunSummary>();

            foreach (var task in tasks)
            {
                try
                {
                    summaries.Add(await task);
                }
                catch (TaskCanceledException)
                {
                }
            }

            return new RunSummary
            {
                Total = summaries.Sum(s => s.Total),
                Failed = summaries.Sum(s => s.Failed),
                Skipped = summaries.Sum(s => s.Skipped)
            };
        }
    }
}