using System;
using System.Collections.Generic;
using System.Linq;
using Drawing.CenterViewWPF.Centering.Interfaces;
using Drawing.CenterViewWPF.Common;
using Drawing.CenterViewWPF.Common.Enums;
using Drawing.CenterViewWPF.Common.Interfaces;
using Drawing.CenterViewWPF.Common.Strategies;
using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Centering.TeklaWrapper;

public class DrawingModel
{
    public Tekla.Structures.Drawing.Drawing TeklaDrawing { get; private set; }
    public DrawingType DrawingType { get; private set; }
    private List<View> _views = new();
    private readonly DrawingOperator _drawingOperator = new();
    public bool IsValid {get; private set;}
    private bool _isGuiMode;

    public DrawingModel(Tekla.Structures.Drawing.Drawing teklaDrawing, bool isGuiMode = false)
    {
        _isGuiMode = isGuiMode;
        TeklaDrawing = teklaDrawing;
        DrawingType = Utility.GetValueFromDescription<DrawingType>(teklaDrawing.DrawingTypeStr);
        GetAndAddViews();
        IsValid = IsValidToCenter();
    }

    private bool IsValidToCenter()
    {
        if (_views.Exists(v => !v.TeklaView.IsSheet && v.TeklaView is ContainerView))
        {
            foreach (var view in _views.ToList())
            {
                if (view.TeklaView is ContainerView containerView)
                {
                    // Ensure the container view is removed
                    _views.Remove(view);
                }
            }

            return true;
        }
        else
        {
            var count = _views.Count(view => view.IsValid);
            return count == 1;
        }
    }

    private void GetAndAddViews()
    {
        var v = TeklaDrawing.GetSheet().GetViews();
        var s = GetViewTypeStrategy();
        while (v.MoveNext()) _views.Add(new View((Tekla.Structures.Drawing.ViewBase)v.Current, s));
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

    public bool CenterDrawing(IViewCenteringStrategy strategy)
    {
        if (!IsValid) return false;
        foreach (var view in _views.Where(view => view.IsValid))
        {
            _drawingOperator.SetUp(this);
            var result = strategy.Center(view, _isGuiMode);
            _drawingOperator.SetUda(this, result ? "C" : "NC");
            if (!_isGuiMode)
            {
                _drawingOperator.SaveAndClose(this);
            }
        }

        return true;
    }

    public List<View> GetViews()
    {
        return _views;
    }

    public override string ToString()
    {
        return TeklaDrawing.DrawingTypeStr;
    }
}