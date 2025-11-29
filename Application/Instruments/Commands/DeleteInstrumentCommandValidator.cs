using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Instruments.Commands
{
    public class DeleteInstrumentCommandValidator : AbstractValidator<DeleteInstrumentCommand>
    {
        public DeleteInstrumentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
