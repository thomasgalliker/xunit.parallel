using System.Threading;
using System.Threading.Tasks;

using Xunit.Abstractions;
using Xunit.Parallel.Sdk.Framework.Runners;
using Xunit.Sdk;

namespace Xunit.Parallel.Sdk.Framework
{
    public class ParallelTestCase : XunitTestCase
    {
        public ParallelTestCase(
            IMessageSink diagnosticMessageSink,
            TestMethodDisplay defaultMethodDisplay,
            ITestMethod testMethod,
            object[] testMethodArguments = null)
            : base(diagnosticMessageSink, defaultMethodDisplay, testMethod, testMethodArguments)
        {
        }

        /// <inheritdoc/>
        public override Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
                                                 IMessageBus messageBus,
                                                 object[] constructorArguments,
                                                 ExceptionAggregator aggregator,
                                                 CancellationTokenSource cancellationTokenSource)
            => new ParallelTestCaseRunner(this, this.DisplayName, this.SkipReason, constructorArguments, this.TestMethodArguments, messageBus, aggregator, cancellationTokenSource).RunAsync();
    }
}
