using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax.Immutable
{
    public class ImmutableListPropertyDeclarationSyntaxGenerator : IListPropertyDeclarationSyntaxGenerator
    {
        public PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName, SyntaxKind underlyingSyntaxKind)
        {
            var typeSyntax = ListPropertyDeclarationSyntaxGenerator.GetTypeSyntax(propertyName, underlyingSyntaxKind);

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
                                                                 SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                                            })));
        }
    }
}