using System;
using System.Linq;
using System.Xml.Serialization;

namespace GRaff
{

    [Serializable]
    public class FontInfo
    {
        [XmlAttribute("face")]
        public string? Face { get; set; }

        [XmlAttribute("size")]
        public int Size { get; set; }

		[XmlAttribute("bold")]
        public int Bold { get; set; }

        [XmlAttribute("italic")]
        public int Italic { get; set; }

        [XmlAttribute("charset")]
        public string? CharSet { get; set; }

        [XmlAttribute("unicode")]
        public int Unicode { get; set; }

        [XmlAttribute("stretchH")]
        public int StretchHeight { get; set; }

        [XmlAttribute("smooth")]
        public int Smooth { get; set; }

        [XmlAttribute("aa")]
        public int SuperSampling { get; set; }

        [NonSerialized]
        private IntRectangle _Padding;

        [XmlAttribute("padding")]
        public string? Padding
        {
            get
            {
                return _Padding.Left + "," + _Padding.Top + "," + _Padding.Width + "," + _Padding.Height;
            }
            set
            {
                if (value == null)
                    _Padding = IntRectangle.Zero;
                else
                {
                    var paddingStr = value.Split(',');
                    if (paddingStr.Length != 4)
                        throw new ArgumentException("Padding must be a string on the format \"l,t,w,h\".");
                    var padding = paddingStr.Select(s => Int32.TryParse(s, out int result) ? result : throw new ArgumentException("Padding must be a string on the format \"l,t,w,h\".")).ToArray();
                    _Padding = new IntRectangle(padding[0], padding[1], padding[2], padding[3]);
                }
            }
        }

        [NonSerialized]
        private IntVector _Spacing;

        [XmlAttribute("spacing")]
        public string? Spacing
        {
            get
            {
                return _Spacing.X + "," + _Spacing.Y;
            }
            set
            {
                if (value == null)
                    _Spacing = IntVector.Zero;
                else
                {
                    var spacingStr = value.Split(',');
                    if (spacingStr.Length != 4)
                        throw new ArgumentException("Spacing must be a string on the format \"w,h\".");
                    var spacing = spacingStr.Select(s => Int32.TryParse(s, out int result) ? result : throw new ArgumentException("Spacing must be a string on the format \"w,h\".")).ToArray();
                    _Spacing = new IntVector(spacing[0], spacing[1]);
                }
            }
        }

        [XmlAttribute("outline")]
        public int OutLine { get; set; }
    }

}