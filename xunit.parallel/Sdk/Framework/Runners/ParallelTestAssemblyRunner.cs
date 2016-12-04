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

        /// <inheritdoc/>
        protected override string GetTestFrameworkEnvironment()
        {
            var environment = base.GetTestFrameworkEnvironment().Replace("collection-per-class", "collection-per-method");
            return environment;
        }

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