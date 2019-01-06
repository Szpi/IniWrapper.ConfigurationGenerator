using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.UsingSyntax
{
    public class UsingSyntaxGenerator
    {
        public UsingDirectiveSyntax GetUsingSyntax(string @using)
        {
            return SyntaxFactory.UsingDirective(
                           SyntaxFactory.IdentifierName(@using))
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