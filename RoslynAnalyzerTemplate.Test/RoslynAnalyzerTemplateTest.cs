using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dena.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RoslynAnalyzerTemplate.Test
{
    /// <summary>
    /// This test is an examples of using the Dena.CodeAnalysis.Testing test helper library.
    /// <see cref="https://github.com/DeNA/Dena.CodeAnalysis.Testing"/>
    /// </summary>
    [TestClass]
    public class RoslynAnalyzerTemplateTest
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            const string Test = "";

            var analyzer = new RoslynAnalyzerTemplateAnalyzer();
            var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, Test);

            Assert.AreEqual(0, diagnostics.Length);
        }

        [TestMethod]
        public async Task TestMethod2()
        {
            var analyzer = new RoslynAnalyzerTemplateAnalyzer();
            var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, ReadCodes("TypeName.cs"));

            var actual = diagnostics
                .Where(x => x.Id != "CS1591") // Ignore "Missing XML comment for publicly visible type or member"
                .Where(x => x.Id != "CS8019") // Ignore "Unnecessary using directive"
                .ToArray();

            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("RoslynAnalyzerTemplate", actual.First().Id);
            Assert.AreEqual("Type name 'TypeName' contains lowercase letters", actual.First().GetMessage());

            LocationAssert.HaveTheSpan(
                new LinePosition(9, 10),
                new LinePosition(9, 18),
                actual.First().Location
            );
        }

        private static string[] ReadCodes(params string[] sources)
        {
            const string Path = "../../../TestData";
            return sources.Select(file => File.ReadAllText($"{Path}/{file}", Encoding.UTF8)).ToArray();
        }
    }
}
