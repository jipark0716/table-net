namespace TableNet.TypeWrapper.Validate;

public interface IValidator;

public interface IValidator<T> : IValidator
{
    ValidateErrorCode ErrorCode { get; }
    bool IsValid(ref T value);
}

public static class ValidatorExtensions
{
    public static ValidateErrorCode? IsValid<T>(this IValidator<T> validator, ref T value)
        => validator.IsValid(ref value) ?
            validator.ErrorCode :
            null;
    
    public static ValidateErrorCode? IsValid<T>(this IValidatorFactory validator, ref T value)
        => validator.IsValid(ref value) ?
            validator.ErrorCode :
            null;
}