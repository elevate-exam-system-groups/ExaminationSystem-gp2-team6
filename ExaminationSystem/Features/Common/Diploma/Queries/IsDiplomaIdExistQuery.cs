using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.Common.Diploma.Queries
{
    public record IsDiplomaIdExistQuery(int DiplomaId) : IRequest<RequestResult<bool>>;
}
