using System.Xml.Serialization;

namespace Instagraph.Data.Dto.Import
{
    [XmlType("comment")]
    public class ImportCommentDto
    {
        [XmlElement("content")]
        public string Content { get; set; }

        [XmlElement("user")]
        public string Username { get; set; }

        [XmlElement("post")]
        public ImportSinglePostDto ImportSinglePostDto { get; set; }
    }

    //<comments>
    //<comment>
    //<content>Wow! Wow, epic!! How?</content>
    //<user>RoundAntigaBel</user>
    //<post id="22" />
    //</comment>
}