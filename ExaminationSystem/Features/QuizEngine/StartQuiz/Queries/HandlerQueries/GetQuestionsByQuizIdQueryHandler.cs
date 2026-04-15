using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.QuizEngine.StartQuiz.Dtos;
using ExaminationSystem.Infrastructure.Persistence.DB.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Queries.HandlerQueries
{
    public class GetQuestionsByQuizIdQueryHandler : IRequestHandler<GetQuestionsByQuizIdQuery, RequestResult<IEnumerable<QuestionDto>>>
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        public GetQuestionsByQuizIdQueryHandler(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<RequestResult<IEnumerable<QuestionDto>>> Handle(GetQuestionsByQuizIdQuery request, CancellationToken cancellationToken)
        {
            var query = _db.Questions.Where(eq => eq.QuizId == request.QuizId);
            var questions = await query.ProjectTo<QuestionDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            return questions.Any() 
                ? RequestResult<IEnumerable<QuestionDto>>.Success(questions) 
                : RequestResult<IEnumerable<QuestionDto>>.Failure(ErrorCode.NotFound, "No questions found for the specified quiz.");
        }
    }
}   