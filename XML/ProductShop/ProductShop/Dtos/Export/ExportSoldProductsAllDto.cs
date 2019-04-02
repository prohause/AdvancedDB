﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    //[XmlType("SoldProducts")]
    public class ExportSoldProductsAllDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public List<ExportSoldProductsDto> ExportSoldProductsDtos { get; set; }
    }
}