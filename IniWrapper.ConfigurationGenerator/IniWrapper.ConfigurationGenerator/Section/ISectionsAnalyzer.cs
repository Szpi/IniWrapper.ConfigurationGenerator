using System.Collections.Generic;

namespace IniWrapper.ConfigurationGenerator.Section
{
    public interface ISectionsAnalyzer
    {
        (List<string> SeparateSections, List<(string className, string firstSectionInIniFile)> ComplexDataSections) AnalyzeSections(IEnumerable<string> sections);
    }
}