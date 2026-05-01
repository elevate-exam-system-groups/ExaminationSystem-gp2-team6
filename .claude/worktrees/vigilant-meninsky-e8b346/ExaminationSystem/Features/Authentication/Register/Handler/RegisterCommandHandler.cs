using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Contracts.Hasher;
using ExaminationSystem.Contracts.Token;
using ExaminationSystem.Domain.Entities.Shared.Enums.User;
using ExaminationSystem.Domain.Entities.User;
using ExaminationSystem.Domain.Entities.User.UserRefreshToken;
using ExaminationSystem.Features.Authentication.Register.Command;
using ExaminationSystem.Features.Authentication.Register.Dto;
using ExaminationSystem.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Authentication.Register.Handler;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RequestResult<RegisterDto>>
{
    private UserManager<ApplicationUser> _userManager;
    private readonly IGenerateTokenService _generateTokenService;
    private readonly IGenerateRefreshTokenService _refreshTokenService;
    private readonly IHasherService _hasher;
    private readonly AppDbContext _context;
    
    public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IGenerateTokenService generateTokenService, IGenerateRefreshTokenService refreshTokenService, IHasherService hasher, AppDbContext context)
    {
        _userManager = userManager;
        _generateTokenService = generateTokenService;
        _refreshTokenService = refreshTokenService;
        _hasher = hasher;
        _context = context;
    }
    
    public async Task<RequestResult<RegisterDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var userIsExists = await _userManager.FindByNameAsync(request.Email);

        if (userIsExists != null) return RequestResult<RegisterDto>.Failure(ErrorCode.UserAlreadyExists, "User already exists");
        
        var user = new ApplicationUser()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            UserType = UserType.Student,
            FullName = request.FirstName + " " + request.LastName,
            UserName = request.Email,
            EmailConfirmed = true
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);
        
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return RequestResult<RegisterDto>.Failure(ErrorCode.ValidationError, errors);
        }
        
        #region Token - Refresh Token

        var accessToken = await _generateTokenService.GenerateTokenAsync(user);

        var refreshToken = _refreshTokenService.GenerateRefreshToken();
        var hashedToken = _hasher.Hash(refreshToken);

        var refreshTokenEntity = new UserRefreshToken
        {
            Token = hashedToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        _context.UserRefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync(cancellationToken);

        #endregion
        
        await _userManager.AddToRoleAsync(user, "Student");
        
        var data = new RegisterDto()
        {
            Email = request.Email,
            FullName = request.FirstName + " " + request.LastName,
            UserType = user.UserType,
            AccessToken = accessToken,
            RefreshToken =  refreshToken,
        };

        return RequestResult<RegisterDto>.Success(data, "Success");
    }
}