using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ShoppingList4.Api.Domain.Entities;
using ShoppingList4.Api.Domain.Interfaces;

namespace ShoppingList4.Api.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<RegisterUserCommandHandler> logger,
        IPasswordHasher<User> passwordHasher) : IRequestHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<RegisterUserCommandHandler> _logger = logger;
        private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _userRepository.Add(user);

            _logger.LogInformation("User {@User} has been created.", user);
        }
    }
}
