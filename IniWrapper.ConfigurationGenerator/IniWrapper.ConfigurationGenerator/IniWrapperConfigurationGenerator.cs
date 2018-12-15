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
        private readonly string _namespace;

        public IniWrapperConfigurationGenerator(IIniParserWrapper iniParserWrapper, string outputDictionary,
                                                IFileSystem fileSystem, string @namespace)
        {
            _iniParserWrapper = iniParserWrapper;
            _outputDictionary = outputDictionary;
            _fileSystem = fileSystem;
            _namespace = @namespace;
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

            var classSyntax = GetClassSyntax(mainConfiguration, members);

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

            foreach (var iniLine in propertiesNames)
            {
                var propertyDeclaration = GetPropertyDeclarationSyntax(iniLine.Key, iniLine.Value);
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

        private CompilationUnitSyntax GetClassSyntax(string sectionName, SyntaxList<MemberDeclarationSyntax> members)
        {
            return CompilationUnit()
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(NamespaceDeclaration(
                                                                   IdentifierName(_namespace))
                                                               .WithMembers(SingletonList<MemberDeclarationSyntax>(ClassDeclaration( Identifier(TriviaList(), sectionName, TriviaList(new[]{Space,LineFeed})))
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

        private static PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName, string iniValue)
        {
            var valueType = GetSyntaxKind(iniValue);
            var propertyDeclaration =
                PropertyDeclaration(
                        PredefinedType(
                            Token(
                                TriviaList(),
                                valueType,
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

        private static SyntaxKind GetSyntaxKind(string value)
        {
            if (Boolean.TryParse(value, out var boolResult))
            {
                return SyntaxKind.BoolKeyword;
            }

            if (int.TryParse(value, out var intResult))
            {
                return SyntaxKind.IntKeyword;
            }

            if (float.TryParse(value, out var floatResult))
            {
                return SyntaxKind.FloatKeyword;
            }

            if (double.TryParse(value, out var doubleResult))
            {
                return SyntaxKind.DoubleKeyword;
            }

            if (byte.TryParse(value, out var byteResult))
            {
                return SyntaxKind.ByteKeyword;
            }

            return SyntaxKind.StringKeyword;
        }
    }
}