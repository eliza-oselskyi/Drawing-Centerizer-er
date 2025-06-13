using Drawing.CenterViewWPF.Centering.Interfaces;
using Drawing.CenterViewWPF.Centering.TeklaWrapper;
using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Centering;

/// <summary>
///     Represents an implementation of the <see cref="IDrawingOperation" /> interface,
///     providing various operations for managing and interacting with Tekla drawings.
/// </summary>
public class DrawingOperator : IDrawingOperation
{
    /// <summary>
    ///     Prepares the active drawing environment in the Tekla Structures Drawing environment.
    ///     If no active drawing is detected and the given drawing is valid,
    ///     it sets the Tekla drawing as the active drawing.
    /// </summary>
    /// <param name="drawingModel">The drawing model containing the Tekla drawing to set up as active.</param>
    public void SetUp(DrawingModel drawingModel)
    {
        if (DrawingHandler.Instance.GetActiveDrawing() == null && drawingModel.TeklaDrawing.Title3 != "X")
            DrawingHandler.Instance.SetActiveDrawing(drawingModel.TeklaDrawing, false); // set to false in production
    }

    /// <summary>
    ///     Cleans up the active drawing environment in the Tekla Structures Drawing environment.
    ///     If the drawing is set to a specific state, it resets the state, commits the changes,
    ///     and saves and closes the drawing if it matches the active drawing.
    /// </summary>
    /// <param name="drawingModel">The drawing model containing the Tekla drawing to be cleaned up.</param>
    public void CleanUp(DrawingModel drawingModel)
    {
        if (drawingModel.TeklaDrawing.Title3 == "X") return;
        var commitMessage = $"Revert title_3: {drawingModel.TeklaDrawing.Title3} -> Empty";
        drawingModel.TeklaDrawing.Title3 = "";
        drawingModel.TeklaDrawing.Modify();
        drawingModel.TeklaDrawing.CommitChanges(commitMessage);

        if (drawingModel.TeklaDrawing.IsSameDatabaseObject(DrawingHandler.Instance.GetActiveDrawing()))
            SaveAndClose(drawingModel);
    }

    /// <summary>
    ///     Sets a specified user-defined attribute (UDA) on the active Tekla drawing's Title3 property.
    ///     This method commits the change to the Tekla drawing database with a meaningful commit message.
    /// </summary>
    /// <param name="drawingModel">The drawing model containing the Tekla drawing to modify.</param>
    /// <param name="uda">The value to set for the specified UDA on the drawing.</param>
    public void SetUda(DrawingModel drawingModel, string uda) // Gets set in the DrawingModel class
    {
        drawingModel.TeklaDrawing.Title3 = uda;
        drawingModel.TeklaDrawing.CommitChanges($"Change title_3: {drawingModel.TeklaDrawing.Title3} to {uda}");
    }

    public void SetCommitMessage(DrawingModel drawingModel, string commitMessage)
    {
        drawingModel.TeklaDrawing.CommitChanges(commitMessage);
    }

    /// <summary>
    ///     Saves the currently active Tekla drawing and closes it if it is open.
    /// </summary>
    /// <param name="drawingModel">The model representing the Tekla drawing to be saved and closed.</param>
    public void SaveAndClose(DrawingModel drawingModel) // Gets set in the DrawingModel class
    {
        DrawingHandler.Instance.SaveActiveDrawing();
        if (DrawingHandler.Instance.GetActiveDrawing() != null) DrawingHandler.Instance.CloseActiveDrawing(true);
    }

    /// <summary>
    ///     Saves the active Tekla drawing in the Tekla Structures Drawing environment.
    ///     Ensures that the current state of the drawing is committed to the underlying database.
    /// </summary>
    /// <param name="drawingModel">The drawing model containing the active Tekla drawing to be saved.</param>
    public void Save(DrawingModel drawingModel) // Will be used in the GUI mode
    {
        DrawingHandler.Instance.SaveActiveDrawing();
    }
}