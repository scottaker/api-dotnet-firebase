using System.Reflection;
using FirebaseDotnet.Data.Models;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SimpleApi.Domain.Models.Complaints;
using SimpleApi.Domain.Services;
using SimpleApi.Tests.util;
using Complaint = FirebaseDotnet.Data.Models.Complaint;
using DomainModels = SimpleApi.Domain.Models.Complaints;
using Employee = FirebaseDotnet.Data.Models.Employee;

namespace SimpleApi.Tests.seed
{
    public class Firestore_Seed
    {

        [Test]
        public async Task Complaints_Can_Retrive()
        {
            var services = new ServiceLocator();
            var client = services.Get<IComplaintService>();


            DomainModels.ComplaintRequest request = new DomainModels.ComplaintRequest
            {
                Paging = new DomainModels.PagingInfo
                {
                    PageSize = 10,
                    CurrentPage = 1
                },
                Sort = DomainModels.ComplaintSort.Default,
                SortAscending = true
            };
            var data = await client.GetComplaints(request);

            ClassicAssert.IsNotNull(data);
            Show(data);
        }

        [Test]
        public async Task Seed_Complaints()
        {
            // open files
            var seed_data = LoadSeedData();

            var project = "banking-complaints";
            var keyFileName = "SimpleApi.Tests.data.credentials.firebase-key.json";

            var db = await GetFirestoreDb(project);


            bool write_types = false, write_dependencies = false;
            if (write_types)
            {
                WriteDocuments(db, "complaint_types", seed_data.ComplaintTypes, ct => ct.Id.ToString());
                WriteDocuments(db, "status_types", seed_data.StatusTypes, ct => ct.Id.ToString());
                WriteDocuments(db, "event_types", seed_data.EventTypes, ct => ct.Id.ToString());
            }
            if (write_dependencies)
            {
                WriteDocuments(db, "branches", seed_data.Branches, ct => ct.Id.ToString());
                WriteDocuments(db, "employees", seed_data.Employees, ct => ct.EmployeeID.ToString(), this.AssignEmployee);
            }


            //var temp = seed_data.Complaints.Take(10).ToList();
            var temp = seed_data.Complaints.ToList();
            WriteDocuments(db, "complaints", temp, ct => ct.Id.ToString(), this.AssignComplaint);
            WriteDocuments(db, "events", seed_data.Events, ct => ct.EventID.ToString());
        }

        [Test]
        public async Task Single_File_Export()
        {
            var seed_data = LoadSeedData();

            var complaints = seed_data.Complaints;

            var complaintTypes = seed_data.ComplaintTypes.ToDictionary(x => x.Id, x => x);
            var eventTypes = seed_data.EventTypes.ToDictionary(x => x.Id, x => x);
            var statusTypes = seed_data.StatusTypes.ToDictionary(x => x.Id, x => x);

            var branches = seed_data.Branches.ToDictionary(x => int.Parse(x.Id), x => x);
            var employees = seed_data.Employees.ToDictionary(x => x.EmployeeID, x => x);

            // set event lookup types
            foreach (var e in seed_data.Events)
            {
                e.EventType = eventTypes[Convert.ToInt32(e.EventType)];
                e.Employee = employees[(string)e.EmployeeID];
            }

            var events = seed_data.Events.GroupBy(x => x.ComplaintID).ToDictionary(x => x.Key, x => x.ToList());


            foreach (var complaint in complaints)
            {
                // attach lookup data
                complaint.ComplaintType = complaintTypes[Convert.ToInt32(complaint.ComplaintType)];
                complaint.Status = complaintTypes[Convert.ToInt32(complaint.Status)];

                // attach branch
                complaint.Branch = branches[Convert.ToInt32(complaint.Branch)];


                // attach employees
                complaint.Employee = employees[(string)complaint.Employee];

                // attach events
                if (events.ContainsKey(complaint.Id.ToString()))
                {
                    var complaintEvents = events[complaint.Id.ToString()];
                    complaint.Events = complaintEvents;
                }
            }

            string json = JsonConvert.SerializeObject(complaints, Formatting.Indented);
            File.WriteAllText("C:\\files\\source\\api-dotnet-firebase\\SimpleApi.Tests\\data\\complaints-full.json", json);


        }

        private void AssignEmployee(FirestoreDb firestore, Employee employee, DocumentReference document)
        {
            var branchRef = firestore.Collection("status_types").Document(employee.BranchID.ToString());
            employee.BranchID = branchRef.ToString();
        }

        public void AssignComplaint(FirestoreDb firestore, Complaint complaint, DocumentReference document)
        {
            var statusRef = firestore.Collection("status_types").Document(complaint.Status.ToString());
            var typeRef = firestore.Collection("complaint_types").Document(complaint.ComplaintType.ToString());
            var branchRef = firestore.Collection("branches").Document(complaint.Branch.ToString());
            var employeeRef = firestore.Collection("employees").Document(complaint.Employee.ToString());


            complaint.Status = statusRef;
            complaint.ComplaintType = typeRef;
            complaint.Branch = branchRef;
            complaint.Employee = employeeRef;

            if (complaint.EventIds != null)
            {
                var eventsRefs = complaint.EventIds.Select(x => firestore.Collection("events").Document(x)).ToArray();
                var complaintEvents = FieldValue.ArrayUnion(eventsRefs);
                complaint.Events = complaintEvents;
            }
            //DocumentReference newDocRef = await _firestoreDb.Collection("collection-name").AddAsync(data);

        }


