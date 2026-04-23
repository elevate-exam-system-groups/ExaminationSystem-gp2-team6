using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Common.Diploma.Queries.HandlerQueries
{
    public class IsDiplomaIdExistQueryHandler : IRequestHandler<IsDiplomaIdExistQuery, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;

        public IsDiplomaIdExistQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<RequestResult<bool>> Handle(IsDiplomaIdExistQuery request, CancellationToken cancellationToken)
        {
            var diplomaExists = await _uow.Diplomas.GetById(request.DiplomaId).FirstOrDefaultAsync(cancellationToken);
            if (diplomaExists != null)
                return RequestResult<bool>.Success(true);
                
            return RequestResult<bool>.Failure(ErrorCode.NotFound, "Diploma not found");
        }
    }
}
