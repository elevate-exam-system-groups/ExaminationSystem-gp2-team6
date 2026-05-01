using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.Authentication.SendOtp.Dto;
using MediatR;

namespace ExaminationSystem.Features.Authentication.SendOtp.Command;

public record SendOtpCommand(string Email) : IRequest<RequestResult<SendOtpDto>>;