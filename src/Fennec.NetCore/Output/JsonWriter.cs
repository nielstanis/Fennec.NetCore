using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Fennec.NetCore.Result;

namespace Fennec.NetCore.Output
{
    public class JsonWriter : Writer
    {
        
        public JsonWriter() : base()
        {

        }

        public JsonWriter(string outputFolder) : base(outputFolder)
        {
        }

        public override async Task<bool> WriteOutputAsync(AssemblyResult assemblyResult)
        {
            string filename = System.IO.Path.GetFileNameWithoutExtension(assemblyResult.FilePath);
            string outputFile = System.IO.Path.Combine(_outputFolder, $"{filename}.json");

            bool result = true;
            try
            {
                base.EnsureFolderCreated();
                using (var f = System.IO.File.Create(outputFile))
                {
                    _ = await WriteOutputAsync(assemblyResult, f);
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public override async Task<bool> WriteOutputAsync(AssemblyResult assemblyResult, Stream stream)
        {
            bool result = true;
            try
            {
                await JsonSerializer.SerializeAsync<AssemblyResult>(stream, assemblyResult);
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
    }
}