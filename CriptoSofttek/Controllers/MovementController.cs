﻿using CriptoSofttek.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CriptoSofttek.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MovementController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public MovementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Obtener ultimos movimientos de la cuenta
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _unitOfWork.UserRepository.GetUserByEmail(email);
            var movements = await _unitOfWork.MovementRepository.GetMovementsByUserId(user.Id);
            return Ok(movements);
        }



    }
}
