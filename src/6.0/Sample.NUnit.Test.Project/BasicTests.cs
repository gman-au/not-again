using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Not.Again.NUnit;

namespace Sample.NUnit.Test.Project
{
    public class Tests
    {
        [SetUp]
        public async Task Setup() => await NotAgain.SetupAsync();

        [TearDown]
        public async Task Teardown() => await NotAgain.TearDownAsync();

        [Test]
        public void Test_A_Thing()
        {
            const string sample = "DJFH_#JSIJD!!";
            var regex = new Regex("#");

            var result =
                regex
                    .Split(sample);

            Assert
                .That(
                    result,
                    Has.Length.EqualTo(3)
                );
        }

        [Test]
        public void Test_Another_Thing()
        {
            Assert.Pass();
        }
    }
}