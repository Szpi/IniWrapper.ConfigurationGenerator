using IniWrapper.ConfigurationGenerator.IniParser;
using IniWrapper.ConfigurationGenerator.PropertySyntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using IniWrapper.ConfigurationGenerator.Configuration;
using IniWrapper.ConfigurationGenerator.PropertySyntax.Kind;
using IniWrapper.ConfigurationGenerator.Section;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IniWrapper.ConfigurationGenerator
{
    public class IniWrapperConfigurationGenerator : IIniWrapperConfigurationGenerator
    {
        private readonly IFileSystem _fileSystem;
        private readonly IIniParserWrapper _iniParserWrapper;
        private readonly GeneratorConfiguration _generatorConfiguration;

        private readonly PropertyDeclarationSyntaxGenerator _propertyDeclarationSyntaxGenerator;
        private readonly ListPropertyDeclarationSyntaxGenerator _listPropertyDeclarationSyntaxGenerator;
        private readonly ListComplexDataDeclarationSyntaxGenerator _listComplexDataDeclarationSyntaxGenerator;
        private readonly ISyntaxKindManager _syntaxKindManager;
        private readonly ISectionsAnalyzer _sectionsAnalyzer;

        public IniWrapperConfigurationGenerator(IIniParserWrapper iniParserWrapper,
                                                GeneratorConfiguration generatorConfiguration,
                                                IFileSystem fileSystem,
                                                ListPropertyDeclarationSyntaxGenerator listPropertyDeclarationSyntaxGenerator,
                                                PropertyDeclarationSyntaxGenerator propertyDeclarationSyntaxGenerator, 
                                                ISyntaxKindManager syntaxKindManager, 
                                                ISectionsAnalyzer sectionsAnalyzer, 
                                                ListComplexDataDeclarationSyntaxGenerator listComplexDataDeclarationSyntaxGenerator)
        {
            _iniParserWrapper = iniParserWrapper;
            _fileSystem = fileSystem;
            _generatorConfiguration = generatorConfiguration;
            _listPropertyDeclarationSyntaxGenerator = listPropertyDeclarationSyntaxGenerator;
            _propertyDeclarationSyntaxGenerator = propertyDeclarationSyntaxGenerator;
            _syntaxKindManager = syntaxKindManager;
            _sectionsAnalyzer = sectionsAnalyzer;
            _listComplexDataDeclarationSyntaxGenerator = listComplexDataDeclarationSyntaxGenerator;
        }

        public void Generate()
        {
            CreateOutputDictionary();

            var sectionNames = _iniParserWrapper.GetSectionsFromFile();

            var (separateSections, complexDataSections) = _sectionsAnalyzer.AnalyzeSections(sectionNames);

            foreach (var sectionName in separateSections)
            {
                var membersFromIni = _iniParserWrapper.ReadAllFromSection(sectionName);
                GeneratePropertyClass(sectionName, membersFromIni);
            }

            foreach (var complexDataSection in complexDataSections)
            {
                var originalPrefixOfSection = $"{complexDataSection}{_generatorConfiguration.ComplexDataSeparator}";
                var originalSectionFromFile = sectionNames.FirstOrDefault(x => x.StartsWith(originalPrefixOfSection));
                var membersFromIni = _iniParserWrapper.ReadAllFromSection(originalSectionFromFile);
                GeneratePropertyClass(complexDataSection, membersFromIni);
            }

            GenerateMainConfigurationClass(_generatorConfiguration.MainConfiguration, separateSections, complexDataSections);
        }

        private void GenerateMainConfigurationClass(string mainConfiguration, List<string> sectionNames,
                                                    List<string> complexDataSections)
        {
            var members = new SyntaxList<MemberDeclarationSyntax>();

            foreach (var iniLine in sectionNames)
            {
                var propertyDeclaration = GetClassPropertyDeclarationSyntax(iniLine);

                members = members.Add(propertyDeclaration);
            }

            foreach (var complexDataSection in complexDataSections)
            {
                var listOfComplexData = _listComplexDataDeclarationSyntaxGenerator.GetPropertyDeclarationSyntax(complexDataSection);
                members = members.Add(listOfComplexData);
            }

            var classSyntax = GetClassSyntax(mainConfiguration, members, complexDataSections.Any());

            var generatedClass = FormatSyntax(classSyntax);

            _fileSystem.File.WriteAllText(GenerateClassFilePath(mainConfiguration), generatedClass);
        }

        private PropertyDeclarationSyntax GetClassPropertyDeclarationSyntax(string iniLine)
        {
            var propertyDeclaration =
                 PropertyDeclaration(
                         IdentifierName(iniLine),
                         Identifier(
                             TriviaList(),
                             iniLine,
                             TriviaList(
                                 Space)))
                     .WithModifiers(
                         TokenList(
                             Token(
                                 TriviaList(),
                                 SyntaxKind.PublicKeyword,
                                 TriviaList(
                                     Space))))
                     .WithAccessorList(
                         AccessorList(
                                 List<AccessorDeclarationSyntax>(
                                     new AccessorDeclarationSyntax[]
                                     {
                                        AccessorDeclaration(
                                                SyntaxKind.GetAccessorDeclaration)
                                            .WithSemicolonToken(
                                                Token(SyntaxKind.SemicolonToken)),
                                        AccessorDeclaration(
                                                SyntaxKind.SetAccessorDeclaration)
                                            .WithSemicolonToken(
                                                Token(SyntaxKind.SemicolonToken))
                                     }))
                             .WithCloseBraceToken(
                                 Token(
                                     TriviaList(),
                                     SyntaxKind.CloseBraceToken,
                                     TriviaList(
                                         new[] { Space, LineFeed }))));
            return propertyDeclaration;
        }

        private void CreateOutputDictionary()
        {
            if (!_fileSystem.Directory.Exists(_generatorConfiguration.OutputFolder))
            {
                _fileSystem.Directory.CreateDirectory(_generatorConfiguration.OutputFolder);
            }
        }

        private void GeneratePropertyClass(string sectionName, IDictionary<string, string> propertiesNames)
        {
            var members = new SyntaxList<MemberDeclarationSyntax>();

            var generateGenericUsing = false;
            foreach (var iniLine in propertiesNames)
            {
                var (propertyDeclaration, shouldGenerateGenericUsing) = GetPropertyDeclarationSyntax(iniLine.Key, iniLine.Value);
                generateGenericUsing |= shouldGenerateGenericUsing;
                members = members.Add(propertyDeclaration);
            }

            var classSyntax = GetClassSyntax(sectionName, members, generateGenericUsing);

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

        private CompilationUnitSyntax GetClassSyntax(string sectionName, SyntaxList<MemberDeclarationSyntax> members,
                                                     bool generateGenericUsing)
        {
            var compilationUnit = CompilationUnit();
            if (generateGenericUsing)
            {
                compilationUnit = compilationUnit.WithUsings(
                    SingletonList<UsingDirectiveSyntax>(
                        UsingDirective(
                                QualifiedName(
                                    QualifiedName(
                                        IdentifierName("System"),
                                        IdentifierName("Collections")),
                                    IdentifierName("Generic")))
                            .WithUsingKeyword(
                                Token(
                                    TriviaList(),
                                    SyntaxKind.UsingKeyword,
                                    TriviaList(
                                        Space)))
                            .WithSemicolonToken(
                                Token(
                                    TriviaList(),
                                    SyntaxKind.SemicolonToken,
                                    TriviaList(
                                        LineFeed)))));
            }

            return compilationUnit
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(NamespaceDeclaration(
                                                                   IdentifierName(_generatorConfiguration.NameSpace))
                                                               .WithMembers(SingletonList<MemberDeclarationSyntax>(ClassDeclaration(Identifier(TriviaList(), sectionName, TriviaList(new[] { Space, LineFeed })))
                                                            .WithKeyword(Token(TriviaList(),
                                                                               SyntaxKind.ClassKeyword,
                                                                               TriviaList(
                                                                                   Space)))
                                                            .WithOpenBraceToken(
                                                                Token(
                                                                    TriviaList(),
                                                                    SyntaxKind.OpenBraceToken,
                                                                    TriviaList(
                                                                        new[] { Space, LineFeed })))
                                                            .WithMembers(members)))));
        }

        private ( PropertyDeclarationSyntax GeneratedProperty, bool ShouldGenerateUsingGeneric) GetPropertyDeclarationSyntax(string propertyName, string iniValue)
        {
            var valueType = _syntaxKindManager.GetSyntaxKind(iniValue);

            if (valueType == SyntaxKind.List)
            {
                return (_listPropertyDeclarationSyntaxGenerator.GetPropertyDeclarationSyntax(propertyName, iniValue, valueType), true);
            }

            return (_propertyDeclarationSyntaxGenerator.GetPropertyDeclarationSyntax(propertyName, iniValue, valueType), false);
        }

        private string GenerateClassFilePath(string sectionName)
        {
            return _fileSystem.Path.Combine(_generatorConfiguration.OutputFolder, $"{sectionName}.cs");
        }

        
    }
}