        private static void WriteDocuments<T>(FirestoreDb db, string collectionName, List<T> entities, Func<T, string> getKey, Action<FirestoreDb, T, DocumentReference> assignRefs = null)
        {
            var collection = db.Collection(collectionName);

            var tasks = new List<Task>();
            foreach (var item in entities)
            {
                var key = getKey(item);
                var document = collection.Document(key);
                if (assignRefs != null) assignRefs(db, item, document);
                var task = document.SetAsync(item);
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());
        }


        [Test]
        public async Task Delete_Collection_Data()
        {
            var project = "banking-complaints";
            //var collectionNames = new string[] { "complaint_types" };
            var collectionNames = new string[] { "complaints" };

            var db = await GetFirestoreDb(project);

            foreach (var name in collectionNames)
            {

                var collection = db.Collection(name);
                var snapshot = await collection.GetSnapshotAsync();
                var batch = db.StartBatch();

                foreach (var document in snapshot.Documents)
                {
                    batch.Delete(document.Reference);
                }

                // Commit the batch to delete all documents in the collection
                await batch.CommitAsync();

                var count = snapshot.Documents.Count;

                ClassicAssert.Zero(count);
                Console.WriteLine($"{name}: {count}");
            }
        }

        private async Task<FirestoreDb> GetFirestoreDb(string project)
        {
            var keyFileName = "SimpleApi.Tests.data.credentials.firebase-key.json";


            var keyData = ReadKeyFile(keyFileName);
            GoogleCredential creds = GoogleCredential.FromJson(keyData);
            FirestoreClientBuilder builder = new FirestoreClientBuilder
            {
                ChannelCredentials = creds.ToChannelCredentials()
            };
            var client = await builder.BuildAsync();
            FirestoreDb db = await FirestoreDb.CreateAsync(project, client);
            return db;
        }


        [Test]
        [Ignore("Delete - used only on reinit of data")]
        public async Task Delete_Complaints()
        {


        }

        private ComplaintHolder LoadSeedData()
        {
            var helper = new ComplaintSeedData();
            var data = helper.Load("./data/");
            return data;
        }

        [Test]
        public async Task Create_Cities()
        {
            var project = "banking-complaints";
            var keyFileName = "SimpleApi.Tests.data.credentials.firebase-key.json";

            var keyData = ReadKeyFile(keyFileName);
            GoogleCredential creds = GoogleCredential.FromJson(keyData);
            FirestoreClientBuilder builder = new FirestoreClientBuilder
            {
                ChannelCredentials = creds.ToChannelCredentials()
            };
            var client = await builder.BuildAsync();
            FirestoreDb db = await FirestoreDb.CreateAsync(project, client);




            CollectionReference citiesRef = db.Collection("cities");
            await citiesRef.Document("SF").SetAsync(new Dictionary<string, object>(){
                { "Name", "San Francisco" },
                { "State", "CA" },
                { "Country", "USA" },
                { "Capital", false },
                { "Population", 860000 }
            });
            await citiesRef.Document("LA").SetAsync(new Dictionary<string, object>(){
                { "Name", "Los Angeles" },
                { "State", "CA" },
                { "Country", "USA" },
                { "Capital", false },
                { "Population", 3900000 }
            });
            await citiesRef.Document("DC").SetAsync(new Dictionary<string, object>(){
                { "Name", "Washington D.C." },
                { "State", null },
                { "Country", "USA" },
                { "Capital", true },
                { "Population", 680000 }
            });
            await citiesRef.Document("TOK").SetAsync(new Dictionary<string, object>(){
                { "Name", "Tokyo" },
                { "State", null },
                { "Country", "Japan" },
                { "Capital", true },
                { "Population", 9000000 }
            });
            await citiesRef.Document("BJ").SetAsync(new Dictionary<string, object>(){
                { "Name", "Beijing" },
                { "State", null },
                { "Country", "China" },
                { "Capital", true },
                { "Population", 21500000 }
            });
            Console.WriteLine("Added example cities data to the cities collection.");

        }


        private string ReadKeyFile(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            //var resourceName = "firebase-key.json";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var result = reader.ReadToEnd();
                return result;
            }
        }

        private void Show(DomainModels.ComplaintResponse data)
        {
            Console.WriteLine("count: " + data.Paging.TotalCount);
            foreach (var item in data.Complaints)
            {
                Console.WriteLine($"{item.Id,-5} {item.Description}");
            }
        }

        //public static async Task<string> ReadJsonFileAsync(string relativePath)
        //{
        //    // Get the absolute path from the relative path
        //    string absolutePath = Path.Combine(AppDomain.CurrentDomain.dire .BaseDirectory., relativePath);

        //    var assembly = Assembly.GetExecutingAssembly();
        //    var resourceName = "firebase-key.json";

        //    // Read the file content asynchronously
        //    using (StreamReader reader = new StreamReader(absolutePath))
        //    {
        //        return await reader.ReadToEndAsync();
        //    }
        //}

    }



    //[FirestoreData]
    //public class Complaint
    //{
    //    [FirestoreProperty]
    //    public string Id { get; set; }

    //    [FirestoreProperty]
    //    public string Description { get; set; }

    //    [FirestoreProperty]
    //    public string Status { get; set; }

    //    [FirestoreProperty]
    //    public Timestamp Date { get; set; }
    //}
}
