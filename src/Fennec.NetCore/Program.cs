using System;
using System.Threading.Tasks;
using Fennec.NetCore.Output;
using McMaster.Extensions.CommandLineUtils;

namespace Fennec.NetCore
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var app = new CommandLineApplication(throwOnUnexpectedArg: false);
            app.Description = "Dump used API's of .NET assembly/assemblies.";
            app.AddName("Fennec.NetCore");
            app.FullName = "Fennec.NetCore - .NET Core API dumper";
            app.HelpOption("-h|--help");
            CommandLineApplicationExtensions.VersionOptionFromAssemblyAttributes(app, "-v|--version", typeof(Program).Assembly);
            var ass = app.Argument("assembly", "List of assemblies or file pattern", true);
            ass.IsRequired();

            var outputFolder = app.Option("-o|--output <FOLDER>", "Output Folder", CommandOptionType.SingleOrNoValue);
            var outputType = app.Option("-f|--format <FORMAT>", "File Format, either JSON or FXT", CommandOptionType.SingleOrNoValue);

            Console.WriteLine(app.FullName);
            app.OnExecuteAsync(async cancellationToken => 
            {
                string folder = outputFolder.HasValue() ? outputFolder.Value() : ".";
                string type = outputType.HasValue() ? outputType.Value() : "fxt";
                var writer = Output.WriterFactory.CreateWriter(type, folder);

                foreach (var arg in ass.Values)
                {
                    if (System.IO.File.Exists(arg))
                    {
                        await AnalyzeAndWrite(writer, arg);
                    }
                    else
                    {
                        var files = System.IO.Directory.GetFiles(AppContext.BaseDirectory, arg);
                        foreach (var file in files)
                        {
                            await AnalyzeAndWrite(writer, file);
                        }
                    }
                }
            });

            return await app.ExecuteAsync(args);
        }

        private static async Task AnalyzeAndWrite(Writer writer, string assembly)
        {
            var loadedAssembly = new Analyze.MonoAssemblyAnalyzer(assembly);
            var ass = loadedAssembly.Analyse();
            if (ass.HasError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unable to analyse '{0}'", ass.FilePath);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Analyzing and writing output of: {0}", ass.Assembly);
                Console.ResetColor();
                _ = await writer.WriteOutputAsync(ass);
            }
        }
    }
}