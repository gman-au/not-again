using System;
using System.Linq;
using System.Reflection;

namespace Not.Again.Infrastructure
{
    public static class AssemblyLoader
    {
        public static Assembly GetAssemblyFromClassName(string className)
        {
            var referencedAssemblies =
                AppDomain
                    .CurrentDomain
                    .GetAssemblies();

            var assembly =
                referencedAssemblies
                    .FirstOrDefault(
                        o =>
                            o.GetTypes()
                                .Any(t => t.FullName == className)
                    );

            return assembly;
        }

        public static Type GetTypeFromAssembly(string className)
        {
            var assembly =
                GetAssemblyFromClassName(className);

            if (assembly == null) return null;

            var type =
                assembly
                    .GetTypes()
                    .FirstOrDefault(o => o.FullName == className);

            return type;
        }
    }
}