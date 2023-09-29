using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.Mapping_Profiles;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.UnitTests.Features.LeaveTypes.Commands
{
    public class CreateLeaveTypeCommandTests
    {
        private Mock<ILeaveTypeRepository> _mockRepo;
        private IMapper _mapper;

        public CreateLeaveTypeCommandTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetMockLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveTypeProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidCategory_AddedCategoriesRepo()
        {
            var handler = new CreateLeaveTypeCommandHandler(_mapper, _mockRepo.Object);
            var result = await handler.Handle(new CreateLeaveTypeCommand() { Name = "tet", DefaultDays = 15}, CancellationToken.None);
            var leaveTypes = await _mockRepo.Object.GetAsync();
            leaveTypes.Count.ShouldBe(4);
            
        }
    }
}
