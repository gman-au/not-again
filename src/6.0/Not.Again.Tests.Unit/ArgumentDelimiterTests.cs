using System;
using System.Collections.Generic;
using Not.Again.Database;

namespace Not.Again.Tests.Unit
{
    public class ArgumentDelimiterTests
    {
        private readonly TestContext _context = new();

        [Fact]
        public void Test_Null_Arguments()
        {
            _context.ArrangeNullArguments();
            _context.ActSplit();
            _context.AssertEmptyResult();
        }

        [Fact]
        public void Test_Mixed_Arguments()
        {
            _context.ArrangeMixedArguments();
            _context.ActSplit();
            _context.AssertMixedResult();
        }

        private class TestContext
        {
            private readonly ArgumentDelimiter _sut;
            private IEnumerable<object> _value;
            private string _result;

            public TestContext()
            {
                _sut = new ArgumentDelimiter();
            }

            public void ArrangeNullArguments() => _value = null;
            
            public void ArrangeMixedArguments() => _value = new List<object>
            {
                "John Smith",
                3948,
                0x1B,
                90.928348F
            };
            
            public void ActSplit() => _result = _sut.Perform(_value);

            public void AssertEmptyResult() => Assert.True(string.IsNullOrEmpty(_result));
            
            public void AssertMixedResult() => Assert.Equal("27|3948|90.928345|John Smith", _result);
        }
    }
}