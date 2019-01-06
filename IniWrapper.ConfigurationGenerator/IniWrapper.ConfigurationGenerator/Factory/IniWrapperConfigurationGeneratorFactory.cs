using System.Collections.Generic;
using IniWrapper.ConfigurationGenerator.Configuration;
using IniWrapper.ConfigurationGenerator.Ini;
using IniWrapper.ConfigurationGenerator.Ini.Using;
using IniWrapper.ConfigurationGenerator.IniParser;
using IniWrapper.ConfigurationGenerator.Section;
using IniWrapper.ConfigurationGenerator.Syntax;
using IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax;
using IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax.Kind;
using IniWrapper.ConfigurationGenerator.Syntax.UsingSyntax;
using System.IO.Abstractions;
using IniWrapper.ConfigurationGenerator.Syntax.Class;
using IniWrapper.ConfigurationGenerator.Syntax.Generators;
using IniWrapper.ConfigurationGenerator.Syntax.Visitor;
using IniWrapper.ConfigurationGenerator.Syntax.Visitor.ClassDeclarationVisitors;
using IniWrapper.ConfigurationGenerator.Syntax.Visitor.CompilationUnitVisitors;

namespace IniWrapper.ConfigurationGenerator.Factory
{
    public class IniWrapperConfigurationGeneratorFactory : IIniWrapperConfigurationGeneratorFactory
    {
        public IIniWrapperConfigurationGenerator Create(GeneratorConfiguration configuration)
        {
            var iniWrapper = new IniParserWrapper(configuration.FilePath, configuration.BufferSize, new ReadSectionsParser());
            var syntaxManager = new SyntaxKindManager(configuration.ListSeparator);

            var syntaxGeneratorFacade = new SyntaxGeneratorFacade(new IniOptionsAttributeSyntaxGenerator(configuration.GenerateIniOptionAttribute),
                                                                    new ListPropertyDeclarationSyntaxGenerator(syntaxManager, configuration.ListSeparator),
                                                                    new PropertyDeclarationSyntaxGenerator(),
                                                                    new UsingSyntaxGenerator(),
                                                                    new ClassPropertyDeclarationSyntaxGenerator(),
                                                                    new ClassDeclarationSyntaxGenerator());

            var iniFileAnalyzer = new IniFileAnalyzer(iniWrapper,
                                                      new SectionsAnalyzer(configuration.ComplexDataSeparator),
                                                      syntaxManager,
                                                      new IniFileUsingsAnalyzer(configuration));


            var classDeclarationVisitors = new List<IClassDeclarationVisitor>()
            {
                new PropertySyntaxVisitor(syntaxGeneratorFacade),
                new PropertyListSyntaxVisitor(syntaxGeneratorFacade)
            };

            var classSyntaxVisitors = new List<IClassToGenerateVisitor>()
            {
                new UsingSyntaxVisitor(syntaxGeneratorFacade),
                new ClassSyntaxVisitor(syntaxGeneratorFacade,classDeclarationVisitors)
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