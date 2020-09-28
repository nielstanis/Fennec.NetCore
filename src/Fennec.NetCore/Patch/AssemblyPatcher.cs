using dnlib.DotNet;
using System;
using System.Linq;

namespace Fennec.NetCore.Patch
{
    public class AssemblyPatcher
    {
        public bool Patch(string assembly, string methodToReplace, string replaceAssembly, string replaceWithMethod)
        {
            if (string.IsNullOrEmpty(assembly))
            {
                throw new ArgumentException($"'{nameof(assembly)}' cannot be null or empty", nameof(assembly));
            }

            if (string.IsNullOrEmpty(methodToReplace))
            {
                throw new ArgumentException($"'{nameof(methodToReplace)}' cannot be null or empty", nameof(methodToReplace));
            }

            if (string.IsNullOrEmpty(replaceAssembly))
            {
                throw new ArgumentException($"'{nameof(replaceAssembly)}' cannot be null or empty", nameof(replaceAssembly));
            }

            if (string.IsNullOrEmpty(replaceWithMethod))
            {
                throw new ArgumentException($"'{nameof(replaceWithMethod)}' cannot be null or empty", nameof(replaceWithMethod));
            }

            bool result = false;

            if (System.IO.File.Exists(assembly) && System.IO.File.Exists(replaceAssembly))
            {
                try
                {
                    var targetAssembly = AssemblyDef.Load(assembly);
                    var sourceAssembly = AssemblyDef.Load(replaceAssembly);

                    var partsToFind = methodToReplace.Split("::");
                    var targetType = targetAssembly.Find(partsToFind[0], true);
                    var methodToFind = targetType.Methods.FirstOrDefault(x => x.FullName.Split(" ")[1] == methodToReplace);

                    var partsToReplace = replaceWithMethod.Split("::");
                    var sourceType = sourceAssembly.Find(partsToReplace[0], true);
                    var sourceMethod = sourceType.Methods.FirstOrDefault(x => x.FullName.Split(" ")[1] == replaceWithMethod);

                    Patch(methodToFind, sourceMethod);
                    targetAssembly.Write(assembly);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    result = false;
                }

            }

            return result;
        }

        private void Patch(MethodDef toBeReplaced, MethodDef replaceWith)
        {
            toBeReplaced.Body.Instructions.Clear();
            foreach (var inst in replaceWith.Body.Instructions)
            {
                toBeReplaced.Body.Instructions.Add(inst);
            }
           
        }
    }
}