using BugTicketingSystem.BL.Constants;
using BugTicketingSystem.BL.Dtos.BugDtos;
using BugTicketingSystem.DAL.UnitOfWork;
using FluentValidation;

namespace BugTicketingSystem.BL.Validators.BugValidators
{
    public class BugAddValidator: AbstractValidator<BugAddDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public BugAddValidator(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
            RuleFor(b => b.Title)
                .NotEmpty()
                .WithMessage(ErrorConstants.ErrorMessages.NameRequired)
                .WithErrorCode(ErrorConstants.ErrorCodes.NameRequired)

                .MinimumLength(3)
                .WithMessage(ErrorConstants.ErrorMessages.NameCannotBeShorterThan3Characters)
                .WithErrorCode(ErrorConstants.ErrorCodes.NameCannotBeShorterThan3Characters);

        }
    }
}
