using System.Collections.Generic;
using IniWrapper.ConfigurationGenerator.Ini.Class;

namespace IniWrapper.ConfigurationGenerator.Ini
{
    public class IniFileContext
    {
        public IReadOnlyList<ClassToGenerate> ClassesToGenerate { get;}
        public IReadOnlyList<ClassToGenerate> ComplexClassesToGenerate { get;}
        public IReadOnlyList<string> UsingsToGenerate { get; }

        public IniFileContext(IReadOnlyList<ClassToGenerate> classesToGenerate,
                              IReadOnlyList<ClassToGenerate> complexClassesToGenerate,
                              IReadOnlyList<string> usingsToGenerate)
        {
            ClassesToGenerate = classesToGenerate;
            ComplexClassesToGenerate = complexClassesToGenerate;
            UsingsToGenerate = usingsToGenerate;
        }
    }
}