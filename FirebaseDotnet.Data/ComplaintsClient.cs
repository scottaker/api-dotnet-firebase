using System.Text;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using SimpleApi.Domain.Models.Complaints;
using SimpleApi.Domain.Services;
using DataModels = FirebaseDotnet.Data.Models;
using DomainModels = SimpleApi.Domain.Models.Complaints;


namespace FirebaseDotnet.Data;

public class ComplaintClient : IComplaintClient
{
    private const string ProjectName = "banking-complaints";
    private const string CountUrl = "https://us-central1-banking-complaints.cloudfunctions.net/collection_count";

    private readonly FirestoreClient _client;
    private readonly ISupportDataClient _support;
    private readonly IMapper _mapper;

    public ComplaintClient(FirestoreClient client, ISupportDataClient support, IMapper mapper)
    {
        _client = client;
        _support = support;
        _mapper = mapper;
    }

    public async Task<Tuple<IEnumerable<SimpleApi.Domain.Models.Complaints.Complaint>, int>> Get(ComplaintRequest request)
    {
        var firestore = await _client.Get(ProjectName);

        var sortColumn = GetSortColumn(request);

        var complaints = firestore.Collection("complaints");
        var firstQuery = complaints.OrderBy(sortColumn).Limit(3);

        // Get the last document from the results
        var querySnapshot = await firstQuery.GetSnapshotAsync();
        var lastSortValue = GetSortValue(querySnapshot, request.Sort, sortColumn);

        var secondQuery = complaints.OrderBy(sortColumn)
                                    .StartAfter(lastSortValue)
                                    .Limit(request.Paging.PageSize);
        var snapshots = await secondQuery.GetSnapshotAsync();
        var supports = await GetSupports(snapshots);
        var data = snapshots.Documents.Select(Map).ToList();

        // get the count of items
        var fxRequest = new CountRequest { collection = "complaints" };
        var countResponse = await CallFunction<CountRequest, CountResponse>(fxRequest, CountUrl);
        var count = countResponse.count;


        //var eRequest = new CountRequest { collection = "events" };
        //var eResponse = await CallFunction<CountRequest, CountResponse>(eRequest, CountUrl);
        //var ecount = eResponse.count;
        //Console.WriteLine("EVENT-COUNT: " + ecount);


        var response = data.Select(x => Map(x, supports)).ToList();
        return new Tuple<IEnumerable<Complaint>, int>(response, count);
    }

    private async Task<TResponse> CallFunction<TRequest, TResponse>(TRequest request, string url)
    {
        // Function URL
        var jsonData = JsonConvert.SerializeObject(request);
        var response = await CallFirebase(url, jsonData);
        Console.WriteLine(response);
        var value = JsonConvert.DeserializeObject<TResponse>(response);

        return value;
    }

