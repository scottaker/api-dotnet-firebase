using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using System.Reflection;

namespace FirebaseDotnet.Data;

public class FirestoreClient 
{
    public async Task<FirestoreDb> Get(string project)
    {
        const string keyFileName = "FirebaseDotnet.Data.resources.firebase-key.json";
        var keyData = ReadKeyFile(keyFileName);
        var credentials = GoogleCredential.FromJson(keyData);
        var builder = new FirestoreClientBuilder
        {
            ChannelCredentials = credentials.ToChannelCredentials()
        };
        var client = await builder.BuildAsync();
        FirestoreDb? db = await FirestoreDb.CreateAsync(project, client);
        return db;
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

}