namespace TableNet.TypeWrapper.Validate;

public interface IValidatorFactory : IValidator
{
    ValidateErrorCode ErrorCode { get; }
    bool IsValid<T>(ref T value);
}