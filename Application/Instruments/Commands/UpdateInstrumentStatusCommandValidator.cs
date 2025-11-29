using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Instruments.Commands
{
    public class UpdateInstrumentStatusCommandValidator : AbstractValidator<UpdateInstrumentStatusCommand>
    {
        public UpdateInstrumentStatusCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}
