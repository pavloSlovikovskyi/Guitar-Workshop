using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.InstrumentPassports;
using Domain.Instruments;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstrumentPassports.Commands
{
    public class CreateInstrumentPassportCommandHandler : IRequestHandler<CreateInstrumentPassportCommand, Result<InstrumentPassportId>>
    {
        private readonly IInstrumentPassportRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public CreateInstrumentPassportCommandHandler(
            IInstrumentPassportRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result<InstrumentPassportId>> Handle(CreateInstrumentPassportCommand request, CancellationToken cancellationToken)
        {
            var guard = AccessGuard.EnsureMaster(_currentUser);
            if (!guard.IsSuccess)
                return Result<InstrumentPassportId>.Failure(guard.Error!);

            var passport = InstrumentPassport.New(
                InstrumentPassportId.New(),
                request.InstrumentId,
                request.IssueDate,
                request.Details
            );

            await _repository.AddAsync(passport, cancellationToken);

            return Result<InstrumentPassportId>.Success(passport.Id);
        }
    }
}
