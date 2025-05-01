using System.Collections;

namespace Drawing.CenterView.Library;

public class DrawingModel
{

    /// <summary>
    /// The Tekla Drawing object associated with this instance.
    /// </summary>
    public Tekla.Structures.Drawing.Drawing Drawing { get; set; }
    /// <summary>
    /// The name of the drawing.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// The Title_3 property of the drawing.
    /// </summary>
    private string _title3;
    /// <summary>
    /// The Tekla drawing type of the current drawing.
    /// </summary>
    public Type DrawingType { get; set; }
    /// <summary>
    /// All the views contained in the sheet.
    /// </summary>
    private Tekla.Structures.Drawing.DrawingObjectEnumerator Views { get; set; }
    /// <summary>
    /// The list of ViewModel that are valid for centering. Currently, if this list is greater than one,
    /// the drawing gets skipped for centering.
    /// </summary>
    public List<Tekla.Structures.Drawing.View> ValidViews { get; } = [];
    /// <summary>
    /// True if drawing is excluded from being centered.
    /// Currently, this value is derived from the Title_3 property;
    /// if it is "X", the drawing is excluded.
    /// </summary>
    public bool Excluded { get; }
    /// <summary>
    /// The value that represents whether the drawing is open in the drawing editor.
    /// If true, the GUI will appear.
    /// </summary>
    private bool _guiMode;
    

    public DrawingModel(Tekla.Structures.Drawing.Drawing drawing)
    {
        Drawing = drawing;
        DrawingType = Drawing.GetType();
        Views = Drawing.GetSheet()
                       .GetViews();
    }

}
