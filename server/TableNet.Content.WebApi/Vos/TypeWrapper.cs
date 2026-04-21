using TableNet.TypeWrapper.Validate;

namespace TableNet.WebApi.Vos;

[Wrapper<ulong>]
public readonly partial record struct MessageId;

[Wrapper<string>]
[Validate<NotNullValidator>]
public readonly partial record struct MessageContent;

[Wrapper<ulong>]
public readonly partial record struct RoomId;

[Wrapper<ulong>]
public readonly partial record struct SessionId;