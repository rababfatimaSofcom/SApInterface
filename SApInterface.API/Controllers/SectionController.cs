using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SApInterface.API.Repositry;
using System.Reflection;
using SApInterface.API.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.Azure.Cosmos;

namespace SApInterface.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class SectionController : Controller
    {
        private readonly ISectionRepositry sectionRepository;
        private readonly IMapper mapper;

        public SectionController(ISectionRepositry sectionRepository, IMapper mapper)
        {
            this.sectionRepository = sectionRepository;
            this.mapper = mapper;
        }


        [HttpGet]

        public async Task<IActionResult> GetAllSections()
        {
            var section = await sectionRepository.GetAsync();


            var sectionsDTO = mapper.Map<List<Model.DTO.SectionDTO>>(section);
            return Ok(sectionsDTO);

        }
       
        [HttpGet]
        [Route("sectiondetail")]
        public async Task<IActionResult> GetSectionDetail()
        {
            var section = await sectionRepository.GetSectionDetails();


            //var sectionsDTO = new Model.Domain.SectionDetail()
            //{
            //    sectionCode = section.id,
            //    sectionName = addSectionRequest.sectionName,
            //};

         //   var sectionsDTO = mapper.Map<List<Model.DTO.SectionDTO>>(section);
            return Ok(section);

        }

        [HttpGet]
        [Route("{id}")]
        [ActionName("GetSectionAsync")]
        ///actionname used in post method to get response
        public async Task<IActionResult> GetSectionAsync(string id)
        {
            var section = await sectionRepository.GetSectionAsync(id);
           
            if (section == null)
            {
                return NotFound();
            }

            var sectionDTO = mapper.Map<Model.DTO.SectionDTO>(section);
            return Ok(sectionDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Model.DTO.AddSectionRequest addSectionRequest)
        {
            // Validate The Request
            //if (!ValidateAddRegionAsync(addSectionRequest))
            //{
            //    return BadRequest(ModelState);
            //}

            // Request(DTO) to Domain model
            var section = new Model.Domain.Section()
            {
                sectionCode = addSectionRequest.sectionCode,
                sectionName = addSectionRequest.sectionName,
            };

            // Pass details to Repository
            section = await sectionRepository.AddAsync(section);

            // Convert back to DTO

            var sectionDTO = new Model.DTO.SectionDTO
            {
                sectionCode = section.sectionCode,
                sectionName = section.sectionName,
            };

            return CreatedAtAction(nameof(GetSectionAsync), new { id = sectionDTO.sectionCode }, sectionDTO);
        }
    }
}
