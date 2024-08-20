using System.Collections.Generic;
using System.Linq;
using Not.Again.Interfaces;

namespace Not.Again.Database
{
    public class ArgumentDelimiter : IArgumentDelimiter
    {
        public string Perform(IEnumerable<object> arguments)
        {
            return
                string
                    .Join(
                        "|",
                        arguments
                            .Select(o => o.ToString())
                            .OrderBy(o => o)
                    );
        }
    }
}