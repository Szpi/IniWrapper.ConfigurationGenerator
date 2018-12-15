using System.IO.Abstractions;
using IniWrapper.ConfigurationGenerator.Configuration;
using IniWrapper.ConfigurationGenerator.IniParser;
using IniWrapper.ConfigurationGenerator.PropertySyntax;
using IniWrapper.ConfigurationGenerator.PropertySyntax.Kind;

namespace IniWrapper.ConfigurationGenerator.Factory
{
    public class IniWrapperConfigurationGeneratorFactory : IIniWrapperConfigurationGeneratorFactory
    {
        public IIniWrapperConfigurationGenerator Create(GeneratorConfiguration configuration)
        {
            var iniWrapper = new IniParserWrapper(configuration.FilePath, configuration.BufferSize, new ReadSectionsParser());
            var syntaxManager = new SyntaxKindManager(configuration.ListSeparator);
            return new IniWrapperConfigurationGenerator(iniWrapper,
                                                        configuration.OutputFolder,
                                                        new FileSystem(),
                                                        configuration.NameSpace,
                                                        new ListPropertyDeclarationSyntaxGenerator(syntaxManager, configuration.ListSeparator),
                                                        new PropertyDeclarationSyntaxGenerator(),
                                                        syntaxManager);
        }
    }
}