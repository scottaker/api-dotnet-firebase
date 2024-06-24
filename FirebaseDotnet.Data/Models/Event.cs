using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace FirebaseDotnet.Data.Models;

[FirestoreData]
public class Event
{
    [FirestoreProperty("id")]
    [JsonProperty("id")]
    public string EventID { get; set; }

    [FirestoreProperty("complaint_id")]
    [JsonProperty("complaint_id")]
    public string ComplaintID { get; set; }

    [FirestoreProperty("event_type")]
    [JsonProperty("event_type")]
    public object EventType { get; set; }

    [FirestoreProperty("comments")]
    [JsonProperty("comments")]
    public string Comments { get; set; }

    [FirestoreProperty("timestamp")]
    [JsonProperty("timestamp")]
    public string Timestamp { get; set; }

    [FirestoreProperty("employee_id")]
    [JsonProperty("employee_id")]
    public string EmployeeID { get; set; }
    
    [FirestoreProperty("employee")]
    [JsonProperty("employee")]
    public object Employee { get; set; }
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