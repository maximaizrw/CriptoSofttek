using CriptoSofttek.DTOs;
using CriptoSofttek.Entities;
using CriptoSofttek.Helpers;
using CriptoSofttek.Services;
using Microsoft.AspNetCore.Mvc;

namespace CriptoSofttek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (await _unitOfWork.UserRepository.UserExists(dto.Email)) return BadRequest("El usuario ya existe");

            var user = new User(dto);
            await _unitOfWork.UserRepository.Add(user);

            // Esperar a que el usuario se agregue a la base de datos
            await _unitOfWork.Complete();

            // Obtener el ID del usuario recién registrado
            var id = await _unitOfWork.UserRepository.GetIdByEmail(dto.Email);

            // Crear cuentas
            var fiatAccount = new FiatAccount(dto, id);
            var cryptoAccount = new CryptoAccount(dto, id);
            await _unitOfWork.FiatAccountRepository.Add(fiatAccount);
            await _unitOfWork.CryptoAccountRepository.Add(cryptoAccount);

            // Esperar a que las cuentas se agreguen a la base de datos
            await _unitOfWork.Complete();

            // Obtener los IDs de las cuentas después de que se han agregado a la base de datos
            var fiatAccountId = fiatAccount.Id;
            var cryptoAccountId = cryptoAccount.Id;

            // Modificar el id de las cuentas en el usuario
            user.FiatAccountId = fiatAccountId;
            user.CryptoAccountId = cryptoAccountId;

            // Actualizar el usuario en la base de datos para reflejar los cambios en las cuentas
            await _unitOfWork.Complete();

            return Ok();
        }




    }
}
