namespace TOTools.Seeding;

/// <summary>
/// Draws lines like so
/// --|
///   |--
/// --|
/// </summary>
/// <param name="lineColor">The color that lines should be.</param>
/// <param name="rowSpan">
/// The number of rows the lines span.
/// This is used to ensure the horizontal lines line up with the targets on both side of the drawable.
/// </param>
public class BracketDrawable(Color lineColor, int rowSpan) : IDrawable
{
    void IDrawable.Draw(ICanvas canvas, RectF dirtyRect)
    {
        var width = dirtyRect.Width;
        var height = dirtyRect.Height;

        canvas.StrokeColor = lineColor;

        // Divides the rows into two equal sizes doubling the number of rows
        var rowHeight = height / (2 * rowSpan);
        var rowBeforeLast = (2 * rowSpan) - 1;
        var middleRow = rowSpan;

        var centerX = dirtyRect.Left + width / 2;
        var centerY = middleRow * rowHeight;
        var bottomLineY = rowBeforeLast * rowHeight;
        canvas.DrawLine(
            dirtyRect.Left,
            rowHeight,
            centerX,
            rowHeight
        ); // Top player to middle

        canvas.DrawLine(
            dirtyRect.Left,
            bottomLineY,
            centerX,
            bottomLineY
        ); // Bottom player to middle

        canvas.DrawLine(
            centerX,
            centerY,
            dirtyRect.Right,
            centerY
        ); // Middle to winner

        canvas.DrawLine(
            centerX,
            rowHeight,
            centerX,
            bottomLineY
        ); // Top player to bottom player center line
    }
}