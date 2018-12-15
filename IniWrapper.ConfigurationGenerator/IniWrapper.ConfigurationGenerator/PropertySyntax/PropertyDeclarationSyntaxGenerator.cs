using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IniWrapper.ConfigurationGenerator.PropertySyntax
{
    public class PropertyDeclarationSyntaxGenerator
    {
        public PropertyDeclarationSyntax GetPropertyDeclarationSyntax(string propertyName, string iniValue, SyntaxKind syntaxKind)
        {
            return PropertyDeclaration(
                        PredefinedType(
                            Token(
                                TriviaList(),
                                syntaxKind,
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
                                        new[] { Space, LineFeed }))));
        }
    }
}