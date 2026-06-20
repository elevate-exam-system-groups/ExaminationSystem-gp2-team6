using AutoMapper;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.AttemptAnswer;
using MediatR;
using Microsoft.EntityFrameworkCore; // محتاجينها عشان الـ FirstOrDefaultAsync

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Commands.HandlerCommand
{
    public class AnswerQuestionCommandHandler : IRequestHandler<AnswerQuestionCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AnswerQuestionCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<RequestResult<bool>> Handle(AnswerQuestionCommand request, CancellationToken cancellationToken)
        {
            var exsistingAnswer = await _uow.AttemptAnswers
                .GetAll(withNoTracking: false)
                .FirstOrDefaultAsync(ans => ans.AttemptId == request.AttemptId && ans.QuestionId == request.QuestionId, cancellationToken);

            if (exsistingAnswer == null)
            {
                await _uow.AttemptAnswers.AddAsync(_mapper.Map<AttemptAnswer>(request));
            }
            else
            {
                exsistingAnswer.SelectedOptionId = request.OptionId;
                exsistingAnswer.AnsweredAt = DateTime.UtcNow;

                _uow.AttemptAnswers.Update(exsistingAnswer);
            }

            return await _uow.SaveChangesAsync(cancellationToken) > 0
                ? RequestResult<bool>.Success(true, "Answer saved successfully.")
                : RequestResult<bool>.Failure(ErrorCode.DatabaseError, "Failed to save the answer.");
        }
    }
}