using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Contracts.Hasher;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.User;
using ExaminationSystem.Domain.Entities.User.UserOtp;
using ExaminationSystem.Features.Authentication.SendOtp.Command;
using ExaminationSystem.Features.Authentication.SendOtp.Dto;
using ExaminationSystem.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Authentication.SendOtp.Handler;

public class SendOtpCommandHandler : IRequestHandler<SendOtpCommand, RequestResult<SendOtpDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IGenerateOtpService _otpService;
    private readonly IHasherService _hasher;
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;

    public SendOtpCommandHandler(UserManager<ApplicationUser> userManager, IGenerateOtpService otpService, IHasherService hasher, AppDbContext context, IEmailService emailService)
    {
        _userManager = userManager;
        _otpService = otpService;
        _hasher = hasher;
        _context = context;
        _emailService = emailService;
    }

    public async Task<RequestResult<SendOtpDto>> Handle(SendOtpCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null) return RequestResult<SendOtpDto>.Failure(ErrorCode.UserNotFound, "User not found");

        var recentOtp = await _context.UserOtps
            .Where(x => x.UserId == user.Id)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (recentOtp != null && !recentOtp.IsUsed && recentOtp.CreatedAt > DateTime.UtcNow.AddMinutes(-1))
            return RequestResult<SendOtpDto>.Failure(ErrorCode.TooManyRequests, "Try again after 1 minute");
        
        // generate otp
        var otp = _otpService.GenerateOtp();

        // hash otp
        var hashedOtp = _hasher.Hash(otp);

        // remove old OTPs
        var oldOtps = await _context.UserOtps
            .Where(x => x.UserId == user.Id && !x.IsUsed)
            .ToListAsync(cancellationToken);
        
        // Soft Delete
        foreach (var otpItem in oldOtps)
        {
            otpItem.IsUsed = true;
            otpItem.IsDeleted = true;
        }

        // save new OTP
        var otpEntity = new UserOtp
        {
            Code = hashedOtp,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
            IsUsed = false,
            Attempts = 0
        };

        _context.UserOtps.Add(otpEntity);

        await _context.SaveChangesAsync(cancellationToken);

        // send email
        await _emailService.SendAsync(
            user.Email!,
            "Your OTP Code",
            $"Your verification code is: {otp}"
        );

        var dto = new SendOtpDto()
        {
            Email = user.Email!,
            Message = "OTP sent successfully",
            OtpCode = otpEntity.Code,
        };
        
        return RequestResult<SendOtpDto>.Success(dto, "Success");
    }
}