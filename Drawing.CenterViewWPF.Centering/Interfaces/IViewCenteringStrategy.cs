using Drawing.CenterViewWPF.Centering.TeklaWrapper;

namespace Drawing.CenterViewWPF.Centering.Interfaces;

/// <summary>
/// Represents a strategy for centering a view in a drawing.
/// </summary>
public interface IViewCenteringStrategy
{
    bool Center(View view, bool isGuiMode);
}