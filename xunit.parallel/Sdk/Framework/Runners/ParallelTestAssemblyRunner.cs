using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Parallel.Sdk.Framework.Runners
{
    public class ParallelTestAssemblyRunner : XunitTestAssemblyRunner
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Xunit.Sdk.XunitTestAssemblyRunner" /> class.
        /// </summary>
        /// <param name="testAssembly">The assembly that contains the tests to be run.</param>
        /// <param name="testCases">The test cases to be run.</param>
        /// <param name="diagnosticMessageSink">The message sink to report diagnostic messages to.</param>
        /// <param name="executionMessageSink">The message sink to report run status to.</param>
        /// <param name="executionOptions">The user's requested execution options.</param>
        public ParallelTestAssemblyRunner(
            ITestAssembly testAssembly,
            IEnumerable<IXunitTestCase> testCases,
            IMessageSink diagnosticMessageSink,
            IMessageSink executionMessageSink,
            ITestFrameworkExecutionOptions executionOptions)
            : base(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions)
        {
        }

        //protected override async Task<RunSummary> RunTestCollectionsAsync(IMessageBus messageBus, CancellationTokenSource cancellationTokenSource)
        //{
        //    this.originalSyncContext = SynchronizationContext.Current;
        //    if (this.disableParallelization)
        //        return await base.RunTestCollectionsAsync(messageBus, cancellationTokenSource);

        //    this.SetupSyncContext(2);
        //    Func<Func<Task<RunSummary>>, Task<RunSummary>> taskRunner;
        //    if (SynchronizationContext.Current != null)
        //    {
        //        TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        //        taskRunner = (Func<Func<Task<RunSummary>>, Task<RunSummary>>)(code => Task.Factory.StartNew<Task<RunSummary>>(code, cancellationTokenSource.Token, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler, scheduler).Unwrap<RunSummary>());
        //    }
        //    else
        //        taskRunner = (Func<Func<Task<RunSummary>>, Task<RunSummary>>)(code => Task.Run<RunSummary>(code, cancellationTokenSource.Token));
        //    Task<RunSummary>[] array = this.OrderTestCollections().Select<Tuple<ITestCollection, List<IXunitTestCase>>, Task<RunSummary>>((Func<Tuple<ITestCollection, List<IXunitTestCase>>, Task<RunSummary>>)(collection => taskRunner((Func<Task<RunSummary>>)(() => this.RunTestCollectionAsync(messageBus, collection.Item1, (IEnumerable<IXunitTestCase>)collection.Item2, cancellationTokenSource))))).ToArray<Task<RunSummary>>();
        //    List<RunSummary> summaries = new List<RunSummary>();
        //    Task<RunSummary>[] taskArray = array;
        //    for (int index = 0; index < taskArray.Length; ++index)
        //    {
        //        Task<RunSummary> task = taskArray[index];
        //        try
        //        {
        //            List<RunSummary> runSummaryList = summaries;
        //            RunSummary runSummary = await task;
        //            runSummaryList.Add(runSummary);
        //            runSummaryList = (List<RunSummary>)null;
        //        }
        //        catch (TaskCanceledException ex)
        //        {
        //        }
        //    }
        //    taskArray = (Task<RunSummary>[])null;
        //    return new RunSummary()
        //    {
        //        Total = summaries.Sum<RunSummary>((Func<RunSummary, int>)(s => s.Total)),
        //        Failed = summaries.Sum<RunSummary>((Func<RunSummary, int>)(s => s.Failed)),
        //        Skipped = summaries.Sum<RunSummary>((Func<RunSummary, int>)(s => s.Skipped))
        //    };
        //}
        /// <inheritdoc />
        /// <inheritdoc />
        protected override Task<RunSummary> RunTestCollectionAsync(
            IMessageBus messageBus,
            ITestCollection testCollection,
            IEnumerable<IXunitTestCase> testCases,
            CancellationTokenSource cancellationTokenSource)
        {
            return
                new ParallelTestCollectionRunner(
                    testCollection,
                    testCases,
                    this.DiagnosticMessageSink,
                    messageBus,
                    this.TestCaseOrderer,
                    new ExceptionAggregator(this.Aggregator),
                    cancellationTokenSource).RunAsync();
        }
    }
}