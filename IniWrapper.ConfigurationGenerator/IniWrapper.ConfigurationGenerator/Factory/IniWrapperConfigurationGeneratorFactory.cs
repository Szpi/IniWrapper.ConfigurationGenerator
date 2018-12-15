using IniWrapper.ConfigurationGenerator.Configuration;
using IniWrapper.ConfigurationGenerator.IniParser;
using IniWrapper.ConfigurationGenerator.PropertySyntax;
using IniWrapper.ConfigurationGenerator.PropertySyntax.Kind;
using IniWrapper.ConfigurationGenerator.Section;
using System.IO.Abstractions;

namespace IniWrapper.ConfigurationGenerator.Factory
{
    public class IniWrapperConfigurationGeneratorFactory : IIniWrapperConfigurationGeneratorFactory
    {
        public IIniWrapperConfigurationGenerator Create(GeneratorConfiguration configuration)
        {
            var iniWrapper = new IniParserWrapper(configuration.FilePath, configuration.BufferSize, new ReadSectionsParser());
            var syntaxManager = new SyntaxKindManager(configuration.ListSeparator);

            return new IniWrapperConfigurationGenerator(iniWrapper,
                                                        configuration,
                                                        new FileSystem(),
                                                        new ListPropertyDeclarationSyntaxGenerator(syntaxManager, configuration.ListSeparator),
                                                        new PropertyDeclarationSyntaxGenerator(),
                                                        syntaxManager,
                                                        new SectionsAnalyzer(configuration.ComplexDataSeparator),
                                                        new ListComplexDataDeclarationSyntaxGenerator());
        }
    }
}