using System;
using System.Collections.Generic;
using Drawing.CenterViewWPF.Common;
using Drawing.CenterViewWPF.Common.Enums;
using Drawing.CenterViewWPF.Common.Interfaces;
using Drawing.CenterViewWPF.Common.Strategies;

namespace Drawing.CenterViewWPF.Centering.TeklaWrapper;

public class DrawingModel
{
    public Tekla.Structures.Drawing.Drawing TeklaDrawing { get; private set; }
    public DrawingType DrawingType { get; private set; }
    private List<View> _views = new();
    private readonly DrawingOperator _drawingOperator = new();
    public bool IsValid {get; private set;}

    public DrawingModel(Tekla.Structures.Drawing.Drawing teklaDrawing)
    {
        TeklaDrawing = teklaDrawing;
        DrawingType = Utility.GetValueFromDescription<DrawingType>(teklaDrawing.DrawingTypeStr);
        GetAndAddViews();
        IsValid = IsValidToCenter();
    }

    private bool IsValidToCenter()
    {
        var count = _views.Count;
        return count == 1;
    }

    private void GetAndAddViews()
    {
        var v = TeklaDrawing.GetSheet().GetViews();
        var s = GetViewTypeStrategy();
        while (v.MoveNext()) _views.Add(new View((Tekla.Structures.Drawing.View)v.Current, s));
    }

    private IGetViewTypeStrategy GetViewTypeStrategy()
    {
        return DrawingType switch
        {
            DrawingType.Assembly => new GetFabViewTypeStrategy(),
            DrawingType.GeneralArrangement => new GetGaViewTypeStrategy(),
            _ => throw new ArgumentException("Unknown drawing type.")
        };
    }

    public override string ToString()
    {
        return TeklaDrawing.DrawingTypeStr;
    }
}