using System.IO.Abstractions;
using IniWrapper.ConfigurationGenerator.IniParser;

namespace IniWrapper.ConfigurationGenerator.Factory
{
    public class IniWrapperConfigurationGeneratorFactory : IIniWrapperConfigurationGeneratorFactory
    {
        public IIniWrapperConfigurationGenerator Create(string iniFilePath, string outputFolder,int iniFileParsingBufferSize)
        {
            var iniWrapper = new IniParserWrapper(iniFilePath, iniFileParsingBufferSize, new ReadSectionsParser());
            return new IniWrapperConfigurationGenerator(iniWrapper, outputFolder, new FileSystem());
        }
    }
}