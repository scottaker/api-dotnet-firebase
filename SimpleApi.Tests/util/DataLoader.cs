using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;

namespace SimpleApi.Tests.util;

public class DataLoader
{


    public IEnumerable<string> ReadFile(string relativePath)
    {

        //string relativePath = "relative/path/to/your/file.csv";
        var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

        //Console.WriteLine("Loading file from: " + absolutePath);
        //string relativePath = "relative/path/to/your/file.csv";
        //var basePath = AppDomain.CurrentDomain.BaseDirectory;
        //var absolutePath1 = Path.Combine(basePath, relativePath);



        var lines = File.ReadAllLines(absolutePath);
        return lines;
    }

    public List<T> ReadEntities<T>(string filepath)
    {
        // read file into a string and deserialize JSON to a type
        var data = JsonConvert.DeserializeObject<T[]>(File.ReadAllText(filepath));
        return data.ToList();

        //// deserialize JSON directly from a file
        //using (StreamReader file = File.OpenText(@"c:\movie.json"))
        //{
        //    JsonSerializer serializer = new JsonSerializer();
        //    Movie movie2 = (Movie)serializer.Deserialize(file, typeof(Movie));
        //}
    }

    public IEnumerable<dynamic> ReadEntities_1(string filepath)
    {
        var objects = new List<dynamic>();


        using (var reader = (new StreamReader(filepath)))
        {
            var header_line = reader.ReadLine();

            if (header_line == null) throw new Exception("CSV file is empty");

            var headers = header_line.Split(',');

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line == null) break;

                var values = line.Split(',');
                var expando = new ExpandoObject() as IDictionary<string, object>;
                for (var i = 0; i < headers.Length; i++)
                {

                    expando[headers[i]] = values[i];
                }
                objects.Add(expando);
            }
        }
        return objects;
    }



}