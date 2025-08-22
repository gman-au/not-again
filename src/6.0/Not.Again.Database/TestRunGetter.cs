using System;
using System.Collections.Generic;
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
        public async Task<IEnumerable<TestRun>> GetTestRunsAsync(
            Guid? assemblyId,
            Guid? testRecordId
            )
        {
            var lastTestRun =
                await
                    _context
                        .TestRun
                        .Where(o =>
                            (!assemblyId.HasValue || assemblyId.Value == o.TestRecord.TestAssembly.TestAssemblyId) &&
                            (!testRecordId.HasValue || testRecordId.Value == o.TestRecord.TestRecordId))
                        .Include(o => o.TestRecord)
                        .ThenInclude(o => o.TestAssembly)
                        .OrderByDescending(o => o.RunDate)
                        .ToListAsync();

            return lastTestRun;
        }

    }
}