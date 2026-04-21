using TableNet.Common;
using TableNet.TypeWrapper.Validate;

namespace TableNet.WebApi.Vos;

public static class ProtoExtensions
{
    public static InvalidRequestError ToInvalidRequestError(this IEnumerable<ValidateErrorCode> errors, string fieldName)
        => new()
        {
            FieldName = fieldName,
            ErrorCods =
            {
                errors.Select(x => x.Value),
            },
        };

    public static InvalidRequestErrors CreateInvalidRequestErrors(
        params (IEnumerable<ValidateErrorCode> errors, string fieldName)[] args) =>
        new()
        {
            Errors =
            {
                args.Select(o => o.errors.ToInvalidRequestError(o.fieldName)),
            },
        };
}