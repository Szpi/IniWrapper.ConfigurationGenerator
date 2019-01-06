using IniWrapper.ConfigurationGenerator.Ini.Class;
using IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.PropertyDeclarationModifiers;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.ClassDeclarationGenerators
{
    public class PropertyListSyntaxGenerator : IClassDeclarationGenerator
    {
        private readonly ISyntaxGeneratorFacade _syntaxGeneratorFacade;
        private readonly IPropertyDeclarationSyntaxModifier _propertyDeclarationSyntaxModifier;

        public PropertyListSyntaxGenerator(ISyntaxGeneratorFacade syntaxGeneratorFacade, IPropertyDeclarationSyntaxModifier propertyDeclarationSyntaxModifier)
        {
            _syntaxGeneratorFacade = syntaxGeneratorFacade;
            _propertyDeclarationSyntaxModifier = propertyDeclarationSyntaxModifier;
        }

        public ClassDeclarationSyntax Generate(ClassDeclarationSyntax classDeclarationSyntax, ClassToGenerate classToGenerate)
        {
            return classToGenerate.PropertyDescriptors
                                  .Where(propertyDescriptor => propertyDescriptor.SyntaxKind == SyntaxKind.List)
                                  .Select(propertyDescriptor => _syntaxGeneratorFacade.GetListPropertyDeclarationSyntax(propertyDescriptor.Name, propertyDescriptor.UnderlyingSyntaxKind))
                                  .Aggregate(classDeclarationSyntax, (current, propertySyntax) => current.AddMembers(_propertyDeclarationSyntaxModifier.Modify(classDeclarationSyntax, propertySyntax)));
        }
    }
}