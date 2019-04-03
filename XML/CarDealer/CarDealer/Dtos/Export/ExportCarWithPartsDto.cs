using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("car")]
    public class ExportCarWithPartsDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long Distance { get; set; }

        [XmlArray("parts")]
        public List<ExportSinglePartDto> ExportSinglePartDtos { get; set; }
    }
}