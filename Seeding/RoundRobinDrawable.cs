using TOTools.Models.Startgg;

namespace TOTools.Seeding;

internal class RoundRobinDrawable(List<string> uniquePlayers, List<Set> sets, Color color) : IDrawable
{
    private const int MinFont = 4;
    private const int Padding = 4;
    private const int DoublePadding = Padding * 2;

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        var width = dirtyRect.Width;
        var height = dirtyRect.Height;

        canvas.StrokeColor = color;
        var rowHeight = height / (uniquePlayers.Count);
        var columnWidth = width / (uniquePlayers.Count);
        SetFontSize(canvas, uniquePlayers, rowHeight, columnWidth);


        for (var i = 0; i < uniquePlayers.Count + 1; i++)
        {
            var x = (i * columnWidth);
            var y = (i * rowHeight);
            canvas.DrawLine(0, y, width, y);
            canvas.DrawLine(x, 0, x, height);
        }

        foreach (var set in sets)
        {
            var rowIndex = uniquePlayers.IndexOf(set.Player1);
            var colIndex = uniquePlayers.IndexOf(set.Player2);
            var x = (colIndex * columnWidth);
            var y = (rowIndex * rowHeight);
            var fillOnly = rowIndex == colIndex;
            FillCell(canvas, x, y, columnWidth, rowHeight, fillOnly, set);
            x = (rowIndex * columnWidth);
            y = (colIndex * rowHeight);
            FillCell(canvas, x, y, columnWidth, rowHeight, fillOnly, set);
        }
    }

    private static void SetFontSize(
        ICanvas canvas, 
        List<string> playerNames, 
        float rowHeight, float columnWidth)
    {
        var minFontSize = Math.Min(rowHeight, columnWidth);
        foreach (var text in playerNames)
        {
            var fontSize = Math.Min(rowHeight, columnWidth);
            var size = canvas.GetStringSize(text, null, fontSize);
            while (fontSize > MinFont && (size.Width > (columnWidth - DoublePadding) || size.Height > rowHeight - DoublePadding))
            {
                fontSize -= 1;
                size = canvas.GetStringSize(text, null, fontSize);
            }
            minFontSize = Math.Min(minFontSize, fontSize);
        }
        canvas.FontSize = minFontSize;
    }

    private static void FillCell(
        ICanvas canvas,
        float x, float y,
        float columnWidth, float rowHeight,
        bool fillOnly,
        Set set)
    {
        canvas.FillRectangle(
            x + Padding, 
            y + Padding, 
            columnWidth - DoublePadding, 
            rowHeight - DoublePadding);
        if (fillOnly)
        {
            return;
        }

        var text = set.Winner ?? "TBD";
        canvas.DrawString(
            text,
            x,
            y,
            columnWidth,
            rowHeight,
            HorizontalAlignment.Center,
            VerticalAlignment.Center
        );
    }
}