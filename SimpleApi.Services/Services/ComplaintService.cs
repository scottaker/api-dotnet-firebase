using SimpleApi.Domain.Models.Complaints;
using SimpleApi.Domain.Services;

namespace SimpleApi.Services.Services;

public class ComplaintService : IComplaintService
{
    private readonly IComplaintClient _client;
    private readonly ISupportDataClient _supportData;
    private readonly IBranchClient _branchClient;

    public ComplaintService(IComplaintClient client, ISupportDataClient supportData, IBranchClient branchClient)
    {
        _client = client;
        _supportData = supportData;
        _branchClient = branchClient;
    }


    public async Task<ComplaintResponse> GetComplaints(ComplaintRequest request)
    {
        var complaints = await _client.Get(request);

        var paging = new PagingInfo
        {
            PageSize = request.Paging.PageSize,
            CurrentPage = request.Paging.CurrentPage,
            TotalCount = complaints.Item2
        };

        var response = new ComplaintResponse
        {
            Complaints = complaints.Item1,
            Paging = paging
        };
        return response;
    }

   


    public async Task<SupportData> GetSupportData()
    {
        var complaintsTypes = _supportData.GetComplaintTypes();
        var eventTypes =  _supportData.GetEventTypes();
        var statusTypes =  _supportData.GetStatusTypes();
        var branches = _branchClient.GetAll();

        var tasks = new List<Task>()
        {
            complaintsTypes,
            eventTypes,
            statusTypes,
            branches
        };
        await Task.WhenAll(tasks);

        var response = new SupportData
        {
            ComplaintTypes = complaintsTypes.Result,
            EventTypes = eventTypes.Result,
            StatusTypes = statusTypes.Result,
            Branches = branches.Result
        };
        return response;

    }

}