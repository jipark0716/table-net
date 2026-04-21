using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TableNet.TypeWrapper;

[Generator]
public class StrongTypeGenerator : IIncrementalGenerator
{
    private readonly record struct Meta(
        string FileName,
        string NameSpace,
        string ClassName,
        string TypeName,
        bool IsReferenceType,
        List<AttributeData> ValidatorAttribute
    );

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<Meta?> strongTypeStructs =
            context.SyntaxProvider.CreateSyntaxProvider(
                predicate: (node, _) => node is RecordDeclarationSyntax,
                transform: Transform
            );
    
        context.RegisterSourceOutput(strongTypeStructs, _generate);
    }

    private static Meta? Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        if (
            context.Node is not RecordDeclarationSyntax decl ||
            context.SemanticModel.GetDeclaredSymbol(decl) is not INamedTypeSymbol alias
        ) return null; 
        AttributeData? wrapperAttribute = alias.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name is "Wrapper" or "WrapperAttribute");
        
        if (
            wrapperAttribute is null ||
            wrapperAttribute.AttributeClass!.TypeArguments.Length != 1 ||
            wrapperAttribute.AttributeClass.TypeArguments[0] is not INamedTypeSymbol origin
        ) return null;
        
        List<AttributeData> validatorAttribute = alias
            .GetAttributes()
            .Where(a => a.AttributeClass?.Name is "Validate" or "ValidateAttribute").ToList();

        return new Meta(
            alias.Name,
            alias.ContainingNamespace.ToString(),
            alias.Name,
            origin.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            origin.IsReferenceType,
            validatorAttribute
        );
    }

    private readonly Action<SourceProductionContext, Meta?> _generate =
        (context, target) =>
        {
            if (target is null) return;
            
            context.AddSource(
                $"{target.Value.FileName}.g.cs",
                GenerateContent(target.Value)
            );
        };

    static string GenerateContent(Meta meta) =>
        $$"""
          using TableNet.TypeWrapper.Validate;
          
          namespace {{meta.NameSpace}};

          public readonly partial record struct {{meta.ClassName}} : IWrapperType<{{meta.ClassName}}, {{meta.TypeName}}>
          {
              public {{meta.TypeName}} Value { get; }
              private {{meta.ClassName}}({{meta.TypeName}} value) => Value = value;
          
              public static ParseResult<{{meta.ClassName}}> Parse({{meta.TypeName}} value)
              {
                  List<ValidateErrorCode?> errors = [
                      {{
                          string.Join("\n", meta
                              .ValidatorAttribute
                              .Select(GenerateValidateContent)
                              .ToList())
                      }}
                  ];
                  
                  return ParseResult<{{meta.ClassName}}>.Create(new {{meta.ClassName}}(value), errors.OfType<ValidateErrorCode>().ToList());
              } 
          }
          """;

    static string GenerateValidateContent(AttributeData attribute) =>
        $"new {attribute.AttributeClass!.Name}().Run(ref value),";
}