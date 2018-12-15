using System.Collections.Generic;
using System.Linq;

namespace IniWrapper.ConfigurationGenerator.Section
{
    public class SectionsAnalyzer : ISectionsAnalyzer
    {
        private readonly char _separator;

        public SectionsAnalyzer(char separator)
        {
            _separator = separator;
        }

        public (List<string> SeparateSections, List<string> ComplexDataSections) AnalyzeSections(IEnumerable<string> sections)
        {
            var sortedSections = sections.ToList();
            
            var complexDataSections = new List<string>();
            var basicSections = new List<string>();
            for (var i = sortedSections.Count - 1; i >= 0; i--)
            {
                var section = sortedSections[i];
                var lastIndexOfSeparator = section.LastIndexOf(_separator);
                if (lastIndexOfSeparator < 0)
                {
                    basicSections.Add(section);
                    continue;
                }

                var intersectedComplexDataName = section.Substring(0, lastIndexOfSeparator + 1);

                complexDataSections.Add(section.Substring(0, lastIndexOfSeparator));
                var removedItems = sortedSections.RemoveAll(x => x.StartsWith(intersectedComplexDataName));
                i -= removedItems - 1;
            }

            return (basicSections, complexDataSections);
        }
    }
}