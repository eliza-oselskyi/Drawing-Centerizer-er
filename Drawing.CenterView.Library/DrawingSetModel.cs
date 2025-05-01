using Tekla.Structures.Drawing;

namespace Drawing.CenterView.Library;

public class DrawingSetModel
{
    private DrawingEnumerator RawDrawingList { get; set; }
    /// <summary>
    /// The list that contains all the valid drawings for centering in the set.
    /// </summary>
    public List<DrawingModel> FilteredDrawingsList { get; }
    /// <summary>
    /// Represents the number of drawings to be processed. This is taken from the FilteredDrawingsList.
    /// </summary>
    public int Count  { get; }
    /// <summary>
    /// Represents the current progress. Increments by one when moved to the next drawing.
    /// </summary>
    public int Progress { get; }
    /// <summary>
    /// The current drawing in the set.
    /// </summary>
    public DrawingModel CurrentDrawing { get;  }
}