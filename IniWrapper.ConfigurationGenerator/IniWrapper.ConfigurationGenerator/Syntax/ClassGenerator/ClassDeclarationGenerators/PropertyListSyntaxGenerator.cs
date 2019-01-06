using System.Linq;
using IniWrapper.ConfigurationGenerator.Ini.Class;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.ClassDeclarationGenerators
{
    public class PropertyListSyntaxGenerator : IClassDeclarationGenerator
    {
        private readonly ISyntaxGeneratorFacade _syntaxGeneratorFacade;

        public PropertyListSyntaxGenerator(ISyntaxGeneratorFacade syntaxGeneratorFacade)
        {
            _syntaxGeneratorFacade = syntaxGeneratorFacade;
        }

        public ClassDeclarationSyntax Accept(ClassDeclarationSyntax classDeclarationSyntax,
                                             ClassToGenerate classToGenerate)
        {
            return classToGenerate.PropertyDescriptors
                                  .Where(propertyDescriptor => propertyDescriptor.SyntaxKind == SyntaxKind.List)
                                  .Select(propertyDescriptor => _syntaxGeneratorFacade.GetListPropertyDeclarationSyntax(propertyDescriptor.Name, propertyDescriptor.UnderlyingSyntaxKind))
                                  .Aggregate(classDeclarationSyntax, (current, propertySyntax) => current.AddMembers(propertySyntax));
        }
    }
}