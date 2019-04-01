using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("User")]
    public class ExportSoldProductsUserDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlArray("soldProducts")]
        public List<ExportSoldProductsDto> SoldProducts { get; set; }
    }

    //<User>
    //<firstName>Almire</firstName>
    //<lastName>Ainslee</lastName>
    //<soldProducts>
    //<Product>
    //<name>olio activ mouthwash</name>
    //<price>206.06</price>
    //</Product>
}