using System.Linq;
using IniWrapper.ConfigurationGenerator.Ini.Class;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.Visitor.ClassDeclarationVisitors
{
    public class PropertySyntaxVisitor : IClassDeclarationVisitor
    {
        private readonly ISyntaxGeneratorFacade _syntaxGeneratorFacade;

        public PropertySyntaxVisitor(ISyntaxGeneratorFacade syntaxGeneratorFacade)
        {
            _syntaxGeneratorFacade = syntaxGeneratorFacade;
        }

        public ClassDeclarationSyntax Accept(ClassDeclarationSyntax classDeclarationSyntax, ClassToGenerate classToGenerate)
        {
            return classToGenerate.PropertyDescriptors
                                  .Where(propertyDescriptor => propertyDescriptor.SyntaxKind != SyntaxKind.List)
                                  .Select(propertyDescriptor => _syntaxGeneratorFacade.GetPropertyDeclarationSyntax(propertyDescriptor.Name, propertyDescriptor.SyntaxKind))
                                  .Aggregate(classDeclarationSyntax, (current, propertySyntax) => current.AddMembers(propertySyntax));
        }
    }
}