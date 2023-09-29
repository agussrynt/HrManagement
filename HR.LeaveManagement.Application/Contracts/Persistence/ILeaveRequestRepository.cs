using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Contracts.Persistence
{
    public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
    {
        Task<LeaveRequest> GetApprovedRequest(bool approved);

        Task<LeaveRequest> GetLeaveRequestWithDetails(int id);

        Task<List<LeaveRequest>> GetLeaveRequestWithDetails();

        Task<List<LeaveRequest>> GetLeaveRequestWithDetails(string userId);

        Task AddRequests(List<LeaveRequest> allocations);

        Task<LeaveRequest> GetRequestById(string userId, int leaveTypeId);
    }
}