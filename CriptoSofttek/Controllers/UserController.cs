using CriptoSofttek.DTOs;
using CriptoSofttek.Entities;
using CriptoSofttek.Services;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Registrar nuevo usuario
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (await _unitOfWork.UserRepository.UserExists(dto.Email)) return BadRequest("El usuario ya existe");

            var user = new User(dto);
            await _unitOfWork.UserRepository.Add(user);

            await _unitOfWork.Complete();

            return Ok();
        }

        /// <summary>
        /// Obtener usuario actual
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _unitOfWork.UserRepository.GetUserFromClaims(User);
            return Ok(user);
        }

        /// <summary>
        /// Transferir dinero a cuentas de terceros
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Transfer")]
        public async Task<IActionResult> Transfer(TransferDTO dto)
        {
            var user = await _unitOfWork.UserRepository.GetUserFromClaims(User);
            var fiatAccount = new FiatAccount();
            var cryptoAccount = new CryptoAccount();
            if (dto.CBUOrUUIDDestination == dto.CBUOrUUID) return BadRequest("No se puede transferir a la misma cuenta");
            switch (dto.Currency)
            {
                case 1:
                    fiatAccount = await _unitOfWork.FiatAccountRepository.GetFiatAccountByCBU(dto.CBUOrUUID);
                    if (fiatAccount.UserId != user.Id) return BadRequest("La cuenta no pertenece al usuario");
                    if (fiatAccount.PesosBalance < dto.Amount)
                    {
                        return BadRequest("No tiene saldo suficiente");
                    }
                    else
                    {
                        fiatAccount.PesosBalance -= dto.Amount;
                        var accountTo = await _unitOfWork.FiatAccountRepository.GetFiatAccountByCBU(dto.CBUOrUUIDDestination);
                        accountTo.PesosBalance += dto.Amount;
                        await _unitOfWork.MovementRepository.SaveTransaction(user.Id, dto.Amount, "Pesos", "Transferencia");
                    }
                    break;
                case 2:
                    fiatAccount = await _unitOfWork.FiatAccountRepository.GetFiatAccountByCBU(dto.CBUOrUUID);
                    if (fiatAccount.UserId != user.Id) return BadRequest("La cuenta no pertenece al usuario");
                    if (fiatAccount.USDBalance < dto.Amount)
                    {
                        return BadRequest("No tiene saldo suficiente");
                    }
                    else
                    {
                        fiatAccount.USDBalance -= dto.Amount;
                        var accountTo = await _unitOfWork.FiatAccountRepository.GetFiatAccountByCBU(dto.CBUOrUUIDDestination);
                        accountTo.USDBalance += dto.Amount;
                        await _unitOfWork.MovementRepository.SaveTransaction(user.Id, dto.Amount, "USD", "Transferencia");
                    }
                    break;
                case 3:
                    cryptoAccount = await _unitOfWork.CryptoAccountRepository.GetCryptoAccountByUUID(dto.CBUOrUUID);
                    if (cryptoAccount.UserId != user.Id) return BadRequest("La cuenta no pertenece al usuario");
                    if (cryptoAccount.Balance < dto.Amount)
                    {
                        return BadRequest("No tiene saldo suficiente");
                    }
                    else
                    {
                        cryptoAccount.Balance -= dto.Amount;
                        var accountTo = await _unitOfWork.CryptoAccountRepository.GetCryptoAccountByUUID(dto.CBUOrUUIDDestination);
                        accountTo.Balance += dto.Amount;
                        await _unitOfWork.MovementRepository.SaveTransaction(user.Id, dto.Amount, "BTC", "Transferencia");
                    }
                    break;
                default:
                    return BadRequest("Moneda no valida");
            }
            await _unitOfWork.Complete();
            return Ok();
        }
    }
}
