namespace SimpleApi.Domain.Models.Complaints;

public class ComplaintRequest
{
    public PagingInfo Paging { get; set; }
    public ComplaintSort Sort { get; set; }
    public bool SortAscending { get; set; }
}

public class ComplaintResponse
{
    public IEnumerable<Complaint> Complaints { get; set; }
    public PagingInfo Paging { get; set; }
}

public enum ComplaintSort
{
    Default = 0,
    Severity,
    Branch,
    Date
}

public class PagingInfo
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    //public int PageCount { get; set; }
    public int TotalCount { get; set; }
}


public class SupportData
{

    public IEnumerable<StatusType> StatusTypes { get; set; }
    public IEnumerable<ComplaintType> ComplaintTypes { get; set; }
    public IEnumerable<EventType> EventTypes { get; set; }

    public IEnumerable<Branch> Branches { get; set; }
}