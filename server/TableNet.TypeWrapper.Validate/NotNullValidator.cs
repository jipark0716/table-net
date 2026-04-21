namespace TableNet.TypeWrapper.Validate;

public struct NotNullValidator : IValidatorFactory
{
    public ValidateErrorCode ErrorCode => new(1);

    public bool IsValid<T>(ref T value) => value is not null;
}