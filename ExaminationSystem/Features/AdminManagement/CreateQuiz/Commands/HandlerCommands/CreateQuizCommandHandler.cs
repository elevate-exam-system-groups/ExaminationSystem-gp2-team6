using AutoMapper;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Quiz;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.CreateQuiz.Commands.HandlerCommands
{
    public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateQuizCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<RequestResult<bool>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = _mapper.Map<Quiz>(request);

            _uow.Quizzes.Add(quiz);
            return await _uow.SaveChangesAsync(cancellationToken) >0
                ? RequestResult<bool>.Success(true, "Quiz created successfully.")
                : RequestResult<bool>.Failure(ErrorCode.DatabaseError, "Failed to create quiz.");

        }
    }
}
