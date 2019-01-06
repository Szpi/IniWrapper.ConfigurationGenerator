using System.Linq;
using IniWrapper.ConfigurationGenerator.Ini.Class;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.Visitor.CompilationUnitVisitors
{
    public class UsingSyntaxVisitor : IClassToGenerateVisitor
    {
        private readonly ISyntaxGeneratorFacade _syntaxGeneratorFacade;

        public UsingSyntaxVisitor(ISyntaxGeneratorFacade syntaxGeneratorFacade)
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