using System;
using System.Linq;
using System.Collections.Generic;
using Fennec.NetCore.Result;

namespace Fennec.NetCore.Analyze
{
    public static class AssemblyResultExtensions
    {
        public static Stack<string> GetFirstExecutionPath(this AssemblyResult assemblyResult, string invocation)
        {
            return GetExecutionPaths(assemblyResult, invocation).First();
        }

        public static List<Stack<string>> GetExecutionPaths(this AssemblyResult assemblyResult, string invocation)
        {
            var result = new List<Stack<string>>();
            var found = assemblyResult.Invocations.FindAll(x => x.Invocation == invocation);
            if (found.Count > 0)
            {
                foreach (var f in found)
                {
                    Stack<string> foundPath = new Stack<string>();
                    foundPath.Push(invocation);
                    GetExecutionPathImp(assemblyResult, f.Method, foundPath, result);
                    result.Add(foundPath);
                }
            }
            return result;
        }

        private static void GetExecutionPathImp(this AssemblyResult assemblyResult, MethodResult method, Stack<string> currentStack, List<Stack<string>> result)
        {
            string invocation = $"{method.ClassType.ClassType}::{method.Name}({method.Parameters})";

            if (currentStack.Contains(invocation)) //Break if we already have this one in the list.. 
                return;

            currentStack.Push(invocation);
            var found = assemblyResult.Invocations.FindAll(x => x.Invocation == invocation);
            var retainStack = currentStack.Clone();

            for (int i = 0; i < found.Count; i++)
            {
                var f = found[i];
                if (i == 0)
                {
                    GetExecutionPathImp(assemblyResult, f.Method, currentStack, result);
                }
                else
                {
                    var newStack = retainStack.Clone();
                    GetExecutionPathImp(assemblyResult, f.Method, newStack, result);
                    result.Add(newStack);
                }
            }
            return;
        }

        public static Stack<T> Clone<T>(this Stack<T> stack)
        {
            return new Stack<T>(new Stack<T>(stack));
        }
    }
}