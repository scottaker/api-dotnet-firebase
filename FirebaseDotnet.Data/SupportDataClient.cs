using Google.Cloud.Firestore;
using SimpleApi.Domain.Models.Complaints;
using SimpleApi.Domain.Services;
using DataModels = FirebaseDotnet.Data.Models;

namespace FirebaseDotnet.Data;

public class SupportDataClient : FirestoreClientBase, ISupportDataClient
{
    private const string DbName = "banking-complaints";

    public SupportDataClient(FirestoreClient client, IMapper mapper) : base(client, mapper)
    {
    }

    public async Task<IEnumerable<ComplaintType>> GetComplaintTypes()
    {
        var values = await GetCollection<DataModels.ComplaintType, ComplaintType>(DbName, "complaint_types");
        return values;
    }

    public async Task<IEnumerable<ComplaintType>> GetComplaintTypes(IEnumerable<int> ids)
    {


        var values = await GetCollection<DataModels.ComplaintType, ComplaintType>(DbName, "complaint_types", ids.ToList());
        return values;
    }

    public Task<IEnumerable<EventType>> GetEventTypes()
    {
        var values = GetCollection<DataModels.EventType, EventType>(DbName, "event_types");
        return values;
    }

    public Task<IEnumerable<EventType>> GetEventTypes(IEnumerable<int> ids)
    {
        var values = GetCollection<DataModels.EventType, EventType>(DbName, "event_types", ids.ToList());
        return values;
    }

    public Task<IEnumerable<StatusType>> GetStatusTypes()
    {
        var values = GetCollection<DataModels.StatusType, StatusType>(DbName, "status_types");
        return values;
    }

    public Task<IEnumerable<StatusType>> GetStatusTypes(IEnumerable<int> ids)
    {
        var values = GetCollection<DataModels.StatusType, StatusType>(DbName, "status_types", ids.ToList());
        return values;
    }

    /*
    private async Task<IEnumerable<TDomain>> GetCollection<TData, TDomain>(string dbName, string collectionName)
    {
        var firestore = await _client.Get(dbName);

        var collection = firestore.Collection(collectionName);
        var snapshot = await collection.GetSnapshotAsync();
        var firestoreTypes = snapshot.Documents.Select(Map<TData>);
        var types = firestoreTypes.Select(Map<TData, TDomain>);
        return types;
    }


    private TOut Map<TIn, TOut>(TIn data)
    {
        var value = _mapper.Map<TIn, TOut>(data);
        return value;
    }

    private T Map<T>(DocumentSnapshot document)
    {
        var value = document.ConvertTo<T>();
        return value;
    }
    */
}