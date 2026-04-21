using JetBrains.Annotations;

namespace TableNet.TypeWrapper.Validate;

[AttributeUsage(AttributeTargets.Struct)]
public class WrapperAttribute<[UsedImplicitly] T> : Attribute;

[AttributeUsage(AttributeTargets.Struct)]
public class ValidateAttribute<[UsedImplicitly] T> : Attribute
    where T : IValidator;

public interface IWrapperType<T, TInner>
    where T : struct
{
    static abstract ParseResult<T> Parse(ref TInner value);
}

public readonly struct ParseResult<T>
    where T : struct
{
    public readonly bool IsSuccess;
    public readonly T Value;
    public readonly List<ValidateErrorCode> Errors;

    private ParseResult(bool isSuccess, T value, List<ValidateErrorCode> errors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
    }

    private static ParseResult<T> Success(T value) => new(true, value, []);
    private static ParseResult<T> Fail(List<ValidateErrorCode> errors) => new(false, default, []);

    public static ParseResult<T> Create(T value, List<ValidateErrorCode> errors) =>
        errors.Count != 0 ? Fail(errors) : Success(value);
}