using AIHolding_task.DTOs.Read;
using AIHolding_task.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AIHolding_task.Repos
{
    public interface IAuthRepository
    {
        Task<User> Login(string value, string password);
        Task<List<string>> UserExists(string username, string email, string phone);
        Task<UserReadDto> GetUserById(int id);
        string HashPassword(string pass);
        ClaimsPrincipal ValidateToken(string JwtToken);
    }
}
