using System;

namespace Fennec.NetCore.Output
{
    public static class WriterFactory
    {
        public static Writer CreateWriter(string writerType, string outputFolder)
        {
            Writer res = new FxtWriter(outputFolder);
            if (writerType.ToLower().Trim()=="json")
            {
                res = new JsonWriter(outputFolder);
            }
            return res;
        }

        public static Writer CreateWriter(string writerType)
        {
            return CreateWriter(writerType, AppContext.BaseDirectory);
        }
    }
}