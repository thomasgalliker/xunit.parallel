using System.Threading;

using Xunit;

namespace xunit.parallel.tests
{
    public class ParallelUnitTests
    {
        [Fact]
        [Trait("Category", "UnitTest")]
        public void UnitTestFactMethodName1()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void UnitTestFactMethodName2()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void UnitTestFactMethodName3()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void UnitTestFactMethodName4()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void UnitTestFactMethodName5()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void UnitTestFactMethodName6()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        [Trait("Category", "UnitTest")]
        public void UnitTestFactMethodName7()
        {
            Thread.Sleep(1000);
        }
    }

    [Collection("IntegrationTests")]
    public class ParallelIntegrationTests
    {
        [Fact]
        [Trait("Category", "IntegrationTest")]
        public void IntegrationTestFactMethod1()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        [Trait("Category", "IntegrationTest")]
        public void IntegrationTestFactMethod2()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        [Trait("Category", "IntegrationTest")]
        public void IntegrationTestFactMethod3()
        {
            Thread.Sleep(1000);
        }

        [Fact]
        [Trait("Category", "IntegrationTest")]
        public void IntegrationTestFactMethod4()
        {
            Thread.Sleep(1000);
        }
    }
}
