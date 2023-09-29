using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(HrDatabaseContext context) : base(context)
        {
        }

        public async Task AddRequests(List<LeaveRequest> requests)
        {
            await _context.AddRangeAsync(requests);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestWithDetails()
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType).ToListAsync();

            return leaveRequests;
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestWithDetails(string userId)
        {
            var leaveRequests = await _context.LeaveRequests.Where(q => q.RequestingEmployeeId == userId)
                .Include(q => q.LeaveType).ToListAsync();

            return leaveRequests;
        }

        public async Task<LeaveRequest> GetLeaveRequestWithDetails(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(q => q.LeaveType).FirstOrDefaultAsync(q => q.Id == id);

            return leaveRequest;
        }

        public async Task<LeaveRequest> GetRequestById(string userId, int leaveTypeId)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(q => q.LeaveType).FirstOrDefaultAsync(q => q.RequestingEmployeeId == userId && q.LeaveTypeId == leaveTypeId);

            return leaveRequest;
        }

        public async Task<LeaveRequest> GetApprovedRequest(bool statusRequest)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(q => q.LeaveType).FirstOrDefaultAsync(q => q.Approved == statusRequest);

            return leaveRequest;
        }
    }
}