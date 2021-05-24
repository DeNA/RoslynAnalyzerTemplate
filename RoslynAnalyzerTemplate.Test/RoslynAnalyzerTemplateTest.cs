using System.Linq;
using System.Threading.Tasks;
using Dena.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RoslynAnalyzerTemplate.Test
{
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
            const string Test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {
        }
    }";

            var analyzer = new RoslynAnalyzerTemplateAnalyzer();
            var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, Test);

            var warns = diagnostics
                .Where(x => x.Severity >= DiagnosticSeverity.Warning) // Ignore Hidden and Info
                .Where(x => x.Id != "CS1591") // Ignore "Missing XML comment for publicly visible type or member"
                .ToArray();
            Assert.AreEqual(1, warns.Length);

            var actual = warns.First();
            Assert.IsNotNull(actual);
            Assert.AreEqual("RoslynAnalyzerTemplate", actual.Id);
            Assert.AreEqual("Type name 'TypeName' contains lowercase letters", actual.GetMessage());

            LocationAssert.HaveTheSpan(
                new LinePosition(10, 14),
                new LinePosition(10, 22),
                actual.Location
            );
        }
    }
}
