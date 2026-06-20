using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.Authentication.Register.Dto;
using MediatR;

namespace ExaminationSystem.Features.Authentication.Register.Command;

public record RegisterCommand(string FirstName, string LastName, string Email, string PhoneNumber, string Password, string ConfirmedPassword) : IRequest<RequestResult<RegisterDto>>;