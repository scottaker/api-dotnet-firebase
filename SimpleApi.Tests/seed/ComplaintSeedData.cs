using FirebaseDotnet.Data.Models;
using Newtonsoft.Json;
//using SimpleApi.Domain.Models.Complaints;
using SimpleApi.Tests.util;

namespace SimpleApi.Tests.seed;

public class ComplaintSeedData
{

    public ComplaintHolder Load(string path)
    {
        var loader = new DataLoader();
        var holder = new ComplaintHolder
        {
            EventTypes = load<EventType>("event-types.json"),
            StatusTypes = load<StatusType>("status-types.json"),
            ComplaintTypes = load<ComplaintType>("complaint-types.json"),


            Complaints = load<Complaint>("complaints.json"),
            Events = load<Event>("events.json"),
            Employees = load<Employee>("employees.json"),
            Branches = load<Branch>("branches.json"),
        };

        AssignDependencies(holder);
        return holder;


        /** -- loader method -- **/
        List<T> load<T>(string filename)
        {
            return loader.ReadEntities<T>(Path.Join(path, filename));
        }

    }

    private void AssignDependencies(ComplaintHolder holder)
    {
        // assign event ids to the complaints
        var events = holder.Events.GroupBy(x => x.ComplaintID).ToDictionary(x => x.Key, x => x.ToList());
        foreach (var complaint in holder.Complaints)
        {
            var id = complaint.Id.ToString();
            if (events.ContainsKey(id))
            {
                var workingEvents = events[id];
                complaint.EventIds = workingEvents.Select(x => x.EventID).ToList();
            }
        }

    }


    //private static ComplaintHolder LoadData(string path)
    //{
    //    var loader = new DataLoader();

    //    var holder = new ComplaintHolder
    //    {
    //        EventTypes = load<EventType>("event-types.json"),
    //        StatusTypes = load<StatusType>("status-types.json"),
    //        ComplaintTypes = load<ComplaintType>("complaint-types.json"),


    //        Complaints = load<Complaint>("complaints.json"),
    //        Events = load<Event>("events.json"),
    //        Employees = load<Employee>("employees.json"),
    //        Branches = load<Branch>("branches.json"),
    //    };
    //    return holder;


    //    /** -- loader method -- **/
    //    List<T> load<T>(string filename)
    //    {
    //        return loader.ReadEntities<T>(Path.Join(path, filename));
    //    }
    //}


    //var holder = new ComplaintHolder
    //{
    //    Complaints = Convert<Complaint>(data.Complaints),
    //    Employees = Convert<Employee>(data.Employees),
    //    Events = Convert<Event>(data.Employees),
    //    Branches = Convert<Branch>(data.Employees),
    //    ComplaintTypes = Convert<ComplaintType>(data.ComplaintTypes),
    //    EventTypes = Convert<EventType>(data.EventTypes),
    //    StatusTypes = Convert<StatusType>(data.StatusTypes),
    //};

    //holder = AssignDependencies(holder, path);
    //return holder;
    //private ComplaintHolder AssignDependencies(ComplaintHolder holder, string path)
    //{
    //    var loader = new DataLoader();
    //    var texts = loader.ReadFile(path + "entities.txt").ToList();
    //    var random = new Random();

    //    var branches = holder.Branches.Take(100).ToList();

    //    var text_count = texts.Count();
    //    var employee_count = holder.Employees.Count();
    //    foreach (var item in holder.Complaints)
    //    {
    //        var index = random.Next(text_count);
    //        item.Description = texts.ElementAt(index);

    //        var branch_index = random.Next(99);
    //        item.BranchID = branches[branch_index].BranchID;

    //        var employee_index = random.Next(employee_count);
    //        item.EmployeeID = holder.Employees[employee_index].EmployeeID;

    //    }


    //    return holder;
    //}

    //private static Holder LoadData_1(string path)
    //{
    //    var loader = new DataLoader();
    //    var entity_list = loader.ReadFile(path + "entities.txt");

    //    var holder = new Holder();


    //    foreach (var entity_type in entity_list)
    //    {
    //        Console.WriteLine("loading: " + entity_type);

    //        var entities = loader.ReadEntities($"./data/{entity_type}.json");


    //        switch (entity_type)
    //        {
    //            case "branches":
    //                holder.Branches = entities.ToList();
    //                break;
    //            case "complaints":
    //                holder.Complaints = entities.ToList();
    //                break;
    //            case "employees":
    //                holder.Employees = entities.ToList();
    //                break;
    //            case "events":
    //                holder.Events = entities.ToList();
    //                break;
    //            case "complaint-types":
    //                holder.ComplaintTypes = entities.ToList();
    //                break;
    //            case "event-types":
    //                holder.EventTypes = entities.ToList();
    //                break;
    //            case "status-types":
    //                holder.StatusTypes = entities.ToList();
    //                break;
    //        }

    //        var i = 1;
    //    }

    //    return holder;
    //}

    private List<T> Convert<T>(IEnumerable<dynamic> data)
    {
        Func<dynamic, T> convert = d =>
        {
            //JsonSerializer.Serialize(d);
            var serializer = Newtonsoft.Json.JsonSerializer.Create();
            var json = JsonConvert.SerializeObject(d);
            var c = JsonConvert.DeserializeObject<T>(json);
            //JsonSerializer.Deserialize<T>(json);
            return c;
        };

        var typed_data = data.Select(convert).ToList();
        return typed_data;
    }


    //public ComplaintHolder Convert(Holder holder)
    //{
    //    var json = JsonSerializer.Serialize(anonymous);
    //    var c = JsonSerializer.Deserialize<C>(json);

    //    var test = entity_list;
    //    return null;
    //}


    //public class Holder
    //{
    //    public List<dynamic> Branches { get; set; }
    //    public List<dynamic> Complaints { get; set; }
    //    public List<dynamic> Employees { get; set; }
    //    public List<dynamic> Events { get; set; }
    //    public List<dynamic> ComplaintTypes { get; set; }
    //    public List<dynamic> EventTypes { get; set; }
    //    public List<dynamic> StatusTypes { get; set; }
    //}





}

public class ComplaintHolder
{
    public List<Branch> Branches { get; set; }
    public List<Complaint> Complaints { get; set; }
    public List<Employee> Employees { get; set; }
    public List<Event> Events { get; set; }
    public List<ComplaintType> ComplaintTypes { get; set; }
    public List<EventType> EventTypes { get; set; }
    public List<StatusType> StatusTypes { get; set; }
}
