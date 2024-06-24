using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace FirebaseDotnet.Data.Models;

[FirestoreData]
public class ComplaintType
{
    [FirestoreProperty("id")]
    [JsonProperty("id")]
    public int Id { get; set; }

    [FirestoreProperty("name")]
    [JsonProperty("name")]
    public string Name { get; set; }

    [FirestoreProperty("description")]
    [JsonProperty("description")]
    public string Description { get; set; }
}

[FirestoreData]
public class EventType
{
    [FirestoreProperty("id")]
    [JsonProperty("id")]
    public int Id { get; set; }

    [FirestoreProperty("name")]
    [JsonProperty("name")]
    public string Name { get; set; }

    [FirestoreProperty("description")]
    [JsonProperty("description")]
    public string Description { get; set; }
}

[FirestoreData]
public class StatusType
{
    [FirestoreProperty("id")]
    [JsonProperty("id")]
    public int Id { get; set; }

    [FirestoreProperty("name")]
    [JsonProperty("name")]
    public string Name { get; set; }

    [FirestoreProperty("description")]
    [JsonProperty("description")]
    public string Description { get; set; }
}