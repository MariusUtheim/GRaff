﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GRaff.Graphics.Text
{
    /// <summary>
    /// Statistics gathered by a canvas.
    /// </summary>
    public class CanvasStats
    {
        /// <summary>
        /// Number of times an attempt was made to add an image to the canvas used by the mapper.
        /// </summary>
        public int RectangleAddAttempts { get; set; }

        /// <summary>
        /// Number of cells generated by the canvas.
        /// </summary>
        public int NbrCellsGenerated { get; set; }

        /// <summary>
        /// See ICanvasStats
        /// </summary>
        public int LowestFreeHeightDeficit { get; set; }
    }
}
