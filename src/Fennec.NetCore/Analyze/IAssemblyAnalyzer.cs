using Fennec.NetCore.Result;

namespace Fennec.NetCore.Analyze
{
    public interface IAssemblyAnalyzer
    {
        AssemblyResult Analyse();
    }
}