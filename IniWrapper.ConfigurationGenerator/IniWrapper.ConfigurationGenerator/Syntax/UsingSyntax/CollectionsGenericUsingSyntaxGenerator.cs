using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.UsingSyntax
{
    public class CollectionsGenericUsingSyntaxGenerator
    {
        public UsingDirectiveSyntax GetCollectionsGenericUsingSyntax()
        {
            return SyntaxFactory.UsingDirective(
                       SyntaxFactory.QualifiedName(
                           SyntaxFactory.QualifiedName(
                               SyntaxFactory.IdentifierName("System"),
                               SyntaxFactory.IdentifierName("Collections")),
                           SyntaxFactory.IdentifierName("Generic")))
                   .WithUsingKeyword(
                       SyntaxFactory.Token(
                           SyntaxFactory.TriviaList(),
                           SyntaxKind.UsingKeyword,
                           SyntaxFactory.TriviaList(
                               SyntaxFactory.Space)))
                   .WithSemicolonToken(
                       SyntaxFactory.Token(
                           SyntaxFactory.TriviaList(),
                           SyntaxKind.SemicolonToken,
                           SyntaxFactory.TriviaList(
                               SyntaxFactory.LineFeed)));
        }
    }
}