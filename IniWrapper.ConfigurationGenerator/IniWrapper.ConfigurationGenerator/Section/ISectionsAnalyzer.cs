using System.Collections.Generic;

namespace IniWrapper.ConfigurationGenerator.Section
{
    public interface ISectionsAnalyzer
    {
        (List<string> SeparateSections, List<string> ComplexDataSections) AnalyzeSections(IEnumerable<string> sections);
    }
}