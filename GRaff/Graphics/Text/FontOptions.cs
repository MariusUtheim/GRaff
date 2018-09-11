using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Graphics.Text
{
    [Flags]
    public enum FontOptions
    {
        None = 0b0000,
        Bold = 0b0001,
        Italic = 0b0010,
        BoldItalic = 0b0011,
        IgnoreKerning = 0b0100,
    }
}
