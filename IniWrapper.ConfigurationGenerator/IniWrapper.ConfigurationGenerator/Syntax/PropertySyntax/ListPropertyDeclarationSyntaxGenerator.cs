using System;
using System.Linq;
using IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax.Kind;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax
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

        public PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName, string iniValue)
        {
            var valueType = GetBestSyntaxKind(iniValue);

            return SyntaxFactory.PropertyDeclaration(
                       SyntaxFactory.GenericName(
                               SyntaxFactory.Identifier("List"))
                           .WithTypeArgumentList(
                               SyntaxFactory.TypeArgumentList(
                                   SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                       SyntaxFactory.PredefinedType(
                                           SyntaxFactory.Token(valueType))))),
                       SyntaxFactory.Identifier(propertyName))
                   .WithModifiers(
                       SyntaxFactory.TokenList(
                           SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                   .WithAccessorList(
                       SyntaxFactory.AccessorList(
                           SyntaxFactory.List<AccessorDeclarationSyntax>(
                               new AccessorDeclarationSyntax[]{
                                   SyntaxFactory.AccessorDeclaration(
                                           SyntaxKind.GetAccessorDeclaration)
                                       .WithSemicolonToken(
                                           SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                                   SyntaxFactory.AccessorDeclaration(
                                           SyntaxKind.SetAccessorDeclaration)
                                       .WithSemicolonToken(
                                           SyntaxFactory.Token(SyntaxKind.SemicolonToken))})));
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