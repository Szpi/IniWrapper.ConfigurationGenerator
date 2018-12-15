using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using IniWrapper.ConfigurationGenerator.Configuration;
using IniWrapper.ConfigurationGenerator.Factory;

namespace ConfigurationGenerator
{
    internal class Program
    {
        private const int DefaultBufferSize = 100_000;
        private const string DefaultNameSpace = "Configuration";

        [Option("-f|--file <file_path>", "Path to ini configuration file, which should be used as model to classes generation", CommandOptionType.SingleValue)]
        [Required]
        public string FilePath { get; }

        [Option("-o|--output <directory_path>", "Path directory where new file will be generated", CommandOptionType.SingleValue)]
        [Required]
        public string OutputFolder { get; }

        [Option("-n|--namespace <namespace>", "Namespace that will be used for all generated classes (default value Configuration)", CommandOptionType.SingleValue)]
        public string NameSpace { get; }

        [Option("-m|--mainclass <class_name>", "Main configuration class name that will contain all section classes", CommandOptionType.SingleValue)]
        public string MainConfiguration { get; }

        [Option("-b|--buffer <int>", "Reading buffer size (pass only when files are really big)", CommandOptionType.SingleValue)]
        public int BufferSize { get; } = 0;

        private void OnExecute()
        {
            var nameSpace = string.IsNullOrEmpty(NameSpace) ? DefaultNameSpace : NameSpace;
            var bufferSize = BufferSize > 0 ? BufferSize : DefaultBufferSize;
            var mainConfiguration = string.IsNullOrEmpty(MainConfiguration) ? "" : MainConfiguration;

            var configuration = new GeneratorConfiguration(FilePath, OutputFolder, nameSpace, mainConfiguration, bufferSize);

            var generator = new IniWrapperConfigurationGeneratorFactory().Create(configuration);

            generator.Generate();
        }

        public static int Main(string[] args)
        {
            return CommandLineApplication.Execute<Program>(args);
        }
    }
}
