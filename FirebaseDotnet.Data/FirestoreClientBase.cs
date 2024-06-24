using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using SimpleApi.Domain.Services;
using System.Reflection;

namespace FirebaseDotnet.Data
{

    public class FirestoreClientBase
    {
        private readonly FirestoreClient _client;
        private readonly IMapper _mapper;
        //private const string DbName = "banking-complaints";

        public FirestoreClientBase(FirestoreClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        protected async Task<IEnumerable<TDomain>> GetCollection<TData, TDomain>(string dbName, string collectionName, List<int> ids)
        {
            var firestore = await _client.Get(dbName);

            var collection = firestore.Collection(collectionName);
            var snapshot = await collection.WhereIn("id", ids).GetSnapshotAsync();

            var firestoreTypes = snapshot.Documents.Select(Map<TData>);
            var domainTypes = firestoreTypes.Select(Map<TData, TDomain>);
            return domainTypes;
        }

        protected async Task<IEnumerable<TDomain>> GetCollection<TData, TDomain>(string dbName, string collectionName)
        {
            var firestore = await _client.Get(dbName);

            var collection = firestore.Collection(collectionName);
            var snapshot = await collection.GetSnapshotAsync();
            var firestoreTypes = snapshot.Documents.Select(Map<TData>);
            var types = firestoreTypes.Select(Map<TData, TDomain>);
            return types;
        }


        private TOut Map<TIn, TOut>(TIn data)
        {
            var value = _mapper.Map<TIn, TOut>(data);
            return value;
        }

        private T Map<T>(DocumentSnapshot document)
        {
            var value = document.ConvertTo<T>();
            return value;
        }
    }

    /*
    public class Admin
    {

        private void Test()
        {
            var keyFile = ReadKeyFile();
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson(keyFile),
                ProjectId = "banking-complaints"
            });
            //var client = new FirebaseClient()
        }

        private string ReadKeyFile()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "firebase-key.json";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var result = reader.ReadToEnd();
                return result;
            }

        }

    }
*/


}
