using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ShoppingList4.Api.Application.Common;
using ShoppingList4.Api.Application.Users.Dtos;
using ShoppingList4.Api.Domain.Entities;
using ShoppingList4.Api.Domain.Exceptions;
using ShoppingList4.Api.Domain.Interfaces;

namespace ShoppingList4.Api.Application.Users.Commands.Login
{
    public class LoginCommandHandler(
        IUserRepository userRepository,
        JwtSettings jwtSettings,
        ILogger<LoginCommandHandler> logger,
        IPasswordHasher<User> passwordHasher) : IRequestHandler<LoginCommand, UserDto>
    {
        private readonly JwtSettings _jwtSettings = jwtSettings;
        private readonly ILogger<LoginCommandHandler> _logger = logger;
        private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<UserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Get(request.Email)
                       ?? throw new NotFoundException(nameof(User), request.Email);

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Invalid login attempt for user {User}.", request.Email);
                throw new BadHttpRequestException("Invalid username or password");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()), new(ClaimTypes.Name, $"{user.Email}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Today.AddDays(_jwtSettings.ExpireDays),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return new UserDto { Id = user.Id, Email = user.Email, Name = user.Name, ApiToken = jwt };
        }
    }
}