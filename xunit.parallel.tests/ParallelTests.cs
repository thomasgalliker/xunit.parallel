using System.Threading;

using Xunit;

[assembly: TestFramework("xunit.parallel.Sdk.Framework.ParallelTestFramework", "xunit.parallel")]
[assembly: CollectionBehavior(DisableTestParallelization = false, MaxParallelThreads = 0)]

namespace xunit.parallel.tests
{
    public class ParallelTests
    {
        [Fact]
        public void FactMethodName1()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        public void FactMethodName2()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        public void FactMethodName3()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        public void FactMethodName4()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        public void FactMethodName5()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        public void FactMethodName6()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        public void FactMethodName7()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        public void FactMethodName8()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        public void FactMethodName9()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        public void FactMethodName10()
        {
            Thread.Sleep(1000);
        }
    }
}
