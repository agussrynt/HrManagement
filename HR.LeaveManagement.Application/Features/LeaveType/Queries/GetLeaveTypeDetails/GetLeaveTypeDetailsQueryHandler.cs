using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails
{
    public class GetLeaveTypeDetailsQueryHandler : IRequestHandler<GetLeaveTypeDetailsQuery, LeaveTypeDetailsDto>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public GetLeaveTypeDetailsQueryHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            this._mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<LeaveTypeDetailsDto> Handle(GetLeaveTypeDetailsQuery req, CancellationToken cancellationToken)
        {
            //Query Database
            var leaveTypes = await _leaveTypeRepository.GetByIdAsync(req.Id);

            // Verify Record Exists
            if (leaveTypes == null)
                throw new NotFoundExceptions(nameof(leaveTypes), req.Id);

            //Convert Data Object To DTO Object
            var data = _mapper.Map<LeaveTypeDetailsDto>(leaveTypes);

            //Return list of DTO Object
            return data;
        }
    }
}