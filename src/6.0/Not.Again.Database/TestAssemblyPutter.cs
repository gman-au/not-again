using System.Threading.Tasks;
using Not.Again.Domain;
using Not.Again.Interfaces;

namespace Not.Again.Database
{
    public class TestAssemblyPutter : ITestAssemblyPutter
    {
        private const string UnknownTestRunner = "Unknown";
        
        private readonly NotAgainDbContext _context;   
        private readonly ITestAssemblyGetter _testAssemblyGetter;

        public TestAssemblyPutter(
            NotAgainDbContext context,
            ITestAssemblyGetter testAssemblyGetter)
        {
            _testAssemblyGetter = testAssemblyGetter;
            _context = context;
        }

        public async Task<TestAssembly> AddOrUpdateTestAssemblyAsync(
            string assemblyName,
            string testRunner
        )
        {
            testRunner = string.IsNullOrEmpty(testRunner) ? UnknownTestRunner : testRunner;

            var testAssembly = new TestAssembly
            {
                TestAssemblyName = assemblyName,
                TestRunner = testRunner
            };

            var dbTestAssembly =
                await
                    _testAssemblyGetter
                        .GetAsync(testAssembly);

            if (dbTestAssembly == null)
            {
                dbTestAssembly =
                    (await
                        _context
                            .TestAssembly
                            .AddAsync(testAssembly))
                    .Entity;

                await
                    _context
                        .SaveChangesAsync();
            }

            return dbTestAssembly;
        }
    }
}