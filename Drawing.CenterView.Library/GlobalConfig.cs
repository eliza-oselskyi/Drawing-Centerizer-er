namespace Drawing.CenterView.Library;

/// <summary>
/// GlobalConfig Singelton
/// </summary>
public sealed class GlobalConfig
{
    /// <summary>
    /// Global DrawingHandler object.
    /// </summary>
    public static readonly Tekla.Structures.Drawing.DrawingHandler
        DrawingHandler = new Tekla.Structures.Drawing.DrawingHandler();
    private int DrawingSetInstances { get; set; } = 0;
    private static readonly Lazy<GlobalConfig> Lazy = new Lazy<GlobalConfig>(() => new GlobalConfig());
    public static GlobalConfig Instance => Lazy.Value;
    private GlobalConfig() { }

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