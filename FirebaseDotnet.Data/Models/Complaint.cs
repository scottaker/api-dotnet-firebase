using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace FirebaseDotnet.Data.Models;

[FirestoreData]
public class Complaint
{
    [FirestoreProperty("id")]
    [JsonProperty("id")]
    public int Id { get; set; }


    /* -------- COMPLAINT INFO -------- */

    [FirestoreProperty("complaint_type")]
    [JsonProperty("complaint_type")]
    public object ComplaintType { get; set; }

    [FirestoreProperty("status")]
    [JsonProperty("status")]
    public object Status { get; set; }

    [FirestoreProperty("severity")]
    [JsonProperty("severity")]
    public int Severity { get; set; }

    [FirestoreProperty("description")]
    [JsonProperty("description")]
    public string Description { get; set; }

    [FirestoreProperty("resolution")]
    [JsonProperty("resolution")]
    public string Resolution { get; set; }


    /* -------- CUSTOMER -------- */

    [FirestoreProperty("customer_id")]
    [JsonProperty("customer_id")]
    public string CustomerId { get; set; }

    [FirestoreProperty("customer_fname")]
    [JsonProperty("customer_fname")]
    public string CustomerFName { get; set; }

    [FirestoreProperty("customer_lname")]
    [JsonProperty("customer_lname")]
    public string CustomerLName { get; set; }

    [FirestoreProperty("customer_phone")]
    [JsonProperty("customer_phone")]
    public string CustomerPhone { get; set; }
    [FirestoreProperty("customer_email")]
    [JsonProperty("customer_email")]
    public string CustomerEmail { get; set; }


    /* -------- BRANCH / EMPLOYEE -------- */

    [FirestoreProperty("branch")]
    [JsonProperty("branch")]
    public object Branch { get; set; }

    [FirestoreProperty("employee")]
    [JsonProperty("employee")]
    public object Employee { get; set; }


    /* -------- EVENTS -------- */
    [FirestoreProperty("events")]
    [JsonProperty("events")]
    //[Newtonsoft.Json.JsonIgnore]
    public object Events { get; set; }
    //Dictionary<string, object>



    /* -------- DATES -------- */
    [FirestoreProperty("recorded_date")]
    [JsonProperty("recorded_date")]
    public DateTime RecordedDate { get; set; }

    [FirestoreProperty("incident_date")]
    [JsonProperty("incident_date")]
    public DateTime IncidentDate { get; set; }

    [FirestoreProperty("resolution_date")]
    [JsonProperty("resolution_date")]
    public DateTime? ResolutionDate { get; set; }


    // utility for creation
    [Newtonsoft.Json.JsonIgnore]
    public IEnumerable<string> EventIds { get; set; }

}