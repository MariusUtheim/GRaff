using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using GRaff;

namespace GRaff.Graphics.Text
{
    public class RectanglePacker
    {
		private Canvas _canvas = new Canvas();

#warning Return info like total size (return a RectConfiguration)

		public static IntRectangle[] QuickMap(IntVector[] sizes)
		{
			var configuration = new RectanglePacker().QuickPack(sizes.Indexed((i, sz) => new IndexSize(i, sz.X, sz.Y)).ToArray());
			return configuration.MappedImages.OrderBy(i => i.Index).Select(i => new IntRectangle(i.X, i.Y, i.Width, i.Height)).ToArray();
		}

		public static IEnumerable<IntRectangle> QuickPack(IntVector[] sizes)
		{
			var configuration = new RectanglePacker().QuickPack(sizes.Indexed((i, sz) => new IndexSize(i, sz.X, sz.Y)).ToArray());
			return configuration.MappedImages.Select(i => new IntRectangle(i.X, i.Y, i.Width, i.Height)).ToArray();
		}

		public static IntRectangle[] Map(IntVector[] sizes)
		{
			return Map(sizes, 1, Int32.MaxValue, (w, h) => w * w + h * h * GMath.Phi);
		}

		public static IntRectangle[] Map(IntVector[] sizes, double cutoffFrequency, int maxIterations, Func<int, int, double> cost)
		{
			var configuration = new RectanglePacker().Mapping(sizes.Indexed((i, sz) => new IndexSize(i, sz.X, sz.Y)), cutoffFrequency, maxIterations, r => cost(r.Width, r.Height));
			var mapped = configuration.MappedImages;
			return mapped.OrderBy(i => i.Index).Select(i => new IntRectangle(i.X, i.Y, i.Width, i.Height)).ToArray();
		}

		public static IEnumerable<IntRectangle> Pack(IEnumerable<IntVector> sizes)
			=> Pack(sizes, 1, Int32.MaxValue, (w, h) => w * w + h * h);

		public static IEnumerable<IntRectangle> Pack(IEnumerable<IntVector> sizes, double cutoffFrequency, int maxIterations, Func<int, int, double> cost)
		{
			var configuration = new RectanglePacker().Mapping(sizes.Indexed((i, sz) => new IndexSize(i, sz.X, sz.Y)), cutoffFrequency, maxIterations, r => cost(r.Width, r.Height));
            return configuration.MappedImages.Select(i => new IntRectangle(i.X, i.Y, i.Width, i.Height));
		}

		private class IndexSize
		{
			public int Index;
			public int X, Y;
			public IndexSize(int i, int x, int y)
			{
				Index = i;
				X = x;
				Y = y;
			}
		}
		private class IndexRect
		{
			public int Index;
			public int X, Y, Width, Height;
			public IndexRect(int index, int x, int y, int w, int h)
			{
				Index = index;
				X = x;
				Y = y;
				Width = w;
				Height = h;
			}
		}
		private class RectConfiguration
		{
			public int Height { get; private set; }
			public int Width { get; private set; }

			public List<IndexRect> MappedImages { get; } = new List<IndexRect>();

			public void AddMappedImage(IndexRect mappedImage)
			{
				MappedImages.Add(mappedImage);
				Width = GMath.Max(Width, mappedImage.X + mappedImage.Width);
				Height = GMath.Max(Height, mappedImage.Y + mappedImage.Height);
			}
		}

		private RectConfiguration QuickPack(IndexSize[] sizes)
		{
			var maxW = sizes.Max(sz => sz.X);
			var maxH = sizes.Max(sz => sz.Y);

			var xCells = (int)GMath.Ceiling((double)maxH / maxW * GMath.Sqrt(sizes.Length));

			var rects = new RectConfiguration();
			for (var y = 0; ; y++)
			{
				for (var x = 0; x < xCells; x++)
				{
					int i = x + y * xCells;
					rects.AddMappedImage(new IndexRect(sizes[i].Index, x * maxW, y * maxH, sizes[i].X, sizes[i].Y));
					if (i == sizes.Length - 1)
						return rects;
				}
			}
		}

