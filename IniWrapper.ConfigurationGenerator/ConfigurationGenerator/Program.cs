using IniWrapper.ConfigurationGenerator.Configuration;
using IniWrapper.ConfigurationGenerator.Factory;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace ConfigurationGenerator
{
    internal class Program
    {

        [Option("-f|--file <file_path>", "Path to ini configuration file or dictionary, which should be used as model to classes generation", CommandOptionType.SingleValue)]
        [Required]
        public string FilePath { get; }

        [Option("-o|--output <directory_path>", "Path directory where new files will be generated", CommandOptionType.SingleValue)]
        [Required]
        public string OutputFolder { get; }

        [Option("-n|--namespace <namespace>", "Namespace that will be used for all generated classes (default value 'Configuration')", CommandOptionType.SingleValue)]
        public string NameSpace { get; } = "Configuration";

        [Option("-m|--mainclass <class_name>", "Main configuration class name that will contain all section classes (default value 'MainConfiguration')", CommandOptionType.SingleValue)]
        public string MainConfiguration { get; } = "MainConfiguration";

        [Option("-b|--buffer <int>", "Reading buffer size (pass only when files are really big default value 100_000)", CommandOptionType.SingleValue)]
        public int BufferSize { get; } = 100_000;

        [Option("-s|--separator <char>", "Many value separator in case of test=a|b|c ( Key=Value) pass '|' (default separator ',')",
            CommandOptionType.SingleValue)]
        public string ListSeparator { get; } = ",";

        [Option("-a|--attribute <bool>", "Values (true/false or 1/0) Determines if generate IniOptions attribute (default false)",
            CommandOptionType.SingleValue)]
        public bool GenerateIniOptionAttribute { get; } = false;

        [Option("-i|--immutable <bool>", "Values (true/false or 1/0) Determines if generate immutable configuration (default false)",
            CommandOptionType.SingleValue)]
        public bool ImmutableConfiguration { get; } = false;

        private void OnExecute()
        {
            if (!Directory.Exists(FilePath))
            {
                GenerateClassesForIniFile(FilePath);
                return;
            }

            var iniFilesInDirectory = Directory.GetFiles(FilePath, "*.ini", SearchOption.TopDirectoryOnly);

            foreach (var iniFile in iniFilesInDirectory)
            {
                GenerateClassesForIniFile(iniFile);
            }
        }

        private void GenerateClassesForIniFile(string pathToIniFile)
        {
            var configuration = new GeneratorConfiguration(pathToIniFile,
                                                           OutputFolder,
                                                           NameSpace,
                                                           MainConfiguration,
                                                           BufferSize,
                                                           ListSeparator.ToCharArray().FirstOrDefault(),
                                                           GenerateIniOptionAttribute,
                                                           ImmutableConfiguration);

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
