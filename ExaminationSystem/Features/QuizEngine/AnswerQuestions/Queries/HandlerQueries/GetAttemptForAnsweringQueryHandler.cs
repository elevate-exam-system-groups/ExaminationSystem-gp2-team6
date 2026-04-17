using AutoMapper;
using AutoMapper.QueryableExtensions;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Features.QuizEngine.AnswerQuestions.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Queries.HandlerQueries
{
    public class GetAttemptForAnsweringQueryHandler : IRequestHandler<GetAttemptForAnsweringQuery, RequestResult<QuizAttemptDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAttemptForAnsweringQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<RequestResult<QuizAttemptDto>> Handle(GetAttemptForAnsweringQuery request, CancellationToken cancellationToken)
        {
            var attempt =await _unitOfWork.QuizAttempts.GetById(request.attemptId).ProjectTo<QuizAttemptDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            
            return (attempt == null)
                ? RequestResult<QuizAttemptDto>.Failure(ErrorCode.NotFound, "Attempt not found")
                : RequestResult<QuizAttemptDto>.Success(attempt);
        }
    }
}
