namespace Lima.AuthenticationServer.Api.Services
{
    public interface ITokenValidatorService
    {
        bool Validate(string refreshToken);
    }
}
