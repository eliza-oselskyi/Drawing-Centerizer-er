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

/// <summary>
///     Represents a model for interacting with Tekla Structures drawings, encapsulating
///     drawing information, its type, and the validity of its state for centering operations.
/// </summary>
public class DrawingModel
{
    private readonly DrawingOperator _drawingOperator = new();
    private readonly bool _isGuiMode;
    private readonly List<View> _views = new();

    /// Represents a wrapper around a Tekla drawing, providing properties and methods
    /// for processing and manipulating drawing views and centering strategies.
    public DrawingModel(Tekla.Structures.Drawing.Drawing teklaDrawing, bool isGuiMode = false)
    {
        _isGuiMode = isGuiMode;
        TeklaDrawing = teklaDrawing;
        DrawingType = Utility.GetValueFromDescription<DrawingType>(teklaDrawing.DrawingTypeStr);
        GetAndAddViews();
        IsValid = IsValidToCenter();
    }

    public Tekla.Structures.Drawing.Drawing TeklaDrawing { get; }
    public DrawingType DrawingType { get; }
    public bool IsValid { get; }

    /// Determines if the drawing is valid for centering based on the views it contains
    /// and their associated characteristics. A drawing is considered valid for centering
    /// if it satisfies specific conditions related to the number or type of views it holds.
    /// Invalid or redundant container views are removed during this validation process.
    /// <returns>
    ///     True if the drawing satisfies the conditions to be centered; otherwise, false.
    /// </returns>
    private bool IsValidToCenter()
    {
        if (_views.Exists(v => !v.TeklaView.IsSheet && v.TeklaView is ContainerView))
        {
            foreach (var view in _views.ToList())
                if (view.TeklaView is ContainerView containerView)
                    // Ensure the container view is removed
                    _views.Remove(view);

            return true;
        }
        else
        {
            var count = _views.Count(view => view.IsValid);
            return count == 1;
        }
    }

    /// Retrieves all views from the Tekla drawing's sheet and adds them to the internal
    /// collection of views for further processing. The method determines the view type
    /// strategy based on the drawing type and uses it to classify the views while
    /// initializing them.
    private void GetAndAddViews()
    {
        var v = TeklaDrawing.GetSheet().GetViews();
        var s = GetViewTypeStrategy();
        while (v.MoveNext()) _views.Add(new View((ViewBase)v.Current, s));
    }

    /// Determines and returns the appropriate strategy for identifying the type
    /// of a drawing view, based on the current drawing type.
    private IGetViewTypeStrategy GetViewTypeStrategy()
    {
        return DrawingType switch
        {
            DrawingType.Assembly => new GetFabViewTypeStrategy(),
            DrawingType.GeneralArrangement => new GetGaViewTypeStrategy(),
            DrawingType.SinglePartDrawing => new GetFabViewTypeStrategy(),
            _ => throw new ArgumentException("Unknown drawing type.")
        };
    }

    /// Centers the drawing using a specified centering strategy for its views.
    /// Validates the drawing and each view before attempting to center, using the strategy provided.
    /// Updates user-defined attributes for each view and handles saving or closing the drawing depending on mode settings.
    /// <param name="strategy">The implementation of the centering strategy to apply to the drawing's views.</param>
    /// <returns>True if the drawing is successfully processed; otherwise, false.</returns>
    public bool CenterDrawing(IViewCenteringStrategy strategy)
    {
        if (!IsValid) return false;
        foreach (var view in _views.Where(view => view.IsValid))
        {
            _drawingOperator.SetUp(this);
            var result = strategy.Center(view, _isGuiMode);
            //_drawingOperator.SetUda(this, result ? "C" : "NC");
            _drawingOperator.SetCommitMessage(this, $"Centering {view.ViewType}");
            if (!_isGuiMode) _drawingOperator.SaveAndClose(this);
        }

        return true;
    }

    /// Retrieves the list of drawing views contained within the DrawingModel.
    /// <returns>
    ///     A list of views representing Tekla drawing views associated with the model.
    /// </returns>
    public List<View> GetViews()
    {
        return _views;
    }

    public override string ToString()
    {
        return TeklaDrawing.DrawingTypeStr;
    }
}