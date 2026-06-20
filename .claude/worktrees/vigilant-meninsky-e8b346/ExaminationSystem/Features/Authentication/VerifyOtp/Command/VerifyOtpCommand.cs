using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.Authentication.VerifyOtp.Dto;
using MediatR;

namespace ExaminationSystem.Features.Authentication.VerifyOtp.Command;

public record VerifyOtpCommand(Guid UserId, string VerifyCode) : IRequest<RequestResult<VerifyOtpDto>>;