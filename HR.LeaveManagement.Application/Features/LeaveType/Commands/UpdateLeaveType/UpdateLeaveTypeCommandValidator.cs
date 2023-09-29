using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType
{
    public class UpdateLeaveTypeCommandValidator : AbstractValidator<UpdateLeaveTypeCommand>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public UpdateLeaveTypeCommandValidator(ILeaveTypeRepository leaveTypeRepository)
        {
            //RuleFor(p => p.Id)
            //    .NotNull()
            //    .MustAsync(LeaveTypeMustExists);
            RuleFor(p => p.Name)
                 .NotEmpty().WithMessage("{PropertyName} is Required")
                 .NotNull()
                 .MaximumLength(70).WithMessage("{PropertyName} must be fewer than 70 characters");

            RuleFor(p => p.DefaultDays)
                .GreaterThan(1).WithMessage("{PropertyName} cannot exceed 100")
                .LessThan(100).WithMessage("{PropertyName} cannot be less than 1");

            //RuleFor(q => q)
            //    .MustAsync(LeaveTypeNameUnique)
            //    .WithMessage("Leave Type Name Is Already Exist");

            this._leaveTypeRepository = leaveTypeRepository;
        }

        private async Task<bool> LeaveTypeMustExists(int id, CancellationToken cancellationToken)
        {
            var leaveType = await _leaveTypeRepository.GetByIdAsync(id);
            return leaveType != null;
        }

        private Task<bool> LeaveTypeNameUnique(UpdateLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            return _leaveTypeRepository.IsLeaveTypeUnique(command.Name);
        }
    }
}