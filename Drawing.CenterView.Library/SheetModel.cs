using Tekla.Structures.Drawing;
using Tekla.Structures.Geometry3d;

namespace Drawing.CenterView.Library;

public class SheetModel
{
    private Tekla.Structures.Drawing.Drawing Drawing;
    private static ContainerView SheetView { get; set; }
    public Tuple<double, double> Size { get; set; } = new  Tuple<double, double>(SheetView.Width, SheetView.Height);
    private double TitleBlockWidth { get; set; }
    private double TemplateBlockHeight { get; set; }
    private Point AdjustedCenter { get; set; }
    public Tuple<double, double> Offset { get; set; }

    public SheetModel(ContainerView sheet)
    {
        SheetView = sheet;
    }
}