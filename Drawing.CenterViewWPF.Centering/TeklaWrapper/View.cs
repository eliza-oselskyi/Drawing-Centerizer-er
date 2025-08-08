using System;
using Drawing.CenterViewWPF.Centering.Validation;
using Drawing.CenterViewWPF.Common.Interfaces;
using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Centering.TeklaWrapper;

/// <summary>
///     Represents a Tekla drawing view and provides functionality for validation, type determination,
///     and position manipulation within the drawing.
/// </summary>
public class View
{
    private readonly string _viewTypeString;

    /// Represents a Tekla drawing view with associated validation and type identification.
    /// A `View` encapsulates a Tekla drawing view and uses a strategy to determine its type.
    /// It also validates its configuration based on the determined type.
    /// Constructor:
    /// - Accepts a Tekla.Structures.Drawing.ViewBase object representing the actual Tekla view.
    /// - Requires an `IGetViewTypeStrategy` implementation to identify the view type based on criteria.
    /// Validation:
    /// - Differentiates between various view types (e.g., "A" for fabrication and "G" for general arrangement).
    /// - Uses corresponding validators like `FabViewValidator` and `GaViewValidator`.
    /// - Throws an exception for unknown or unsupported view types.
    /// Properties:
    /// - `IsValid`: Indicates whether the view passes validation.
    public View(ViewBase teklaView, IGetViewTypeStrategy viewTypeStrategy)
    {
        TeklaView = teklaView;
        ViewType = viewTypeStrategy.GetViewType(teklaView, out _viewTypeString);
        var viewChecker = _viewTypeString switch
        {
            "A" => new ViewChecker(new FabViewValidator()),
            "G" => new ViewChecker(new GaViewValidator()),
            "W" => new ViewChecker(new FabViewValidator()),
            _ => throw new ArgumentException("Unknown view type")
        };

        IsValid = viewChecker.IsValid(this);
    }

    public ViewBase TeklaView { get; }
    public Enum ViewType { get; }

    public bool IsValid { get; }

    /// Adjusts the position of the associated Tekla drawing view in a specified direction.
    /// Shifts the view either by a small or large increment based on the provided parameters.
    /// The `Shift` method modifies the `Origin` property of the Tekla view to reposition it.
    /// It performs the operation in one of the four cardinal directions: "up", "down", "left", or "right".
    /// The shift magnitude is determined by whether a large shift is requested.
    /// Throws an exception if an invalid direction string is provided.
    /// <param name="direction">
    ///     The direction in which to shift the view. Acceptable values are "up", "down", "left", or
    ///     "right".
    /// </param>
    /// <param name="isBigShift">
    ///     Determines the magnitude of the shift. `true` indicates a large shift, `false` indicates a
    ///     small shift. Default is `false`.
    /// </param>
    public void Shift(string direction, bool isBigShift = false)
    {
        var shiftAmount = isBigShift ? 20.0 : 10.0;

        switch (direction.ToLower())
        {
            case "up":
                TeklaView.Origin.Y += shiftAmount;
                break;
            case "down":
                TeklaView.Origin.Y -= shiftAmount;
                break;
            case "left":
                TeklaView.Origin.X -= shiftAmount;
                break;
            case "right":
                TeklaView.Origin.X += shiftAmount;
                break;
            default:
                throw new ArgumentException($"Invalid direction: {direction}");
        }

        TeklaView.Modify();
        TeklaView.GetDrawing().CommitChanges($"Shift {direction} {shiftAmount}");
    }
}