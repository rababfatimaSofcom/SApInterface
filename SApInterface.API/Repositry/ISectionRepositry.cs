using SApInterface.API.Model.Domain;

namespace SApInterface.API.Repositry
{
    public interface ISectionRepositry
    {
        Task<List<SectionDetail>> GetSectionDetails();

        Task<List<Section>> GetAsync();

        Task<Section> GetSectionAsync(string code);

        Task<Section> AddAsync(Section section);

        Task<SectionDetail> AddSectionDetail(SectionDetail section);

        Task<SectionDetail> GetCoosmosSectionbyId(string id);
    }
}
