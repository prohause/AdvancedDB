using System.Xml.Serialization;

namespace Instagraph.Data.Dto.Import
{
    [XmlType("post")]
    public class ImportPostDto
    {
        [XmlElement("caption")]
        public string Caption { get; set; }

        [XmlElement("user")]
        public string Username { get; set; }

        [XmlElement("picture")]
        public string PicturePath { get; set; }
    }

    //<posts>
    //<post>
    //<caption>#everything #swag #sunglasses #smiley #justdoit #ocean</caption>
    //<user>ScoreAntigarein</user>
    //<picture>src/folders/resources/images/story/blocked/png/1S2el3wJ3v.png</picture>
    //</post>
}