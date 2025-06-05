using Drawing.CenterViewWPF.Centering.Interfaces;
using Drawing.CenterViewWPF.Centering.TeklaWrapper;
using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Centering;
    
public class DrawingOperator : IDrawingOperation
{
    public void SetUp(DrawingModel drawingModel)
    {
        if (DrawingHandler.Instance.GetActiveDrawing() == null && drawingModel.TeklaDrawing.Title3 != "X")
        {
            DrawingHandler.Instance.SetActiveDrawing(drawingModel.TeklaDrawing, false); // set to false in production
        }
    }

    public void CleanUp(DrawingModel drawingModel)
    {
        if (drawingModel.TeklaDrawing.Title3 == "X") return; 
        var commitMessage = $"Revert title_3: {drawingModel.TeklaDrawing.Title3} -> Empty";
        drawingModel.TeklaDrawing.Title3 = "";
        drawingModel.TeklaDrawing.Modify();
        drawingModel.TeklaDrawing.CommitChanges(commitMessage);

        if (drawingModel.TeklaDrawing.IsSameDatabaseObject(DrawingHandler.Instance.GetActiveDrawing())) SaveAndClose(drawingModel);
    }

    public void SetUda(DrawingModel drawingModel, string uda) // Gets set in the DrawingModel class
    {
        drawingModel.TeklaDrawing.Title3 = uda;
        drawingModel.TeklaDrawing.CommitChanges($"Change title_3: {drawingModel.TeklaDrawing.Title3} to {uda}");
    }

    public void SaveAndClose(DrawingModel drawingModel) // Gets set in the DrawingModel class
    {
        DrawingHandler.Instance.SaveActiveDrawing();
        if (DrawingHandler.Instance.GetActiveDrawing() != null) DrawingHandler.Instance.CloseActiveDrawing(true);
    }

    public void Save(DrawingModel drawingModel) // Will be used in the GUI mode
    {
        DrawingHandler.Instance.SaveActiveDrawing();
    }
}