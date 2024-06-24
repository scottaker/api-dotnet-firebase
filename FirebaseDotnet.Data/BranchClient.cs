using SimpleApi.Domain.Models.Complaints;
using SimpleApi.Domain.Services;
using DataModels = FirebaseDotnet.Data.Models;
namespace FirebaseDotnet.Data;


public class BranchClient : FirestoreClientBase, IBranchClient
{
    private const string DbName = "banking-complaints";

    public BranchClient(FirestoreClient client, IMapper mapper) : base(client, mapper)
    {
    }

    public Task<IEnumerable<Branch>> GetAll()
    {
        var branches = GetCollection<DataModels.Branch, Branch>(DbName, "branches");
        return branches;
    }
}