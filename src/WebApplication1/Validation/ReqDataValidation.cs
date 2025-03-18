using FluentValidation;
using WebApplication1.Models;

namespace WebApplication1.Validation
{
    public class ReqDataValidation : AbstractValidator<ReqData>
    {
        public ReqDataValidation() {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");
        }
    }
}
