using AutoMapper;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.QuizEngine.StartQuiz.CreateQuiz;
using ExaminationSystem.Features.QuizEngine.StartQuiz.Dtos;
using ExaminationSystem.Infrastructure.Persistence.Context;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Queries.HandlerQueries
{
    public class BuildStartQuizResponseQueryHandler : IRequestHandler<BuildStartQuizResponseQuery, RequestResult<IEnumerable<QuestionDto>>>
    {
        private readonly IMediator _mediator;

        public BuildStartQuizResponseQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResult<IEnumerable<QuestionDto>>> Handle(BuildStartQuizResponseQuery request, CancellationToken cancellationToken)
        {
            var quiz = await _mediator.Send(new IsQuizIdExistQuery(request.QuizId), cancellationToken);
            if (!quiz.IsSuccess)
                return RequestResult<IEnumerable<QuestionDto>>.Failure(quiz.ErrorCode, quiz.Message);

            var questions = await _mediator.Send(new GetQuestionsByQuizIdQuery(request.QuizId), cancellationToken);
            return questions.IsSuccess
                ? RequestResult<IEnumerable<QuestionDto>>.Success(questions.Data, questions.Message)
                : RequestResult<IEnumerable<QuestionDto>>.Failure(questions.ErrorCode, questions.Message);
        }
    }
}
