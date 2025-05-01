using Tekla.Structures.Drawing;

namespace Drawing.CenterView.Library;

public class DrawingSetModel : IDrawingOperations
{
    private DrawingEnumerator RawDrawingList { get; set; }
    /// <summary>
    /// The list that contains all the valid drawings for centering in the set.
    /// </summary>
    public List<DrawingModelBase> FilteredDrawingsList { get; protected set; }
    /// <summary>
    /// Represents the number of drawings to be processed. This is taken from the FilteredDrawingsList.
    /// </summary>
    public int Count  { get; protected set; }
    /// <summary>
    /// Represents the current progress. Increments by one when moved to the next drawing.
    /// </summary>
    public int Progress { get; protected set; }
    /// <summary>
    /// The current drawing in the set.
    /// </summary>
    public DrawingModelBase CurrentDrawing { get; protected set; }
    private DrawingSetOptions _options;

    public DrawingSetModel()
    {
        var selectedDrawings = GlobalConfig.DrawingHandler.GetDrawingSelector().GetSelected();
    }

    /// <summary>
    /// When option provided, only certain sets of drawings will be included in the centering routine.
    /// </summary>
    /// <param name="options"></param>
    public DrawingSetModel(DrawingSetOptions options)
    {
        _options = options;
        switch (_options)
        {
            case DrawingSetOptions.Fab:
            case DrawingSetOptions.Ga:
            case DrawingSetOptions.All:
            case DrawingSetOptions.Selected:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        // Call FilterValid
    }

    private void CreateDrawingList(Type type)
    {
        while (RawDrawingList.MoveNext())
        {
            var currDrawing = RawDrawingList.Current;
            FilteredDrawingsList = [];
            Type drawingType;

            if (type == typeof(Tekla.Structures.Drawing.GADrawing))
            {
                drawingType = typeof(GaDrawingModel);
            }
            else if (type == typeof(Tekla.Structures.Drawing.AssemblyDrawing))
            {
                drawingType = typeof(GaDrawingModel);
            }
            else
            {
                drawingType = typeof(Tekla.Structures.Drawing.Drawing);
            }

            if (currDrawing.GetType() == type)
            {
                
            }
        }
    }
    

    public void Center()
    {
        throw new NotImplementedException();
    }

    public void FilterValid()
    {
        FilteredDrawingsList = new List<DrawingModelBase>();
        switch (_options)
        {
            case DrawingSetOptions.Fab:
            case DrawingSetOptions.Ga:
            case DrawingSetOptions.All:
            case DrawingSetOptions.Selected:
            default:
                break;
        }
    }
}

public enum DrawingSetOptions
{
    Fab,
    Ga,
    All,
    Selected
}