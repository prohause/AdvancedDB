using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class ImportOfficerPrisonerDto
    {
        [Required]
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Money")]
        public decimal Money { get; set; }

        [Required]
        [XmlElement("Position")]
        public string Position { get; set; }

        [Required]
        [XmlElement("Weapon")]
        public string Weapon { get; set; }

        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public List<ImportPrisonersForOfficerDto> Prisoners { get; set; }
    }

    [XmlType("Prisoner")]
    public class ImportPrisonersForOfficerDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }

    //<Officer>
    //<Name>Minerva Kitchingman</Name>
    //<Money>2582</Money>
    //<Position>Invalid</Position>
    //<Weapon>ChainRifle</Weapon>
    //<DepartmentId>2</DepartmentId>
    //<Prisoners>
    //<Prisoner id="15" />
    //</Prisoners>
    //</Officer>
}