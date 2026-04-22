using AutoMapper;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Quiz;
using ExaminationSystem.Features.Common.Quiz.Queries;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.UpdateQuiz.Commands.HandlerCommands
{
    public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;       

        public UpdateQuizCommandHandler(IUnitOfWork uow, IMapper mapper, IMediator mediator)
        {
            _uow = uow;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<RequestResult<bool>> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
        {
            // Get Quiz
            var quizResult = await _mediator.Send(new GetQuizByIdQuery(request.Id), cancellationToken);
            if (!quizResult.IsSuccess)
                return RequestResult<bool>.Failure(quizResult.ErrorCode, quizResult.Message);

            if (quizResult.Data.Title != request.Title&& !string.IsNullOrEmpty(request.Title))
            {
                var titleExists = await _mediator.Send(new IsQuizTitleExistQuery(request.Title), cancellationToken);
                if (titleExists.IsSuccess)
                {
                    return RequestResult<bool>.Failure(titleExists.ErrorCode, titleExists.Message);
                }
            }
            var quiz = _mapper.Map<Quiz>(request);

            _uow.Quizzes.Update(quiz);

            return await _uow.SaveChangesAsync(cancellationToken) > 0
                ? RequestResult<bool>.Success(true, "Quiz updated successfully.")
                : RequestResult<bool>.Failure(ErrorCode.DatabaseError, "Failed to update quiz.");
        }
    }
}
