using AutoMapper;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Question;
using ExaminationSystem.Features.AdminManagement.ManageQuestions.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AdminManagement.ManageQuestions.Commands.HandlerCommands
{
    public class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, RequestResult<int>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AddQuestionCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<RequestResult<int>> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
        {
            
            var quizExists = await _uow.Quizzes.GetAll().AnyAsync(q => q.Id == request.QuizId, cancellationToken);
            if (!quizExists)
            {
                return RequestResult<int>.Failure(ErrorCode.NotFound, "Quiz not found.");
            }

            
            var currentMaxIndex = await _uow.Questions.GetAll()
                .Where(q => q.QuizId == request.QuizId)
                .MaxAsync(q => (int?)q.OrderIndex, cancellationToken) ?? 0;

            
            var question = _mapper.Map<Question>(request);
            question.OrderIndex = currentMaxIndex + 1;

            await _uow.Questions.AddAsync(question);

            return await _uow.SaveChangesAsync(cancellationToken) > 0
                ? RequestResult<int>.Success(question.Id, "Question added successfully.")
                : RequestResult<int>.Failure(ErrorCode.DatabaseError, "Failed to add question.");
        }
    }
}
