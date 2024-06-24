using Google.Cloud.Firestore;

namespace FirebaseDotnet.Data.Models;

[FirestoreData]
public class Customer
{
    [FirestoreProperty]
    public string CustomerID { get; set; }

    [FirestoreProperty]
    public string FirstName { get; set; }

    [FirestoreProperty]
    public string LastName { get; set; }

    [FirestoreProperty]
    public string Email { get; set; }

    [FirestoreProperty]
    public string Phone { get; set; }

    [FirestoreProperty]
    public string Address { get; set; }

    [FirestoreProperty]
    public string City { get; set; }

    [FirestoreProperty]
    public string State { get; set; }

    [FirestoreProperty]
    public string ZipCode { get; set; }
}