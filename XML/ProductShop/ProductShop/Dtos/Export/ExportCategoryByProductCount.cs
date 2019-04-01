using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Category")]
    public class ExportCategoryByProductCount
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("count")]
        public int Count { get; set; }

        [XmlElement("averagePrice")]
        public decimal AveragePrice { get; set; }

        [XmlElement("totalRevenue")]
        public decimal TotalRevenue { get; set; }
    }

    //Get all categories. For each category select its name, the number of products, the average price of those products and the total revenue (total price sum) of those products (regardless if they have a buyer or not). Order them by the number of products (descending) then by total revenue.
}