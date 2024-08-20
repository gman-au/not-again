using System.Collections.Generic;

namespace Not.Again.Interfaces
{
    public interface IArgumentDelimiter
    {
        string Perform(IEnumerable<object> arguments);
    }
}