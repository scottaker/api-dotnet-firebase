using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace SimpleApi.Domain.Models.Complaints;

[FirestoreData]
public class Employee
{
    [JsonProperty("id")]
    public string EmployeeID { get; set; }

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("last_name")]
    public string LastName { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("phone")]
    public string Phone { get; set; }

    [JsonProperty("position")]
    public string Position { get; set; }

    [JsonProperty("department")]
    public string Department { get; set; }

    [JsonProperty("branch_id")]
    public string BranchID { get; set; }
}