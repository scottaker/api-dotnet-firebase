using Google.Cloud.Firestore;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FirebaseDotnet.Data.Models;

[FirestoreData]
public class Branch
{
    [FirestoreProperty("id")]
    [JsonProperty("id")]
    public string Id { get; set; }

    [FirestoreProperty("name")]
    [JsonProperty("name")]
    public string Name { get; set; }

    [FirestoreProperty("address")]
    [JsonProperty("address")]
    public string Address { get; set; }

    [FirestoreProperty("city")]
    [JsonProperty("city")]
    public string City { get; set; }

    [FirestoreProperty("state")]
    [JsonProperty("state")]
    public string State { get; set; }

    [FirestoreProperty("zip_code")]
    [JsonProperty("zip_code")]
    public string ZipCode { get; set; }

    [FirestoreProperty("phone")]
    [JsonProperty("phone")]
    public string Phone { get; set; }

    [FirestoreProperty("email")]
    [JsonProperty("email")]
    public string Email { get; set; }
}