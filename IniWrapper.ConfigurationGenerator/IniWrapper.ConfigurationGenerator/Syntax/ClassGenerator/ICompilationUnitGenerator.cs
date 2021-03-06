﻿using IniWrapper.ConfigurationGenerator.Ini.Class;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IniWrapper.ConfigurationGenerator.Syntax.ClassGenerator
{
    public interface ICompilationUnitGenerator
    {
        CompilationUnitSyntax Generate(CompilationUnitSyntax compilationUnitSyntax, ClassToGenerate classToGenerate);
    }
}