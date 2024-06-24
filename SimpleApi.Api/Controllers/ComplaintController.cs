using Microsoft.AspNetCore.Mvc;
using SimpleApi.Domain.Models.Complaints;
using SimpleApi.Domain.Services;

namespace SimpleApi.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _service;

        public ComplaintController(IComplaintService service)
        {
            _service = service;
        }

        [HttpPost("")]
        public Task<ComplaintResponse> GetComplaints([FromBody] ComplaintRequest request)
        {
            var response = _service.GetComplaints(request);
            return response;
        }

        [HttpGet("support")]
        public Task<SupportData> GetSupportData()
        {
            var support = _service.GetSupportData();
            return support;
        }


    }
}
