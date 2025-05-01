namespace Drawing.CenterView.Library;

public class CurrentDrawing{

  public Tekla.Structures.Drawing.Drawing Drawing {get; set;}
  public Tekla.Structures.Drawing.DrawingObjectEnumerator Views {get; set;}
  public bool IsValidForCenter {get; set;}

  public CurrentDrawing(Tekla.Structures.Drawing.Drawing drawing)
  {
    Drawing = drawing;
    Views = Drawing.GetSheet().GetViews();
  }
}
