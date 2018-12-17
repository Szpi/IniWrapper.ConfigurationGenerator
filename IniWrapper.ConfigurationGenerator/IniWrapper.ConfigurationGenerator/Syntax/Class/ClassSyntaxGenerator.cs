using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IniWrapper.ConfigurationGenerator.Syntax.Class
{
    public class ClassSyntaxGenerator
    {
        public CompilationUnitSyntax GetClassSyntax(string sectionName,
                                                     SyntaxList<MemberDeclarationSyntax> members,
                                                     SyntaxList<UsingDirectiveSyntax> usingDirectiveSyntax,
                                                     string nameSpace)
        {
            var compilationUnit = CompilationUnit();
            compilationUnit = compilationUnit.WithUsings(usingDirectiveSyntax);

            return compilationUnit
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(NamespaceDeclaration(
                                                                   IdentifierName(nameSpace))
                                                               .WithMembers(SingletonList<MemberDeclarationSyntax>(
                        ClassDeclaration(Identifier(TriviaList(), 
                                                    sectionName, 
                                                    TriviaList(new[] { Space, LineFeed })))
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
    }
}