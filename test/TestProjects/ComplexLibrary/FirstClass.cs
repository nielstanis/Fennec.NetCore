using System;

namespace ComplexLibrary
{
    public class FirstClass
    {
        public void MethodOne(string input, string output)
        {
            var file = System.IO.File.OpenRead(input);
            var random = System.Guid.NewGuid();
        }

        public string MethodTwo(string da)
        {
            var t = new ThirdClass();
            return t.ProcessData(da);
        }

        public void MethodThree(string agh)
        {
            MethodFourPrivate(agh);
        }

        public void AnotherRoute(string x3f)
        {
            MethodFourPrivate(x3f);
        }

        private void MethodFourPrivate(string input)
        {
            if (input.Length > 4)
            {
                MethodFourPrivate(input);
            }
            var t = new ThirdClass();
            t.AnotherMethod(input);
        }

        public int FirstProccess(string data)
        {
            var f = new FourthClass();
            return f.Start(data);
        }

        public int AnotherProcess(string xp)
        {
            var x = new ThirdClass();
            return x.StartProc(xp);
        }
    }
}
