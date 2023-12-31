﻿using CriptoSofttek.DataAccess.Repositories.Interfaces;
using CriptoSofttek.DTOs;
using CriptoSofttek.Entities;
using CriptoSofttek.Helpers;
using CriptoSofttek.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace CriptoSofttek.DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> GetIdByEmail(string email)
        {
            return await _context.Users.Where(x => x.Email == email).Select(x => x.Id).FirstOrDefaultAsync();
        }

        // Obtener usuario por email
        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
        }
        
        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }
        public async Task<User?> AuthenticateCredentials(AuthenticateDTO dto)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Email == dto.Email && x.Password == PasswordEncryptHelper.EncryptPassword(dto.Password, dto.Email));
        }
        public async Task<User?> GetUserFromClaims(ClaimsPrincipal claims)
        {
            var email = claims.FindFirst(ClaimTypes.Email)?.Value;
            return await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
        }

    }
}
