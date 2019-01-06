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

        public PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName, SyntaxKind underlyingSyntaxKind)
        {
            var typeSyntax = GetTypeSyntax(propertyName, underlyingSyntaxKind);

            return SyntaxFactory.PropertyDeclaration(
                       SyntaxFactory.GenericName(
                               SyntaxFactory.Identifier("List"))
                           .WithTypeArgumentList(
                               SyntaxFactory.TypeArgumentList(
                                   SyntaxFactory.SingletonSeparatedList<TypeSyntax>(typeSyntax))),
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

        private static TypeSyntax GetTypeSyntax(string propertyName, SyntaxKind underlyingSyntaxKind)
        {
            return underlyingSyntaxKind == SyntaxKind.ClassDeclaration
                ? SyntaxFactory.IdentifierName(propertyName) as TypeSyntax
                : SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(underlyingSyntaxKind));
        }
    }
}