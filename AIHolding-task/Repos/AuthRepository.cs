using AIHolding_task.Data;
using AIHolding_task.DTOs.Read;
using AIHolding_task.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AIHolding_task.Repos
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        public AuthRepository(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<User> Login(string value, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == value || x.Phone == value || x.Email == value);
            if (user == null) return null;
            if (user.Password == HashPassword(password)) return user;
            return null;
        }

        public bool isNullOrEmpty(string value)
        {
            if(value != null && value != "")
            {
                return false;
            }
            return true;
        }

        public async Task<List<string>> UserExists(string username, string email, string phone)
        {
            List<string> resp = new List<string>();
            if(!isNullOrEmpty(email) && await _context.Users.AnyAsync(x=>x.Email == email))
            {
                resp.Add("email");
            }
            if (!isNullOrEmpty(email) && await _context.Users.AnyAsync(x => x.Phone == phone))
            {
                resp.Add("phone");
            }
            if (!isNullOrEmpty(email) && await _context.Users.AnyAsync(x => x.Username == username))
            {
                resp.Add("username");
            }
            return resp;
        }

        public ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }

        public async Task<UserReadDto> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(s => s.ID == id);

            UserReadDto userDto = new UserReadDto()
            {
                Email = user.Email,
                Phone = user.Phone,
                Username = user.Username,
                ID = id,
                Fullname = user.Name + " " + user.Surname
            };
            return userDto;
        }

        public string HashPassword(string pass)
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: pass,
                salt: Encoding.ASCII.GetBytes("4xgAaBS@:sASFb14J#BSAADq13wVDS-bVSvFQKGSKMAiasDSGbgfBGdafAFSAs"),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
