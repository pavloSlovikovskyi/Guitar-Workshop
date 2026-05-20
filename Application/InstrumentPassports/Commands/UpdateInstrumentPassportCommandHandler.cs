using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Domain.InstrumentPassports;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.InstrumentPassports.Commands
{
    public class UpdateInstrumentPassportCommandHandler : IRequestHandler<UpdateInstrumentPassportCommand, Result>
    {
        private readonly IInstrumentPassportRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public UpdateInstrumentPassportCommandHandler(
            IInstrumentPassportRepository repository,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(UpdateInstrumentPassportCommand request, CancellationToken cancellationToken)
        {
            var guard = AccessGuard.EnsureMaster(_currentUser);
            if (!guard.IsSuccess)
                return guard;

            var passport = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (passport == null)
                return Result.Failure("Instrument passport not found");

            passport.UpdateDetails(request.IssueDate, request.Details);

            await _repository.UpdateAsync(passport, cancellationToken);

            return Result.Success();
        }
    }
}
