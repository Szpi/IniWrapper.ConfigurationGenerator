using IniWrapper.ConfigurationGenerator.Configuration;
using IniWrapper.ConfigurationGenerator.Ini;
using IniWrapper.ConfigurationGenerator.Ini.Using;
using IniWrapper.ConfigurationGenerator.IniParser;
using IniWrapper.ConfigurationGenerator.Section;
using IniWrapper.ConfigurationGenerator.Syntax;
using IniWrapper.ConfigurationGenerator.Syntax.Class;
using IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator;
using IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.ClassDeclarationGenerators;
using IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.PropertyDeclarationModifiers;
using IniWrapper.ConfigurationGenerator.Syntax.Generators;
using IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax;
using IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax.Immutable;
using IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax.Kind;
using System.Collections.Generic;
using System.IO.Abstractions;
using ICompilationUnitGenerator = IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.ICompilationUnitGenerator;
using UsingSyntaxGenerator = IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.CompilationUnitGenerators.UsingSyntaxGenerator;

namespace IniWrapper.ConfigurationGenerator.Factory
{
    public class IniWrapperConfigurationGeneratorFactory : IIniWrapperConfigurationGeneratorFactory
    {
        public IIniWrapperConfigurationGenerator Create(GeneratorConfiguration configuration)
        {
            var iniWrapper = new IniParserWrapper(configuration.FilePath, configuration.BufferSize, new ReadSectionsParser());
            var syntaxManager = new SyntaxKindManager(configuration.ListSeparator);

            var syntaxGeneratorFacade = GetSyntaxGeneratorFacade(configuration);

            var iniFileAnalyzer = new IniFileAnalyzer(iniWrapper,
                                                      new SectionsAnalyzer(configuration.ComplexDataSeparator),
                                                      syntaxManager,
                                                      new IniFileUsingsAnalyzer(configuration),
                                                      configuration.MainConfigurationClassName);

            var attributePropertyModifier = GetPropertyDeclarationSyntaxModifier(configuration, syntaxGeneratorFacade);
            var classDeclarationGenerators = new List<IClassDeclarationGenerator>()
            {
                new PropertySyntaxGenerator(syntaxGeneratorFacade, attributePropertyModifier),
                new PropertyListSyntaxGenerator(syntaxGeneratorFacade,attributePropertyModifier),

            };

            if (configuration.ImmutableConfiguration)
            {
                classDeclarationGenerators.Add(new ConstructorSyntaxGenerator(syntaxGeneratorFacade));
            }

            var compilationUnitGenerators = new List<ICompilationUnitGenerator>()
            {
                new UsingSyntaxGenerator(syntaxGeneratorFacade),
                new ClassSyntaxGenerator(syntaxGeneratorFacade,classDeclarationGenerators, configuration.NameSpace)
            };

            var syntaxGenerators = new List<Syntax.Generators.ICompilationUnitGenerator>()
            {
                new ClassCompilationUnitGenerator(compilationUnitGenerators.AsReadOnly(), syntaxGeneratorFacade)
            };

            return new IniWrapperConfigurationGenerator(syntaxGenerators,
                                                        iniFileAnalyzer,
                                                        new FileSystem(),
                                                        configuration);
        }

        private static SyntaxGeneratorFacade GetSyntaxGeneratorFacade(GeneratorConfiguration configuration)
        {
            if (configuration.ImmutableConfiguration)
            {
                return new SyntaxGeneratorFacade(new IniOptionsAttributeSyntaxGenerator(),
                                                 new ImmutableListPropertyDeclarationSyntaxGenerator(),
                                                 new ImmutablePropertyDeclarationSyntaxGenerator(),
                                                 new Syntax.UsingSyntax.UsingSyntaxGenerator(),
                                                 new ClassDeclarationSyntaxGenerator(),
                                                 new ConstructorDeclarationSyntaxGenerator());
            }

            return new SyntaxGeneratorFacade(new IniOptionsAttributeSyntaxGenerator(),
                                             new ListPropertyDeclarationSyntaxGenerator(),
                                             new PropertyDeclarationSyntaxGenerator(),
                                             new Syntax.UsingSyntax.UsingSyntaxGenerator(),
                                             new ClassDeclarationSyntaxGenerator(),
                                             new ConstructorDeclarationSyntaxGenerator());
        }

        private static IPropertyDeclarationSyntaxModifier GetPropertyDeclarationSyntaxModifier(GeneratorConfiguration configuration, SyntaxGeneratorFacade syntaxGeneratorFacade)
        {
            return configuration.GenerateIniOptionAttribute ? new AttributePropertyDeclarationSyntaxModifier(syntaxGeneratorFacade) as IPropertyDeclarationSyntaxModifier : new NullPropertyDeclarationSyntaxModifier();
        }
    }
}