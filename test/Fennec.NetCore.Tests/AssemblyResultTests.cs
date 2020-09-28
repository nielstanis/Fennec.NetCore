using System.Linq;
using Fennec.NetCore.Analyze;
using Xunit;
using Xunit.Abstractions;

namespace Fennec.NetCore.Tests
{
    public class AssemblyResultTests
    {
        // 
        private readonly ITestOutputHelper _output;

        public AssemblyResultTests(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void ExecutionPathComplexLibraryFileReadAllText()
        {
            var x = TestResources.GetTestProjectAssembly("ComplexLibrary");
            var d = new DnLibAssemblyAnalyzer();
            var result = d.Analyse(x);

            var exec = result.GetFirstExecutionPath("System.IO.File::ReadAllText(System.String)");
            var res = exec.ToArray();

            string[] path = new string[4] {
                "ComplexLibrary.FirstClass::MethodTwo(System.String)",
                "ComplexLibrary.ThirdClass::ProcessData(System.String)",
                "ComplexLibrary.FourthClass::ProcessDataFourth(System.String)",
                "System.IO.File::ReadAllText(System.String)"
            };

            Assert.Equal(path, res);
        }

        [Fact]
        public void ExecutionPathsConsoleApp()
        {
            var x = TestResources.GetTestProjectAssembly("BasicConsole");
            var d = new DnLibAssemblyAnalyzer();
            var result = d.Analyse(x);

            var exec = result.GetExecutionPaths("System.IO.File::OpenRead(System.String)");
            Assert.Equal(2, exec.Count());

            string[] path1 = new string[2] {
                "BasicConsole.Program::Main(System.String[])",
                "System.IO.File::OpenRead(System.String)"
            };
            Assert.Equal(path1, exec.First().ToArray());

            string[] path2 = new string[3] {
                "BasicConsole.Program::Main(System.String[])",
                "BasicConsole.Program::OtherMethod(System.String)",
                "System.IO.File::OpenRead(System.String)"
            };
            Assert.Equal(path2, exec.Skip(1).First().ToArray());
        }

        [Fact]
        public void ExecutionPathComplexLibraryDeleteRecursive()
        {
            var x = TestResources.GetTestProjectAssembly("ComplexLibrary");
            var d = new DnLibAssemblyAnalyzer();
            var result = d.Analyse(x);

            var exec = result.GetExecutionPaths("System.IO.File::Delete(System.String)");

            var first = exec.First().ToArray();
            string[] path = new string[3] {
                "ComplexLibrary.FirstClass::MethodThree(System.String)",
                "ComplexLibrary.ThirdClass::AnotherMethod(System.String)",
                "System.IO.File::Delete(System.String)"
            };
            Assert.Equal(path, first);

            var second = exec.Skip(1).First().ToArray();
            string[] path2 = new string[3] {
                "ComplexLibrary.FirstClass::MethodFourPrivate(System.String)",
                "ComplexLibrary.ThirdClass::AnotherMethod(System.String)",
                "System.IO.File::Delete(System.String)"
            };
            Assert.Equal(path2, second);
        }

        [Fact]
        public void ExecutionPathComplexLibraryProcessTwoUnique()
        {
            var x = TestResources.GetTestProjectAssembly("ComplexLibrary");
            var d = new DnLibAssemblyAnalyzer();
            var result = d.Analyse(x);

            var exec = result.GetExecutionPaths("System.Diagnostics.Process::Start(System.String)");
            Assert.Equal(2, exec.Count());

            var first = exec.First().ToArray();
            string[] path = new string[4] {
                "ComplexLibrary.FirstClass::AnotherProcess(System.String)",
                "ComplexLibrary.ThirdClass::StartProc(System.String)",
                "ComplexLibrary.FourthClass::Start(System.String)",
                "System.Diagnostics.Process::Start(System.String)"
            };
            Assert.Equal(path, first);

            var second = exec.Skip(1).First().ToArray();
            string[] path2 = new string[3] {
                "ComplexLibrary.FirstClass::FirstProccess(System.String)",
                "ComplexLibrary.FourthClass::Start(System.String)",
                "System.Diagnostics.Process::Start(System.String)"
            };
            Assert.Equal(path2, second);


        }
    }
}