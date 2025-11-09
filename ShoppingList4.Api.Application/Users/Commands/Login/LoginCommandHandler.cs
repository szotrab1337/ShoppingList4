using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ShoppingList4.Api.Application.Interfaces;
using ShoppingList4.Api.Application.Users.Dtos;
using ShoppingList4.Api.Domain.Entities;
using ShoppingList4.Api.Domain.Exceptions;
using ShoppingList4.Api.Domain.Interfaces;

namespace ShoppingList4.Api.Application.Users.Commands.Login
{
    public class LoginCommandHandler(
        IUserRepository userRepository,
        ILogger<LoginCommandHandler> logger,
        IPasswordHasher<User> passwordHasher,
        IJwtTokenService jwtTokenService) : IRequestHandler<LoginCommand, UserDto>
    {
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
        private readonly ILogger<LoginCommandHandler> _logger = logger;
        private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<UserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Get(request.Email)
                       ?? throw new NotFoundException(nameof(User), request.Email);

            var passwordCheck = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (passwordCheck == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Invalid login attempt for user {User}.", request.Email);
                throw new BadHttpRequestException("Invalid username or password");
            }

            var token = _jwtTokenService.GenerateToken(user);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                ApiToken = token
            };
        }
    }
}