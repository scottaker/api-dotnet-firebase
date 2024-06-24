using NUnit.Framework;
using SimpleApi.Domain.Models.Complaints;
using SimpleApi.Domain.Services;
using SimpleApi.Tests.util;

namespace SimpleApi.Tests;

[TestFixture]
public class ComplaintTests
{

    [Test]
    public async Task GetComplaints()
    {
        var request = new ComplaintRequest
        {
            Paging = new PagingInfo
            {
                PageSize = 20,
                CurrentPage = 1,
            },
            Sort = ComplaintSort.Default,
            SortAscending = true
        };

        var locator = GetLocator();
        var service = locator.Get<IComplaintService>();
        var data = await service.GetComplaints(request);

        Console.WriteLine(data);
        Show(data);
    }

    [Test]
    public async Task GetSupportData()
    {
        var locator = GetLocator();
        var service = locator.Get<IComplaintService>();
        var data = await service.GetSupportData();

        Console.WriteLine(data);
        Show(data);
    }

    

    private void Show(SupportData data)
    {
        Console.WriteLine("-- ComplaintTypes --");
        foreach (var type in data.ComplaintTypes)
        {
            Console.WriteLine($"{type.Name}");
        }
        Console.WriteLine("\r\n-- EventTypes --");
        foreach (var type in data.EventTypes)
        {
            Console.WriteLine($"{type.Name}");
        }
        Console.WriteLine("\r\n-- StatusTypes --");
        foreach (var type in data.StatusTypes)
        {
            Console.WriteLine($"{type.Name}");
        }

        Console.WriteLine("\r\n-- Branches --");
        foreach (var type in data.Branches)
        {
            Console.WriteLine($"{type.Name}");
        }

    }


    private void Show(ComplaintResponse data)
    {
        foreach (var item in data.Complaints)
        {
            Console.WriteLine($"{item.Id,-5} {item.ComplaintType.Name,-20} {item.Description} ");
        }
    }

    private ServiceLocator GetLocator()
    {
        return new ServiceLocator();
    }

}