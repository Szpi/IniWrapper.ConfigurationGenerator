namespace IniWrapper.ConfigurationGenerator.Factory
{
    public interface IIniWrapperConfigurationGeneratorFactory
    {
        IIniWrapperConfigurationGenerator Create(string iniFilePath, string outputFolder, int iniFileParsingBufferSize);
    }
}