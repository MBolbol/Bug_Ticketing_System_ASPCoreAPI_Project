using BugTicketingSystem.BL.Constants;
using BugTicketingSystem.BL.Dtos.UserDtos;
using BugTicketingSystem.DAL.UnitOfWork;
using FluentValidation;

namespace BugTicketingSystem.BL.Validators.UserValidators
{
    public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserUpdateValidator(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;

            RuleFor(u => u.Name)
                .NotEmpty()
                .WithMessage(ErrorConstants.ErrorMessages.NameRequired)
                .WithErrorCode(ErrorConstants.ErrorMessages.NameRequired)

                .MinimumLength(3)
                .WithMessage(ErrorConstants.ErrorMessages.NameCannotBeShorterThan3Characters)
                .WithErrorCode(ErrorConstants.ErrorMessages.NameCannotBeShorterThan3Characters);

            RuleFor(u => u.Role)
                .NotEmpty()
                .WithMessage(ErrorConstants.ErrorMessages.UserRoleRequired)
                .WithErrorCode(ErrorConstants.ErrorCodes.UserRoleRequired);
            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage(ErrorConstants.ErrorMessages.EmailRequired)
                .WithErrorCode(ErrorConstants.ErrorCodes.EmailRequired)
                
                .MustAsync(CheckEmailIsUnique)
                .WithMessage(ErrorConstants.ErrorMessages.EmailMustBeUnique)
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
