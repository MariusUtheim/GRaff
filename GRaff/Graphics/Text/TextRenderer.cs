using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GRaff.Graphics.Text
{
	
    public class TextRenderer
    {
        private static readonly Regex NewlineRegex = new Regex("\r\n|\n");
              
        public TextRenderer(Font font, Alignment alignment = Alignment.TopLeft, int? lineWidth = null, double lineSeparation = 0)
        {
            Contract.Requires<ArgumentNullException>(font != null);
            this.Font = font;
            this.Alignment = alignment;
            this.LineWidth = lineWidth;
            this.LineSeparation = lineSeparation;
        }

        [ContractInvariantMethod]
        private void objectInvariants()
        {
            Contract.Invariant(Font != null);
        }
  
        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>The font.</value>
        public Font Font { get; set; }

        /// <summary>
        /// Gets or sets the maximum width of the line. Lines longer than this
        /// will be broken. If set to null, there is no limit.
        /// </summary>
        /// <value>The width of the line.</value>
        public int? LineWidth { get; set; }

        /// <summary>
        /// Gets or sets the line separation. Each new line is offset by the
        /// height of the font plus this value.
        /// </summary>
        /// <value>The line separation.</value>
        public double LineSeparation { get; set; }

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        /// <value>The text alignment.</value>
        public Alignment Alignment { get; set; } = Alignment.TopLeft;

		/// <summary>
        /// Splits the text into lines as they would be rendered by this 
		/// text renderer, taking into account newline characters and the maximum
		/// width of a line. 
        /// </summary>
        /// <returns>The lines as they would be rendered.</returns>
        /// <param name="text">The text to be split.</param>
		public virtual IEnumerable<string> LineSplit(string text)
        {
			if (text == null)
				yield break;
			
            if (LineWidth == null)
            {
                yield return text;
                yield break;
            }
            
			foreach (var paragraph in NewlineRegex.Split(text))
			{            
				var words = paragraph.Split(' ');
				var lengthOfSpace = Font.GetWidth(" ");

				var currentLine = new StringBuilder(words[0]);
				var currentLineLength = Font.GetWidth(words[0]);

				var lengths = words.Select(word => Font.GetWidth(word));

				for (var i = 1; i < words.Length; i++)
				{
					var wordLength = Font.GetWidth(words[i]);
					if (LineWidth == null || currentLineLength + wordLength < LineWidth)
					{
						currentLine.Append(" " + words[i]);
						currentLineLength += lengthOfSpace + wordLength;
					}
					else
					{
						yield return currentLine.ToString();

						currentLine = new StringBuilder(words[i]);
						currentLineLength = wordLength;
					}
				}

				yield return currentLine.ToString().TrimEnd(Environment.NewLine.ToCharArray());
			}
        }


        /// <summary>
        /// If the input string is narrower than the maximum width of a line, that
        /// string is returned. Otherwise, as many characters as possible are
        /// returned, appended by the specified ellipsis.
        /// </summary>
        /// <returns>The truncated text.</returns>
        /// <param name="text">The text to truncate.</param>
        /// <param name="trail">The trail to append to the text if it is too long.</param>
        #warning Do a check for maximum height as well
        public virtual string Truncate(string text, string trail = "...")
        {
            if (text == null)
                return "";
            if (LineWidth == null)
                return text;

			var trailWidth = Font.GetWidth(trail);
            var lowerBound = LineWidth - trailWidth;
            var offset = 0;

            for (var i = 0; i < text.Length; i++)
            {            
                var nextWidth = Font.GetWidth(text[i]);
                var advance = Font.GetAdvance(text, i);

                if (offset + nextWidth > LineWidth)
                    return text.Substring(0, i) + trail;

                if (offset + advance >= lowerBound)
                {
                    offset += advance;
                    for (var j = i + 1; j < text.Length; j++)
                    {
                        nextWidth = Font.GetWidth(text[j]);
                        if (offset + nextWidth > LineWidth)
                            return text.Substring(0, i) + trail;
                        offset += Font.GetAdvance(text, j);
                    }
                    break;
                }

                offset += advance;
            }

            return text;
        }

        /// <summary>
        /// Gets the width of the text as it would be rendered, taking into 
        /// account line splits.
        /// </summary>
        /// <returns>The width of the text.</returns>
        /// <param name="text">The text.</param>
        public virtual int GetWidth(string text) => LineSplit(text).Select(Font.GetWidth).Max();

        /// <summary>
        /// Gets the height of the text as it would be rendered, taking into
        /// account line splits.
        /// </summary>
        /// <returns>The height of the text.</returns>
        /// <param name="text">The text.</param>
        public virtual double GetHeight(string text)
        {
            var n = LineSplit(text).Count();
            return n * Font.Height + (n - 1) * LineSeparation;
        }
        
        /// <summary>
        /// Renders the specified text to a new GRaff.Texture. The text is 
        /// rendered white on a transparent background.
        /// </summary>
        /// <returns>The rendered text.</returns>
        /// <param name="text">The text to be rendered.</param>
        public Texture Render(string text)
        {
            var width = GetWidth(text);
            var height = (int)GMath.Ceiling(GetHeight(text));
            
			double originX, originY;

            switch (this.Alignment & Alignment.Horizontal)
            {
                case Alignment.Left: originX = 0; break;
                case Alignment.Center: originX = width / 2; break;
                case Alignment.Right: originX = width; break;
                default: originX = 0; break;
            }

			switch (this.Alignment & Alignment.Vertical)
            {
                case Alignment.Top: originY = 0; break;
                case Alignment.Center: originY = height / 2; break;
                case Alignment.Bottom: originY = height; break;
                default: originY = 0; break;
            }

			using (var buffer = new Framebuffer(width, height, Colors.Invisible))
            using (buffer.Use())
            {
				Draw.Text(text, this, (originX, originY), Colors.Black);
                return buffer.Texture;
            }
        }

        /// <summary>
        /// Renders spatial and uv-vertices to be used to draw the specified text under the specified transformation.
        /// </summary>
        /// <returns>The spatial and uv-vertices that would be used to render
        /// the specified text.</returns>
        /// <param name="text">The text to be rendered.</param>
		/// <param name="transform">A matrix specifying the transformation of the text.</param>
        /// <remarks> Characters not found in the font are ignored without being rendered. 
		/// Each character is rendered as two triangles for a total of 6 vertices in xy space and uv space. 
		/// If overriding this method, the returned arrays should have the same length, which should be
		/// divisible by 6, in order to ensure compatibility with use in Draw.Text functions.
		/// </remarks>
        public virtual (GraphicsPoint[] vertices, GraphicsPoint[] texCoords) RenderVertices(string text, Matrix transform)
        {
            var lines = LineSplit(text).ToArray();
            var length = lines.SelectMany(str => str.ToCharArray())
                              .Count(Font.HasCharacter);
            double tXScale = 1.0 / Font.Texture.Width, tYScale = 1.0 / Font.Texture.Height;

            var vertices = new GraphicsPoint[6 * length];
            var texCoords = new GraphicsPoint[6 * length];

            var x0 = 0.0;
            var y0 = 0.0;

            switch (Alignment & Alignment.Vertical)
            {
                case Alignment.Top: y0 = 0; break;
                case Alignment.Center: y0 = -((Font.Height + LineSeparation) * (lines.Length - 1) + Font.Height) / 2; break;
                case Alignment.Bottom: y0 = -((Font.Height + LineSeparation) * (lines.Length - 1) + Font.Height); break;
            }

            var idx = 0;
            for (var l = 0; l < lines.Length; l++)
            {
                var lineWidth = Font.GetWidth(lines[l]);
                switch (Alignment & Alignment.Horizontal)
                {
                    case Alignment.Left: x0 = 0; break;
                    case Alignment.Center: x0 = -lineWidth / 2f; break;
                    case Alignment.Right: x0 = -lineWidth; break;
                }

                var x = x0;
                var y = y0 + l * (Font.Height + LineSeparation);
                for (var i = 0; i < lines[l].Length; i++)
                {
                    if (Font.TryGetCharacter(lines[l][i], out FontCharacter c))
                    {
                        vertices[idx] = transform * new GraphicsPoint(x + c.XOffset, y + c.YOffset);
                        vertices[idx + 1] = vertices[idx + 3] = transform * new GraphicsPoint(x + c.XOffset + c.Width, y + c.YOffset);
                        vertices[idx + 2] = vertices[idx + 4] = transform * new GraphicsPoint(x + c.XOffset, y + c.YOffset + c.Height);
                        vertices[idx + 5] = transform * new GraphicsPoint(x + c.XOffset + c.Width, y + c.YOffset + c.Height);
                        x += c.XAdvance;
                        if (i < lines[l].Length - 1)
                            x += Font.GetKerning(lines[l][i], lines[l][i + 1]);

                        texCoords[idx] = new GraphicsPoint(tXScale * c.X, tYScale * c.Y);
                        texCoords[idx + 1] = texCoords[idx + 3] = new GraphicsPoint(tXScale * (c.X + c.Width), tYScale * c.Y);
                        texCoords[idx + 2] = texCoords[idx + 4] = new GraphicsPoint(tXScale * c.X, tYScale * (c.Y + c.Height));
                        texCoords[idx + 5] = new GraphicsPoint(tXScale * (c.X + c.Width), tYScale * (c.Y + c.Height));
                    }
                    idx += 6;
                }
            }

            return (vertices, texCoords);
        }

    }
}
