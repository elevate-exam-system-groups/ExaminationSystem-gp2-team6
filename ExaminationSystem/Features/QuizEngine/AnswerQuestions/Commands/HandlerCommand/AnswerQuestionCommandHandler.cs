using AutoMapper;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.AttemptAnswer;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var exsistingAnswer = await _uow.AttemptAnswers.GetById(request.AttemptId).Where(ans => ans.QuestionId == request.QuestionId).AsTracking().FirstOrDefaultAsync();

            if (exsistingAnswer == null)
            {
                _uow.AttemptAnswers.Add(_mapper.Map<AttemptAnswer>(request));
            }
            else { 
                exsistingAnswer.SelectedOptionId = request.OptionId;
                exsistingAnswer.AnsweredAt = DateTime.UtcNow;
                _uow.AttemptAnswers.Update(exsistingAnswer);
            }
            return await _uow.SaveChangesAsync() > 0
                ? RequestResult<bool>.Success(true, "Answer saved successfully.")
                : RequestResult<bool>.Failure(ErrorCode.DatabaseError, "Failed to save the answer.");
        }
    }
}
