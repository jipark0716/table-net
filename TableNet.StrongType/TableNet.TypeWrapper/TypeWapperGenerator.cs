using System;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TableNet.TypeWrapper;

[Generator]
public class StrongTypeGenerator : IIncrementalGenerator
{
    private const string AttributeClassName = "WapperAttribute";
    private const string AttributeName = "Wapper";

    private readonly record struct Meta(
        string NameSpace,
        string ClassName,
        string TypeName,
        bool IsReferenceType
    );

    internal readonly record struct Target(
        INamedTypeSymbol? Alias,
        INamedTypeSymbol? Origin,
        string? FailMessage = null)
    {
        public static Target Fail(string message) => new(null, null, message);
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<Target> strongTypeStructs =
            context.SyntaxProvider.CreateSyntaxProvider(
                predicate: (node, _) => node is RecordDeclarationSyntax,
                transform: Transform
            );
    
        context.RegisterSourceOutput(strongTypeStructs, _generate);
    }

    private static Target Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        if (context.Node is not RecordDeclarationSyntax decl)
            return Target.Fail($"not support {context.Node.GetType().Name} ({context.GetType().Name})");

        if (context.SemanticModel.GetDeclaredSymbol(decl) is not INamedTypeSymbol alias) return Target.Fail("alias is null");

        AttributeData? strongTypeAttr = alias.GetAttributes()
            .FirstOrDefault(a =>
                a.AttributeClass?.Name is AttributeName or AttributeClassName);

        if (strongTypeAttr is null)
            return Target.Fail($"{alias.Name} miss Attribute");

        if (strongTypeAttr.AttributeClass!.TypeArguments.Length != 1)
            return Target.Fail("not found Attribute Generic argument");

        if (strongTypeAttr.AttributeClass.TypeArguments[0] is not INamedTypeSymbol origin)
            return Target.Fail("Attribute Generic argument is not NamedTypeSymbol");

        return new Target(alias, origin);
    }

    private readonly Action<SourceProductionContext, Target> _generate =
        (context, target) =>
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    new DiagnosticDescriptor(
                        id: "DBG001",
                        title: "debug",
                        messageFormat: target.FailMessage ?? "success",
                        category: "debug",
                        DiagnosticSeverity.Warning,
                        isEnabledByDefault: true),
                    Location.None));
            
            string content = GenerateRefContent(
                new Meta(
                    target.Alias!.ContainingNamespace.ToString(),
                    target.Alias.Name,
                    target.Origin!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    target.Origin!.IsReferenceType
                ));
            context.AddSource($"{target.Alias!.Name}.g.cs", content);
        };

    static string GenerateRefContent(Meta meta) =>
        $$"""
          namespace {{meta.NameSpace}};

          public readonly record struct {{meta.ClassName}}({{meta.TypeName}} Value);
          """;
}