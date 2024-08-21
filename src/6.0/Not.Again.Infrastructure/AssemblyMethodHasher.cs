using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Mono.Cecil;

namespace Not.Again.Infrastructure
{
    public static class AssemblyMethodHasher
    {
        public static long? CalculateTestMethodHash(
            string className,
            string methodName, 
            Assembly[] assemblies = null
        )
        {
            var assembly =
                AssemblyLoader
                    .GetAssemblyFromClassName(className, assemblies);

            var methodDefinition =
                GetMethodDefinitionFromAssembly(
                    assembly,
                    className,
                    methodName
                );

            return
                CalculateHash(methodDefinition);
        }

        internal static MethodDefinition GetMethodDefinitionFromAssembly(
            Assembly assembly,
            string className,
            string methodName
        )
        {
            var assemblyDefinition =
                AssemblyDefinition
                    .ReadAssembly(assembly.Location);

            var methodDefinition =
                assemblyDefinition
                    .Modules
                    .SelectMany(
                        o =>
                            o.GetTypes()
                                .Where(t => t.FullName == className)
                                .SelectMany(m => m.Methods)
                                .Where(e => e.Name == methodName)
                    )
                    .FirstOrDefault();

            return methodDefinition;
        }

        private static long? CalculateHash(MethodDefinition methodDefinition)
        {
            if (methodDefinition == null) return null;

            var instructions =
                methodDefinition
                    .Body
                    .Instructions;

            var hash = 777L;

            foreach (var instruction in instructions)
            {
                hash ^= GetDeterministicHash(instruction.OpCode.ToString());

                if (instruction.Operand is MethodReference mr)
                {
                    hash ^= GetDeterministicHash(mr.FullName);
                }
            }

            return hash;
        }

        private static long GetDeterministicHash(string value)
        {
            var stringBytes =
                Encoding
                    .UTF8
                    .GetBytes(value);

            var hashedBytes =
                new SHA1CryptoServiceProvider()
                    .ComputeHash(stringBytes);

            Array
                .Resize(
                    ref hashedBytes,
                    8
                );

            return
                BitConverter
                    .ToInt64(hashedBytes);
        }
    }
}