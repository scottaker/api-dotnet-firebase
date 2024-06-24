using Newtonsoft.Json;

namespace SimpleApi.Domain.Models.Complaints;

public class Complaint
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    /* -------- COMPLAINT INFO -------- */

    [JsonProperty("complaint_type")]
    public ComplaintType ComplaintType { get; set; }

    [JsonProperty("status")]
    public StatusType Status { get; set; }

    [JsonProperty("severity")]
    public int Severity { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("resolution")]
    public string Resolution { get; set; }


    /* -------- CUSTOMER -------- */

    [JsonProperty("customer_id")]
    public string CustomerId { get; set; }

    [JsonProperty("customer_fname")]
    public string CustomerFName { get; set; }

    [JsonProperty("customer_lname")]
    public string CustomerLName { get; set; }

    [JsonProperty("customer_phone")]
    public string CustomerPhone { get; set; }

    [JsonProperty("customer_email")]
    public string CustomerEmail { get; set; }

    /* -------- BRANCH / EMPLOYEE -------- */

    [JsonProperty("branch")]
    public Branch Branch { get; set; }

    [JsonProperty("employee")]
    public Employee Employee { get; set; }


    /* -------- EVENTS -------- */
    [JsonProperty("events")]
    public IEnumerable<Event> Events { get; set; }

    /* -------- DATES -------- */


    [JsonProperty("recorded_date")]
    public DateTime ComplaintDate { get; set; }

    [JsonProperty("incident_date")]
    public DateTime IncidentDate { get; set; }

    [JsonProperty("resolution_date")]
    public DateTime? ResolutionDate { get; set; }


    //[Newtonsoft.Json.JsonIgnore]
    //public object Events { get; set; }

    //[Newtonsoft.Json.JsonIgnore]
    //public IEnumerable<string> EventIds { get; set; }

}


//using System.Text.Json.Serialization;
//using Google.Cloud.Firestore;
//using Newtonsoft.Json;

//namespace SimpleApi.Domain.Models.Complaints;

//[FirestoreData]
//public class Complaint
//{
//    [FirestoreProperty("id")]
//    [JsonProperty("id")]
//    public int Id { get; set; }

//    [FirestoreProperty("customer_id")]
//    [JsonProperty("customer_id")]
//    public string CustomerID { get; set; }

//    [FirestoreProperty("branch_id")]
//    [JsonProperty("branch_id")]
//    public string BranchID { get; set; }

//    [FirestoreProperty("employee_id")]
//    [JsonProperty("employee_id")]
//    public string EmployeeID { get; set; }

//    [FirestoreProperty("recorded_date")]
//    [JsonProperty("recorded_date")]
//    public DateTime ComplaintDate { get; set; }

//    [FirestoreProperty("incident_date")]
//    [JsonProperty("incident_date")]
//    public DateTime IncidentDate { get; set; }

//    [FirestoreProperty("complaint_type")]
//    [JsonProperty("complaint_type")]
//    public string ComplaintType { get; set; }

//    [FirestoreProperty("description")]
//    [JsonProperty("description")]
//    public string Description { get; set; }

//    [FirestoreProperty("status")]
//    [JsonProperty("status")]
//    public string Status { get; set; }

//    [FirestoreProperty("severity")]
//    [JsonProperty("severity")]
//    public int Severity { get; set; }

//    [FirestoreProperty("resolution")]
//    [JsonProperty("resolution")]
//    public string Resolution { get; set; }

//    [FirestoreProperty("resolution_date")]
//    [JsonProperty("resolution_date")]
//    public DateTime? ResolutionDate { get; set; }

//    [FirestoreProperty("events")]
//    [Newtonsoft.Json.JsonIgnore]
//    public object  Events { get; set; }
//    //Dictionary<string, object>

//    [Newtonsoft.Json.JsonIgnore]
//    public IEnumerable<string> EventIds { get; set; }

//}