using System;
using System.Xml.Serialization;

namespace GRaff.Graphics.Text
{

    [Serializable]
    public class FontPage
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("file")]
        public string? File { get; set; }
    }
}
