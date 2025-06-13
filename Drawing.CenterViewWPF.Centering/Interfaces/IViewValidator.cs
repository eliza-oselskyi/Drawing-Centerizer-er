using Drawing.CenterViewWPF.Centering.TeklaWrapper;

namespace Drawing.CenterViewWPF.Centering.Interfaces;

/// <summary>
/// Defines the contract for validating a Tekla drawing view.
/// Implementations of this interface should determine whether a given view meets
/// specific criteria to be considered valid.
/// </summary>
public interface IViewValidator
{
    bool IsValid(View view);
}