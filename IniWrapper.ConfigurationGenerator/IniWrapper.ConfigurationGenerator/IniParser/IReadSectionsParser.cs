using System.Collections.Generic;

namespace IniWrapper.ConfigurationGenerator.IniParser
{
    internal interface IReadSectionsParser
    {
        IDictionary<string, string> Parse(string readSection);
    }
}