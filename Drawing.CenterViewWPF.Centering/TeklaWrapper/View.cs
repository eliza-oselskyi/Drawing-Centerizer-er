using System;
using Drawing.CenterViewWPF.Centering.Validation;
using Drawing.CenterViewWPF.Common;
using Drawing.CenterViewWPF.Common.Interfaces;

namespace Drawing.CenterViewWPF.Centering.TeklaWrapper;

public class View
{
    private readonly string _viewTypeString;
    public Tekla.Structures.Drawing.View TeklaView { get; private set; }
    public Enum ViewType { get; }
    
    public bool IsValid { get; }

    public View(Tekla.Structures.Drawing.View teklaView, IGetViewTypeStrategy viewTypeStrategy)
    {
        TeklaView = teklaView;
        ViewType = viewTypeStrategy.GetViewType(teklaView, out _viewTypeString);
        var viewChecker = _viewTypeString switch
        {
            "A" => new ViewChecker(new FabViewValidator()),
            "G" => new ViewChecker(new GaViewValidator()),
            _ => throw new ArgumentException("Unknown view type")
        };

        IsValid = viewChecker.IsValid(this);
    }
}