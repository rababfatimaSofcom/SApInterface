using System.Reflection;
using SApInterface.API.Model.Domain;
using SApInterface.API.Model.DTO;
namespace SApInterface.API.Profile
{
    public class SectionProfile : AutoMapper.Profile
    {
        public SectionProfile()
        {
            CreateMap<Section, SectionDTO>()
                .ReverseMap();
        }
    }
}
