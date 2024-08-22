using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Not.Again.Domain;
using Not.Again.Interfaces;

namespace Not.Again.Database
{
    public class TestRunPutter : ITestRunPutter
    {
        private readonly NotAgainDbContext _context;

        public TestRunPutter(NotAgainDbContext context)
        {
            _context = context;
        }

        public async Task<TestRun> AddTestResultAsync(
            Guid testRecordId,
            DateTime runDate,
            int result,
            long duration
        )
        {
            var testRun = new TestRun
            {
                TestRecordId = testRecordId,
                RunDate = runDate,
                Result = result,
                TotalDuration = duration
            };

            var dbTestRun =
                (await
                    _context
                        .TestRun
                        .AddAsync(testRun))
                .Entity;

            await
                _context
                    .SaveChangesAsync();

            return dbTestRun;
        }
    }
}