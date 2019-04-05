using System.Xml.Serialization;

namespace Instagraph.Data.Dto.Import
{
    public class ImportSinglePostDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}