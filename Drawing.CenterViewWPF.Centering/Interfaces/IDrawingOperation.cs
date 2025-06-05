using Drawing.CenterViewWPF.Centering.TeklaWrapper;

namespace Drawing.CenterViewWPF.Centering.Interfaces;

public interface IDrawingOperation
{
    void SetUp(DrawingModel drawingModel);
    void CleanUp(DrawingModel drawingModel);
    void SetUda(DrawingModel drawingModel, string uda);
    void SaveAndClose(DrawingModel drawingModel);
    void Save(DrawingModel drawingModel);
}