using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("customer")]
    public class ExportSingleCustomerDto
    {
        [XmlAttribute("full-name")]
        public string FullName { get; set; }

        [XmlAttribute("bought-cars")]
        public int CarCount { get; set; }

        [XmlAttribute("spent-money")]
        public decimal MoneySpent { get; set; }
    }

    //<customer full-name="Taina Achenbach" bought-cars="1" spent-money="5588.17" />
}