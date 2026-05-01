using AutoMapper;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Features.QuizEngine.AnswerQuestions.Dtos;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Queries.HandlerQueries
{
    public class GetAttemptForAnsweringQueryHandler : IRequestHandler<GetAttemptForAnsweringQuery, RequestResult<QuizAttemptDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAttemptForAnsweringQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RequestResult<QuizAttemptDto>> Handle(GetAttemptForAnsweringQuery request, CancellationToken cancellationToken)
        {
            var attemptEntity = await _unitOfWork.QuizAttempts.GetByIdAsync(request.attemptId);

            if (attemptEntity == null)
            {
                return RequestResult<QuizAttemptDto>.Failure(ErrorCode.NotFound, "Attempt not found");
            }

            var attemptDto = _mapper.Map<QuizAttemptDto>(attemptEntity);

            return RequestResult<QuizAttemptDto>.Success(attemptDto);
        }
    }
}