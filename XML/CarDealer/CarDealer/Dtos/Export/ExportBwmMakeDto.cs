using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlRoot(ElementName = "cars")]
    public class ExportBwmMakeDto
    {
        [XmlElement(ElementName = "car")]
        public List<ExportSingleCasDto> Car { get; set; }
    }

    //<car id="7" model="1M Coupe" travelled-distance="39826890" />
}