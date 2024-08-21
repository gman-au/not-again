using System.Reflection;
using Not.Again.Infrastructure;
using Not.Again.Tests.Unit.Domain;

namespace Not.Again.Tests.Unit
{
    public class AssemblyMethodHasherTests
    {
        private readonly TestContext _context = new();

        [Fact]
        public void Calculate_Method_Hash()
        {
            _context.ArrangeAssembly();
            _context.ActHashMethods();
            _context.AssertHashes();
        }

        private class TestContext
        {
            private Assembly[] _assemblies;
            private long? _resultClassOneMethodOne;
            private long? _resultClassOneMethodTwo;
            private long? _resultClassTwoMethodOne;
            private long? _resultClassTwoMethodTwo;
            private long? _resultClassTwoMethodThree;
            private long? _resultClassOneMethodThree;

            public void ArrangeAssembly()
            {
                _assemblies = new[]
                {
                    Assembly
                        .GetAssembly(typeof(MySampleClassOne))
                };
            }

            public void ActHashMethods()
            {
                const string classNameOne = "Not.Again.Tests.Unit.Domain.MySampleClassOne";
                const string classNameTwo = "Not.Again.Tests.Unit.Domain.MySampleClassTwo";

                _resultClassOneMethodOne = AssemblyMethodHasher.CalculateTestMethodHash(
                    classNameOne,
                    "MyMethodOne",
                    _assemblies
                );
                _resultClassOneMethodTwo = AssemblyMethodHasher.CalculateTestMethodHash(
                    classNameOne,
                    "MyMethodTwo",
                    _assemblies
                );
                _resultClassOneMethodThree = AssemblyMethodHasher.CalculateTestMethodHash(
                    classNameOne,
                    "MyMethodThree",
                    _assemblies
                );
                
                _resultClassTwoMethodOne = AssemblyMethodHasher.CalculateTestMethodHash(
                    classNameTwo,
                    "MyMethodOne",
                    _assemblies
                );
                _resultClassTwoMethodTwo = AssemblyMethodHasher.CalculateTestMethodHash(
                    classNameTwo,
                    "MyMethodTwo",
                    _assemblies
                );
                _resultClassTwoMethodThree = AssemblyMethodHasher.CalculateTestMethodHash(
                    classNameTwo,
                    "MyMethodThree",
                    _assemblies
                );
            }

            public void AssertHashes()
            {
                Assert
                    .NotNull(_resultClassOneMethodOne);

                Assert
                    .NotNull(_resultClassOneMethodTwo);

                Assert
                    .Equal(
                        _resultClassOneMethodOne,
                        _resultClassOneMethodTwo
                    );

                Assert
                    .Equal(
                        _resultClassTwoMethodOne,
                        _resultClassTwoMethodTwo
                    );

                Assert
                    .Equal(
                        _resultClassOneMethodThree,
                        _resultClassTwoMethodThree
                    );

                Assert
                    .NotEqual(
                    _resultClassOneMethodOne,
                    _resultClassTwoMethodOne
                );
            }
        }
    }
}