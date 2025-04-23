using BugTicketingSystem.BL.Constants;
using BugTicketingSystem.BL.Dtos.ProjectDtos;
using BugTicketingSystem.DAL.UnitOfWork;
using FluentValidation;

namespace BugTicketingSystem.BL.Validators.ProjectValidators
{
    public class ProjectAddValidator: AbstractValidator<ProjectAddDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectAddValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage(ErrorConstants.ErrorMessages.NameRequired)
                .WithErrorCode(ErrorConstants.ErrorCodes.NameRequired)
                .MinimumLength(3)
                .WithMessage(ErrorConstants.ErrorMessages.NameCannotBeShorterThan3Characters)
                .WithErrorCode(ErrorConstants.ErrorCodes.NameCannotBeShorterThan3Characters)
                .MustAsync(CheckProjectNameIsUnique)
                .WithMessage(ErrorConstants.ErrorMessages.ProjectNameMustBeUnique)
                .WithErrorCode(ErrorConstants.ErrorCodes.ProjectNameMustBeUnique);
        }
        private async Task<bool> CheckProjectNameIsUnique(
            string projectName,
            CancellationToken cancellationToken)
        {
            var projects = await _unitOfWork.ProjectRepo.GetAllAsync();
            return !projects.Any(p => p.Name == projectName);
        }
    }
}
