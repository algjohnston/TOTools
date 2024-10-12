namespace CS341Project;

public class DoubleElimDrawable(Color lineColor, int rowSpan) : IDrawable
{
    void IDrawable.Draw(ICanvas canvas, RectF dirtyRect)
    {
        var width = dirtyRect.Width;
        var height = dirtyRect.Height;

        canvas.StrokeColor = lineColor;
        var rowHeightMid = height / (rowSpan * 2);
        var beforeLastRow = (rowSpan * 2 - 1);
        var middleRow = (1 + beforeLastRow) / 2;
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
