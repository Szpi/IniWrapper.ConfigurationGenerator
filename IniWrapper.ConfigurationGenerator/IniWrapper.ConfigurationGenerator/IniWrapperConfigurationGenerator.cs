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
                GenerateSectionClass(sectionName);
            }

            //GenerateIniConfigurationClass(sectionNames);
        }

        private void GenerateIniConfigurationClass(string sectionNames)
        {
            throw new NotImplementedException();
        }

        private void CreateOutputDictionary()
        {
            if (!_fileSystem.Directory.Exists(_outputDictionary))
            {
                _fileSystem.Directory.CreateDirectory(_outputDictionary);
            }
        }

        private void GenerateSectionClass(string sectionName)
        {
            var members = new SyntaxList<MemberDeclarationSyntax>();
            var membersFromIni = _iniParserWrapper.ReadAllFromSection(sectionName);

            foreach (var iniLine in membersFromIni)
            {
                var propertyDeclaration = GetPropertyDeclarationSyntax(iniLine);
                members = members.Add(propertyDeclaration);
            }

            var classSyntax = GetClassSyntax(sectionName, members);
            
            var workspace = new AdhocWorkspace();
            var formattedSyntax = Formatter.Format(classSyntax, workspace);
            var generatedClass = formattedSyntax.ToFullString();

            _fileSystem.File.WriteAllText(GenerateClassFilePath(sectionName), generatedClass);
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

        private static PropertyDeclarationSyntax GetPropertyDeclarationSyntax(KeyValuePair<string, string> iniLine)
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
                            iniLine.Key,
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