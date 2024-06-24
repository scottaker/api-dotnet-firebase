

using SimpleApi.Domain.Models.Complaints;

namespace SimpleApi.Domain.Services;

public interface IComplaintService
{
    Task<ComplaintResponse> GetComplaints(ComplaintRequest request);
    Task<SupportData> GetSupportData();
}

public interface IComplaintClient
{
    Task<Tuple<IEnumerable<Complaint>, int>> Get(ComplaintRequest request);
}

public interface IBranchClient
{
    Task<IEnumerable<Branch>> GetAll();
}

public interface ISupportDataClient
{
    Task<IEnumerable<ComplaintType>> GetComplaintTypes();
    Task<IEnumerable<EventType>> GetEventTypes();
    Task<IEnumerable<StatusType>> GetStatusTypes();
    Task<IEnumerable<ComplaintType>> GetComplaintTypes(IEnumerable<int> ids);
    Task<IEnumerable<EventType>> GetEventTypes(IEnumerable<int> ids);
    Task<IEnumerable<StatusType>> GetStatusTypes(IEnumerable<int> ids);
}