using TOTools.Models.Startgg;

namespace TOTools.Seeding;

/// <summary>
/// A drawable that draws a round-robin bracket.
/// </summary>
/// <param name="roundRobin">The round-robin information.</param>
/// <param name="lineColor">The line color of the bracket.</param>
public class RoundRobinDrawable(RoundRobin roundRobin, Color lineColor) : IDrawable
{
    
    /**
     * None of this code is correct!
     * I just kept it for reference!
     * TODO Draw the round-robin
     */
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = lineColor;

        var width = dirtyRect.Width;
        var height = dirtyRect.Height;
        var roundRobinSquareSize = Math.Min(width, height);
        var horizontalOffset = width - roundRobinSquareSize;
        var matchSquareSize = roundRobinSquareSize / (roundRobin.EntrantNames.Count + 1);

        // Draw the names
        for (var i = 0; i < roundRobin.EntrantNames.Count; i++)
        {
            var entrantName = roundRobin.EntrantNames[i];

            // Vertical name
            var leftColumnY = (i * matchSquareSize);
            canvas.DrawString(
                entrantName,
                horizontalOffset,
                leftColumnY + matchSquareSize / 2,
                matchSquareSize,
                matchSquareSize,
                HorizontalAlignment.Left,
                VerticalAlignment.Center
            );

            // Horizontal name
            var topRowX = (i * matchSquareSize) + horizontalOffset;
            canvas.DrawString(
                entrantName,
                topRowX + matchSquareSize / 2,
                0,
                matchSquareSize,
                matchSquareSize,
                HorizontalAlignment.Center,
                VerticalAlignment.Top
            );
        }

        // Draw the squares
        for (var row = 1; row <= roundRobin.EntrantNames.Count; row++)
        {
            for (var col = 1; col <= roundRobin.EntrantNames.Count; col++)
            {
                if (row == col)
                {
                    continue;
                }

                var x = (col * matchSquareSize) + horizontalOffset;
                var y = row * matchSquareSize;
                canvas.DrawRectangle(x, y, matchSquareSize, matchSquareSize);
            }
        }
    }
    
}