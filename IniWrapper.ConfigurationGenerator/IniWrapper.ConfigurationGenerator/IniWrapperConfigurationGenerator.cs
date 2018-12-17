using IniWrapper.ConfigurationGenerator.Configuration;
using IniWrapper.ConfigurationGenerator.IniParser;
using IniWrapper.ConfigurationGenerator.Section;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using IniWrapper.ConfigurationGenerator.Syntax;
using IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax.Kind;

namespace IniWrapper.ConfigurationGenerator
{
    public class IniWrapperConfigurationGenerator : IIniWrapperConfigurationGenerator
    {
        private readonly IFileSystem _fileSystem;
        private readonly IIniParserWrapper _iniParserWrapper;
        private readonly GeneratorConfiguration _generatorConfiguration;

        private readonly ISyntaxKindManager _syntaxKindManager;
        private readonly ISectionsAnalyzer _sectionsAnalyzer;
        private readonly ISyntaxGeneratorFacade _syntaxGeneratorFacade;

        public IniWrapperConfigurationGenerator(IIniParserWrapper iniParserWrapper,
                                                GeneratorConfiguration generatorConfiguration,
                                                IFileSystem fileSystem,
                                                ISyntaxKindManager syntaxKindManager,
                                                ISectionsAnalyzer sectionsAnalyzer, 
                                                ISyntaxGeneratorFacade syntaxGeneratorFacade)
        {
            _iniParserWrapper = iniParserWrapper;
            _fileSystem = fileSystem;
            _generatorConfiguration = generatorConfiguration;
            _syntaxKindManager = syntaxKindManager;
            _sectionsAnalyzer = sectionsAnalyzer;
            _syntaxGeneratorFacade = syntaxGeneratorFacade;
        }

        public void Generate()
        {
            CreateOutputDictionary();

            var sectionNames = _iniParserWrapper.GetSectionsFromFile();

            var (separateSections, complexDataSections) = _sectionsAnalyzer.AnalyzeSections(sectionNames);

            foreach (var sectionName in separateSections)
            {
                var membersFromIni = _iniParserWrapper.ReadAllFromSection(sectionName);
                GenerateSectionClass(sectionName, membersFromIni);
            }

            foreach (var complexDataSection in complexDataSections)
            {
                var originalPrefixOfSection = $"{complexDataSection}{_generatorConfiguration.ComplexDataSeparator}";
                var originalSectionFromFile = sectionNames.FirstOrDefault(x => x.StartsWith(originalPrefixOfSection));
                var membersFromIni = _iniParserWrapper.ReadAllFromSection(originalSectionFromFile);
                GenerateSectionClass(complexDataSection, membersFromIni);
            }

            GenerateMainConfigurationClass(_generatorConfiguration.MainConfiguration, separateSections, complexDataSections);
        }

        private void GenerateMainConfigurationClass(string mainConfiguration, List<string> sectionNames,
                                                    List<string> complexDataSections)
        {
            var members = new SyntaxList<MemberDeclarationSyntax>();

            foreach (var iniLine in sectionNames)
            {
                var propertyDeclaration = _syntaxGeneratorFacade.GetClassPropertyDeclarationSyntax(iniLine);
                members = members.Add(propertyDeclaration);
            }

            foreach (var complexDataSection in complexDataSections)
            {
                var listOfComplexData = _syntaxGeneratorFacade.GetComplexListPropertyDeclarationSyntax(complexDataSection);

                members = members.Add(listOfComplexData);
            }

            var classSyntax = GetClassSyntax(mainConfiguration, members, complexDataSections.Any(), false);

            var generatedClass = FormatSyntax(classSyntax);

            _fileSystem.File.WriteAllText(GenerateClassFilePath(mainConfiguration), generatedClass);
        }
        
        private void CreateOutputDictionary()
        {
            if (!_fileSystem.Directory.Exists(_generatorConfiguration.OutputFolder))
            {
                _fileSystem.Directory.CreateDirectory(_generatorConfiguration.OutputFolder);
            }
        }

        private void GenerateSectionClass(string sectionName, IDictionary<string, string> propertiesNames)
        {
            var members = new SyntaxList<MemberDeclarationSyntax>();

            var generateGenericUsing = false;
            foreach (var iniLine in propertiesNames)
            {
                var (propertyDeclaration, shouldGenerateGenericUsing) = GetPropertyDeclarationSyntax(iniLine.Key, iniLine.Value);
                generateGenericUsing |= shouldGenerateGenericUsing;
                propertyDeclaration = _syntaxGeneratorFacade.AddIniOptionsAttributeToProperty(sectionName, iniLine.Key, propertyDeclaration);
                members = members.Add(propertyDeclaration);
            }

            var classSyntax = GetClassSyntax(sectionName, members, generateGenericUsing, _generatorConfiguration.GenerateIniOptionAttribute);

            var generatedClass = FormatSyntax(classSyntax);

            _fileSystem.File.WriteAllText(GenerateClassFilePath(sectionName), generatedClass);
        }

        private static string FormatSyntax(CompilationUnitSyntax classSyntax)
        {
            var workspace = new AdhocWorkspace();
            var formattedSyntax = Formatter.Format(classSyntax, workspace);
            var generatedClass = formattedSyntax.ToFullString();
            return generatedClass;
        }

        private CompilationUnitSyntax GetClassSyntax(string sectionName, SyntaxList<MemberDeclarationSyntax> members, bool generateGenericUsing, bool generateIniAttributeUsing)
        {
            var usingSyntax = GetNecessaryUsingSyntax(generateGenericUsing, generateIniAttributeUsing);

            return _syntaxGeneratorFacade.GetClassSyntax(sectionName, members, usingSyntax, _generatorConfiguration.NameSpace);
        }

        private SyntaxList<UsingDirectiveSyntax> GetNecessaryUsingSyntax(bool generateGenericUsing, bool generateIniAttributeUsing)
        {
            var usingSyntax = new SyntaxList<UsingDirectiveSyntax>();
            if (generateGenericUsing)
            {
                var collectionGenericUsingSyntax = _syntaxGeneratorFacade.GetCollectionsGenericUsingSyntax();
                usingSyntax = usingSyntax.Add(collectionGenericUsingSyntax);
            }

            if (generateIniAttributeUsing)
            {
                var iniWrapperAttributeUsingSyntax = _syntaxGeneratorFacade.GetIniWrapperAttributeUsingSyntax();
                usingSyntax = usingSyntax.Add(iniWrapperAttributeUsingSyntax);
            }

            return usingSyntax;
        }

        private (PropertyDeclarationSyntax GeneratedProperty, bool ShouldGenerateUsingGeneric) GetPropertyDeclarationSyntax(string propertyName, string iniValue)
        {
            var valueType = _syntaxKindManager.GetSyntaxKind(iniValue);

            if (valueType == SyntaxKind.List)
            {
                return (_syntaxGeneratorFacade.GetListPropertyDeclarationSyntax(propertyName, iniValue), true);
            }

            return (_syntaxGeneratorFacade.GetPropertyDeclarationSyntax(propertyName, iniValue, valueType), false);
        }

        private string GenerateClassFilePath(string sectionName)
        {
            return _fileSystem.Path.Combine(_generatorConfiguration.OutputFolder, $"{sectionName}.cs");
        }
    }
}