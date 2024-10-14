namespace CS341Project;

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
public class DoubleElimDrawable(Color lineColor, int rowSpan) : IDrawable
{
    void IDrawable.Draw(ICanvas canvas, RectF dirtyRect)
    {
        var width = dirtyRect.Width;
        var height = dirtyRect.Height;

        canvas.StrokeColor = lineColor;
        
        // Divides the rows into two equal sizes doubling the number of rows
        var rowHeightMid = height / (2 * rowSpan);
        var beforeLastRow = (2 * rowSpan) - 1;
        var middleRow = rowSpan;
        
        // Top player to middle
        canvas.DrawLine(dirtyRect.Left, rowHeightMid, dirtyRect.Left + width / 2, rowHeightMid);
        // Bottom player to middle
        canvas.DrawLine(dirtyRect.Left, beforeLastRow * rowHeightMid, dirtyRect.Left + width / 2, beforeLastRow * rowHeightMid);
        // Middle to winner
        canvas.DrawLine(dirtyRect.Left + width / 2, middleRow * rowHeightMid, dirtyRect.Right, middleRow * rowHeightMid);
        // Top player to bottom player center line
        canvas.DrawLine(dirtyRect.Left + width / 2, rowHeightMid, dirtyRect.Left + width / 2, beforeLastRow * rowHeightMid);

    }
    
}
