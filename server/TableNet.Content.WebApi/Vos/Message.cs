using TableNet.TypeWrapper.Validate;

namespace TableNet.WebApi.Vos;

[Wrapper<ulong>]
public readonly partial record struct MessageId;

[Wrapper<string>]
public readonly partial record struct MessageContent;