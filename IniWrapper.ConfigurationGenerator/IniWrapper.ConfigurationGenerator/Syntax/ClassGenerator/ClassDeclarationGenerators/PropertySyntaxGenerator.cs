using System.Linq;
using IniWrapper.ConfigurationGenerator.Ini.Class;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.ClassDeclarationGenerators
{
    public class PropertySyntaxGenerator : IClassDeclarationGenerator
    {
        private readonly ISyntaxGeneratorFacade _syntaxGeneratorFacade;

        public PropertySyntaxGenerator(ISyntaxGeneratorFacade syntaxGeneratorFacade)
        {
            _syntaxGeneratorFacade = syntaxGeneratorFacade;
        }

        public ClassDeclarationSyntax Accept(ClassDeclarationSyntax classDeclarationSyntax, ClassToGenerate classToGenerate)
        {
            return classToGenerate.PropertyDescriptors
                                  .Where(propertyDescriptor => propertyDescriptor.SyntaxKind != SyntaxKind.List && propertyDescriptor.SyntaxKind != SyntaxKind.ClassDeclaration)
                                  .Select(propertyDescriptor =>
                                  {
                                      var property = _syntaxGeneratorFacade.GetPropertyDeclarationSyntax(
                                          propertyDescriptor.Name, propertyDescriptor.SyntaxKind);
                                      property.Accept(new AttributeSyntaxGenerator(_syntaxGeneratorFacade));
                                      return property;
                                  })
                                  .Aggregate(classDeclarationSyntax, (current, propertySyntax) => current.AddMembers(propertySyntax));
        }
    }
}