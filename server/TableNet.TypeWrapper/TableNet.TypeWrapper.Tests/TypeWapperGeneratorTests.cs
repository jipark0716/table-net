using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TableNet.TypeWrapper;
using Xunit;

namespace TableNet.StrongType.Tests;

public class StrongTypeGeneratorTests
{
    private const string Source = """
                                  namespace TestNamespace;
                                  
                                  [Wrapper<string>]
                                  [Validate<NotNullValidator>]
                                  public readonly partial record struct UserName;
                                  """;
    
    [Fact]
    public void TestGeneratorDebug()
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Source);
        
        CSharpCompilation compilation = CSharpCompilation.Create(
            "Tests",
            [syntaxTree],
            [],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );
        
        IIncrementalGenerator generator = new StrongTypeGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        
        if (!Debugger.IsAttached)
            Debugger.Launch();
        
        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outComp, out ImmutableArray<Diagnostic> diags);
        
        GeneratorDriverRunResult result = driver.GetRunResult();
        
        Assert.NotEmpty(result.GeneratedTrees);
    }
}