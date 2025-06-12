using Drawing.CenterViewWPF.Centering.Interfaces;
using Tekla.Structures.Drawing;
using View = Drawing.CenterViewWPF.Centering.TeklaWrapper.View;

namespace Drawing.CenterViewWPF.Centering.Validation;

public class FabViewValidator : IViewValidator
{
    public bool IsValid(View view)
    {

        if (view.TeklaView is ContainerView && !view.TeklaView.IsSheet)
        {
            // probably should just return false here
            return true;
        }
        else
        {
            return view.ViewType is not (Tekla.Structures.Drawing.View.ViewTypes.DetailView
                or Tekla.Structures.Drawing.View.ViewTypes.SectionView
                or Tekla.Structures.Drawing.View.ViewTypes.UnknownViewType);
        }
    }
}