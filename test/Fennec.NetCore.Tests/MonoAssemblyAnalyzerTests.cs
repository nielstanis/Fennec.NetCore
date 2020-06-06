using System.Threading.Tasks;
using Fennec.NetCore.Output;
using Fennec.NetCore.Result;
using System.Linq;
using Xunit;
using Fennec.NetCore.Analyze;
using Xunit.Abstractions;

namespace Fennec.NetCore.Tests
{
    public class MonoAssemblyAnalyzerTests
    {
        private readonly ITestOutputHelper _output;
        public MonoAssemblyAnalyzerTests(ITestOutputHelper output)
        {
            this._output = output;
        }
        [Fact]
        public void BasicConsoleResultTest()
        {
            var x = TestResources.GetTestProjectAssembly("BasicConsole");
            var result = new MonoAssemblyAnalyzer(x);
            AssemblyResult assemblyResult = result.Analyse();
            Assert.Equal(2, assemblyResult.Types.Count);
            Assert.Equal("<Module>", assemblyResult.Types[0].ClassType);
            Assert.Equal("BasicConsole.Program", assemblyResult.Types[1].ClassType);
            
            //Focus on 2nd classtype
            var ct = assemblyResult.Types[1];
            Assert.Equal(2, ct.Methods.Count());
            
            //Focus on 2d method
            var mt = ct.Methods[1];
            Assert.Equal("Main", mt.Name);
            Assert.Equal("System.String[]", mt.Parameters);
           
            Assert.Equal(7, mt.Invocations.Count);
        }

        [Fact]
        public async void BasicConsoleFxtResultTest()
        {
            var x = TestResources.GetTestProjectAssembly("BasicConsole");
            var result = new MonoAssemblyAnalyzer(x);
            AssemblyResult assemblyResult = result.Analyse();
            Assert.Equal(2, assemblyResult.Types.Count);
            Assert.Equal("<Module>", assemblyResult.Types[0].ClassType);
            Assert.Equal("BasicConsole.Program", assemblyResult.Types[1].ClassType);

            //Focus on 2nd classtype
            var ct = assemblyResult.Types[1];
            Assert.Equal(2, ct.Methods.Count());

            //Focus on 2d method
            var mt = ct.Methods[1];
            Assert.Equal("Main", mt.Name);
            Assert.Equal("System.String[]", mt.Parameters);

            Assert.Equal(7, mt.Invocations.Count);

            var temp = System.IO.Path.GetTempFileName();
            using (var stream = System.IO.File.OpenWrite(temp))
            {
                FxtWriter writer = new FxtWriter();
                var writ = await writer.WriteOutputAsync(assemblyResult, stream);
                Assert.True(writ);
            }
            _output.WriteLine($"Created: {temp}");
        }
        
    }
}
