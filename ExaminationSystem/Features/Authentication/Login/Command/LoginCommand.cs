using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.Authentication.Login.Dto;
using MediatR;

namespace ExaminationSystem.Features.Authentication.Login.Command;

public record LoginCommand(string Email, string Password) : IRequest<RequestResult<LoginDto>>;