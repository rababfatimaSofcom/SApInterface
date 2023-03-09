using Newtonsoft.Json;

namespace SApInterface.API.Model.Domain
{
    public class SectionDetail
    {
        public string id { get; set; }

        [JsonProperty(PropertyName = "sectioncode")]
        public string sectioncode { get; set; }
        public string SectionDesc { get; set; }

    }
}
