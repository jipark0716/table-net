using TableNet.WebApi.Repository;
using TableNet.WebApi.Vos;

namespace TableNet.WebApi.Services;

public class AuthService(SessionFarm sessionFarm) : IScoped
{
    public Session LoginGuest()
    {
        Session session = new()
        {
            Id = new SessionId(Guid.CreateVersion7()),
        };
        
        sessionFarm.CreateSession(session);

        return session;
    }

    public bool Logout(SessionId id)
    {
        return sessionFarm.DeleteSession(id);
    }
}