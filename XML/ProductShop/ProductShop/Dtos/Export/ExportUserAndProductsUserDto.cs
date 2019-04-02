using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("User")]
    public class ExportUserAndProductsUserDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("SoldProducts")]
        public ExportSoldProductsAllDto ExportSoldProductsAllDto { get; set; }
    }

    //<firstName>Cathee</firstName>
    //<lastName>Rallings</lastName>
    //<age>33</age>
}