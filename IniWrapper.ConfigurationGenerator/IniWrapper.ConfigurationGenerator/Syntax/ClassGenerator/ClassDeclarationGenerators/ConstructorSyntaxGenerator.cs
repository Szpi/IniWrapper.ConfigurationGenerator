using IniWrapper.ConfigurationGenerator.Ini.Class;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.ClassDeclarationGenerators
{
    public class ConstructorSyntaxGenerator : IClassDeclarationGenerator
    {
        private readonly ISyntaxGeneratorFacade _syntaxGeneratorFacade;

        public ConstructorSyntaxGenerator(ISyntaxGeneratorFacade syntaxGeneratorFacade)
        {
            _syntaxGeneratorFacade = syntaxGeneratorFacade;
        }

        public ClassDeclarationSyntax Generate(ClassDeclarationSyntax classDeclarationSyntax, ClassToGenerate classToGenerate)
        {
            return classDeclarationSyntax.AddMembers(_syntaxGeneratorFacade.GetConstructorDeclarationSyntax(classToGenerate.ClassName, classToGenerate.PropertyDescriptors));
        }
    }
}