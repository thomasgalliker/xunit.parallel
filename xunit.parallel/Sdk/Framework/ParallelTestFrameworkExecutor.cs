using System.Collections.Generic;
using System.Reflection;

using Xunit.Abstractions;
using Xunit.Parallel.Sdk.Framework.Runners;
using Xunit.Sdk;

namespace Xunit.Parallel.Sdk.Framework
{
    public class ParallelTestFrameworkExecutor : XunitTestFrameworkExecutor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Xunit.Sdk.XunitTestFrameworkExecutor"/> class.
        /// </summary>
        /// <param name="assemblyName">Name of the test assembly.</param><param name="sourceInformationProvider">The source line number information provider.</param><param name="diagnosticMessageSink">The message sink to report diagnostic messages to.</param>
        public ParallelTestFrameworkExecutor(
            AssemblyName assemblyName,
            ISourceInformationProvider sourceInformationProvider,
            IMessageSink diagnosticMessageSink)
            : base(assemblyName, sourceInformationProvider, diagnosticMessageSink)
        {
        }

        /// <inheritdoc/>
        protected override async void RunTestCases(
            IEnumerable<IXunitTestCase> testCases,
            IMessageSink executionMessageSink,
            ITestFrameworkExecutionOptions executionOptions)
        {
            using (var assemblyRunner = this.CreateTestAssemblyRunner(testCases, executionMessageSink, executionOptions))
            {
                await assemblyRunner.RunAsync();
            }
        }

        protected virtual TestAssemblyRunner<IXunitTestCase> CreateTestAssemblyRunner(
            IEnumerable<IXunitTestCase> testCases,
            IMessageSink executionMessageSink,
            ITestFrameworkExecutionOptions executionOptions)
        {
            return new ParallelTestAssemblyRunner(this.TestAssembly, testCases, this.DiagnosticMessageSink, executionMessageSink, executionOptions);
        }
    }
}
