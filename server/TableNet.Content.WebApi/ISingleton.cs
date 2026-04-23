using JetBrains.Annotations;

namespace TableNet.WebApi;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)] public interface ISingleton;
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)] public interface IScoped;
[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)] public interface IGrpcServer : IScoped;