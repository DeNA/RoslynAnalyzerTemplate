// Copyright (c) 2020-2023 DeNA Co., Ltd.
// This software is released under the MIT License.

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dena.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace RoslynAnalyzerTemplate.Test;

/// <summary>
/// This test is an examples of using the Dena.CodeAnalysis.Testing test helper library.
/// <see href="https://github.com/DeNA/Dena.CodeAnalysis.Testing"/>
/// </summary>
[TestFixture]
public class RoslynAnalyzerTemplateTest
{
    /// <summary>
    /// Test analyze for empty source code
    /// </summary>
    [Test]
    public async Task EmptySourceCode_NoDiagnosticReport()
    {
        const string Source = "";
        var analyzer = new RoslynAnalyzerTemplate();
        var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, Source);

        Assert.That(diagnostics, Is.Empty);
    }

    /// <summary>
    /// Test analyze for containing lowercase type name in source code
    /// </summary>
    [Test]
    public async Task TypeNameContainingLowercase_ReportOneDiagnostic()
    {
        var source = ReadCodes("TypeNameContainingLowercase.cs");
        var analyzer = new RoslynAnalyzerTemplate();
        var diagnostics = await DiagnosticAnalyzerRunner.Run(analyzer, source);

        var actual = diagnostics
            .Where(x => x.Id != "CS1591") // Ignore "Missing XML comment for publicly visible type or member"
            .Where(x => x.Id != "CS8019") // Ignore "Unnecessary using directive"
            .ToArray();

        Assert.That(actual, Has.Length.EqualTo(1));
        Assert.That(actual.First().Id, Is.EqualTo("RoslynAnalyzerTemplate0001"));
        Assert.That(actual.First().GetMessage(), Is.EqualTo("Type name 'TypeName' contains lowercase letters"));

        LocationAssert.HaveTheSpan(
            new LinePosition(12, 10),
            new LinePosition(12, 18),
            actual.First().Location
        );
    }

    private static string[] ReadCodes(params string[] sources)
    {
        const string Path = "../../../TestData";
        return sources.Select(file => File.ReadAllText($"{Path}/{file}", Encoding.UTF8)).ToArray();
    }
}
