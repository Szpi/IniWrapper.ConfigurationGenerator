using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using IniWrapper.ConfigurationGenerator.Factory;

namespace ConfigurationGenerator
{
    internal class Program
    {
        private const int DefaultBufferSize = 100_000;

        [Option("-f|--file <file_path>", "Path to ini configuration file, which should be used as model to classes generation", CommandOptionType.SingleValue)]
        [Required]
        public string FilePath { get; }

        [Option("-o|--output <directory_path>", "Path directory where new file will be generated", CommandOptionType.SingleValue)]
        [Required]
        public string OutputFolder { get; }

        [Option("-b|--buffer <int>", "Reading buffer size (pass only when files are really big)", CommandOptionType.SingleValue)]
        public int BufferSize { get; } = 0;

        private void OnExecute()
        {
            var bufferSize = BufferSize > 0 ? BufferSize : DefaultBufferSize;
            var generator = new IniWrapperConfigurationGeneratorFactory().Create(FilePath, OutputFolder, bufferSize);

            generator.Generate();
        }

        public static int Main(string[] args)
        {
            return CommandLineApplication.Execute<Program>(args);
        }
    }
}
