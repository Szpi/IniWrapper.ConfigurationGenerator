using System.Linq;
using IniWrapper.ConfigurationGenerator.Ini.Class;
using IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.PropertyDeclarationModifiers;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.ClassDeclarationGenerators
{
    public class PropertySyntaxGenerator : IClassDeclarationGenerator
    {
        private readonly ISyntaxGeneratorFacade _syntaxGeneratorFacade;
        private readonly IPropertyDeclarationSyntaxModifier _propertyDeclarationSyntaxModifier;
        public PropertySyntaxGenerator(ISyntaxGeneratorFacade syntaxGeneratorFacade, IPropertyDeclarationSyntaxModifier propertyDeclarationSyntaxModifier)
        {
            _syntaxGeneratorFacade = syntaxGeneratorFacade;
            _propertyDeclarationSyntaxModifier = propertyDeclarationSyntaxModifier;
        }

        public ClassDeclarationSyntax Generate(ClassDeclarationSyntax classDeclarationSyntax, ClassToGenerate classToGenerate)
        {
            return classToGenerate.PropertyDescriptors
                                  .Where(propertyDescriptor => propertyDescriptor.SyntaxKind != SyntaxKind.List)
                                  .Select(propertyDescriptor => _syntaxGeneratorFacade.GetPropertyDeclarationSyntax( propertyDescriptor.Name, propertyDescriptor.SyntaxKind))
                                  .Aggregate(classDeclarationSyntax, (current, propertySyntax) => current.AddMembers(_propertyDeclarationSyntaxModifier.Modify(classDeclarationSyntax, propertySyntax)));
        }
    }
}