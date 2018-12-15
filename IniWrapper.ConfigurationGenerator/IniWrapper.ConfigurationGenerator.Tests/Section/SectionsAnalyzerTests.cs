using FluentAssertions;
using IniWrapper.ConfigurationGenerator.Section;
using NUnit.Framework;
using System.Collections.Generic;

namespace IniWrapper.ConfigurationGenerator.Tests.Section
{
    [TestFixture]
    public class SectionsAnalyzerTests
    {
        private readonly SectionsAnalyzer _sectionsAnalyzer = new SectionsAnalyzer('_');

        [Test]
        public void AnalyzeSections_ShouldReturnAllSections_And_EmptyComplexData_WhenNoComplexSectionsArePresent()
        {
            var sectionsFromIni = new List<string>()
            {
                "test",
                "test1",
                "test2",
                "test3",
                "test4",
            };

            var (sections, complexDataSections) = _sectionsAnalyzer.AnalyzeSections(sectionsFromIni);

            sections.Should().BeEquivalentTo(sectionsFromIni);
            complexDataSections.Should().NotBeNull().And.BeEmpty();
        }

        [Test]
        public void AnalyzeSections_ShouldReturnAllSections_And_ComplexData_WhenComplexSectionsArePresent()
        {
            var sectionsFromIni = new List<string>()
            {
                "test",
                "test1",
                "test2",
                "test3",
                "test4",
                "test_0",
                "test_1",
                "test_2",
            };

            var (sections, complexDataSections) = _sectionsAnalyzer.AnalyzeSections(sectionsFromIni);
            var expectedBasicSections = new List<string>()
            {
                "test",
                "test1",
                "test2",
                "test3",
                "test4",
            };

            sections.Should().BeEquivalentTo(expectedBasicSections);
            complexDataSections.Should().BeEquivalentTo(new List<string> { "test" });
        }

        [Test]
        public void AnalyzeSections_ShouldReturnAllSections_And_ManyComplexData_WhenComplexSectionsArePresent()
        {
            var sectionsFromIni = new List<string>()
            {
                "test",
                "test1",
                "test2",
                "test3",
                "test4",
                "test_0",
                "test_1",
                "test_2",
                "complex_0",
                "complex_1",
                "dom_0",
                "dom_1",
            };

            var (sections, complexDataSections) = _sectionsAnalyzer.AnalyzeSections(sectionsFromIni);

            var expectedBasicSections = new List<string>()
            {
                "test",
                "test1",
                "test2",
                "test3",
                "test4",
            };

            sections.Should().BeEquivalentTo(expectedBasicSections);

            var expected = new List<string> {"test", "complex", "dom"};
            complexDataSections.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void AnalyzeSections_ShouldReturnAllSections_And_ManyComplexData_WhenComplexSectionsArePresent_And_GivenInDifferentOrder()
        {
            var sectionsFromIni = new List<string>()
            {
                "test",
                "dom_1",
                "test1",
                "test_0",
                "test2",
                "test_1",
                "test3",
                "complex_0",
                "test4",
                "test_2",
                "dom_0",
                "complex_1",
            };

            var (sections, complexDataSections) = _sectionsAnalyzer.AnalyzeSections(sectionsFromIni);
            var expectedBasicSections = new List<string>()
            {
                "test",
                "test1",
                "test2",
                "test3",
                "test4",
            };

            sections.Should().BeEquivalentTo(expectedBasicSections);

            var expected = new List<string> { "test", "complex", "dom" };
            complexDataSections.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void AnalyzeSections_ShouldReturnAllSections_And_ManyComplexData_WhenComplexSectionsArePresent_And_GivenInDifferentOrderWithManyOneSection()
        {
            var sectionsFromIni = new List<string>()
            {
                "test",
                "test1",
                "test_0",
                "test2",
                "test_1",
                "test3",
                "complex_0",
                "complex_2",
                "complex_3",
                "complex_4",
                "complex_5",
                "test4",
                "test_2",
                "dom_0",
                "dom_1",
                "complex_1",
            };

            var (sections, complexDataSections) = _sectionsAnalyzer.AnalyzeSections(sectionsFromIni);
            var expectedBasicSections = new List<string>()
            {
                "test",
                "test1",
                "test2",
                "test3",
                "test4",
            };

            sections.Should().BeEquivalentTo(expectedBasicSections);

            var expected = new List<string> { "test", "complex", "dom" };
            complexDataSections.Should().BeEquivalentTo(expected);
        }
    }
}