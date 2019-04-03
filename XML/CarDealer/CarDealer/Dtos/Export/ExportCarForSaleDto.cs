using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    public class ExportCarForSaleDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long Distance { get; set; }
    }
}