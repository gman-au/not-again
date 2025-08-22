using System.Linq;

namespace Not.Again.Api.Host.Extensions
{
    public static class StringEx
    {
        public static string ToShortName(this string fullAssemblyName)
        {
            return
                fullAssemblyName?
                    .Split(',')
                    .FirstOrDefault();
        }
    }
}