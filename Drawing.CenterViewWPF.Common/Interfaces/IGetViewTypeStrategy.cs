using System;
using Tekla.Structures.Drawing;

namespace Drawing.CenterViewWPF.Common.Interfaces;

/// <summary>
/// Defines a strategy for determining the type of a drawing view.
/// </summary>
public interface IGetViewTypeStrategy
{
    Enum GetViewType(ViewBase view, out string typeString);
}