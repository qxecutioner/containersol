using FluentValidation;
using WebApplication4.Models;

namespace WebApplication4.Validation
{
    public class ReqDataValidation : AbstractValidator<ReqData>
    {
        public ReqDataValidation() {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");
        }
    }
}
