using Drawing.CenterViewWPF.Centering.TeklaWrapper;

namespace Drawing.CenterViewWPF.Centering.Interfaces;

public interface IViewCenteringStrategy
{
    bool Center(View view, bool isGuiMode);
}