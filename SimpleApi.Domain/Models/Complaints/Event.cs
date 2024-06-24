using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SimpleApi.Domain.Models.Complaints;

[FirestoreData]
public class Event
{
    [JsonProperty("id")]
    public string EventID { get; set; }

    [JsonProperty("complaint_id")]
    public string ComplaintID { get; set; }

    [JsonProperty("event_type")]
    public string EventType { get; set; }

    [JsonProperty("comments")]
    public string Comments { get; set; }

    [JsonProperty("timestamp")]
    public string Timestamp { get; set; }

    [JsonProperty("employee_id")]
    public string EmployeeID { get; set; }
}


//[FirestoreProperty]
//public string EventDescription { get; set; }

//[FirestoreProperty]
//public DateTime Timestamp { get; set; }


//[FirestoreProperty]
//public string BranchID { get; set; } // Nullable

//[FirestoreProperty]
//public string EventStatus { get; set; }

//[FirestoreProperty]
//public string Comments { get; set; } // Optional