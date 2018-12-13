using IniWrapper.ConfigurationGenerator.IniParser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IniWrapper.ConfigurationGenerator
{
    public class IniWrapperConfigurationGenerator : IIniWrapperConfigurationGenerator
    {
        private readonly IFileSystem _fileSystem;
        private readonly IIniParserWrapper _iniParserWrapper;
        private readonly string _outputDictionary;

        public IniWrapperConfigurationGenerator(IIniParserWrapper iniParserWrapper, string outputDictionary,
                                                IFileSystem fileSystem)
        {
            _iniParserWrapper = iniParserWrapper;
            _outputDictionary = outputDictionary;
            _fileSystem = fileSystem;
        }

        public void Generate()
        {
            CreateOutputDictionary();

            var sectionNames = _iniParserWrapper.GetSectionsFromFile();

            foreach (var sectionName in sectionNames)
            {
                var membersFromIni = _iniParserWrapper.ReadAllFromSection(sectionName);
                GeneratePropertyClass(sectionName, membersFromIni.Values);
            }

            GenerateIniConfigurationClass(sectionNames);
        }

        private void GenerateIniConfigurationClass(string[] sectionNames)
        {
            GenerateMainConfigurationClass("MainConfiguration", sectionNames);
        }

        private void GenerateMainConfigurationClass(string mainconfiguration, string[] sectionNames)
        {
            var members = new SyntaxList<MemberDeclarationSyntax>();

            foreach (var iniLine in sectionNames)
            {
                var propertyDeclaration = GetClassPropertyDeclarationSyntax(iniLine);

                members = members.Add(propertyDeclaration);
            }

            var classSyntax = GetClassSyntax(mainconfiguration, members);

            var generatedClass = FormatSyntax(classSyntax);

            _fileSystem.File.WriteAllText(GenerateClassFilePath(mainconfiguration), generatedClass);
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

        private void GeneratePropertyClass(string sectionName, IEnumerable<string> propertiesNames)
        {
            var members = new SyntaxList<MemberDeclarationSyntax>();

            foreach (var iniLine in propertiesNames)
            {
                var propertyDeclaration = GetPropertyDeclarationSyntax(iniLine);
                members = members.Add(propertyDeclaration);
            }

            var classSyntax = GetClassSyntax(sectionName, members);
            
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

        private static CompilationUnitSyntax GetClassSyntax(string sectionName, SyntaxList<MemberDeclarationSyntax> members)
        {
            return CompilationUnit()
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>( ClassDeclaration( Identifier(TriviaList(), sectionName, TriviaList(new[]{Space,LineFeed})))
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
                                                            .WithMembers(members)));
        }

        private static PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName)
        {
            var propertyDeclaration =
                PropertyDeclaration(
                        PredefinedType(
                            Token(
                                TriviaList(),
                                SyntaxKind.StringKeyword,
                                TriviaList(
                                    Space))),
                        Identifier(
                            TriviaList(),
                            propertyName,
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
                                        new[] {Space, LineFeed}))));
            return propertyDeclaration;
        }

        private string GenerateClassFilePath(string sectionName)
        {
            return _fileSystem.Path.Combine(_outputDictionary, $"{sectionName}.cs");
        }
    }
}