namespace Drawing.CenterView.Library;

public class GlobalConfig
{
    /// <summary>
    /// Global DrawingHandler object.
    /// </summary>
    public static Tekla.Structures.Drawing.DrawingHandler
        DrawingHandler = new Tekla.Structures.Drawing.DrawingHandler();
    private int DrawingSetInstances { get; set; } = 0;

    /// <summary>
    /// Initializes a new DrawingSetModel.
    /// Ensures that there is only one instance.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void InitializeDrawingSet()
    {
        throw new NotImplementedException();
    }
}