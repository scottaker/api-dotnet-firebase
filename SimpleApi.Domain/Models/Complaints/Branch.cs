using Google.Cloud.Firestore;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SimpleApi.Domain.Models.Complaints;

[FirestoreData]
public class Branch
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("zip_code")]
    public string ZipCode { get; set; }

    [JsonProperty("phone")]
    public string Phone { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }
}