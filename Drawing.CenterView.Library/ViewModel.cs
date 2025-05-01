using System.Reflection;
using Tekla.Structures.Geometry3d;

namespace Drawing.CenterView.Library;

public class ViewModel(Tekla.Structures.Drawing.View view)
{
    private Tekla.Structures.Drawing.View _view = view;
    /// <summary>
    /// Whether or not the view can be centered, based on the rules.
    /// </summary>
    public bool CanBeCentered { get; set; }

    /// <summary>
    /// The view type enum derived from the "ViewType" UDA of the view.
    /// </summary>
    public enum ViewType;
    /// <summary>
    /// The associated SheetModel instance of this view
    /// </summary>
    private SheetModel _sheet { get; set; }
    /// <summary>
    /// The view type Tekla assigns to this view. This is useful for fabrication drawings.
    /// </summary>
    private Tekla.Structures.Drawing.View.ViewTypes TeklaViewType { get; set; }
    /// <summary>
    /// The associated DrawingModel instance of this view.
    /// </summary>
    private DrawingModel Drawing { get; set; }
    /// <summary>
    /// Origin point of the view.
    /// </summary>
    public Point Origin  { get; set; }
    /// <summary>
    /// The centerpoint of the view.
    /// </summary>
    public Point Center { get; set; }
    /// <summary>
    /// The Tekla drawing type of the view.
    /// </summary>
    private Type ParentDrawingType { get; set; }
    /// <summary>
    /// The list of views contained in current view.
    /// </summary>
    public List<ViewModel> ChildViews { get; set; } 

}
