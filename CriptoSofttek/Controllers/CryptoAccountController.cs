using CriptoSofttek.DTOs;
using CriptoSofttek.Entities;
using CriptoSofttek.Helpers;
using CriptoSofttek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CriptoSofttek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CrytoAccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CrytoAccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Crear una cuenta de cripto
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAccount()
        {
            var user = await _unitOfWork.UserRepository.GetUserFromClaims(User);

            var cryptoAccount = new CryptoAccount(user.Id);
            await _unitOfWork.CryptoAccountRepository.Add(cryptoAccount);
            await _unitOfWork.Complete();
            return Ok();
        }

        /// <summary>
        /// Obtener balance de cuentas propias de cripto
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _unitOfWork.UserRepository.GetUserFromClaims(User);
            var cryptoAccounts = await _unitOfWork.CryptoAccountRepository.GetCryptoAccountsByUserId(user.Id);

            if (cryptoAccounts == null)
            {
                return BadRequest("No tiene cuenta de cripto");
            }
            var cryptoBalance = new List<CryptoBalanceDTO>();
            foreach (var account in cryptoAccounts)
            {
                var crypto = new CryptoBalanceDTO();
                crypto.UUID = account.UUID;
                crypto.Balance = account.Balance;
                cryptoBalance.Add(crypto);
            }
            return Ok(cryptoBalance);
        }

        /// <summary>
        /// Depositar BTC en cuenta propia de cripto
        /// </summary>
        /// <param name="cryptoDepositDTO"></param>
        /// <returns></returns>
        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] CryptoDepositDTO cryptoDepositDTO)
        {
            var user = await _unitOfWork.UserRepository.GetUserFromClaims(User);
            var cryptoAccount = await _unitOfWork.CryptoAccountRepository.GetCryptoAccountByUUID(cryptoDepositDTO.UUID);
            if (cryptoAccount == null)
            {
                return BadRequest("Invalid UUID");
            }
            if (cryptoDepositDTO.Amount <= 0)
            {
                return BadRequest("Invalid amount");
            }
            cryptoAccount.Balance += cryptoDepositDTO.Amount;
            await _unitOfWork.MovementRepository.SaveTransaction(user.Id, cryptoDepositDTO.Amount, "BTC", "Deposito");
            await _unitOfWork.Complete();
            return Ok();
        }

        /// <summary>
        /// Retirar BTC en cuenta propia de cripto
        /// </summary>
        /// <param name="cryptoDepositDTO"></param>
        /// <returns></returns>
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(string uuid, decimal amount)
        {
            var user = await _unitOfWork.UserRepository.GetUserFromClaims(User);
            var cryptoAccount = await _unitOfWork.CryptoAccountRepository.GetCryptoAccountByUUID(uuid);
            if (cryptoAccount == null)
            {
                return BadRequest("Invalid UUID");
            }
            if (amount <= 0)
            {
                return BadRequest("Invalid amount");
            }
            cryptoAccount.Balance -= amount;
            await _unitOfWork.MovementRepository.SaveTransaction(user.Id, amount, "BTC", "Retiro");
            await _unitOfWork.Complete();
            return Ok();
        }

        /// <summary>
        /// Comprar criptomonedas ingresando los dolares a gastar. 1USD = 0.000033BTC
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BuyCrypto")]
        public async Task<IActionResult> BuyCrypto(BuyCryptoDTO dto)
        {
            var user = await _unitOfWork.UserRepository.GetUserFromClaims(User);
            var fiatAccount = await _unitOfWork.FiatAccountRepository.GetFiatAccountByAccountNumber(dto.FiatAccountNumber);
            if (fiatAccount == null)
            {
                return BadRequest("Invalid account number");
            }
            // Verificar si el usuario tiene suficientes pesos para comprar la cantidad deseada en dólares
            decimal pesosRequired = ExchangeHelper.ConvertUSDToPesos(dto.USDAmount);
            if (fiatAccount.PesosBalance < pesosRequired)
            {
                return BadRequest("No tiene suficientes pesos para comprar esta cantidad de criptomonedas.");
            }
            // Actualizar el saldo en pesos restando la cantidad requerida
            fiatAccount.PesosBalance -= pesosRequired;
            await _unitOfWork.Complete();
            decimal btcAmount = ExchangeHelper.ConvertUSDToBTC(dto.USDAmount);
            var cryptoAccount = await _unitOfWork.CryptoAccountRepository.GetCryptoAccountByUserId(user.Id);
            cryptoAccount.Balance += btcAmount;
            await _unitOfWork.MovementRepository.SaveTransaction(user.Id, dto.USDAmount, "USD", "Compra BTC");
            await _unitOfWork.Complete();
            return Ok("Compra de BTC exitosa.");
        }
    }
}
