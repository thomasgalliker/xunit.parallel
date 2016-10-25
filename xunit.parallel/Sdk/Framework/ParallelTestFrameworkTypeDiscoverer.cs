using System;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Parallel.Sdk.Framework
{
    public class ParallelTestFrameworkTypeDiscoverer : ITestFrameworkTypeDiscoverer
    {
        /// <summary>
        /// Gets the type that implements <see cref="T:Xunit.Abstractions.ITestFramework"/> to be used to discover
        ///             and run tests.
        /// </summary>
        /// <param name="attribute">The test framework attribute that decorated the assembly</param>
        /// <returns>
        /// The test framework type
        /// </returns>
        public Type GetTestFrameworkType(IAttributeInfo attribute)
        {
            return typeof(ParallelTestFramework);
        }
    }
}
