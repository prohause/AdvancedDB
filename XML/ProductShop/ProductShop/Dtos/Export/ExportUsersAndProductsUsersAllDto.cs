using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlRoot("Users")]
    public class ExportUsersAndProductsUsersAllDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public List<ExportUserAndProductsUserDto> Users { get; set; }
    }
}