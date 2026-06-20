using AutoMapper;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.AnswerOption;
using ExaminationSystem.Domain.Entities.Question;
using ExaminationSystem.Features.AdminManagement.ManageQuestions.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AdminManagement.ManageQuestions.Commands.HandlerCommands
{
    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateQuestionCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<RequestResult<bool>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _uow.Questions.GetByIdAsync(request.QuestionId, q => q.AnswerOptions);

            if (question == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Question not found.");
            }

            question.QuestionText = request.Text;
            question.Explanation = request.Explanation;

            var existingOptionIds = question.AnswerOptions.Select(o => o.Id).ToList();
            var incomingOptionIds = request.Options.Where(o => o.Id.HasValue).Select(o => o.Id!.Value).ToList();

            var optionsToRemove = question.AnswerOptions.Where(o => !incomingOptionIds.Contains(o.Id)).ToList();
            foreach (var optionToRemove in optionsToRemove)
            {
                _uow.AnswerOptions.SoftDelete(optionToRemove);
            }

            
            foreach (var optionDto in request.Options)
            {
                if (optionDto.Id.HasValue && existingOptionIds.Contains(optionDto.Id.Value))
                {
                    
                    var existingOption = question.AnswerOptions.First(o => o.Id == optionDto.Id.Value);
                    existingOption.Text = optionDto.Text;
                    existingOption.IsCorrect = optionDto.IsCorrect;
                }
                else
                {
                    
                    var newOption = new AnswerOption
                    {
                        Text = optionDto.Text,
                        IsCorrect = optionDto.IsCorrect,
                        QuestionId = question.Id
                    };
                    question.AnswerOptions.Add(newOption);
                }
            }

            return await _uow.SaveChangesAsync(cancellationToken) > 0
                ? RequestResult<bool>.Success(true, "Question updated successfully.")
                : RequestResult<bool>.Failure(ErrorCode.DatabaseError, "Failed to update question.");
        }
    }
}
