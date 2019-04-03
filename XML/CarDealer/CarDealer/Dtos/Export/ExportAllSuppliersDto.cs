using System.Collections.Generic;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Export
{
    [XmlType("suppliers")]
    public class ExportAllSuppliersDto
    {
        [XmlElement("suplier")]
        public List<ExportSupplierListDto> ExportSupplierListDtos { get; set; }
    }
}