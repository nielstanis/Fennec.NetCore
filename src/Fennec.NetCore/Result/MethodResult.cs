using System.Collections.Generic;

namespace Fennec.NetCore.Result
{
    public class MethodResult
    {
        private readonly string _name;
        private readonly string _parameters;
        public ClassTypeResult ClassType { get; }

        public MethodResult(ClassTypeResult classType, string name, string parameters)
        {
            this.ClassType = classType;
            Invocations = new List<InvocationResult>();
            _name = name;
            _parameters = parameters;
        }
        public List<InvocationResult> Invocations { get; private set; }
        public string Name { get { return _name; } }
        public string Parameters { get { return _parameters; } }
    }
}