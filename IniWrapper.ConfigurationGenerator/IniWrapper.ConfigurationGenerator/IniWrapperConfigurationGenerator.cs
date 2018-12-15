using IniWrapper.ConfigurationGenerator.IniParser;
using IniWrapper.ConfigurationGenerator.PropertySyntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using IniWrapper.ConfigurationGenerator.PropertySyntax.Kind;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IniWrapper.ConfigurationGenerator
{
    public class IniWrapperConfigurationGenerator : IIniWrapperConfigurationGenerator
    {
        private readonly IFileSystem _fileSystem;
        private readonly IIniParserWrapper _iniParserWrapper;
        private readonly string _outputDictionary;
        private readonly string _namespace;

        private readonly PropertyDeclarationSyntaxGenerator _propertyDeclarationSyntaxGenerator;
        private readonly ListPropertyDeclarationSyntaxGenerator _listPropertyDeclarationSyntaxGenerator;
        private readonly ISyntaxKindManager _syntaxKindManager;

        public IniWrapperConfigurationGenerator(IIniParserWrapper iniParserWrapper, string outputDictionary,
                                                IFileSystem fileSystem, string @namespace,
                                                ListPropertyDeclarationSyntaxGenerator listPropertyDeclarationSyntaxGenerator,
                                                PropertyDeclarationSyntaxGenerator propertyDeclarationSyntaxGenerator, 
                                                ISyntaxKindManager syntaxKindManager)
        {
            _iniParserWrapper = iniParserWrapper;
            _outputDictionary = outputDictionary;
            _fileSystem = fileSystem;
            _namespace = @namespace;
            _listPropertyDeclarationSyntaxGenerator = listPropertyDeclarationSyntaxGenerator;
            _propertyDeclarationSyntaxGenerator = propertyDeclarationSyntaxGenerator;
            _syntaxKindManager = syntaxKindManager;
        }

        public void Generate()
        {
            CreateOutputDictionary();

            var sectionNames = _iniParserWrapper.GetSectionsFromFile();

            foreach (var sectionName in sectionNames)
            {
                var membersFromIni = _iniParserWrapper.ReadAllFromSection(sectionName);
                GeneratePropertyClass(sectionName, membersFromIni);
            }

            GenerateIniConfigurationClass(sectionNames);
        }

        private void GenerateIniConfigurationClass(string[] sectionNames)
        {
            GenerateMainConfigurationClass("MainConfiguration", sectionNames);
        }

        private void GenerateMainConfigurationClass(string mainConfiguration, string[] sectionNames)
        {
            var members = new SyntaxList<MemberDeclarationSyntax>();

            foreach (var iniLine in sectionNames)
            {
                var propertyDeclaration = GetClassPropertyDeclarationSyntax(iniLine);

                members = members.Add(propertyDeclaration);
            }

            var classSyntax = GetClassSyntax(mainConfiguration, members, false);

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
            if (!_fileSystem.Directory.Exists(_outputDictionary))
            {
                _fileSystem.Directory.CreateDirectory(_outputDictionary);
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
                                                                   IdentifierName(_namespace))
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
            return _fileSystem.Path.Combine(_outputDictionary, $"{sectionName}.cs");
        }

        
    }
}