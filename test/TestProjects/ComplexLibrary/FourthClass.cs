namespace ComplexLibrary
{
    public class FourthClass
    {
        public string ProcessDataFourth(string input)
        {
            var data = System.IO.File.ReadAllText(input);
            return data;
        }

        public int Start(string input)
        {
            return System.Diagnostics.Process.Start(input).Id;
        }
    }
}