using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Fennec.NetCore.Result
{
    public class AssemblyResult
    {
        private readonly string _assembly;
        private readonly string _filePath;
        public string Assembly { get { return _assembly; } }
        [JsonIgnore]
        public string FilePath { get { return _filePath; }}
        public AssemblyResult(string assembly, string filePath)
        {
            _assembly = assembly;
            _filePath = filePath;
            Types = new List<ClassTypeResult>();
            Invocations = new List<InvocationResult>();
        }

        public List<ClassTypeResult> Types {get; private set;}
        [JsonIgnore]
        public List<InvocationResult> Invocations {get; private set;}

        [JsonIgnore]
        public System.Exception ExceptionOccurred { get; private set;} 
        public void HandleException(System.Exception ex)
        {
            this.ExceptionOccurred = ex;
        }
        [JsonIgnore]
        public bool HasError
        {
            get
            {
                return this.ExceptionOccurred != null;
            }
        }
    }
}