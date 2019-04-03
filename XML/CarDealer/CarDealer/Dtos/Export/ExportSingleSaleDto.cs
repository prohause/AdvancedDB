using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("sale")]
    public class ExportSingleSaleDto
    {
        [XmlElement("car")]
        public ExportCarForSaleDto ExportCarForSaleDto { get; set; }

        [XmlElement("discount")]
        public decimal Discount { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("price-with-discount")]
        public string PriceWithDiscount { get; set; }
    }

    //<sale>
    //<car make=\"BMW\" model=\"M5 F10\" travelled-distance=\"435603343\" />
    //<discount>30</discount>
    //<customer-name>Hipolito Lamoreaux</customer-name>
    //<price>139.97</price>
    //<price-with-discount>97.979</price-with-discount>
    //</sale>
}