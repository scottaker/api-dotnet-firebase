using AutoMapper;
using System.Drawing;
using Data = FirebaseDotnet.Data.Models;
using Models = SimpleApi.Domain.Models;

namespace SimpleApi.API.Infrastructure;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Firestore -> Domain
        CreateMap<Models.Complaints.Complaint, Data.Complaint>()
            .ForMember(x => x.ComplaintType, x => x.Ignore())
            .ForMember(x => x.Status, x => x.Ignore())
            .ForMember(x => x.Branch, x => x.Ignore())
            .ForMember(x => x.Employee, x => x.Ignore())
            .ForMember(x => x.Events, x => x.Ignore())
            ;

        // Domain -> Data
        CreateMap<Data.Complaint, Models.Complaints.Complaint>()
            .ForMember(x => x.ComplaintType, x => x.Ignore())
            .ForMember(x => x.Status, x => x.Ignore())
            .ForMember(x => x.Branch, x => x.Ignore())
            .ForMember(x => x.Employee, x => x.Ignore())
            .ForMember(x => x.Events, x => x.Ignore())
            ;


        CreateMap<Data.Branch, Models.Complaints.Branch>()
            .ReverseMap();
        CreateMap<Data.Employee, Models.Complaints.Employee>()
            .ReverseMap();
        CreateMap<Data.Event, Models.Complaints.Event>()
            .ReverseMap();

        CreateMap<Data.EventType, Models.Complaints.EventType>()
            .ReverseMap();
        CreateMap<Data.ComplaintType, Models.Complaints.ComplaintType>()
            .ReverseMap();
        CreateMap<Data.StatusType, Models.Complaints.StatusType>()
            .ReverseMap();


        //CreateMap<Point, GeoLocation>()
        //    .ConstructUsing(x => new GeoLocation(x.Coordinate.X, x.Coordinate.Y));

        ///*
        // * DATA => DOMAIN
        // */
        //CreateMap<Data.Models.Brand, Brand>()
        //    .Ignore(x => x.Id)
        //    .ReverseMap();

        //CreateMap<Data.Models.Dispensary, Dispensary>();

    }
}
