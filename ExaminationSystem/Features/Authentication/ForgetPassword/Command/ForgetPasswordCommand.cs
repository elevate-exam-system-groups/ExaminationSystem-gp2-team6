using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.Authentication.ForgetPassword.Dto;
using MediatR;

namespace ExaminationSystem.Features.Authentication.ForgetPassword.Command;

public record ForgetPasswordCommand (string Email): IRequest<RequestResult<ForgetPasswordDto>>;