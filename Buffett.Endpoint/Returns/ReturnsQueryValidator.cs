using Buffett.Contracts.Query;
using ServiceStack.FluentValidation;

namespace Buffett.Endpoint.Returns
{
    public class ReturnsQueryValidator : AbstractValidator<GetReturnsQuery>
    {
        public ReturnsQueryValidator()
        {
            RuleFor(request => request.FromDate)
                .NotNull().WithMessage("FromDate is invalid.")
                .LessThanOrEqualTo(request => request.ToDate).WithMessage("FromDate must be the same as or before ToDate.");

            RuleFor(request => request.ToDate)
                .NotNull().WithMessage("ToDate invalid.")
                .Must(IsTodayOrBefore).WithMessage("ToDate cannot be in the future");
        }

        private bool IsTodayOrBefore(DateTime? toDate)
        {
            return toDate?.Date <= DateTime.Now.Date;
        }
    }
}
