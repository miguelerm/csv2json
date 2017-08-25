using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace CsvToJson
{
    class Program
    {
        static void Main(string[] args)
        {

            var app = new CommandLineApplication();
            app.Name = "CSV2JSON";
            app.Description = "A simple csv to json converter";

            app.HelpOption("-?|-h|--help");

            var separator = app.Option("-s|--separator",
                "Separator (by default comma \",\")",
                CommandOptionType.SingleValue);

            var inputFile = app.Argument("input", "Imput csv file path");
            var outputFile = app.Argument("output", "Output file path");

            app.OnExecute(() => {

                var inputFilePath = inputFile.Value;

                if (string.IsNullOrWhiteSpace(inputFilePath)) 
                {
                    app.ShowHint();
                } 
                else 
                {
                    Task.Run(async () => {

                        if (!System.IO.File.Exists(inputFilePath)) 
                        {
                            Console.WriteLine($"File {inputFilePath} do not exists.");
                            return;
                        }

    
                        using(var input = new StreamReader(inputFilePath, Encoding.UTF8)) 
                        {
                            var csv = new CsvReader(input, separator.Value());

                            StreamWriter output;

                            if (string.IsNullOrWhiteSpace(outputFile.Value))
                            {
                                output = new StreamWriter(Console.OpenStandardOutput());
                                Console.SetOut(output);
                            } else {
                                output = new StreamWriter(outputFile.Value, false, Encoding.UTF8);
                            }

                            
                            using(var json = new JsonWriter(output)) {
                                Dictionary<string, object> obj;
                                while((obj = await csv.ReadAsync()) != null) {
                                    await json.WriteAsync(obj);
                                }
                            }

                        }
                
                    }).Wait();
                }
                return 0;
            });

            app.Execute(args);
        }
    }
}
