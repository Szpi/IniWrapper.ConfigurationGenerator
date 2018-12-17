# IniWrapper.ConfigurationGenerator
If you are using [IniWrapper](https://github.com/Szpi/IniWrapper) this console application will help you generate classes to easier write/read from .ini file.

[Configuration Generator](./Readme/CommandLine.PNG)


Example of generated code:

```csharp
using System.Collections.Generic;

namespace Configuration
{
    class MainConfiguration
    {
        public ComplexType1 ComplexType1 { get; set; }
        public TestConfiguration TestConfiguration { get; set; }
        public List<ComplexTypes> ComplexTypes { get; set; }
    }
}


using IniWrapper.Attribute;

namespace Configuration
{
    class ComplexTypes
    {
        [IniOptions(Section = "ComplexTypes", Key = "IntTest")]
        public int IntTest { get; set; }
        [IniOptions(Section = "ComplexTypes", Key = "StringTest")]
        public string StringTest { get; set; }
    }
}

or 

namespace Configuration
{
    class ComplexTypes
    {
        public int IntTest { get; set; }
        public string StringTest { get; set; }
    }
}
```