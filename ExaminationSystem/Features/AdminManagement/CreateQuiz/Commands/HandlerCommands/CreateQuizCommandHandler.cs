using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.CreateQuiz.Commands.HandlerCommands
{
    public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, RequestResult<bool>>
    {
        public async Task<RequestResult<bool>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {

            throw new NotImplementedException();
            return RequestResult<bool>.Success(true);
        }
    }
}
