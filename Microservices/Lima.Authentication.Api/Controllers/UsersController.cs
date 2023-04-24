using Lima.AuthenticationServer.Api.Model;
using Lima.AuthenticationServer.Api.Repositories;
using Lima.AuthenticationServer.Api.Services;
using Lima.AuthenticationServer.Api.ViewModel;
using Lima.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Lima.AuthenticationServer.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IAccessTokenService _accessTokenService;
        private IRefreshTokenService _refreshTokenService;
        private ITokenValidatorService _tokenValidatorService;

        private IUserRepository _userRepository;

        public UsersController(IAccessTokenService accessTokenService,
            IRefreshTokenService refreshTokenService,
            ITokenValidatorService tokenValidatorService,
            IUserRepository userRepository)
        {
            _accessTokenService = accessTokenService;
            _refreshTokenService = refreshTokenService;
            _tokenValidatorService = tokenValidatorService;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Производит аутентификацию в системе.
        /// </summary>
        /// <param name="authenticationRequest"></param>
        /// <returns></returns>
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate(string login, string password, bool remember)
        {
            //string salt = SecurePasswordHasher.Salt();
          ///string password = BCrypt.Net.BCrypt.HashPassword("test");

            //authenticationViewModel.Password = SecurePasswordHasher.Hash(authenticationViewModel.Password);

            User findedUser = await _userRepository.FindAsync(login);

            if (findedUser == null || !BCrypt.Net.BCrypt.Verify(password, findedUser.Password))
            {
                return BadRequest(new LimaResponse() 
                { 
                    Code = -1,
                    Message = "Неверный логин или пароль."
                });
            }

            findedUser.Grants = (await _userRepository.GetUserGrantsAsync(findedUser.Id, findedUser.CompanyUId)).ToList();

            //string refreshToken = _refreshTokenService.Generate(findedUser);
            string accessToken = _accessTokenService.Generate(findedUser);

            //await _userRepository.UpdateRefreshTokenAsync(findedUser, refreshToken);

            return Ok(new 
            {
                AccessToken = accessToken,
                Grants = findedUser.Grants,
            });
        }

        /// <summary>
        /// Производит обновление токена доступа.
        /// </summary>
        /// <param name="refreshTokenRequest">Refresh-токен.</param>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            bool validateRefreshToken = _tokenValidatorService.Validate(refreshTokenRequest.RefreshToken);

            if (!validateRefreshToken)
            {
                return BadRequest(new LimaResponse()
                {
                    Code = -1,
                    Message = "Невалидный токен обновления.",
                });
            }

            var user = await _userRepository.FindUserbyRefreshTokenAsync(refreshTokenRequest.RefreshToken);

            if (user == null)
            {
                return BadRequest(new LimaResponse()
                {
                    Code = -1,
                    Message = "Пользователь не найден.",
                });
            }

            //string refreshToken = _refreshTokenService.Generate(user);
            string accessToken = _accessTokenService.Generate(user);

            //await _userRepository.UpdateRefreshTokenAsync(user, refreshToken);

            return Ok(new
            {
                AccessToken = accessToken,
                //RefreshToken = refreshToken,
            });
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok("Test");
        }
    }
}
