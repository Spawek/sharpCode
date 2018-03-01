using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konkurs_C_Sharp
{
    class Program
    {
        static string inputDirectory = @"C:\maciek\sharpCode\Konkurs_C_Sharp\Konkurs_C_Sharp\Dane\Input\";
        static string outputDirectory = @"C:\maciek\sharpCode\Konkurs_C_Sharp\Konkurs_C_Sharp\Dane\Output\";

        class InputStructure
        {
        };

        class OutputStructure
        {
        };

        static InputStructure Parse(string fileName)
        {
            Console.WriteLine($"parsing file started: {fileName}");

            var inputStructure = new InputStructure();

            using (var sr = new StreamReader(fileName))
            {
                ////////// DO THE LOGIC
            }

            Console.WriteLine($"parsing file finished: {fileName}");
            return inputStructure;
        }

        static void WriteResultToFile(OutputStructure outputStructure, string fileName)
        {
            Console.WriteLine($"writing result file started: {fileName}");
            using (var sw = new StreamWriter(fileName))
            {
                ////////// DO THE LOGIC
            }

            Console.WriteLine($"writing result file finished: {fileName}");
        }

        static OutputStructure Solve(InputStructure inputStructure)
        {
            ////////// DO THE LOGIC

            return new OutputStructure();
        }

        static void ProcessFile(string baseFileName)
        {
            Console.WriteLine($"processing file started {baseFileName}");

            var input = Parse(inputDirectory + baseFileName + ".in");
            var output = Solve(input);
            WriteResultToFile(output, outputDirectory + baseFileName + ".out");

            Console.WriteLine($"processing file finished {baseFileName}");
        }

        static void Main(string[] args)
        {
            ProcessFile("aaa");
            ProcessFile("bbb");
            ProcessFile("ccc");
            ProcessFile("ddd");
        }
    }
}
