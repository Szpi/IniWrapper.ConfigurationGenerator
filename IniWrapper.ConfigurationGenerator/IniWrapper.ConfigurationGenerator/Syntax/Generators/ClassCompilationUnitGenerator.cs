using System.Collections.Generic;
using System.Linq;
using IniWrapper.ConfigurationGenerator.Ini;
using IniWrapper.ConfigurationGenerator.Syntax.Visitor;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.Generators
{
    public class ClassCompilationUnitGenerator : ICompilationUnitGenerator
    {
        private readonly IReadOnlyList<IClassToGenerateVisitor> _classToGenerateVisitors;
        private readonly ISyntaxGeneratorFacade _syntaxGeneratorFacade;

        public ClassCompilationUnitGenerator(IReadOnlyList<IClassToGenerateVisitor> classToGenerateVisitors, ISyntaxGeneratorFacade syntaxGeneratorFacade)
        {
            _classToGenerateVisitors = classToGenerateVisitors;
            _syntaxGeneratorFacade = syntaxGeneratorFacade;
        }

        public List<(CompilationUnitSyntax compilationUnitsSyntax, string className)> Accept(IniFileContext iniFileContext)
        {
            var compilationUnitsSyntax = new List<(CompilationUnitSyntax compilationUnitsSyntax, string className)>();

            foreach (var classToGenerate in iniFileContext.ClassesToGenerate.Concat(iniFileContext.ComplexClassesToGenerate))
            {
                var compilationUnitSyntax = _syntaxGeneratorFacade.GetCompilationUnitSyntax();
                compilationUnitSyntax = _classToGenerateVisitors
                                       .Aggregate(compilationUnitSyntax, (current, classToGenerateVisitor) => classToGenerateVisitor.Accept(current, classToGenerate));

                compilationUnitsSyntax.Add((compilationUnitSyntax, classToGenerate.ClassName));
            }
            return compilationUnitsSyntax;
        }
    }
}