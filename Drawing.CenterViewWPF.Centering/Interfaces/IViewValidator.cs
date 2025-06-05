using Drawing.CenterViewWPF.Centering.TeklaWrapper;

namespace Drawing.CenterViewWPF.Centering.Interfaces;

public interface IViewValidator
{
    bool IsValid(View view);
}