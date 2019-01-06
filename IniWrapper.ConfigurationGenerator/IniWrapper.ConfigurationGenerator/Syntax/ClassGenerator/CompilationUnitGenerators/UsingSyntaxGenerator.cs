using System.Linq;
using IniWrapper.ConfigurationGenerator.Ini.Class;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator.CompilationUnitGenerators
{
    public class UsingSyntaxGenerator : IClassToGenerateGenerator
    {
        private readonly ISyntaxGeneratorFacade _syntaxGeneratorFacade;

        public UsingSyntaxGenerator(ISyntaxGeneratorFacade syntaxGeneratorFacade)
        {
            _syntaxGeneratorFacade = syntaxGeneratorFacade;
        }

        public CompilationUnitSyntax Accept(CompilationUnitSyntax compilationUnitSyntax, ClassToGenerate classToGenerate)
        {
            return classToGenerate.UsingsToGenerate
                                  .Select(@using => _syntaxGeneratorFacade.GetUsingSyntax(@using))
                                  .Aggregate(compilationUnitSyntax, (current, @using) => current.AddUsings(@using));
        }
    }
}