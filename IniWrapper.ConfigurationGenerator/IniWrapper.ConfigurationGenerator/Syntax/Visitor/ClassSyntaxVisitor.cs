using System.Collections.Generic;
using IniWrapper.ConfigurationGenerator.Ini.Class;
using IniWrapper.ConfigurationGenerator.Syntax.Visitor.ClassDeclarationVisitors;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.Visitor
{
    public class ClassSyntaxVisitor : IClassToGenerateVisitor
    {
        private readonly ISyntaxGeneratorFacade _syntaxGeneratorFacade;
        private readonly IReadOnlyList<IClassDeclarationVisitor> _classDeclarationVisitors;

        public ClassSyntaxVisitor(ISyntaxGeneratorFacade syntaxGeneratorFacade, IReadOnlyList<IClassDeclarationVisitor> classDeclarationVisitors)
        {
            _syntaxGeneratorFacade = syntaxGeneratorFacade;
            _classDeclarationVisitors = classDeclarationVisitors;
        }

        public CompilationUnitSyntax Accept(CompilationUnitSyntax compilationUnitSyntax, ClassToGenerate classToGenerate)
        {
            var classDeclarationSyntax = _syntaxGeneratorFacade.GetClassSyntax(classToGenerate.ClassName);

            foreach (var classDeclarationVisitor in _classDeclarationVisitors)
            {
                classDeclarationSyntax = classDeclarationVisitor.Accept(classDeclarationSyntax, classToGenerate);
            }

            return compilationUnitSyntax.AddMembers(classDeclarationSyntax);
        }
    }
}