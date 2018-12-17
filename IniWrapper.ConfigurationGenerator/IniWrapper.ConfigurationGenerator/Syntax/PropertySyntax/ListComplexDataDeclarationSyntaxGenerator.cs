using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax
{
    public class ListComplexDataDeclarationSyntaxGenerator
    {
        public PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName)
        {
            return SyntaxFactory.PropertyDeclaration(
                       SyntaxFactory.GenericName(
                               SyntaxFactory.Identifier("List"))
                           .WithTypeArgumentList(
                               SyntaxFactory.TypeArgumentList(
                                   SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                       SyntaxFactory.IdentifierName(propertyName)))),
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
    }
}