using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit.Parallel.Sdk
{
    public class CollectionPerMethodTestCollectionFactory : IXunitTestCollectionFactory
    {
        private readonly Dictionary<string, ITypeInfo> collectionDefinitions;
        private readonly ITestAssembly testAssembly;
        private readonly ConcurrentDictionary<string, ITestCollection> testCollections = new ConcurrentDictionary<string, ITestCollection>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionPerMethodTestCollectionFactory" /> class.
        /// </summary>
        /// <param name="testAssembly">The assembly info.</param>
        /// <param name="diagnosticMessageSink">The message sink used to send diagnostic messages</param>
        public CollectionPerMethodTestCollectionFactory(ITestAssembly testAssembly, IMessageSink diagnosticMessageSink)
        {
            this.testAssembly = testAssembly;

            this.collectionDefinitions = TestCollectionFactoryHelper.GetTestCollectionDefinitions(testAssembly.Assembly, diagnosticMessageSink);
        }

        public ITestCollection Get(ITypeInfo testClass)
        {
            string collectionName;
            var collectionAttribute = testClass.GetCustomAttributes(typeof(CollectionAttribute)).SingleOrDefault();
            if (collectionAttribute == null)
            {
                collectionName = "Default collection for " + testClass.Name;
                return this.testCollections.GetOrAdd(collectionName, this.CreateDefaultCollection);
            }
            else
            {
                collectionName = (string)collectionAttribute.GetConstructorArguments().First();
                return this.testCollections.GetOrAdd(collectionName, this.CreateCollection);
            }
        }

        private ITestCollection CreateCollection(string name)
        {
            ITypeInfo definitionType;
            this.collectionDefinitions.TryGetValue(name, out definitionType);
            return new TestCollection(this.testAssembly, definitionType, name);
        }

        private ITestCollection CreateDefaultCollection(string name)
        {
            ITypeInfo definitionType;
            this.collectionDefinitions.TryGetValue(name, out definitionType);
            return new DefaultTestCollection(this.testAssembly, definitionType, name);
        }

        /// <inheritdoc />
        public string DisplayName
        {
            get
            {
                return "collection-per-method";
            }
        }
    }
}