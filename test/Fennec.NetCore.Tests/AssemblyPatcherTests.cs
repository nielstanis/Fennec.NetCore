
using Xunit;
using Fennec.NetCore.Analyze;
using Xunit.Abstractions;
using Fennec.NetCore.Patch;
using Fennec.NetCore.Output;

namespace Fennec.NetCore.Tests
{
    public class AssemblyPatcherTests
    {
        private readonly ITestOutputHelper _output;

        public AssemblyPatcherTests(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void BasicConsolePatchTest()
        {
            var x = TestResources.GetTestProjectAssembly("ComplexLibrary");
            var d = new DnLibAssemblyAnalyzer();


            var preassemblyResult = d.Analyse(x);

            var ss = new AssemblyPatcher();
            ss.Patch(x, "ComplexLibrary.FirstClass::MethodOne(System.String,System.String)", x, "ComplexLibrary.SecondClass::ThisWillThrow()");   
            var assemblyResult = d.Analyse(x);
        }
    }
}
