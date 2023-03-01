using MediatR;
using SApInterface.API.Model.Domain;
using SApInterface.API.Repositry;
using SApInterface.API.Queries;

namespace SApInterface.API.Handler
{
   
    public class GetSectionListHandler : IRequestHandler<GetSectionListQuery, List<Section>>
    {
        private readonly ISectionRepositry _sectionRepository;

        public GetSectionListHandler(ISectionRepositry sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<List<Section>> Handle(GetSectionListQuery query, CancellationToken cancellationToken)
        {
            return await _sectionRepository.GetAsync();
        }
    }
}
