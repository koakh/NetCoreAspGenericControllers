using FluentValidation;

namespace NetCoreAspGenericControllers.ViewModels.Validations
{
    public class AttendeeViewModelValidator : AbstractValidator<AttendeeViewModel>
    {
        public AttendeeViewModelValidator()
        {
            RuleFor(attendee => attendee.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(attendee => attendee.Profession).NotEmpty().WithMessage("Profession cannot be empty");
            RuleFor(attendee => attendee.Avatar).NotEmpty().WithMessage("Profession cannot be empty");
        }
    }
}
