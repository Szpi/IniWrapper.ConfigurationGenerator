using IniWrapper.ConfigurationGenerator.Configuration;
using IniWrapper.ConfigurationGenerator.Ini;
using IniWrapper.ConfigurationGenerator.Ini.Using;
using IniWrapper.ConfigurationGenerator.IniParser;
using IniWrapper.ConfigurationGenerator.Section;
using IniWrapper.ConfigurationGenerator.Syntax;
using IniWrapper.ConfigurationGenerator.Syntax.Class;
using IniWrapper.ConfigurationGenerator.Syntax.Generators;
using IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax;
using IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax.Kind;
using IniWrapper.ConfigurationGenerator.Syntax.UsingSyntax;
using System.Collections.Generic;
using System.IO.Abstractions;
using IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator;
using IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.ClassDeclarationGenerators;
using UsingSyntaxGenerator = IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.CompilationUnitGenerators.UsingSyntaxGenerator;

namespace IniWrapper.ConfigurationGenerator.Factory
{
    public class IniWrapperConfigurationGeneratorFactory : IIniWrapperConfigurationGeneratorFactory
    {
        public IIniWrapperConfigurationGenerator Create(GeneratorConfiguration configuration)
        {
            var iniWrapper = new IniParserWrapper(configuration.FilePath, configuration.BufferSize, new ReadSectionsParser());
            var syntaxManager = new SyntaxKindManager(configuration.ListSeparator);

            var syntaxGeneratorFacade = new SyntaxGeneratorFacade(  new IniOptionsAttributeSyntaxGenerator(),
                                                                    new ListPropertyDeclarationSyntaxGenerator(syntaxManager, configuration.ListSeparator),
                                                                    new PropertyDeclarationSyntaxGenerator(),
                                                                    new Syntax.UsingSyntax.UsingSyntaxGenerator(),
                                                                    new ClassDeclarationSyntaxGenerator());

            var iniFileAnalyzer = new IniFileAnalyzer(iniWrapper,
                                                      new SectionsAnalyzer(configuration.ComplexDataSeparator),
                                                      syntaxManager,
                                                      new IniFileUsingsAnalyzer(configuration),
                configuration.MainConfigurationClassName);


            var classDeclarationVisitors = new List<IClassDeclarationGenerator>()
            {
                new PropertySyntaxGenerator(syntaxGeneratorFacade),
                new PropertyListSyntaxGenerator(syntaxGeneratorFacade)
            };

            var classSyntaxVisitors = new List<IClassToGenerateGenerator>()
            {
                new UsingSyntaxGenerator(syntaxGeneratorFacade),
                new ClassSyntaxGenerator(syntaxGeneratorFacade,classDeclarationVisitors)
            };

            var syntaxGenerators = new List<ICompilationUnitGenerator>()
            {
                new ClassCompilationUnitGenerator(classSyntaxVisitors.AsReadOnly(), syntaxGeneratorFacade)
            };

            return new IniWrapperConfigurationGenerator(syntaxGenerators,
                                                        iniFileAnalyzer,
                                                        new FileSystem(),
                                                        configuration);
        }
    }
}