using System.Collections;

namespace Drawing.CenterView.Library;

public class CurrentDrawing
{

    public Tekla.Structures.Drawing.Drawing Drawing { get; set; }
    public Tekla.Structures.Drawing.DrawingObjectEnumerator Views { get; set; }
    public bool IsValidForCenter { get; set; }
    private List<Tekla.Structures.Drawing.View> ValidViews { get; set; } = new List<Tekla.Structures.Drawing.View>();

    public CurrentDrawing(Tekla.Structures.Drawing.Drawing drawing)
    {
        Drawing = drawing;
        Views = Drawing.GetSheet()
                       .GetViews();
    }

    private void TestMethod()
    {
        var viewsCount = ValidViews.Count;
        IsValidForCenter = (viewsCount > 1) ? false : true;
    }
}
