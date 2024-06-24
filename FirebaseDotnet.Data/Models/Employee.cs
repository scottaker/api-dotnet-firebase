using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace FirebaseDotnet.Data.Models;

[FirestoreData]
public class Employee
{
    [FirestoreProperty("id")]
    [JsonProperty("id")]
    public string EmployeeID { get; set; }

    [FirestoreProperty("first_name")]
    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [FirestoreProperty("last_name")]
    [JsonProperty("last_name")]
    public string LastName { get; set; }

    [FirestoreProperty("email")]
    [JsonProperty("email")]
    public string Email { get; set; }

    [FirestoreProperty("phone")]
    [JsonProperty("phone")]
    public string Phone { get; set; }

    [FirestoreProperty("position")]
    [JsonProperty("position")]
    public string Position { get; set; }

    [FirestoreProperty("department")]
    [JsonProperty("department")]
    public string Department { get; set; }

    [FirestoreProperty("branch_id")]
    [JsonProperty("branch_id")]
    public string BranchID { get; set; }
}