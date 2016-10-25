using System.Reflection;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Parallel.Sdk.Framework
{
    public class ParallelTestFramework : XunitTestFramework
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Xunit.Sdk.XunitTestFramework"/> class.
        /// </summary>
        /// <param name="messageSink">The message sink used to send diagnostic messages</param>
        public ParallelTestFramework(IMessageSink messageSink)
            : base(messageSink)
        {
        }

        /// <inheritdoc/>
        protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
        {
            return new ParallelTestFrameworkExecutor(assemblyName, this.SourceInformationProvider, this.DiagnosticMessageSink);
        }
    }
}
