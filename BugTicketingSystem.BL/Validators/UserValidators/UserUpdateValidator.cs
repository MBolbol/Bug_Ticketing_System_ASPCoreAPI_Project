using BugTicketingSystem.BL.Constants;
using BugTicketingSystem.BL.Dtos.UserDtos;
using BugTicketingSystem.DAL.UnitOfWork;
using BugTicketingSystem.Shared;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BugTicketingSystem.BL.Validators.UserValidators
{
    public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public UserUpdateValidator(
            IUnitOfWork unitOfWork,
            IStringLocalizer<SharedResources> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;

            RuleFor(u => u.Name)
                .NotEmpty()
                .WithMessage(_localizer[ErrorConstants.ErrorMessages.NameRequired])
                .WithErrorCode(ErrorConstants.ErrorCodes.NameRequired)

                .MinimumLength(3)
                .WithMessage(_localizer[ErrorConstants.ErrorMessages.NameCannotBeShorterThan3Characters])
                .WithErrorCode(ErrorConstants.ErrorCodes.NameCannotBeShorterThan3Characters);

            RuleFor(u => u.Role)
                .NotEmpty()
                .WithMessage(_localizer[ErrorConstants.ErrorMessages.UserRoleRequired])
                .WithErrorCode(ErrorConstants.ErrorCodes.UserRoleRequired);
            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage(_localizer[ErrorConstants.ErrorMessages.EmailRequired])
                .WithErrorCode(ErrorConstants.ErrorCodes.EmailRequired)

                .MustAsync(CheckEmailIsUnique)
                .WithMessage(_localizer[ErrorConstants.ErrorMessages.EmailMustBeUnique])
                .WithErrorCode(ErrorConstants.ErrorCodes.EmailMustBeUnique);
        }
        private async Task<bool> CheckEmailIsUnique(
            string email,
            CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepo.GetAllAsync();
            return !user.Any(u => u.Email == email);
        }
    }
}
