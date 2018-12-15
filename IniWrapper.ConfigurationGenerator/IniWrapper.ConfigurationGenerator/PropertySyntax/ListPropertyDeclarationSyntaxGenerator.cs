using System;
using System.Linq;
using IniWrapper.ConfigurationGenerator.PropertySyntax.Kind;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IniWrapper.ConfigurationGenerator.PropertySyntax
{
    public class ListPropertyDeclarationSyntaxGenerator
    {
        private readonly ISyntaxKindManager _syntaxKindManager;
        private readonly char _listSeparator;

        public ListPropertyDeclarationSyntaxGenerator(ISyntaxKindManager syntaxKindManager, char listSeparator)
        {
            _syntaxKindManager = syntaxKindManager;
            _listSeparator = listSeparator;
        }

        public PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName, string iniValue, SyntaxKind syntaxKind)
        {
            var valueType = GetBestSyntaxKind(iniValue);

            return PropertyDeclaration(
                       GenericName(
                               Identifier("List"))
                           .WithTypeArgumentList(
                               TypeArgumentList(
                                   SingletonSeparatedList<TypeSyntax>(
                                       PredefinedType(
                                           Token(valueType))))),
                       Identifier(propertyName))
                   .WithModifiers(
                       TokenList(
                           Token(SyntaxKind.PublicKeyword)))
                   .WithAccessorList(
                       AccessorList(
                           List<AccessorDeclarationSyntax>(
                               new AccessorDeclarationSyntax[]{
                                   AccessorDeclaration(
                                           SyntaxKind.GetAccessorDeclaration)
                                       .WithSemicolonToken(
                                           Token(SyntaxKind.SemicolonToken)),
                                   AccessorDeclaration(
                                           SyntaxKind.SetAccessorDeclaration)
                                       .WithSemicolonToken(
                                           Token(SyntaxKind.SemicolonToken))})));
        }

        private SyntaxKind GetBestSyntaxKind(string iniValue)
        {
            var splitValues = iniValue.Split(new[] {_listSeparator}, StringSplitOptions.RemoveEmptyEntries);

            var splitValuesSyntaxKind = splitValues.Select(x => _syntaxKindManager.GetSyntaxKind(x)).Distinct().ToList();

            return splitValuesSyntaxKind.Count() > 1
                ? SyntaxKind.StringKeyword
                : splitValuesSyntaxKind.FirstOrDefault();
        }
    }
}