    public static async Task<string> CallFirebase(string url, string jsonData)
    {
        using (var client = new HttpClient())
        {
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }


    private class CountRequest
    {
        public string collection { get; set; }
    }
    private class CountResponse
    {
        public string collection { get; set; }
        public int count { get; set; }
    }


    private async Task<T> CallFunction1<T>()
    {
        try
        {
            var firestore = await _client.Get(ProjectName);

            //CloudFunctionsServiceClient client = await CloudFunctionsServiceClient.CreateAsync();
            //LocationName parent = new LocationName(projectId, "-"); // The "-" means all locations
            //var response = client.ListFunctions(parent);

            //foreach (var function in response)
            //{
            //    Console.WriteLine($"Function name: {function.Name}");
            //}


            // Assuming `get_count` is a function that exists in your Firestore database
            var functionResult = await firestore.Collection("functions")
                                                .Document("collection_count")
                                                .GetSnapshotAsync();
            if (functionResult.Exists)
            {
                // Assuming the function result contains a field named 'count'
                var count = functionResult.GetValue<T>("count");
                return count;
            }
            else
            {
                throw new Exception("Function result does not exist");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error getting count: {e.Message}");
            throw;
        }
    }


    private class SupportHolder
    {
        public Dictionary<string, DomainModels.ComplaintType> ComplaintTypes { get; set; }
        public Dictionary<string, DomainModels.StatusType> StatusTypes { get; set; }
        public Dictionary<string, DomainModels.EventType> EventTypes { get; set; }
        public Dictionary<string, DomainModels.Branch> Branches { get; set; }
        public Dictionary<string, DomainModels.Employee> Employees { get; set; }
        public Dictionary<string, List<DomainModels.Event>> Events { get; set; }
    }


    private async Task<SupportHolder> GetSupports(QuerySnapshot snapshots)
    {
        var firestore = await _client.Get(ProjectName);

        var complaintTypes = await GetSupportData<DataModels.ComplaintType, DomainModels.ComplaintType>("complaint_type");
        var statusTypes = await GetSupportData<DataModels.StatusType, DomainModels.StatusType>("status");
        //var eventTypes = await GetSupportData<DataModels.EventType, DomainModels.EventType>("event_type");
        var branches = await GetSupportData<DataModels.Branch, DomainModels.Branch>("branch");
        var employees = await GetSupportData<DataModels.Employee, DomainModels.Employee>("employee");

        // events
        IEnumerable<DocumentReference> eventRefs = snapshots.Select(x => x.GetValue<List<DocumentReference>>("events"))
                                                            .Where(x => x != null)
                                                            .SelectMany(x => x)
                                                            .ToList();
        var values = await firestore.GetAllSnapshotsAsync(eventRefs);
        var events = values.Select(Map<DataModels.Event, DomainModels.Event>)
                           .GroupBy(x => x.ComplaintID)
                           .ToDictionary(x => x.Key, x => x.ToList());

        return new SupportHolder
        {
            ComplaintTypes = complaintTypes,
            StatusTypes = statusTypes,
            Branches = branches,
            Employees = employees,
            Events = events,
        };

        async Task<Dictionary<string, TDomain>> GetSupportData<TData, TDomain>(string fieldname)
        {
            var refs = snapshots.Select(x => x.GetValue<DocumentReference>(fieldname)).Distinct();
            var values = await firestore.GetAllSnapshotsAsync(refs);
            var data = values.Select(x => new KeyValuePair<string, TDomain>(x.Id, Map<TData, TDomain>(x)));


            return data.ToDictionary(x => x.Key, x => x.Value);
        }
    }


    //private void Associate(IEnumerable<DataModels.Complaint> data)
    //{
    //    // Step 2: Extract the DocumentReference from the retrieved document
    //    DocumentReference referencedDocRef = snapshot.GetValue<DocumentReference>("yourDocumentReferenceFieldName");

    //    // Step 3: Fetch the referenced document
    //    DocumentSnapshot referencedSnapshot = await referencedDocRef.GetSnapshotAsync();

    //    var types = _support.GetComplaintTypes(typeIds)


    //}


    private static object GetSortValue(QuerySnapshot querySnapshot, ComplaintSort requestSort, string sortColumn)
    {
        object lastSortValue = 0;
        Func<DocumentSnapshot, object> sortFx;
        switch (requestSort)
        {
            default:
            case ComplaintSort.Default:

                sortFx = document => document.GetValue<DateTime>(sortColumn);
                break;

            case ComplaintSort.Branch:
                sortFx = (document) => { return document.GetValue<int>(sortColumn); };
                break;
            case ComplaintSort.Severity:
                sortFx = (document) => { return document.GetValue<int>(sortColumn); };
                break;
            case ComplaintSort.Date:
                sortFx = (document) => { return document.GetValue<DateTime>(sortColumn); };
                break;
        }

        foreach (var document in querySnapshot.Documents)
        {
            // document.GetValue<long>(sortColumn);
            lastSortValue = sortFx(document);
        }

        return lastSortValue;
    }

    public async Task<IEnumerable<SimpleApi.Domain.Models.Complaints.Complaint>> GetComplaints(ComplaintRequest request)
    {

        var firestore = await _client.Get(ProjectName);

        var sortColumn = GetSortColumn(request);

        CollectionReference complaints = firestore.Collection("complaints");
        Query firstQuery = complaints.OrderBy(sortColumn).Limit(3);

        // Get the last document from the results
        QuerySnapshot querySnapshot = await firstQuery.GetSnapshotAsync();
        long lastSortValue = 0;
        foreach (var document in querySnapshot.Documents)
        {
            lastSortValue = document.GetValue<long>(sortColumn);
        }

        // Construct a new query starting at this document.
        // Note: this will not have the desired effect if multiple cities have the exact same population value
        Query secondQuery = complaints.OrderBy(sortColumn)
                                      .StartAfter(lastSortValue)
                                      .Limit(request.Paging.PageSize);


        var snapshots = await secondQuery.GetSnapshotAsync();

        // convert to local type
        var data = snapshots.Documents.Select(Map);
        return null;
        //var response = data.Select(data1 => Map(data1));
        //return response;
    }


    private string GetSortColumn(ComplaintRequest request)
    {
        switch (request.Sort)
        {
            default:
            case ComplaintSort.Default:
                return "recorded_date";
                break;
            case ComplaintSort.Branch:
                return "branch_id";
                break;
            case ComplaintSort.Severity:
                return "severity";
                break;
            case ComplaintSort.Date:
                return "recorded_date";
                break;
        }

    }
    private string GetSortFx(ComplaintRequest request)
    {
        switch (request.Sort)
        {
            default:
            case ComplaintSort.Default:
                return "recorded_date";
                break;
            case ComplaintSort.Branch:
                return "branch_id";
                break;
            case ComplaintSort.Severity:
                return "severity";
                break;
            case ComplaintSort.Date:
                return "recorded_date";
                break;
        }

    }

    /* ----- MAPPING ----- */

    private T Map<T>(DocumentSnapshot data)
    {
        var value = data.ConvertTo<T>();
        return value;
    }

    private TDomain Map<TData, TDomain>(DocumentSnapshot data)
    {
        var value = data.ConvertTo<TData>();
        var domain = Map<TData, TDomain>(value);
        return domain;
    }

    private DataModels.Complaint Map(DocumentSnapshot data)
    {
        var complaint = data.ConvertTo<DataModels.Complaint>();
        return complaint;
    }

    private TDomain Map<TData, TDomain>(TData data)
    {
        var model = _mapper.Map<TData, TDomain>(data);
        return model;
    }

    private Complaint Map(DataModels.Complaint data, SupportHolder supports)
    {
        var model = _mapper.Map<DataModels.Complaint, SimpleApi.Domain.Models.Complaints.Complaint>(data);

        if (data.ComplaintType is DocumentReference complaintTypeRef)
        {
            model.ComplaintType = supports.ComplaintTypes[complaintTypeRef.Id.ToString()];
        }
        if (data.Status is DocumentReference statsRef)
        {
            model.Status = supports.StatusTypes[statsRef.Id.ToString()];
        }
        if (data.Branch is DocumentReference branchRef)
        {
            model.Branch = supports.Branches[branchRef.Id.ToString()];
        }
        if (data.Employee is DocumentReference employee)
        {
            model.Employee = supports.Employees[employee.Id.ToString()];
        }
        if (data.Events != null) //; data.Events is DocumentReference events)
        {
            if (supports.Events != null && supports.Events.ContainsKey(data.Id.ToString()))
            {
                model.Events = supports.Events[data.Id.ToString()];
            }

            //var ids = (events as List<DocumentReference>).Select(x => x.Id).ToList();
            //var complaintEvents = events
            //var e = supports.Events.Where()
            //var events =  supports.Events.Where(x => ids.Contains(x.Key)).Select(x => x.Value);
            //model.Events = events;
        }

        return model;
    }



}