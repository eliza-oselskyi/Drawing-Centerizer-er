using Drawing.CenterViewWPF.Centering.Interfaces;
using Drawing.CenterViewWPF.Centering.TeklaWrapper;

namespace Drawing.CenterViewWPF.Centering.Validation;


public class ViewChecker
{

    private readonly IViewValidator _viewValidator;
    public ViewChecker(IViewValidator viewValidator)
    {
        _viewValidator = viewValidator;
    }
    public bool IsValid(View view)
    {
        return _viewValidator.IsValid(view);
    }
}