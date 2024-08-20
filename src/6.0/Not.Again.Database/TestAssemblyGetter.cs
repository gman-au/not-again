using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Not.Again.Domain;
using Not.Again.Interfaces;

namespace Not.Again.Database
{
    public class TestAssemblyGetter : ITestAssemblyGetter
    {
        private readonly NotAgainDbContext _context;

        public TestAssemblyGetter(NotAgainDbContext context)
        {
            _context = context;
        }

        public async Task<TestAssembly> GetAsync(TestAssembly testAssembly)
        {
            try
            {
                var dbTestAssembly =
                    await
                        _context
                            .TestAssembly
                            .FirstOrDefaultAsync(
                                o =>
                                    EF.Functions.Like(
                                        o.TestAssemblyName,
                                        testAssembly.TestAssemblyName
                                    )
                            );

                return dbTestAssembly;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}