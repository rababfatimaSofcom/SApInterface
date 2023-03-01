using MediatR;
using Microsoft.AspNetCore.Mvc;
using SApInterface.API.Model.Domain;
using SApInterface.API.Queries;

namespace SApInterface.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SectionMediatRController : Controller
    {
        private readonly IMediator mediator;

        public SectionMediatRController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<List<Section>> GetSectionListAsync()
        {
            var sectionDetails = await mediator.Send(new GetSectionListQuery());

            return sectionDetails;
        }
    }
}
