namespace IniWrapper.ConfigurationGenerator.Configuration
{
    public class GeneratorConfiguration
    {
        public string FilePath { get; }
        public string OutputFolder { get; }
        public string NameSpace { get; }
        public string MainConfiguration { get; }
        public int BufferSize { get; }
        public char ListSeparator { get; }

        public GeneratorConfiguration(string filePath,
                                      string outputFolder,
                                      string nameSpace,
                                      string mainConfiguration,
                                      int bufferSize,
                                      char listSeparator)
        {
            FilePath = filePath;
            OutputFolder = outputFolder;
            NameSpace = nameSpace;
            MainConfiguration = mainConfiguration;
            BufferSize = bufferSize;
            ListSeparator = listSeparator;
        }
    }
}