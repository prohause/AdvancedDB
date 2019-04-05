using System.Xml.Serialization;

namespace Instagraph.Data.Dto.Export
{
    [XmlType("user")]
    public class ExportCommentDto
    {
        [XmlElement("Username")]
        public string Username { get; set; }

        [XmlElement("MostComments")]
        public int MostComments { get; set; }
    }

    //<users>
    //<user>
    //<Username>ScoreAntigarein</Username>
    //<MostComments>3</MostComments>
    //</user>
    //<user>
}