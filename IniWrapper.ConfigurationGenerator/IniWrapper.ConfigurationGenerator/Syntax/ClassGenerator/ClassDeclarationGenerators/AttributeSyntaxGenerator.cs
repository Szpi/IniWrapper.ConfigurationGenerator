using IniWrapper.ConfigurationGenerator.Ini.Class;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.ClassDeclarationGenerators
{
    public class AttributeSyntaxGenerator : IClassDeclarationGenerator
    {
        private readonly ISyntaxGeneratorFacade _syntaxGeneratorFacade;

        public AttributeSyntaxGenerator(ISyntaxGeneratorFacade syntaxGeneratorFacade)
        {
            _syntaxGeneratorFacade = syntaxGeneratorFacade;
        }

        public ClassDeclarationSyntax Accept(ClassDeclarationSyntax compilationUnitSyntax, ClassToGenerate classToGenerate)
        {
            throw new System.NotImplementedException();
        }
    }
}