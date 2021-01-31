using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AIHolding_task.Data;
using AIHolding_task.DTOs;
using AIHolding_task.Models;
using AIHolding_task.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AIHolding_task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        private readonly IConfiguration _config;

        private readonly DataContext _context;

        public AuthController(DataContext context, IAuthRepository repo, IConfiguration config)
        {
            _context = context;
            _config = config;
            _repo = repo;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static async void SendMail(string email, string type, string name, EmailGeneratorDto ed)
        {
            string htmlString = string.Format("<html><table style=\"max-width: 90%; min-width: 320px; width: 550px; margin: 0 auto; height: auto;\"><tr><td colspan=\"3\" style=\"background-image: url('https://www.razlee.com/wp-content/uploads/2017/08/Password-Reset-1-768x768-1.png'); background-position: center; background-size: contain; background-repeat: no-repeat; background-color: #1890FF; height: 250px;\"></td></tr><tr><td colspan=\"3\" style=\"background-color: #F0F2F5; height: auto; text-align: center;\"><div style=\"padding: 20px; height: auto;\"><table style=\"width: 100%;\"><tr><td colspan=\"3\" style=\"text-align: center; width: 100%;\"><h1 style=\"margin: .5rem 0; word-break: keep-all;\">{1}</h2></td></tr><tr><td colspan=\"3\" style=\"text-align: center; width: 100%;\"><h3 style=\"margin: .5rem 0;\">{2}</h3></td></tr><tr><td colspan=\"3\" style=\"text-align: center; width: 100%;\"><a href=\"{0}\" style=\"text-decoration: none; border:none; cursor: pointer; border-radius: 5px; color: #1890ff; font-size: 20px; padding: 15px 10px;\">{3}</a></td></tr></table></div></td></tr><tr><td colspan=\"3\" style=\"background-color: #1890FF; height: 50px; text-align: center;\"><a href=\"https://www.facebook.com/3sualaz-107176074259586/\"><img style=\"cursor: pointer; width: 30px; height: 30px;\" src=\"https://3sual.site/images/icons8-facebook-48.png\"/></a><a href=\"https://www.instagram.com/3sual.az/\"><img style=\"cursor: pointer; width: 30px; height: 30px;\" src=\"https://api.3sual.az/images/icons8-instagram-48.png\"/></a></td></tr></table></html>", ed.Url, ed.Header, ed.Text, ed.Button);
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("info@3sual.az", "Bahruz Aydinli");
            var subject = "AIHoldingTask | " + type;
            var to = new EmailAddress(email, name);
            var htmlContent = htmlString;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            await client.SendEmailAsync(msg);
        }

        [HttpPost("forgot")]
        public async Task<IActionResult> Forgot([FromBody] string email)
        {
            User u = await _context.Users.FirstOrDefaultAsync(us => us.Email == email);
            if (u != null)
            {
                string code = RandomString(25);
                await _context.Confirmations.AddAsync(new Confirmation { Code = code, Expiration = DateTime.Now.AddDays(1), Type = 2, UserID = u.ID });
                await _context.SaveChangesAsync();
                EmailGeneratorDto ed = new EmailGeneratorDto { Button = "Şifrəmi yenilə", Text = "Şifrənizi yeniləmək üçün aşağıdakı keçidə klikləyin.", Url = String.Format("https://3sual.az/password/{0}/{1}", code, email), Header = "Şifrəmi unutmuşam" };
                SendMail(email, "Şifrənin yenilənməsi", u.Name + " " + u.Surname, ed);
                return Ok(new BasicResponse { Success = true, Message = "Şifrənizi yeniləmək üçün elektron poçtunuzu yoxlayın." });
            }
            else
            {
                return Ok(new BasicResponse { Success = false, Message = "Bu elektron poçt istifadə olunmayıb." });
            }
        }

        [HttpPut("forgot")]
        public async Task<IActionResult> ForgotChange(ForgotChangeDto f)
        {
            var identity = _repo.ValidateToken(f.Token);
            int id;
            if (identity != null)
            {
                id = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                return BadRequest();
            }

            try
            {
                User u = await _context.Users.FirstOrDefaultAsync(us => us.ID == id);
                if (u != null)
                {
                    Confirmation c = await _context.Confirmations.FirstOrDefaultAsync(co => co.UserID == u.ID && co.Code == f.Code);
                    if (c != null)
                    {
                        if (DateTime.Compare(c.Expiration, DateTime.Now) > 0)
                        {
                            u.Password = _repo.HashPassword(f.Password);
                            _context.Entry(u).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                            return Ok(new BasicResponse { Success = true, Message = "Şifrədə dəyişiklik uğurludur!" });
                        }
                        else
                        {
                            return Ok(new BasicResponse { Success = false, Message = "Qeyd edilən təsdiq kodunun istifadə müddəti bitmişdir." });
                        }
                    }
                    else
                    {
                        return Ok(new BasicResponse { Success = false, Message = "Bu təsdiq kodu qeyd edilən elektron poçta aid deyil" });
                    }
                }
                else
                {
                    return Ok(new BasicResponse { Success = false, Message = "Bu elektron poçt bazada mövcud deyil" });
                }
            }
            catch
            {
                BasicResponse le = new BasicResponse { Success = false, Message = "Xəta baş verdi. Daha sonra yenə cəhd edin" };
                return Ok(le);
            }
        }

        [HttpPut("confirm")]
        public async Task<IActionResult> ConfirmAccount([FromBody] ConfirmDto cdto)
        {
            var identity = _repo.ValidateToken(cdto.Token);
            int id;
            if (identity != null)
            {
                id = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                return BadRequest();
            }

            try
            {

                User u = await _context.Users.FirstOrDefaultAsync(us => us.ID == id);
                if (u != null)
                {
                    Confirmation c = await _context.Confirmations.FirstOrDefaultAsync(co => co.UserID == u.ID && co.Code == cdto.Code);
                    if (c != null)
                    {
                        if (c.Expiration > DateTime.Now)
                        {
                            u.Confirmed = true;
                            _context.Entry(u).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                            return Ok(new BasicResponse { Message = "Hesab uğurla təsdiqləndi." });
                        }
                        else
                        {
                            return Ok(new BasicResponse { Message = "Qeyd edilən təsdiq kodunun istifadə müddəti bitmişdir." });
                        }
                    }
                    else
                    {
                        return Ok(new BasicResponse { Message = "Bu təsdiq kodu qeyd edilən elektron poçta aid deyil" });
                    }
                }
                else
                {
                    return Ok(new BasicResponse { Message = "Bu elektron poçt bazada mövcud deyil" });
                }
            }
            catch
            {
                BasicResponse le = new BasicResponse { Message = "Xəta baş verdi. Daha sonra yenə cəhd edin" };
                return Ok(le);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            register.Username = register.Username.ToLower();

            List<string> exist = await _repo.UserExists(register.Username, register.Email, register.Phone);
            if(exist.Count > 0)
            {
                return BadRequest(String.Join(", ", exist.ToArray()) + " already exists");
            }

            User new_user = new User()
            {
                Username = register.Username,
                Phone = register.Phone,
                Email = register.Email,
                Name = register.Name,
                Surname = register.Surname,
                Registered = DateTime.Now,
                Confirmed = false,
                RoleID = 1,
                Password = _repo.HashPassword(register.Password)
            };

            await _context.Users.AddAsync(new_user);
            await _context.SaveChangesAsync();

            if (register.isPremium)
            {
                Subscription s = new Subscription { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7), User = new_user, RenewalType = register.RenewalType };
                switch (register.RenewalType)
                {
                    case 1:
                        s.EndDate = DateTime.Now.AddDays(7);
                        break;
                    case 2:
                        s.EndDate = DateTime.Now.AddMonths(1);
                        break;
                    case 3:
                        s.EndDate = DateTime.Now.AddMonths(3);
                        break;
                    case 4:
                        s.EndDate = DateTime.Now.AddYears(1);
                        break;
                    default:
                        break;
                }
                await _context.Subscriptions.AddAsync(s);
                await _context.SaveChangesAsync();
            }

            var code = RandomString(25);
            await _context.Confirmations.AddAsync(new Confirmation { Code = code, Expiration = DateTime.Now.AddDays(1), Type = 1, User = new_user });
            var url = String.Format("https://testurlforpasswordreset.com/confirm/{0}/{1}", code, new_user.Email);
            SendMail(new_user.Email, "Qeydiyyat", new_user.Name, new EmailGeneratorDto { Button = "Hesabımı aktivləşdir", Header = "Qeydiyyatınız uğurludur!", Text = "Hesabınızı aktivləşdirmək üçün aşağıdakı keçidə klikləyin.", Url = url });

            return Ok("Successfully created");
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDto login)
        {
            var userFormRepo = await _repo.Login(login.Value.ToLower(), login.Password);

            if (userFormRepo == null) return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFormRepo.ID.ToString()),
                new Claim(ClaimTypes.Name, userFormRepo.Username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { token = tokenHandler.WriteToken(token) });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetUser()
        {
            int id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var logineduser = await _repo.GetUserById(id);

            if (logineduser == null)
            {
                return NotFound();
            }

            return Ok(logineduser);
        }
    }
}