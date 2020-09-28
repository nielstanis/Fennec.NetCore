namespace ComplexLibrary
{
    public class ThirdClass
    {

        private FourthClass _fourth;
        public ThirdClass()
        {
            _fourth = new FourthClass();
        }
        
        public string ProcessData(string input)
        {
            return _fourth.ProcessDataFourth(input);
        }

        public bool AnotherMethod(string input)
        {
            System.IO.File.Delete(input);
            return true;
        }

        public int StartProc(string def)
        {
            return _fourth.Start(def);
        }
    }
}