using Drawing.CenterViewWPF.Centering.TeklaWrapper;

namespace Drawing.CenterViewWPF.Centering.Interfaces;

/// <summary>
///     Provides an interface for performing drawing operations within the Tekla Structures environment.
/// </summary>
public interface IDrawingOperation
{
    void SetUp(DrawingModel drawingModel);
    void CleanUp(DrawingModel drawingModel);
    void SetUda(DrawingModel drawingModel, string uda);
    void SaveAndClose(DrawingModel drawingModel);
    void Save(DrawingModel drawingModel);
}