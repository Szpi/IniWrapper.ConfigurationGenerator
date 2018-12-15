using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using IniWrapper.ConfigurationGenerator.Configuration;
using IniWrapper.ConfigurationGenerator.Factory;

namespace ConfigurationGenerator
{
    internal class Program
    {

        [Option("-f|--file <file_path>", "Path to ini configuration file, which should be used as model to classes generation", CommandOptionType.SingleValue)]
        [Required]
        public string FilePath { get; }

        [Option("-o|--output <directory_path>", "Path directory where new file will be generated", CommandOptionType.SingleValue)]
        [Required]
        public string OutputFolder { get; }

        [Option("-n|--namespace <namespace>", "Namespace that will be used for all generated classes (default value Configuration)", CommandOptionType.SingleValue)]
        public string NameSpace { get; } = "Configuration";

        [Option("-m|--mainclass <class_name>", "Main configuration class name that will contain all section classes (default MainConfiguration)", CommandOptionType.SingleValue)]
        public string MainConfiguration { get; } = "MainConfiguration";

        [Option("-b|--buffer <int>", "Reading buffer size (pass only when files are really big)", CommandOptionType.SingleValue)]
        public int BufferSize { get; } = 100_000;

        [Option("-s|--separator <char>", "Many value separator in case of Key=Value test=a|b|c write '|' default ',' ",
            CommandOptionType.SingleValue)]
        public string ListSeparator { get; } = ",";

        private void OnExecute()
        {
            var configuration = new GeneratorConfiguration(FilePath, OutputFolder, NameSpace, MainConfiguration, BufferSize, ListSeparator.ToCharArray().FirstOrDefault());

            var generator = new IniWrapperConfigurationGeneratorFactory().Create(configuration);

            generator.Generate();
        }

        public static int Main(string[] args)
        {
            try
            {
                return CommandLineApplication.Execute<Program>(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
                return 0;
            }
        }
    }
}
