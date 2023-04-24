using Lima.AuthenticationServer.Api.Model;

namespace Lima.AuthenticationServer.Api.Services
{
    public interface ITokenService
    {
        string Generate(User user);
    }
}
