using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.Authentication.ResetPassword.Dto;
using MediatR;

namespace ExaminationSystem.Features.Authentication.ResetPassword.Command;

public record ResetPasswordCommand (Guid UserId, string Token, string Password, string ConfirmedPassword): IRequest<RequestResult<ResetPasswordDto>>;