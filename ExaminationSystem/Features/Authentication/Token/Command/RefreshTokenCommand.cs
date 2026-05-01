using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.Authentication.Token.Dto;
using MediatR;

namespace ExaminationSystem.Features.Authentication.Token.Command;

public record RefreshTokenCommand (string RefreshToken): IRequest<RequestResult<TokensDto>>;