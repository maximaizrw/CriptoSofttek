using CriptoSofttek.DTOs;
using CriptoSofttek.DTOs.Balances;
using CriptoSofttek.Entities;
using CriptoSofttek.Helpers;
using CriptoSofttek.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

            var id = await _unitOfWork.UserRepository.GetIdByEmail(dto.Email);

            var fiatAccount = new FiatAccount(dto, id);
            var cryptoAccount = new CryptoAccount(dto, id);
            await _unitOfWork.FiatAccountRepository.Add(fiatAccount);
            await _unitOfWork.CryptoAccountRepository.Add(cryptoAccount);

            await _unitOfWork.Complete();

            var fiatAccountId = fiatAccount.Id;
            var cryptoAccountId = cryptoAccount.Id;

            user.FiatAccountId = fiatAccountId;
            user.CryptoAccountId = cryptoAccountId;

            await _unitOfWork.Complete();

            return Ok();
        }

        /// <summary>
        /// Obtener usuario actual
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
            return Ok(user);
        }

        /// <summary>
        /// Consultar saldo de la cuenta actual seleccionando el tipo de moneda
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAccountBalance")]
        public async Task<IActionResult> GetAccountBalance(int currency)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
            var fiatAccount = await _unitOfWork.FiatAccountRepository.GetFiatAccountByUserId(user.Id);
            if (currency == 1)
            {

                var dto = new PesosBalanceDTO();
                
                dto.AccountNumber = fiatAccount.AccountNumber;
                dto.Alias = fiatAccount.Alias;
                dto.CBU = fiatAccount.CBU;
                dto.PesosBalance = fiatAccount.PesosBalance;
                return Ok(dto);
            }
            else if (currency == 2)
            {
                var dto = new USDBalanceDTO();

                dto.AccountNumber = fiatAccount.AccountNumber;
                dto.Alias = fiatAccount.Alias;
                dto.CBU = fiatAccount.CBU;
                dto.USDBalance = fiatAccount.USDBalance;
                return Ok(dto);
                
            }
            else if (currency == 3)
            {
                var cryptoAccount = await _unitOfWork.CryptoAccountRepository.GetCryptoAccountByUserId(user.Id);
                var dto = new CryptoBalanceDTO();
                dto.UUID = cryptoAccount.UUID;
                dto.Balance = cryptoAccount.Balance;
                return Ok(dto);
            }
            else
            {
                return BadRequest("Moneda no valida");
            }
        }


        
        /// <summary>
        /// Transferir dinero a cuentas de terceros
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Transfer")]
        public async Task<IActionResult> Transfer(TransferDTO dto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
            var fiatAccount = await _unitOfWork.FiatAccountRepository.GetFiatAccountByUserId(user.Id);
            var cryptoAccount = await _unitOfWork.CryptoAccountRepository.GetCryptoAccountByUserId(user.Id);
            if (dto.Currency == 1)
            {
                if (fiatAccount.CBU == dto.CBUOrUUID) return BadRequest("No se puede transferir a la misma cuenta");
                if (fiatAccount.PesosBalance < dto.Amount) return BadRequest("No tiene saldo suficiente");
                fiatAccount.PesosBalance -= dto.Amount;
                await _unitOfWork.Complete();
                var accountTo = await _unitOfWork.FiatAccountRepository.GetFiatAccountByCBU(dto.CBUOrUUID);
                accountTo.PesosBalance += dto.Amount;
                //Agregar nuevo movimiento, llenar MovementDTO
                var movementDTO = new MovementDTO();
                movementDTO.UserId = user.Id;
                movementDTO.Currency = "Pesos";
                movementDTO.Amount = dto.Amount;
                movementDTO.TypeMovement = "Transferencia";
                var movement = new Movement(movementDTO);
                await _unitOfWork.MovementRepository.Add(movement);
               
                await _unitOfWork.Complete();
                return Ok();
            }
            else if (dto.Currency == 2)
            {
                if (fiatAccount.CBU == dto.CBUOrUUID) return BadRequest("No se puede transferir a la misma cuenta");
                if (fiatAccount.USDBalance < dto.Amount) return BadRequest("No tiene saldo suficiente");
                fiatAccount.USDBalance -= dto.Amount;
                await _unitOfWork.Complete();
                var accountTo = await _unitOfWork.FiatAccountRepository.GetFiatAccountByCBU(dto.CBUOrUUID);
                accountTo.USDBalance += dto.Amount;

                var movementDTO = new MovementDTO();
                movementDTO.UserId = user.Id;
                movementDTO.Currency = "USD";
                movementDTO.Amount = dto.Amount;
                movementDTO.TypeMovement = "Transferencia";
                var movement = new Movement(movementDTO);
                await _unitOfWork.MovementRepository.Add(movement);

                await _unitOfWork.Complete();
                return Ok();
            }
            else if (dto.Currency == 3)
            {
                if (cryptoAccount.UUID == dto.CBUOrUUID) return BadRequest("No se puede transferir a la misma cuenta");
                if (cryptoAccount.Balance < dto.Amount) return BadRequest("No tiene saldo suficiente");
                cryptoAccount.Balance -= dto.Amount;
                await _unitOfWork.Complete();
                var accountTo = await _unitOfWork.CryptoAccountRepository.GetCryptoAccountByUUID(dto.CBUOrUUID);
                accountTo.Balance += dto.Amount;

                var movementDTO = new MovementDTO();
                movementDTO.UserId = user.Id;
                movementDTO.Currency = "BTC";
                movementDTO.Amount = dto.Amount;
                movementDTO.TypeMovement = "Transferencia";
                var movement = new Movement(movementDTO);
                await _unitOfWork.MovementRepository.Add(movement);

                await _unitOfWork.Complete();
                return Ok();
            }
            else
            {
                return BadRequest("Moneda no valida");
            }
        }

        /// <summary>
        /// Depositar en cuenta propia
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Deposit")]
        public async Task<IActionResult> Deposit(DepositDTO dto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
            var fiatAccount = await _unitOfWork.FiatAccountRepository.GetFiatAccountByUserId(user.Id);
            var cryptoAccount = await _unitOfWork.CryptoAccountRepository.GetCryptoAccountByUserId(user.Id);
            if (dto.Currency == 1)
            {
                fiatAccount.PesosBalance += dto.Amount;
                //Agregar nuevo movimiento, llenar MovementDTO
                var movementDTO = new MovementDTO();
                movementDTO.UserId = user.Id;
                movementDTO.Currency = "Pesos";
                movementDTO.Amount = dto.Amount;
                movementDTO.TypeMovement = "Deposito";
                var movement = new Movement(movementDTO);
                await _unitOfWork.MovementRepository.Add(movement);
                await _unitOfWork.Complete();
                return Ok();
            }
            else if (dto.Currency == 2)
            {
                fiatAccount.USDBalance += dto.Amount;
                var movementDTO = new MovementDTO();
                movementDTO.UserId = user.Id;
                movementDTO.Currency = "USD";
                movementDTO.Amount = dto.Amount;
                movementDTO.TypeMovement = "Deposito";
                var movement = new Movement(movementDTO);
                await _unitOfWork.MovementRepository.Add(movement);
                await _unitOfWork.Complete();
                return Ok();
            }
            else if (dto.Currency == 3)
            {
                cryptoAccount.Balance += dto.Amount;
                var movementDTO = new MovementDTO();
                movementDTO.UserId = user.Id;
                movementDTO.Currency = "BTC";
                movementDTO.Amount = dto.Amount;
                movementDTO.TypeMovement = "Deposito";
                var movement = new Movement(movementDTO);
                await _unitOfWork.MovementRepository.Add(movement);
                await _unitOfWork.Complete();
                return Ok();
            }
            else
            {
                return BadRequest("Moneda no valida");
            }
        }

        /// <summary>
        /// Retirar dinero de cuenta propia
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Withdraw")]
        public async Task<IActionResult> Withdraw(WithdrawDTO dto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
            var fiatAccount = await _unitOfWork.FiatAccountRepository.GetFiatAccountByUserId(user.Id);
            var cryptoAccount = await _unitOfWork.CryptoAccountRepository.GetCryptoAccountByUserId(user.Id);
            if (dto.Currency == 1)
            {
                if (fiatAccount.PesosBalance < dto.Amount) return BadRequest("No tiene saldo suficiente");
                fiatAccount.PesosBalance -= dto.Amount;

                var movementDTO = new MovementDTO();
                movementDTO.UserId = user.Id;
                movementDTO.Currency = "Pesos";
                movementDTO.Amount = dto.Amount;
                movementDTO.TypeMovement = "Retiro";
                var movement = new Movement(movementDTO);
                await _unitOfWork.MovementRepository.Add(movement);

                await _unitOfWork.Complete();
                return Ok();
            }
            else if (dto.Currency == 2)
            {
                if (fiatAccount.USDBalance < dto.Amount) return BadRequest("No tiene saldo suficiente");
                fiatAccount.USDBalance -= dto.Amount;

                var movementDTO = new MovementDTO();
                movementDTO.UserId = user.Id;
                movementDTO.Currency = "USD";
                movementDTO.Amount = dto.Amount;
                movementDTO.TypeMovement = "Retiro";
                var movement = new Movement(movementDTO);
                await _unitOfWork.MovementRepository.Add(movement);

                await _unitOfWork.Complete();
                return Ok();
            }
            else if (dto.Currency == 3)
            {
                if (cryptoAccount.Balance < dto.Amount) return BadRequest("No tiene saldo suficiente");
                cryptoAccount.Balance -= dto.Amount;

                var movementDTO = new MovementDTO();
                movementDTO.UserId = user.Id;
                movementDTO.Currency = "BTC";
                movementDTO.Amount = dto.Amount;
                movementDTO.TypeMovement = "Retiro";
                var movement = new Movement(movementDTO);
                await _unitOfWork.MovementRepository.Add(movement);

                await _unitOfWork.Complete();
                return Ok();
            }
            else
            {
                return BadRequest("Moneda no valida");
            }
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
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
            var fiatAccount = await _unitOfWork.FiatAccountRepository.GetFiatAccountByUserId(user.Id);

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

            await _unitOfWork.Complete();
            return Ok("Compra de BTC exitosa.");

        }



    }
}
