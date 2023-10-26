using CriptoSofttek.DTOs;
using CriptoSofttek.DTOs.Balances;
using CriptoSofttek.Entities;
using CriptoSofttek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CriptoSofttek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FiatAccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public FiatAccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Crear una cuenta fiduciaria en pesos y usd
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAccount()
        {
            var user = await _unitOfWork.UserRepository.GetUserFromClaims(User);
            var fiatAccount = new FiatAccount(user.Id, user.Email, user.LastName);
            await _unitOfWork.FiatAccountRepository.Add(fiatAccount);
            await _unitOfWork.Complete();
            return Ok();
        }

        /// <summary>
        /// Obtener balance de cuenta fiduciaria en pesos y usd
        /// </summary>
        /// <returns></returns>
        [HttpGet("{currency}")]
        public async Task<IActionResult> Get(int currency)
        {
            var user = await _unitOfWork.UserRepository.GetUserFromClaims(User);
            //Obtener todas las cuentas fiduciarias del usuario
            var fiatAccounts = await _unitOfWork.FiatAccountRepository.GetFiatAccountsByUserId(user.Id);
            //Si currency es 1 obtener balance en pesos, si es 2 obtener balance en usd
            if (currency == 1)
            {
                //Crear una lista para devolver el numero de cuenta y su balance, pueden ser varias cuentas
                var pesosBalance = new List<FiatBalanceDTO>();
                foreach (var account in fiatAccounts)
                {
                    var pesos = new FiatBalanceDTO();
                    pesos.AccountNumber = account.AccountNumber;
                    pesos.Balance = account.PesosBalance;
                    pesos.Alias = account.Alias;
                    pesos.CBU = account.CBU;
                    pesosBalance.Add(pesos);
                }
                return Ok(pesosBalance);
            }
            if (currency == 2)
            {
                var usdBalance = new List<FiatBalanceDTO>();
                foreach (var account in fiatAccounts)
                {
                    var usd = new FiatBalanceDTO();
                    usd.AccountNumber = account.AccountNumber;
                    usd.Balance = account.USDBalance;
                    usd.Alias = account.Alias;
                    usd.CBU = account.CBU;
                    usdBalance.Add(usd);
                }
                return Ok(usdBalance);
            }
            return BadRequest("Invalid currency");
        }

        /// <summary>
        /// Depositar dinero en cuenta fiduciaria en pesos y usd
        /// </summary>
        /// <param name="fiatDepositDTO"></param>
        /// <returns></returns>
        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] FiatDepositDTO fiatDepositDTO)
        {
            var user = await _unitOfWork.UserRepository.GetUserFromClaims(User);
            var fiatAccount = await _unitOfWork.FiatAccountRepository.GetFiatAccountByAccountNumber(fiatDepositDTO.AccountNumber);
            if (fiatAccount == null)
            {
                return BadRequest("Invalid account number");
            }
            if (fiatDepositDTO.Amount <= 0)
            {
                return BadRequest("Invalid amount");
            }
            if (fiatDepositDTO.Currency == 1)
            {
                fiatAccount.PesosBalance += fiatDepositDTO.Amount;
                await _unitOfWork.MovementRepository.SaveTransaction(user.Id, fiatDepositDTO.Amount, "Pesos", "Deposito");
            }
            if (fiatDepositDTO.Currency == 2)
            {
                fiatAccount.USDBalance += fiatDepositDTO.Amount;
                await _unitOfWork.MovementRepository.SaveTransaction(user.Id, fiatDepositDTO.Amount, "USD", "Deposito");
            }
            await _unitOfWork.Complete();
            return Ok();
        }

        /// <summary>
        /// Retirar dinero de cuenta fiduciaria en pesos y usd
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(int accountNumber, decimal amount, int currency)
        {
            var user = await _unitOfWork.UserRepository.GetUserFromClaims(User);
            var fiatAccount = await _unitOfWork.FiatAccountRepository.GetFiatAccountByAccountNumber(accountNumber);
            if (fiatAccount == null)
            {
                return BadRequest("Invalid account number");
            }
            if (amount <= 0)
            {
                return BadRequest("Invalid amount");
            }
            if (currency == 1)
            {
                fiatAccount.PesosBalance -= amount;
                await _unitOfWork.MovementRepository.SaveTransaction(user.Id, amount, "Pesos", "Retiro");
            }
            if (currency == 2)
            {
                fiatAccount.USDBalance -= amount;
                await _unitOfWork.MovementRepository.SaveTransaction(user.Id, amount, "USD", "Retiro");
            }
            await _unitOfWork.Complete();
            return Ok();
        }
    }
}
