using System.Collections;

namespace Drawing.CenterView.Library;

public class CurrentDrawing
{

    public Tekla.Structures.Drawing.Drawing Drawing { get; set; }
    public string Name { get; set; }
    private string _title3;
    public Type DrawingType { get; set; }
    private Tekla.Structures.Drawing.DrawingObjectEnumerator Views { get; set; }
    public List<Tekla.Structures.Drawing.View> ValidViews { get; } = [];
    public bool Excluded { get; }
    private bool _guiMode;
    

    public CurrentDrawing(Tekla.Structures.Drawing.Drawing drawing)
    {
        Drawing = drawing;
        DrawingType = Drawing.GetType();
        Views = Drawing.GetSheet()
                       .GetViews();
    }

}
