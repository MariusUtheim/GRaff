using System;
using System.Xml.Serialization;

namespace GRaff.Graphics.Text
{

    [Serializable]
    public class FontKerning
    {
        [XmlAttribute("first")]
        public int Left { get; set; }

        [XmlAttribute("second")]
        public int Right { get; set; }

        [XmlAttribute("amount")]
        public int Amount { get; set; }

        public override string ToString()
        {
            return $"{{FontKerning {nameof(Left)}={(char)Left} {nameof(Right)}={(char)Right} {nameof(Amount)}={Amount}}}";
        }
    }
}
