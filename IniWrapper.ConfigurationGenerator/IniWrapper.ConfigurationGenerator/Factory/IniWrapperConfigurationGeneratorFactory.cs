using IniWrapper.ConfigurationGenerator.Configuration;
using IniWrapper.ConfigurationGenerator.IniParser;
using IniWrapper.ConfigurationGenerator.Section;
using System.IO.Abstractions;
using IniWrapper.ConfigurationGenerator.Syntax;
using IniWrapper.ConfigurationGenerator.Syntax.Class;
using IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax;
using IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax.Kind;
using IniWrapper.ConfigurationGenerator.Syntax.UsingSyntax;

namespace IniWrapper.ConfigurationGenerator.Factory
{
    public class IniWrapperConfigurationGeneratorFactory : IIniWrapperConfigurationGeneratorFactory
    {
        public IIniWrapperConfigurationGenerator Create(GeneratorConfiguration configuration)
        {
            var iniWrapper = new IniParserWrapper(configuration.FilePath, configuration.BufferSize, new ReadSectionsParser());
            var syntaxManager = new SyntaxKindManager(configuration.ListSeparator);

            var syntaxGeneratorFascade = new SyntaxGeneratorFacade(new IniOptionsAttributeSyntaxGenerator(configuration.GenerateIniOptionAttribute),
                                                                    new ListComplexDataDeclarationSyntaxGenerator(),
                                                                    new ListPropertyDeclarationSyntaxGenerator(syntaxManager, configuration.ListSeparator),
                                                                    new PropertyDeclarationSyntaxGenerator(),
                                                                    new IniWrapperAttributeUsingSyntaxGenerator(),
                                                                    new CollectionsGenericUsingSyntaxGenerator(),
                                                                    new ClassPropertyDeclarationSyntaxGenerator(),
                                                                    new ClassSyntaxGenerator());

            return new IniWrapperConfigurationGenerator(iniWrapper,
                                                        configuration,
                                                        new FileSystem(),
                                                        syntaxManager,
                                                        new SectionsAnalyzer(configuration.ComplexDataSeparator),
                                                        syntaxGeneratorFascade);
        }
    }
}