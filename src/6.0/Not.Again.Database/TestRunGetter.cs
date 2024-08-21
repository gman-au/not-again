using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Not.Again.Domain;
using Not.Again.Interfaces;

namespace Not.Again.Database
{
    public class TestRunGetter : ITestRunGetter
    {
        private readonly NotAgainDbContext _context;

        public TestRunGetter(NotAgainDbContext context)
        {
            _context = context;
        }

        public async Task<TestRun> GetLastRunAsync(Guid testRecordId)
        {
            var lastTestRun =
                await
                    _context
                        .TestRun
                        .Where(
                            o =>
                                o.TestRecordId == testRecordId
                        )
                        .OrderByDescending(o => o.RunDate)
                        .FirstOrDefaultAsync();

            return lastTestRun;
        }
    }
}