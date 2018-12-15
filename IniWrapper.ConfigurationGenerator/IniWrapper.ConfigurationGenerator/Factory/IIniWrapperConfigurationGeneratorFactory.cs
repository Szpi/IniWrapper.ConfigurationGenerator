using IniWrapper.ConfigurationGenerator.Configuration;

namespace IniWrapper.ConfigurationGenerator.Factory
{
    public interface IIniWrapperConfigurationGeneratorFactory
    {
        IIniWrapperConfigurationGenerator Create(GeneratorConfiguration configuration);
    }
}