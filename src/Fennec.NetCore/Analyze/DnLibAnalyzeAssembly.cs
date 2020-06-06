using System;
using System.IO;
using System.Linq;
using dnlib.DotNet.Emit;
using Fennec.NetCore.Result;

namespace Fennec.NetCore.Analyze
{
    public class DnLibAssemblyAnalyzer : IAssemblyAnalyzer
    {
        private readonly string _assembly;

        public DnLibAssemblyAnalyzer(string assembly)
        {
            _assembly = assembly;
        }

        public AssemblyResult Analyse()
        {
            try
            {
                var ass = dnlib.DotNet.AssemblyDef.Load(_assembly);
                {
                    var assembly = new AssemblyResult(ass.FullName, _assembly);

                    foreach (var module in ass.Modules.OrderBy(m => m.Name))
                    foreach (var classType in module.GetTypes().OrderBy(z => z.FullName).Where(z => !z.IsInterface))
                    {
                        var typeResult = new ClassTypeResult(classType.FullName, classType.Module.Name);
                        foreach (var method in classType.Methods.Where(e => !e.IsAbstract).OrderBy(e => e.FullName))
                        {
                            int seq = 0;
                            if (method.HasBody)
                            {
                                string parameters = String.Join(",", method.Parameters.Select(x => x.Type));
                                var methodResult = new MethodResult(method.Name, parameters);                       
                                
                                foreach (var instruction in method.Body.Instructions
                                    .Where(u => ((u.OpCode == OpCodes.Call)) 
                                    || (u.OpCode == OpCodes.Callvirt) 
                                    || (u.OpCode == OpCodes.Calli)
                                    || (u.OpCode == OpCodes.Newobj)
                                    ))
                                {
                                    var splits = instruction.Operand.ToString().Split(" ");
                                    var invoke = new InvocationResult(splits[1], splits[0], seq++);
                                    methodResult.Invocations.Add(invoke);
                                }
                                typeResult.Methods.Add(methodResult);
                            }
                        }
                        assembly.Types.Add(typeResult);
                    }
                    return assembly;
                }
            }
            catch (Exception ex)
            {
                var err = new AssemblyResult("NotAvailable", _assembly);
                err.HandleException(ex);
                return err;
            }
        }
    }
}