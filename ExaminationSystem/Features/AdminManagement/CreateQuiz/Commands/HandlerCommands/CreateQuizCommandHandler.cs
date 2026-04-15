using AutoMapper;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Entities.Quiz;
using ExaminationSystem.Features.Common.Quiz.Queries;
using ExaminationSystem.Infrastructure.Persistence.DB.Context;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.CreateQuiz.Commands.HandlerCommands
{
    public class CreateQuizCommandHandler : IRequestHandler<CreateQuizCommand, RequestResult<CreateQuizCommand>>
    {
        private readonly AppDbContext _db;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public CreateQuizCommandHandler(AppDbContext db, IMediator mediator, IMapper mapper)
        {
            _db = db;
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task<RequestResult<CreateQuizCommand>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
        {
            //check if quiz with the same title already exists
            var quizExists = await _mediator.Send(new IsQuizTitleExistQuery(request.Title), cancellationToken);
            if (quizExists.IsSuccess)
                return RequestResult<CreateQuizCommand>.Failure(ExaminationSystem.Common.Data.ErrorCode.AlreadyExists);

            //create quiz
             _db.Quizzes.Add(_mapper.Map<Quiz>(request));

            return RequestResult<CreateQuizCommand>.Success(request);
        }
    }
}