		private RectConfiguration Mapping(IEnumerable<IndexSize> sizes, double CutoffEfficiency, int MaxNbrCandidateSprites, Func<RectConfiguration, double> cost)
        {
            int candidateSpritesGenerated = 0;

            // Sort the images by height descending
            var imageInfosHighestFirst = sizes.OrderByDescending(p => p.Y);

            int totalAreaAllImages =
                (from a in imageInfosHighestFirst select a.X * a.Y).Sum();

            int widthWidestImage =
                (from a in imageInfosHighestFirst select a.X).Max();

            int heightHighestImage = imageInfosHighestFirst.First().Y;

            RectConfiguration bestSprite = null;

            int canvasMaxWidth = Int16.MaxValue;
            int canvasMaxHeight = heightHighestImage;

            while (canvasMaxWidth >= widthWidestImage)
            {
                CanvasStats canvasStats = new CanvasStats();
                int lowestFreeHeightDeficitTallestRightFlushedImage;
                RectConfiguration spriteInfo = 
                    MappingRestrictedBox(imageInfosHighestFirst, canvasMaxWidth, canvasMaxHeight, canvasStats, out lowestFreeHeightDeficitTallestRightFlushedImage);

                if (spriteInfo == null)
                {
                    // Failure - Couldn't generate a SpriteInfo with the given maximum canvas dimensions

                    // Try again with a greater max height. Add enough height so that 
                    // you don't get the same rectangle placement as this time.

                    if (canvasStats.LowestFreeHeightDeficit == Int32.MaxValue)
                        canvasMaxHeight++;
                    else
                        canvasMaxHeight += canvasStats.LowestFreeHeightDeficit;
                }
                else
                {
                    // Success - Managed to generate a SpriteInfo with the given maximum canvas dimensions

                    candidateSpritesGenerated++;

                    // Find out if the new SpriteInfo is better than the current best one
                    if ((bestSprite == null) || (cost(bestSprite) > cost(spriteInfo)))
                    {
                        bestSprite = spriteInfo;

                        var bestEfficiency = totalAreaAllImages / cost(spriteInfo);
                        if (bestEfficiency >= CutoffEfficiency) { break; }
                    }

                    if (candidateSpritesGenerated >= MaxNbrCandidateSprites) { break; }

                    // Try again with a reduce maximum canvas width, to see if we can squeeze out a smaller sprite
                    // Note that in this algorithm, the maximum canvas width is never increased, so a new sprite
                    // always has the same or a lower width than an older sprite.
                    canvasMaxWidth = bestSprite.Width - 1;

                    // Now that we've decreased the width of the canvas to 1 pixel less than the width
                    // taken by the images on the canvas, we know for sure that the images whose
                    // right borders are most to the right will have to move up.
                    //
                    // To make sure that the next try is not automatically a failure, increase the height of the 
                    // canvas sufficiently for the tallest right flushed image to be placed. Note that when
                    // images are placed sorted by highest first, it will be the tallest right flushed image
                    // that will fail to be placed if we don't increase the height of the canvas sufficiently.

                    if (lowestFreeHeightDeficitTallestRightFlushedImage == Int32.MaxValue)
                        canvasMaxHeight++;
                    else
                        canvasMaxHeight += lowestFreeHeightDeficitTallestRightFlushedImage;
                }

                // ---------------------
                // Adjust max canvas width and height to cut out sprites that we'll never accept

                int bestSpriteArea = (int)cost(bestSprite);
                bool candidateBiggerThanBestSprite;
                bool candidateSmallerThanCombinedImages;

                while (
                    (canvasMaxWidth >= widthWidestImage) &&
                    (!CandidateCanvasFeasable(
                            canvasMaxWidth, canvasMaxHeight, bestSpriteArea, totalAreaAllImages,
                            out candidateBiggerThanBestSprite, out candidateSmallerThanCombinedImages)) )
                {
                    if (candidateBiggerThanBestSprite) { canvasMaxWidth--; }
                    if (candidateSmallerThanCombinedImages) { canvasMaxHeight++; }
                }
            }

            return bestSprite;
        }

		private RectConfiguration MappingRestrictedBox(IOrderedEnumerable<IndexSize> images, int maxWidth, int maxHeight, CanvasStats canvasStats, out int lowestFreeHeightDeficitTallestRightFlushedImage)
		{
			lowestFreeHeightDeficitTallestRightFlushedImage = 0;
			_canvas.SetCanvasDimensions(maxWidth, maxHeight);

			RectConfiguration configuration = new RectConfiguration();
			int heightHighestRightFlushedImage = 0;
			int furthestRightEdge = 0;

			foreach (var image in images)
			{
				int xOffset;
				int yOffset;
				int lowestFreeHeightDeficit;
				if (!_canvas.AddRectangle(image.X, image.Y, out xOffset, out yOffset, out lowestFreeHeightDeficit))
				{
					// Not enough room on the canvas to place the rectangle
					_canvas.GetStatistics(canvasStats);
					return null;
				}

				var imageLocation = new IndexRect(image.Index, xOffset, yOffset, image.X, image.Y);
				configuration.AddMappedImage(imageLocation);

				// Update the lowestFreeHeightDeficitTallestRightFlushedImage
				int rightEdge = image.X + xOffset;
				if ((rightEdge > furthestRightEdge) ||
					((rightEdge == furthestRightEdge) && (image.Y > heightHighestRightFlushedImage)))
				{
					// The image is flushed the furthest right of all images, or it is flushed equally far to the right
					// as the furthest flushed image but it is taller. 

					lowestFreeHeightDeficitTallestRightFlushedImage = lowestFreeHeightDeficit;
					heightHighestRightFlushedImage = image.Y;
					furthestRightEdge = rightEdge;
				}
			}

			_canvas.GetStatistics(canvasStats);

			return configuration;
		}

		/// <summary>
		/// Works out whether there is any point in trying to fit the images on a canvas
		/// with the given width and height.
		/// </summary>
		/// <param name="canvasMaxWidth">Candidate canvas width</param>
		/// <param name="canvasMaxHeight">Candidate canvas height</param>
		/// <param name="bestSpriteArea">Area of the smallest sprite produces so far</param>
		/// <param name="totalAreaAllImages">Total area of all images</param>
		/// <param name="candidateBiggerThanBestSprite">true if the candidate canvas is bigger than the best sprite so far</param>
		/// <param name="candidateSmallerThanCombinedImages">true if the candidate canvas is smaller than the combined images</param>
		/// <returns></returns>
		private bool CandidateCanvasFeasable(
            int canvasMaxWidth, int canvasMaxHeight, int bestSpriteArea, int totalAreaAllImages,
            out bool candidateBiggerThanBestSprite, out bool candidateSmallerThanCombinedImages)
        {
            int candidateArea = canvasMaxWidth * canvasMaxHeight;
            candidateBiggerThanBestSprite = (candidateArea > bestSpriteArea);
            candidateSmallerThanCombinedImages = (candidateArea < totalAreaAllImages);

            return !(candidateBiggerThanBestSprite || candidateSmallerThanCombinedImages);
        }
    }
}
