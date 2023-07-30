using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ShoppingList4.Api.Entities;
using ShoppingList4.Api.Exceptions;
using ShoppingList4.Api.Interfaces;
using ShoppingList4.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoppingList4.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly ShoppingListDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(ShoppingListDbContext dbContext, IPasswordHasher<User> passwordHasher,
            AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User
            {
                Email = dto.Email,
                Name = dto.Name
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(x => x.Email == dto.Email);

            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadHttpRequestException("Invalid username or password");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, $"{user.Email}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
