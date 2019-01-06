using IniWrapper.ConfigurationGenerator.Ini.Class;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
namespace IniWrapper.ConfigurationGenerator.Syntax.PropertySyntax
{
    public class ConstructorDeclarationSyntaxGenerator
    {
        public ConstructorDeclarationSyntax GetConstructorDeclarationSyntax(string className, IReadOnlyList<PropertyDescriptor> propertyDescriptors)
        {
            return ConstructorDeclaration(
                    Identifier(className))
                   .WithAttributeLists(
                       SingletonList<AttributeListSyntax>(
                           AttributeList(
                               SingletonSeparatedList<AttributeSyntax>(
                                   Attribute(
                                       IdentifierName("IniConstructor"))))))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(
                    ParameterList(
                        SeparatedList<ParameterSyntax>(
                            GenerateConstructorParameters(propertyDescriptors))))
                .WithBody(
                       Block(
                        GenerateAssignmentExpression(propertyDescriptors)
                           ));
        }

        private IEnumerable<SyntaxNodeOrToken> GenerateConstructorParameters(IReadOnlyList<PropertyDescriptor> propertyDescriptors)
        {
            var generatedParameters = new List<SyntaxNodeOrToken>();

            foreach (var parameter in propertyDescriptors)
            {

                switch (parameter.SyntaxKind)
                {
                    case SyntaxKind.ClassDeclaration:
                        {
                            generatedParameters.Add(GenerateClassParameter(parameter));
                            generatedParameters.Add(Token(SyntaxKind.CommaToken));
                            continue;
                        }
                    case SyntaxKind.List:
                        {
                            generatedParameters.Add(GenerateGenericParameter(parameter));
                            generatedParameters.Add(Token(SyntaxKind.CommaToken));
                            continue;
                        }
                    default:
                        {
                            var generatedParameter = Parameter(
                                    Identifier(FirstLetterToLower(parameter.Name)))
                                .WithType(
                                    PredefinedType(
                                        Token(parameter.SyntaxKind)));

                            generatedParameters.Add(generatedParameter);
                            generatedParameters.Add(Token(SyntaxKind.CommaToken));
                            continue;
                        }
                }
            }

            generatedParameters.RemoveAt(generatedParameters.Count - 1);

            return generatedParameters;
        }

        private SyntaxNodeOrToken GenerateGenericParameter(PropertyDescriptor propertyDescriptor)
        {
            TypeSyntax typeSyntax;
            if (propertyDescriptor.UnderlyingSyntaxKind == SyntaxKind.ClassDeclaration)
            {
                typeSyntax = SyntaxFactory.IdentifierName(propertyDescriptor.Name);
            }
            else
            {
                typeSyntax = PredefinedType(Token(propertyDescriptor.UnderlyingSyntaxKind));
            }

            return Parameter(
                    Identifier(FirstLetterToLower(propertyDescriptor.Name)))
                .WithType(
                    GenericName(
                            Identifier("List"))
                        .WithTypeArgumentList(
                            TypeArgumentList(
                                SingletonSeparatedList<TypeSyntax>(typeSyntax))));
        }

        private SyntaxNodeOrToken GenerateClassParameter(PropertyDescriptor propertyDescriptor)
        {
            return Parameter(
                    Identifier(FirstLetterToLower(propertyDescriptor.Name)))
                .WithType(
                    IdentifierName(propertyDescriptor.Name));
        }


        private IEnumerable<ExpressionStatementSyntax> GenerateAssignmentExpression(IReadOnlyList<PropertyDescriptor> propertyDescriptors)
        {
            var assignmentExpressions = new List<ExpressionStatementSyntax>();

            foreach (var propertyDescriptor in propertyDescriptors)
            {
                var isFirstLetterLower = IsFirstLetterLow(propertyDescriptor.Name);
                var expressionSyntax = isFirstLetterLower
                    ? GenerateThisAccessSyntax(propertyDescriptor)
                    : IdentifierName(propertyDescriptor.Name);

                assignmentExpressions.Add(ExpressionStatement(
                                              AssignmentExpression(
                                                  SyntaxKind.SimpleAssignmentExpression,
                                                  expressionSyntax,
                                                  IdentifierName(FirstLetterToLower(propertyDescriptor.Name)))));
            }
            return assignmentExpressions;
        }

        private static ExpressionSyntax GenerateThisAccessSyntax(PropertyDescriptor propertyDescriptor)
        {
            return MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                ThisExpression(),
                IdentifierName(propertyDescriptor.Name)) as ExpressionSyntax;
        }

        private string FirstLetterToLower(string word)
        {
            return word.FirstOrDefault().ToString().ToLower() + word.Substring(1);
        }

        private bool IsFirstLetterLow(string word)
        {
            return word.FirstOrDefault() == word.FirstOrDefault().ToString().ToLower().FirstOrDefault();
        }
    }
}