using System.Threading.Tasks;
using Not.Again.NUnit;

namespace Sample.NUnit.Test.Project
{
    public class TestCases
    {
        [SetUp]
        public async Task Setup() => await NotAgain.SetupAsync();

        [TearDown]
        public async Task Teardown() => await NotAgain.TearDownAsync();

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void TestCase(long index)
        {
            Assert
                .Pass($"Index: {index}");
        }
    }
}