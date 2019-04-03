using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlRoot(ElementName = "car")]
    public class ExportSingleCarDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute(AttributeName = "model")]
        public string Model { get; set; }

        [XmlAttribute(AttributeName = "travelled-distance")]
        public long Travelleddistance { get; set; }
    }
}