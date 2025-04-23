using BugTicketingSystem.BL.Constants;
using BugTicketingSystem.BL.Dtos.AttachmentDtos;
using BugTicketingSystem.DAL.UnitOfWork;
using FluentValidation;

namespace BugTicketingSystem.BL.Validators.AttachmentValidators
{

    public class AttachmentAddValidator: AbstractValidator<AttachmentAddDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AttachmentAddValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(a => a.FileName)
                .NotEmpty()
                .WithMessage(ErrorConstants.ErrorMessages.NameRequired)
                .WithErrorCode(ErrorConstants.ErrorCodes.NameRequired)
                .MinimumLength(3)
                .WithMessage(ErrorConstants.ErrorMessages.NameCannotBeShorterThan3Characters)
                .WithErrorCode(ErrorConstants.ErrorCodes.NameCannotBeShorterThan3Characters)
                
                .MustAsync(CheckFileNameIsUnique)
                .WithMessage(ErrorConstants.ErrorMessages.FileNameMustBeUnique)
                .WithErrorCode(ErrorConstants.ErrorCodes.FileNameMustBeUnique);


            RuleFor(a => a.FilePath)
                .NotEmpty()
                .WithMessage(ErrorConstants.ErrorMessages.FilePathRequired)
                .WithErrorCode(ErrorConstants.ErrorCodes.FilePathRequired);


        }
        private async Task<bool> CheckFileNameIsUnique(
            string fileName,
            CancellationToken cancellationToken)
        {
            var attachments = await _unitOfWork.AttachmentRepo.GetAllAsync();
            return !attachments.Any(a => a.FileName == fileName);
        }

    }
}
