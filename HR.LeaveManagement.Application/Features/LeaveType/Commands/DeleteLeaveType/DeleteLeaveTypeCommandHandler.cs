using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType
{
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, Unit>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public DeleteLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            this._leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            // Validate Incoming Data
            var leaveTypesDelete = await _leaveTypeRepository.GetByIdAsync(request.Id);

            // Verify Record Exists
            if (leaveTypesDelete == null)
                throw new NotFoundExceptions(nameof(leaveTypesDelete), request.Id);

            // Convert to Domain Entity Object
            // Add to Database
            await _leaveTypeRepository.DeleteAsync(leaveTypesDelete);

            // Return Record ID
            return Unit.Value;
        }
    }
}