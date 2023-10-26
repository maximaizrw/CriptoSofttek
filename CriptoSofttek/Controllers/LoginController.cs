using CriptoSofttek.DTOs;
using CriptoSofttek.Helpers;
using CriptoSofttek.Services;
using Microsoft.AspNetCore.Mvc;

namespace CriptoSofttek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private TokenJwtHelper _tokenJwtHelper;
        private readonly IUnitOfWork _unitOfWork;
        public LoginController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _tokenJwtHelper = new TokenJwtHelper(configuration);
        }

        /// <summary>
        /// Se logea un usuario
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>retorna el token del usuario</returns>
        [HttpPost]
        public async Task<IActionResult> Login(AuthenticateDTO dto)
        {
            var userCredentials = await _unitOfWork.UserRepository.AuthenticateCredentials(dto);
            if (userCredentials is null) return Unauthorized("Credenciales incorrectas");

            var token = _tokenJwtHelper.GenerateToken(userCredentials);

            var user = new UserLoginDTO()
            {
                FirstName = userCredentials.FirstName,
                Email = userCredentials.Email,
                Token = token
            };

            return Ok(user);
        }

    }
}
