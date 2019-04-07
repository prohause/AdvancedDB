using System.Collections.Generic;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class ExportPrisonerDto
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("IncarcerationDate")]
        public string Date { get; set; }

        [XmlArray("EncryptedMessages")]
        public List<ExportMessageDto> MessageDtos { get; set; }
    }

    [XmlType("Message")]
    public class ExportMessageDto
    {
        [XmlElement("Description")]
        public string Description { get; set; }
    }

    //<Prisoners>
    //<Prisoner>
    //<Id>3</Id>
    //<Name>Binni Cornhill</Name>
    //<IncarcerationDate>1967-04-29</IncarcerationDate>
    //<EncryptedMessages>
    //<Message>
    //<Description>!?sdnasuoht evif-ytnewt rof deksa uoy ro orez artxe na ereht sI</Description>
    //</Message>
    //</EncryptedMessages>
    //</Prisoner>
}