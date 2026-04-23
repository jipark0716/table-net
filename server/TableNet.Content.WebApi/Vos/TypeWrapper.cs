using TableNet.TypeWrapper.Validate;

namespace TableNet.WebApi.Vos;

[Wrapper<Guid>]
public readonly partial record struct MessageId;

[Wrapper<string>]
[Validate<NotNullValidator>]
public readonly partial record struct MessageContent;

[Wrapper<Guid>]
public readonly partial record struct RoomId;

[Wrapper<string>]
[Validate<NotNullValidator>]
public readonly partial record struct RoomName;

[Wrapper<Guid>]
public readonly partial record struct SessionId;