using System.Collections.Generic;

namespace Fennec.NetCore.Result
{
    public class ClassTypeResult
    {
        private readonly string _classtype;
        private readonly string _module;
        public AssemblyResult Assembly { get; }

        public ClassTypeResult(AssemblyResult assembly, string classtype, string module)
        {
            this.Assembly = assembly;
            Methods = new List<MethodResult>();

            _classtype = classtype;
            _module = module;
        }
        public List<MethodResult> Methods { get; private set; }
        public string ClassType { get { return _classtype; } }
    }
}