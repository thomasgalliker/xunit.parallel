using System.Diagnostics;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Parallel.Sdk
{
    /// <summary>
    /// The default implementation of <see cref="T:Xunit.Abstractions.ITestCollection" />.
    /// DefaultTestCollection identifies test collections which are not created intentionally
    /// using the [Collection] annotation.
    /// </summary>
    [DebuggerDisplay("\\{ id = {UniqueID}, display = {DisplayName} \\}")]
    public class DefaultTestCollection : TestCollection
    {
        public DefaultTestCollection(ITestAssembly testAssembly, ITypeInfo collectionDefinition, string displayName)
            : base(testAssembly, collectionDefinition, displayName)
        {
        }
    }
}
