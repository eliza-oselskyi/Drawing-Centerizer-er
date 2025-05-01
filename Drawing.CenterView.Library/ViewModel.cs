using System.Reflection;
using Tekla.Structures.Geometry3d;

namespace Drawing.CenterView.Library;

public class ViewModel(Tekla.Structures.Drawing.View view)
{
    private Tekla.Structures.Drawing.View _view = view;
    public bool CanBeCentered { get; set; }

    public enum ViewType;
    private SheetModel _sheet { get; set; }
    private Tekla.Structures.Drawing.View.ViewTypes TeklaViewType { get; set; }
    private Tekla.Structures.Drawing.Drawing Drawing { get; set; }
    public Point Origin  { get; set; }
    public Point Center { get; set; }
    private Type ParentDrawingType { get; set; }
    public List<ViewModel> ChildViews { get; set; } 

}
